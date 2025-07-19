using AutoMapper;
using WebApplication1.Commands.Records.Create;
using WebApplication1.Dto;
using WebApplication1.Models;
using WebApplication1.Users.Commands;

namespace WebApplication1.Common.Mappings;

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