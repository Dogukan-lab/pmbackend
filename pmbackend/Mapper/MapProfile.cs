using System.ComponentModel;
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using pmbackend.Models;
using pmbackend.Models.Dto;

namespace pmbackend.Mapper
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<PmUser, PmUserDto>();
            CreateMap<PmUserDto, PmUser>().ForMember(dst => dst.Id, opt => opt.Ignore());
            CreateMap<Chat, ChatDto>().ReverseMap();
            CreateMap<Message, MessageDto>().ForMember(dest => dest.UserName, opt =>
                opt.MapFrom(src => src.User!.UserName)).ReverseMap();
        }
    }
}