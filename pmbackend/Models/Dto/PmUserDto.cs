using System.Collections.Immutable;
using System.Runtime.InteropServices;
using AutoMapper;
using AutoMapper.Configuration.Annotations;

namespace pmbackend.Models.Dto;

//TODO Update DTO when new data comes into it, for instance the list of Friends
public class PmUserDto
{
    public string Username { get; set; } = string.Empty;
    public bool IsOnline { get; set; }
    public string Bio { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty; 
    public int ProfileIcon { get; set; }
    public int Background { get; set; }

    public ICollection<PmUserDto>? Friends { get; set; }  
}