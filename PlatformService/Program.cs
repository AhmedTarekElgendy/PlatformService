using Microsoft.EntityFrameworkCore;
using PlatformService.DAL.Async;
using PlatformService.DAL.Sync;
using PlatformService.Data;
using PlatformService.Data.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (builder.Environment.IsDevelopment())
    builder.Services.AddDbContext<PlatformDBContext>(option => option.UseInMemoryDatabase("InMemoryDB"));

else
{
    builder.Services.AddDbContext<PlatformDBContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("PlatformDB")));
}

builder.Services.AddScoped<IPlatformService, PlatformService.Data.Servcies.PlatformService>();
builder.Services.AddSingleton<IMessageBusService, MessageBusService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpClient<IHttpCommandsService, HttpCommandsService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

DBPrep.PopulateDB(app, app.Environment.IsDevelopment());

app.Run();
