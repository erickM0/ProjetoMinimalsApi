using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Minimal_Api.Domain.DTOs;
using Minimal_Api.Domain.Entities;

namespace Minimal_Api.Domain.Interfaces
{
     public interface IVehicleService
    {
        List<Vehicle> All(int page = 1, string? name = null, string? brand = null  );
        Vehicle? ShearchById(int id);

        void Include(Vehicle vehicle);
        void Update(Vehicle vehicle);
        void Delete(Vehicle vehicle);
    }
}