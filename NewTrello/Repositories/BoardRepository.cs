using Microsoft.EntityFrameworkCore;
using NewTrello.Context;
using NewTrello.Models;

namespace NewTrello.Repositories;

public class BoardRepository(AppDbContext context) : BaseRepository<Board>(context)
{
    public async Task<IEnumerable<Board>> GetAllWithIncludesAsync()
    {
        return await _context.Boards!
            .Include(b => b.Owner!)
            .Include(b => b.Columns!)
            .ToListAsync();
    }

    public async Task<Board?> GetByIdWithIncludesAsync(Guid id)
    {
        var board = new Board();
        var columns = new Dictionary<Guid, Column>();

        var conn = _context.Database.GetDbConnection();
        await conn.OpenAsync();

        var sql = @"
        SELECT 
            b.Id AS BoardId, b.Name AS BoardName,
            u.Id AS OwnerId, u.Name AS OwnerName, u.Email AS OwnerEmail,
            c.Id AS ColumnId, c.Name AS ColumnName,
            cd.Id AS CardId, cd.Title AS CardTitle, cd.AssignedUserId
        FROM Boards b
        JOIN User u ON b.OwnerId = u.Id
        LEFT JOIN Columns c ON c.BoardId = b.Id
        LEFT JOIN Cards cd ON cd.ColumnId = c.Id
        WHERE b.Id = @id";

        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;

        var param = cmd.CreateParameter();
        param.ParameterName = "@id";
        param.Value = id;
        cmd.Parameters.Add(param);

        using var reader = await cmd.ExecuteReaderAsync();

        Console.WriteLine("----------------");
        for (int i = 0; i < reader.FieldCount; i++)
        {
            Console.WriteLine(reader.GetName(i));
        }

        while (await reader.ReadAsync())
        {
            if (board.Id == Guid.Empty)
            {
                board.Id = reader.GetGuid(reader.GetOrdinal("BoardId"));
                board.Name = reader.GetString(reader.GetOrdinal("BoardName"));
                board.OwnerId = reader.GetGuid(reader.GetOrdinal("OwnerId"));
                board.Owner = new User
                {
                    //Id = reader.GetGuid(reader.GetOrdinal("OwnerId")),
                    Name = reader.GetString(reader.GetOrdinal("OwnerName")),
                    //Email = reader.GetString(reader.GetOrdinal("OwnerEmail"))
                };
                board.Columns = new List<Column>();
            }

            if (!reader.IsDBNull(reader.GetOrdinal("ColumnId")))
            {
                var columnId = reader.GetGuid(reader.GetOrdinal("ColumnId"));

                if (!columns.ContainsKey(columnId))
                {
                    var column = new Column
                    {
                        Id = columnId,
                        Name = reader.GetString(reader.GetOrdinal("ColumnName")),
                        BoardId = reader.GetGuid(reader.GetOrdinal("BoardId")),
                        Cards = new List<Card>()
                    };

                    columns[column.Id] = column;
                    board.Columns!.Add(column);
                }

                if (!reader.IsDBNull(reader.GetOrdinal("CardId")))
                {
                    var column = columns[columnId];

                    var card = new Card
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("CardId")),
                        Title = reader.GetString(reader.GetOrdinal("CardTitle")),
                        ColumnId = columnId,
                        Column = column
                    };

                    if (!reader.IsDBNull(reader.GetOrdinal("AssignedUserId")))
                    {
                        card.AssignedUserId = reader.GetGuid(reader.GetOrdinal("AssignedUserId"));
                    }

                    columns[columnId].Cards!.Add(card);
                }
            }
        }

        return board.Id == Guid.Empty ? null : board;
    }
}
