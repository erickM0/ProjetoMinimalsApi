using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Minimal_Api.Domain.Enum;

namespace Minimal_Api.Domain.DTOs
{
    public class AdmDTO
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required EnumProfiles Profile {get; set;} 

    }
}