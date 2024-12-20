﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace pmbackend.Models
{
    public class PmUser: IdentityUser<int>
    {
        public override int Id { get; set; }
        public int ProfileIcon { get; set; } = 0;
        public int Background { get; set; } = 0;
        public string Bio { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public bool IsOnline { get; set; } = true;
        
        //TODO Figure something out to fix this mess.
        [JsonIgnore]
        public virtual ICollection<PmUser>? Friends { get; set; }
        [JsonIgnore]
        public virtual ICollection<Chat>? Chats { get; set; }
    }
}