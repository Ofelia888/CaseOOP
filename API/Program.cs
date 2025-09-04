using System.Text;
using Core.io;
using Core.Models;
using Newtonsoft.Json;

namespace API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        //app.UseHttpsRedirection();

        app.UseAuthorization();

        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        app.MapGet("/weatherforecast", (HttpContext httpContext) =>
            {
                var forecast = Enumerable.Range(1, 5).Select(index =>
                        new WeatherForecast
                        {
                            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                            TemperatureC = Random.Shared.Next(-20, 55),
                            Summary = summaries[Random.Shared.Next(summaries.Length)]
                        })
                    .ToArray();
                return forecast;
            })
            .WithName("GetWeatherForecast");

        var itemRepository = new CSVRepository<BaseItem>("items.csv");
        var pluckListRepository = new CSVRepository<BasePluckList>("plucklists.csv", ';');
        var storageRepository = new CSVRepository<StorageItem>("storage.csv");

        // TODO: Make common interface for making a REST API from a repository
        var itemsGroup = app.MapGroup("/items");
        itemsGroup.MapGet("/", async (HttpContext context) =>
        {
            var json = JsonConvert.SerializeObject(itemRepository.ReadEntries().Select(entry => new
            {
                entry.ProductID,
                entry.Title,
                entry.Type,
                Amount = storageRepository.ReadEntry(item => item.ProductID.Equals(entry.ProductID))?.Amount ?? 0
            }));
            var result = Results.Bytes(Encoding.UTF8.GetBytes(json), "application/json");
            return await Task.FromResult(result);
        });
        itemsGroup.MapGet("/{id}", async (HttpContext context, string id) =>
        {
            var entry = itemRepository.ReadEntry(item => id.Equals(item.ProductID));
            if (entry == null) return Results.NotFound();
            var json = JsonConvert.SerializeObject(new {
                entry.ProductID,
                entry.Title,
                entry.Type,
                Amount = storageRepository.ReadEntry(item => item.ProductID.Equals(entry.ProductID))?.Amount ?? 0
            });
            var result = Results.Bytes(Encoding.UTF8.GetBytes(json), "application/json");
            return await Task.FromResult(result);
        });
        itemsGroup.MapDelete("/{id}", async (HttpContext context, string id) =>
        {
            return await Task.FromResult(itemRepository.Remove(item => item.ProductID.Equals(id)));
        });
        
        var pluckListGroup = app.MapGroup("/plucklists");
        pluckListGroup.MapGet("/", async (HttpContext context) =>
        {
            var json = JsonConvert.SerializeObject(pluckListRepository.ReadEntries());
            var result = Results.Bytes(Encoding.UTF8.GetBytes(json), "application/json");
            return await Task.FromResult(result);
        });
        pluckListGroup.MapGet("/{id:guid}", async (HttpContext context, Guid id) =>
        {
            var entry = pluckListRepository.ReadEntry(pluckList => pluckList.Id.Equals(id));
            if (entry == null) return Results.NotFound();
            var json = JsonConvert.SerializeObject(entry);
            var result = Results.Bytes(Encoding.UTF8.GetBytes(json), "application/json");
            return await Task.FromResult(result);
        });

        app.Run();
    }
}
