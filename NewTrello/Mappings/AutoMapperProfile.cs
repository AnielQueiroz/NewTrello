using AutoMapper;
using NewTrello.Dtos;
using NewTrello.Models;

namespace NewTrello.Mappings;
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<User, UserResponseDTO>();
        CreateMap<UserCreateRequestDTO, User>();
        CreateMap<UserUpdateRequestDTO, User>();

        CreateMap<Card, CardResponseDTO>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.AssignedUserId))
            .ForMember(dest => dest.ColumnId, opt => opt.MapFrom(src => src.ColumnId));
        CreateMap<CardCreateRequestDTO, Card>();
        CreateMap<CardUpdateRequestDTO, Card>();

        CreateMap<Column, ColumnResponseDTO>()
            .ForMember(dest => dest.Cards, opt => opt.MapFrom(src => src.Cards));
        CreateMap<ColumnCreateRequestDTO, Column>();
        CreateMap<ColumnUpdateRequestDTO, Column>();

        CreateMap<Board, BoardResponseDTO>()
            .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner!.Name))
            .ForMember(dest => dest.Columns, opt => opt.MapFrom(src => src.Columns));
        CreateMap<BoardCreateDTO, Board>();
        CreateMap<BoardUpdateDTO, Board>();
    }
}
