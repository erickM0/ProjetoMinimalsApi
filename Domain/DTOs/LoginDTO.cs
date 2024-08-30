using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minimal_Api.Domain.DTOs
{
    public class LoginDTO
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}