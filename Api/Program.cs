using Api.Data;
using Api.Interfaces;
using Api.Repositories;
using Api.Utilities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var dataStorage = builder.Configuration["DataStorage"]?.ToLower();
if (dataStorage == "jsonfile")
{
    builder.Services.AddScoped<JsonFileHelper>();
    builder.Services.AddScoped<IUserRepository, UserRepositoryJsonFile>();
}
else
{
    //default to DB connection using EntityFramework
    builder.Services.AddDbContext<ApiDbContext>(options => 
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddScoped<IUserRepository, UserRepositoryEF>();
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();