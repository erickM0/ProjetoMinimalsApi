using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public void Create(Adm adm){
           _dbContext.Administradores.Add(adm);
           _dbContext.SaveChanges();

        }

        public List<Adm> All(int? page){

            var query = _dbContext.Administradores.AsQueryable();
            int pageInt = page != null ? (int)page : 1;
            int itemsPerPage = 10;

            query = query.Skip((pageInt - 1) * itemsPerPage).Take(itemsPerPage);

            return query.ToList();  
        }

        public Adm? SearchById(int id){
            
            return _dbContext.Administradores.Where(adm => adm.Id == id).FirstOrDefault();

        }

        public void Delete(Adm adm){
            _dbContext.Administradores.Remove(adm);
            _dbContext.SaveChanges();
        }
    }
}