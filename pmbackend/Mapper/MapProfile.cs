using AutoMapper;
using pmbackend.Models;
using pmbackend.Models.Dto;

namespace pmbackend.Mapper
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<PmUser, PmUserDto>().ReverseMap();
        }
    }
}