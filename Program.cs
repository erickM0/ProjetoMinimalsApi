using Minimal_Api.Domain.Entities;
using Minimal_Api.Domain.DTOs;
using Minimal_Api.Infra.DB;
using Microsoft.EntityFrameworkCore;
using Minimal_Api.Domain.Interfaces;
using Minimal_Api.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Minimal_Api.Domain.ModelsView;
using Minimal_Api.Domain.Enum;
using System.Text.Json.Serialization;

#region  builder

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IAdmService,AdmService>();
builder.Services.AddScoped<IVehicleService,VehicleService>();

// Fixing Enum Serialization
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

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
ValidationsError AdmDTOValidation(AdmDTO admDTO){
     var validationsError = new ValidationsError();
    

    if(string.IsNullOrEmpty(admDTO.Email)) validationsError.Message.Add("The 'Email' field must not be empty!");
    if(string.IsNullOrEmpty(admDTO.Password)) validationsError.Message.Add("The 'Password' field must not be empty!");
    if(!Enum.IsDefined(typeof(EnumProfiles),admDTO.Profile)) validationsError.Message.Add("The 'Profile' must be one of the options:Admin, Edit, Seller!");
    return validationsError;
}

app.MapPost("/admin/login",([FromBody] LoginDTO loginDTO,IAdmService admService) =>{
    if (admService.Login(loginDTO) != null)
    {
        return Results.Ok("Login com sucesso!");
    }
    else{
        return Results.Unauthorized();
    }
}).WithTags("Admin");

app.MapPost("/admin",([FromBody] AdmDTO admDTO, IAdmService admService) =>{

    
    
    ValidationsError validations = AdmDTOValidation(admDTO);

    
    if(validations.Message.Count()>0) return Results.BadRequest(validations);

    var adm = new Adm{
        Email = admDTO.Email,
        Password = admDTO.Password,
        Profile = admDTO.Profile.ToString()
    };

    admService.Create(adm);

    return Results.Created($"/admin/{adm.Id}", adm);

}).WithTags("Admin");

app.MapGet("/admin",([FromQuery] int? page, IAdmService admService  ) =>{
     var list = new List<AdmModelView>();
    var admList = admService.All(page);
    
    foreach(var adm in admList){
        list.Add(new AdmModelView{
            Id = adm.Id,
            Email = adm.Email,
            Profile = adm.Profile.ToString()

        });
    }


    return Results.Ok(list);
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