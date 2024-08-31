using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using Minimal_Api.Domain.Enum;


namespace Minimal_Api.Domain.Entities
{
    public class Adm
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } = default;

        [Required]
        [StringLength(255)]
         public required string Email { get; set; }

        [Required]
        [StringLength(50)]
        public required string Password { get; set; }

        [Required]
        [StringLength(10)]
        public required string Profile { get; set;}
    }
}