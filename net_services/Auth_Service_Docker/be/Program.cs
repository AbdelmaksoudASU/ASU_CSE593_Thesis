using be;
using be.Data;
using be.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

Settings settings;
Settings settings_env = new Settings();

// Add services to the container.

builder.Services.AddControllers();

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
                      });
});

if (Environment.GetEnvironmentVariable("DockerEnv") == "docker")
{
    settings_env.JWT.Add("Secret", Environment.GetEnvironmentVariable("JWTSecret"));
    settings_env.JWT.Add("Issuer", Environment.GetEnvironmentVariable("JWTIssuer"));
    settings_env.JWT.Add("Audience", Environment.GetEnvironmentVariable("JWTAudience"));

    settings_env.ConnectionStrings.Add("DefaultConnection",
        Environment.GetEnvironmentVariable("DatabaseConnectionString"));

    settings_env.ServiceURLS.Add("QuizService", Environment.GetEnvironmentVariable("QuizService"));
    settings_env.ServiceURLS.Add("ApplicationService", Environment.GetEnvironmentVariable("ApplicationService"));
    settings_env.ServiceURLS.Add("ProfileService", Environment.GetEnvironmentVariable("ProfileService"));
    settings_env.ServiceURLS.Add("UniversitiesProgramsService",
        Environment.GetEnvironmentVariable("UniversitiesProgramsService"));

    settings = settings_env;

}
else
{
    settings = builder.Configuration.GetSection("Settings").Get<Settings>();

    
}

builder.Services.AddDbContext<AppDbContext>(
        options => options.UseSqlServer(settings.ConnectionStrings["DefaultConnection"]));

var tokenValidationParameters = new TokenValidationParameters()
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(
        settings.JWT["Secret"])),

    ValidateIssuer = true,
    ValidIssuer = settings.JWT["Issuer"],


    ValidateAudience = true,
    ValidAudience = settings.JWT["Audience"],

    ValidateLifetime = true,
    ClockSkew = TimeSpan.Zero
};

builder.Services.AddSingleton(tokenValidationParameters);
builder.Services.AddSingleton(settings);


builder.Services.AddIdentity<ApplicationUser, IdentityRole>().
    AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
//Add JWT Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = tokenValidationParameters;
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
