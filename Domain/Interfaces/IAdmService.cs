using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Minimal_Api.Domain.DTOs;
using Minimal_Api.Domain.Entities;

namespace Minimal_Api.Domain.Interfaces
{
     public interface IAdmService
    {
        Adm? Login(LoginDTO loginDTO);

        void Create(Adm adm);

        List<Adm> All(int? Page);

        Adm? SearchById(int id);
    }
}