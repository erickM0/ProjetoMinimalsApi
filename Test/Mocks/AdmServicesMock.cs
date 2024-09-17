using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Minimal_Api.Domain.DTOs;
using Minimal_Api.Domain.Entities;
using Minimal_Api.Domain.Interfaces;

namespace Test.Mocks
{
    public class AdmServicesMock : IAdmService
    {
        private static List<Adm> admins = new List<Adm>()
        {
            new Adm 
            {
                Id = 1,
                Email = "teste@teste",
                Password = "teste123",
                Profile = "Admin"
            }
        };
        public List<Adm> All(int? Page)
        {
            return admins;
        }

        public void Create(Adm adm)
        {
            adm.Id = admins.Count + 1;
            admins.Add(adm);
        }

        public void Delete(Adm adm)
        {
           admins.Remove(adm);
        }

        public Adm? Login(LoginDTO loginDTO)
        {
            return admins.Find(a => a.Email == loginDTO.Email && a.Password == loginDTO.Password);
        }

        public Adm? SearchById(int id)
        {
            return admins.Find(a => a.Id == id);
        }
    }
}