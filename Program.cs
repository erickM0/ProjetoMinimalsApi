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

app.MapGet("/", () => Results.Json(new Home()));

#endregion

#region Adm
app.MapPost("/admin/login",([FromBody]LoginDTO loginDTO,IAdmService admService) =>{
    if (admService.Login(loginDTO) != null)
    {
        return Results.Ok("Login com sucesso!");
    }
    else{
        return Results.Unauthorized();
    }
});
#endregion

#region Vehicles
app.MapPost("/vehicles", ([FromBody] VehicleDTO vehicleDTO,IVehicleService vehicleService)=>{
    
    var vehicle = new Vehicle{
        Name = vehicleDTO.Name,
        Brand = vehicleDTO.Brand,
        Year = vehicleDTO.Year};

    vehicleService.Include( vehicle);
    return Results.Created($"/vehicle/{vehicle.Id}", vehicle);
});
#endregion



#region app
app.UseSwagger();
app.UseSwaggerUI();

app.Run();


#endregion