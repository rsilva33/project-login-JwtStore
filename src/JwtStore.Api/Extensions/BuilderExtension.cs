using System.Text;
using JwtStore.Core;
using JwtStore.Infra.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace JwtStore.API.Extensions;

public static class BuilderExtension
{
  public static void AddConfiguration(this WebApplicationBuilder builder)
  {
    Configuration.Database.ConnectionString =
            builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;

        Configuration.Secrets.ApiKey =
            builder.Configuration.GetSection("Secrets").GetValue<string>("ApiKey") ?? string.Empty;
        Configuration.Secrets.JwtPrivateKey =
            builder.Configuration.GetSection("Secrets").GetValue<string>("JwtPrivateKey") ?? string.Empty;
        Configuration.Secrets.PasswordSaltKey =
            builder.Configuration.GetSection("Secrets").GetValue<string>("PasswordSaltKey") ?? string.Empty;
  }

  public static void AddDataBase(this WebApplicationBuilder builder)
  {

    var server = builder.Configuration["DbServer"] ?? "localhost";
    var port = builder.Configuration["DbPort"] ?? "1450";
    var user = builder.Configuration["DbUser"] ?? "SA";
    var password = builder.Configuration["Password"] ?? "1q2w3e4r@#$";
    var database = builder.Configuration["Database"] ?? "jwt-store";

    var connectionString = $"Server={server}, {port};Initial Catalog={database};User ID={user};Password ={password}";
 
    builder.Services.AddDbContext<AppDbContext>(x =>
      x.UseSqlServer(
        connectionString,
        b => b.MigrationsAssembly("JwtStore.Api")));
  }

  public static void AddJwtAuthentication(this WebApplicationBuilder builder)
  {
      builder.Services.AddAuthentication(x => 
    {
      x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x => 
    {
      x.TokenValidationParameters = new TokenValidationParameters
      {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.Secrets.JwtPrivateKey)),
        ValidateIssuer = false,
        ValidateAudience = false
      };
    });
  }
}