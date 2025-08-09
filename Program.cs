using api_iso_med_pg;
using api_iso_med_pg.Data.Interfaces;
using api_iso_med_pg.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
           .UseSnakeCaseNamingConvention());


builder.Services.AddScoped<IEquipamientoRepository, EquipamientoRepository>();
builder.Services.AddScoped<IScrumRepository, ScrumRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<INormaRepository, NormaRepository>();
builder.Services.AddScoped<IEntrevistaRepository, EntrevistaRepository>();
builder.Services.AddScoped<ICompaniaRepository, CompaniaRepository>();
builder.Services.AddScoped<ISucursalRepository, SucursalRepository>();
builder.Services.AddScoped<ITrabajadorRepository, TrabajadorRepository>();
builder.Services.AddScoped<IPreguntaRepository, PreguntaRepository>();
builder.Services.AddScoped<IRespuestaRepository, RespuestaRepository>();

// Registro de AutoMapper
builder.Services.AddAutoMapper(
    typeof(api_iso_med_pg.Mappers.NormaProfile),
    typeof(api_iso_med_pg.Mappers.ScrumProfile),
    typeof(api_iso_med_pg.Mappers.EntrevistaProfile),
    typeof(api_iso_med_pg.Mappers.EquipamientoProfile),
    typeof(api_iso_med_pg.Mappers.CompaniaProfile),
    typeof(api_iso_med_pg.Mappers.SucursalProfile),
    typeof(api_iso_med_pg.Mappers.TrabajadorProfile),
    typeof(api_iso_med_pg.Mappers.PreguntaProfile),
    typeof(api_iso_med_pg.Mappers.RespuestaProfile)
);

builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

app.UseCors("AllowAll");

// Configurar archivos estáticos para la carpeta Files
app.UseStaticFiles();
var filesPath = Path.Combine(Directory.GetCurrentDirectory(), "Files");
if (!Directory.Exists(filesPath))
{
    Directory.CreateDirectory(filesPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(filesPath),
    RequestPath = "/Files"
});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
