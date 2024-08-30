using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;


namespace Minimal_Api.Domain.Entities
{
    public class Vehicle 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } = default;

        [Required]
        [StringLength(150)]
         public required string Name { get; set; } 

        [Required]
        [StringLength(100)]
        public required string Brand { get; set; } 

        [Required]
        public int Year { get; set;} = default;
    }
}