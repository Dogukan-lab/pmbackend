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
            CreateMap<PmUser, PmUserDto>().ReverseMap();
            CreateMap<Chat, ChatDto>().ReverseMap();
            CreateMap<Message, MessageDto>().ReverseMap();
        }
    }
}