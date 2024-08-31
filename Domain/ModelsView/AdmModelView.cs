using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minimal_Api.Domain.ModelsView
{
    public class AdmModelView
    {
        public required int Id { get; set; }
        public required string Email { get; set; }
        public required string Profile {get; set;}
    }
}