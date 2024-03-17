using pmbackend.Models;
using pmbackend.Models.Dto;
using AutoMapper;

namespace pmbackend.Mapper;

public class Mapper
{
   public Mapper()
   {
       CreateMap<PmUser, PmUserDto>().ReverseMap();
   } 
}