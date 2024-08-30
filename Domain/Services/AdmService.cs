using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Minimal_Api.Domain.DTOs;
using Minimal_Api.Domain.Entities;
using Minimal_Api.Domain.Interfaces;
using Minimal_Api.Infra.DB;

namespace Minimal_Api.Domain.Services
{
    public class AdmService : IAdmService
    {
        private readonly DbContexto _dbContext;
        public AdmService(DbContexto dbContext){
            _dbContext =dbContext;
        }
        public Adm? Login(LoginDTO loginDTO){
           
            var list = _dbContext.Administradores?.Where(a => a.Email == loginDTO.Email && a.Password == loginDTO.Password).FirstOrDefault();
            return list;
            
        }
    }
}