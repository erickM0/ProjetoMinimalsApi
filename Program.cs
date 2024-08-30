using Minimal_Api.Domain.Entities;
using Microsoft.OpenApi.Models;
using Minimal_Api.Domain.DTOs;
using Minimal_Api.Infra.DB;
using Microsoft.EntityFrameworkCore;
using Minimal_Api.Domain.Interfaces;
using Minimal_Api.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Swagger;
using Minimal_Api.Domain.ModelsView;

#region  builder

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IAdmService,AdmService>();
builder.Services.AddScoped<IVehicleService,VehicleService>();


builder.Services.AddDbContext<DbContexto>(options =>{
    options.UseMySql(
        builder.Configuration.GetConnectionString("mysql"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql"))
    );
});

var app = builder.Build();
#endregion

#region Home

app.MapGet("/", () => Results.Json(new Home())).WithTags("Home");

#endregion

#region Adm
app.MapPost("/admin/login",([FromBody]LoginDTO loginDTO,IAdmService admService) =>{
    
}).WithTags("Admin");

#endregion

#region Vehicles
ValidationsError VehicleDTOValidation(VehicleDTO vehicleDTO){
     var validationsError = new ValidationsError();
    

    if(string.IsNullOrEmpty(vehicleDTO.Name)) validationsError.Message.Add("The 'Name' field must not be empty!");
    if(string.IsNullOrEmpty(vehicleDTO.Brand)) validationsError.Message.Add("The 'Brand' field must not be empty!");
    if(vehicleDTO.Year < 1950) validationsError.Message.Add("The 'Year' field must be 1950 and above!");
    return validationsError;
}

app.MapPost("/vehicles", ([FromBody] VehicleDTO vehicleDTO,IVehicleService vehicleService)=>{

    ValidationsError validationsError = VehicleDTOValidation(vehicleDTO);

   if(validationsError.Message.Count() > 0) return Results.BadRequest(validationsError);

    var vehicle = new Vehicle{
        Name = vehicleDTO.Name,
        Brand = vehicleDTO.Brand,
        Year = vehicleDTO.Year};

    vehicleService.Include( vehicle);
    return Results.Created($"/vehicle/{vehicle.Id}", vehicle);

}).WithTags("Vehicles");

app.MapPut("/vehicles/{id}", ([FromRoute] int id, VehicleDTO vehicleDTO,IVehicleService vehicleService)=>{
    
        
    var vehicle = vehicleService.ShearchById(id);
    if(vehicle == null) return Results.NotFound();

    ValidationsError validationsError = VehicleDTOValidation(vehicleDTO);
    if(validationsError.Message.Count() > 0) return Results.BadRequest(validationsError);

    
    vehicle.Name = vehicleDTO.Name;
    vehicle.Brand = vehicleDTO.Brand;
    vehicle.Year = vehicleDTO.Year;

    vehicleService.Update(vehicle);
    return Results.Ok(vehicle);
}).WithTags("Vehicles");

app.MapDelete("/vehicles/{id}", ([FromRoute] int id, IVehicleService vehicleService)=>{
    
    var vehicle = vehicleService.ShearchById(id);
    if(vehicle == null) return Results.NotFound();
    
    vehicleService.Delete(vehicle);
    return Results.NoContent();

}).WithTags("Vehicles");


app.MapGet("/vehicles", ([FromQuery] int? page, string? name, string? brand ,IVehicleService vehicleService)=>{

    int pageInt = page != null ? (int)page : 1;

    var vehicles = vehicleService.All(pageInt,name,brand);
    
    return Results.Ok(vehicles);
}).WithTags("Vehicles");

app.MapGet("/vehicles/{id}", ([FromRoute]int id, IVehicleService vehicleService)=>{

    var vehicle = vehicleService.ShearchById(id);
    if(vehicle != null){
        return Results.Ok(vehicle);
    }

    return Results.NotFound();
    
}).WithTags("Vehicles");

#endregion

#region app
app.UseSwagger();
app.UseSwaggerUI();

app.Run();


#endregion