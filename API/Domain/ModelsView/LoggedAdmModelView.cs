using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Minimal_Api.Domain.Enum;

namespace Minimal_Api.Domain.ModelsView
{
    public class LoggedAdmModelView
    {
        public required string Token { get; set; }
        public required string Email { get; set; }
        public required EnumProfiles Profile {get; set;}
    }
}