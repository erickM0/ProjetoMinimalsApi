using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minimal_Api.Domain.DTOs
{
     public record VehicleDTO
    {
        
         public required string Name { get; set; }

        
        public required string Brand { get; set; }

        public int Year { get; set;} = default;
    }
}