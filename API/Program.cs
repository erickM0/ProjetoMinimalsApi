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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Validations.Rules;
using Microsoft.AspNetCore.Authorization;

#region  builder

var builder = WebApplication.CreateBuilder(args);

var key = builder.Configuration.GetSection("Jwt").ToString();
if(String.IsNullOrEmpty(key)){
    key = "12345";
}

builder.Services.AddAuthentication(option =>{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))       
    };
});

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAdmService,AdmService>();
builder.Services.AddScoped<IVehicleService,VehicleService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme{
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Inssert the JWT Token:"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement{
        {new OpenApiSecurityScheme{
            Reference = new OpenApiReference{
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        new string[] {}
        }
    });
});

// Fixing Enum Serialization
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// DBContext
builder.Services.AddDbContext<DbContexto>(options =>{
    options.UseMySql(
        builder.Configuration.GetConnectionString("MySql"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySql"))
    );
});

var app = builder.Build();
#endregion

#region Home

app.MapGet("/", () => Results.Json(new Home())).AllowAnonymous().WithTags("Home");

#endregion

#region Adm
ValidationsError AdmDTOValidation(AdmDTO admDTO){
     var validationsError = new ValidationsError();
    

    if(string.IsNullOrEmpty(admDTO.Email)) validationsError.Message.Add("The 'Email' field must not be empty!");
    if(string.IsNullOrEmpty(admDTO.Password)) validationsError.Message.Add("The 'Password' field must not be empty!");
    if(!Enum.IsDefined(typeof(EnumProfiles),admDTO.Profile)) validationsError.Message.Add("The 'Profile' must be one of the options:Admin, Edit, Seller!");
    return validationsError;
}

string GenerateTokenJwt(Adm adm){
    if(String.IsNullOrEmpty(key)) return String.Empty;
    
    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
    var credentials =new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
    
    var claims = new List<Claim>(){
        new Claim("Email", adm.Email),
        new Claim("Perfil", adm.Profile),
        new Claim(ClaimTypes.Role, adm.Profile)
    };

    var token = new JwtSecurityToken(
        claims: claims,
        expires: DateTime.Now.AddHours(6),
        signingCredentials: credentials
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
}

app.MapPost("/admin/login",([FromBody] LoginDTO loginDTO,IAdmService admService) =>{
    var adm = admService.Login(loginDTO);
    if ( adm != null)
    {
        string token  = GenerateTokenJwt(adm);
        return Results.Ok(
            new LoggedAdmModelView{
                Email = adm.Email,
                Token = token,
                Profile = Enum.Parse<EnumProfiles>(adm.Profile)
            }
        );
    }
    else{
        return Results.Unauthorized();
    }
}).AllowAnonymous()
.WithTags("Admin");

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

}).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin"})
.RequireAuthorization()
.WithTags("Admin");

app.MapGet("/admin",([FromQuery] int? page, IAdmService admService  ) =>{
     var list = new List<AdmModelView>();
    var admList = admService.All(page);
    
    foreach(var adm in admList){
        list.Add(new AdmModelView{
            Id = adm.Id,
            Email = adm.Email,
            Profile = Enum.Parse<EnumProfiles>(adm.Profile)

        });
    }


    return Results.Ok(list);
}).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin, Edit"})
.RequireAuthorization()
.WithTags("Admin");

app.MapGet("/admin/{id}",([FromRoute] int id, IAdmService admService  ) =>{
    
    var adm = admService.SearchById(id);

    if(adm ==null) return Results.NotFound();
    
    var admView = new AdmModelView{
        Id = adm.Id,
        Email = adm.Email,
        Profile =  Enum.Parse<EnumProfiles>(adm.Profile)
    };

    return Results.Ok(admView);
}).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin, Edit"})
.RequireAuthorization()
.WithTags("Admin");

app.MapDelete("/admin/{id}",([FromRoute] int id, IAdmService admService) =>{
    var adm = admService.SearchById(id);

    if(adm == null) return Results.NotFound();

    admService.Delete(adm);
    return Results.Ok();
}).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin"})
.RequireAuthorization()
.WithTags("Admin");

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

}).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin"})
.RequireAuthorization()
.WithTags("Vehicles");

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
}).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin"})
.RequireAuthorization()
.WithTags("Vehicles");

app.MapDelete("/vehicles/{id}", ([FromRoute] int id, IVehicleService vehicleService)=>{
    
    var vehicle = vehicleService.ShearchById(id);
    if(vehicle == null) return Results.NotFound();
    
    vehicleService.Delete(vehicle);
    return Results.NoContent();

}).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin"})
.RequireAuthorization()
.WithTags("Vehicles");

app.MapGet("/vehicles", ([FromQuery] int? page, string? name, string? brand ,IVehicleService vehicleService)=>{

    int pageInt = page != null ? (int)page : 1;

    var vehicles = vehicleService.All(pageInt,name,brand);
    
    return Results.Ok(vehicles);
}).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin, Edit"})
.RequireAuthorization()
.WithTags("Vehicles");

app.MapGet("/vehicles/{id}", ([FromRoute]int id, IVehicleService vehicleService)=>{

    var vehicle = vehicleService.ShearchById(id);
    if(vehicle != null){
        return Results.Ok(vehicle);
    }

    return Results.NotFound();
    
}).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin, Edit"})
.RequireAuthorization()
.WithTags("Vehicles");

#endregion

#region app
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.Run();


#endregion