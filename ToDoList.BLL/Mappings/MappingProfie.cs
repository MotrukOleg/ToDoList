using AutoMapper;
using ToDoList.BLL.Commands.Records.Create;
using ToDoList.BLL.Commands.Users.Create;
using ToDoList.BLL.Dto;
using ToDoList.DAL.Models;

namespace ToDoList.BLL.Mappings;

public class MappingProfie : Profile
{
    public MappingProfie()
    {
        CreateMap<CreateUserCommand, User>().ReverseMap();
        CreateMap<CreateRecordCommand, Record>().ReverseMap();
        CreateMap<RegisterUserDto, User>().ReverseMap();
        CreateMap<OutputUserDto, User>()
            .ForMember(u => u.Records, opt => opt.MapFrom(src => src.Records))
            .ReverseMap();
        CreateMap<Record, OutputRecordDto>()
            .ForMember(r => r.UserName , opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName))
            .ReverseMap();
        CreateMap<User, LoginUserDto>().ReverseMap();

    }
}   