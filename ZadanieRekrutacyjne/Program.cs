using Microsoft.EntityFrameworkCore;
using ZadanieRekrutacyjne.Controllers;
using ZadanieRekrutacyjne.Model;
using ZadanieRekrutacyjne.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddLogging(configure => configure.AddConsole());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<TagController>();
builder.Services.AddDbContext<TagContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient();
builder.Services.AddSingleton<ITagApiConfiguration, ITagApiConfiguration>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var serviceScope = app.Services.CreateScope();
    using var dbContext = serviceScope.ServiceProvider.GetService<TagContext>();
    dbContext?.Database.Migrate();
}

//if (!app.Environment.IsDevelopment())
//{
// }
   app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();

app.Run();
public partial class Program { }