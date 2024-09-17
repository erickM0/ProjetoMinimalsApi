using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Minimal_Api.Domain.Entities;
using Minimal_Api.Domain.Interfaces;


namespace Test.Mocks
{
    public class VehicleServicesMock : IVehicleService
    {
        private static List<Vehicle> vehicles = new List<Vehicle>();
        public List<Vehicle> All(int page = 1, string? name = null, string? brand = null)
        {
            return vehicles;
        }

        public void Delete(Vehicle vehicle)
        {
           vehicles.Remove(vehicle);
        }

        public void Include(Vehicle vehicle)
        {
            vehicle.Id = vehicles.Count + 1;
            vehicles.Add( vehicle);
        }

        public Vehicle? SearchById(int id)
        {
            return vehicles.Find(v => v.Id == id);
        }

        public void Update(Vehicle vehicle)
        {
           vehicles.ForEach(v => 
           {
            if(v.Id == vehicle.Id)
            v = vehicle;
           });
           
        }
    }
}