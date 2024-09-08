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
    public class VehicleService : IVehicleService
    {
        private readonly DbContexto _dbContext;
        public VehicleService(DbContexto dbContext){
            _dbContext = dbContext;
        }
        public List<Vehicle> All(int page = 1, string? name = null, string? brand = null){
           
            var query =  _dbContext.Vehicles.AsQueryable();

            if(!String.IsNullOrEmpty(name)){
              query = query.Where(v => v.Name.Contains(name));
            }

            if(!String.IsNullOrEmpty(brand)){
                query = query.Where(v => v.Brand.Contains(brand));
            }

            int itemsPerPage = 10;
            
           query = query.Skip((page - 1)*itemsPerPage).Take(itemsPerPage);

            return query.ToList();
            
        }

        public void Delete(Vehicle vehicle)
        {
            _dbContext.Vehicles.Remove(vehicle);
            _dbContext.SaveChanges();
        }

        public void Include(Vehicle vehicle)
        {
            _dbContext.Vehicles.Add(vehicle);
            _dbContext.SaveChanges();
            
            
        }

        public Vehicle? SearchById(int id)
        {
            var query =  _dbContext.Vehicles.AsQueryable();
            return query.Where(v=> v.Id == id).FirstOrDefault();
        }

        public void Update(Vehicle vehicle)
        {
             _dbContext.Vehicles.Update(vehicle);
             _dbContext.SaveChanges(); 

        }
    }
}