using Microsoft.EntityFrameworkCore;
using Quiz_Service.Data;

var builder = WebApplication.CreateBuilder(args);

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

string ConString;
if (Environment.GetEnvironmentVariable("DockerEnv") == "docker")
{
    ConString = Environment.GetEnvironmentVariable("DatabaseConnectionString");
}
else
{
    ConString = "Server=127.0.0.1,1433;Database=QuizDB;TrustServerCertificate=True;User Id=SA;Password=aabdelm4@AUC;";
}

builder.Services.AddDbContext<QuizContext>(
        options => options.UseSqlServer(ConString));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

app.UseDeveloperExceptionPage();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();