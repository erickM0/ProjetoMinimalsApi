using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Minimal_Api.Domain.Entities;

namespace Minimal_Api.Infra.DB
{
    public class DbContexto : DbContext
    {

        private readonly IConfiguration _configurationAppSettings;
        public DbContexto(IConfiguration configurationAppSettings){
            _configurationAppSettings = configurationAppSettings;
        }

        public DbSet<Adm> Administradores{ get; set; }
        public DbSet<Vehicle> Vehicles{ get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Adm>().HasData(
                new  Adm {
                Id = 1,
                Email = "teste@teste.com",
                Password = "123456",
                Profile = "Admin"
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){

            if(!optionsBuilder.IsConfigured){

                var connectionString = _configurationAppSettings.GetConnectionString("MySql")?.ToString();

                if(!string.IsNullOrEmpty(connectionString)){
                    optionsBuilder.UseMySql(
                        connectionString
                        , ServerVersion.AutoDetect(connectionString)
                    );
                }
                
            }
            
            
        }
    }

    
}