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

        var itemRepository = new CSVRepository<BaseItem>("items.csv");
        var pluckListRepository = new CSVRepository<BasePluckList>("plucklists.csv", ';');
        var storageRepository = new CSVRepository<StorageItem>("storage.csv");
        var pluckListItemsRepository = new CSVRepository<PluckListItem>("plucklist_items.csv");

        // TODO: Make common interface for making a REST API from a repository
        var itemsGroup = app.MapGroup("/items");
        itemsGroup.MapGet("/", async (HttpContext context) =>
        {
            var json = JsonConvert.SerializeObject(itemRepository.ReadEntries().Select(entry =>
            {
                var amount = storageRepository.ReadEntry(item => item.ProductID.Equals(entry.ProductID))?.Amount ?? 0;
                var reserved = pluckListItemsRepository.ReadEntries(item => item.ProductID!.Equals(entry.ProductID))
                    .Where(item =>
                    {
                        var pluckList = pluckListRepository.ReadEntry(list => list.Id.Equals(item.Id));
                        return !pluckList!.Archived;
                    }).Sum(item => item.Amount);
                return new
                {
                    entry.ProductID,
                    entry.Title,
                    entry.Type,
                    Amount = amount,
                    Reserved = reserved
                };
            }));
            var result = Results.Bytes(Encoding.UTF8.GetBytes(json), "application/json");
            return await Task.FromResult(result);
        });
        itemsGroup.MapGet("/{id}", async (HttpContext context, string id) =>
        {
            var entry = itemRepository.ReadEntry(item => id.Equals(item.ProductID));
            if (entry == null) return Results.NotFound();
            var amount = storageRepository.ReadEntry(item => item.ProductID.Equals(entry.ProductID))?.Amount ?? 0;
            var reserved = pluckListItemsRepository.ReadEntries(item => item.ProductID!.Equals(entry.ProductID)).Sum(item => item.Amount);
            var json = JsonConvert.SerializeObject(new {
                entry.ProductID,
                entry.Title,
                entry.Type,
                Amount = amount,
                Reserved = reserved
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
            var pluckLists = pluckListRepository.ReadEntries()
                .Select(pluckList =>
                {
                    var items = pluckListItemsRepository.ReadEntries(pluckListItem => pluckListItem.Id.Equals(pluckList.Id))
                        .Select(pluckListItem =>
                        {
                            var item = itemRepository.ReadEntry(item => item.ProductID.Equals(pluckListItem.ProductID));
                            if (item == null) return null;
                            return new Item
                            {
                                ProductID = pluckListItem.ProductID,
                                Title = item.Title,
                                Type = item.Type,
                                Amount = pluckListItem.Amount
                            };
                        }).Where(item => item != null).ToList();
                    return new FullPluckList()
                    {
                        Id = pluckList.Id,
                        Name = pluckList.Name,
                        Shipment = pluckList.Shipment,
                        Address = pluckList.Address,
                        Items = items!,
                        Archived = pluckList.Archived
                    };
                }).ToArray();
            var json = JsonConvert.SerializeObject(pluckLists);
            var result = Results.Bytes(Encoding.UTF8.GetBytes(json), "application/json");
            return await Task.FromResult(result);
        });
        pluckListGroup.MapGet("/{id:guid}", async (HttpContext context, Guid id) =>
        {
            var entry = pluckListRepository.ReadEntry(pluckList => pluckList.Id.Equals(id));
            if (entry == null) return Results.NotFound();
            var items = pluckListItemsRepository.ReadEntries(pluckListItem => pluckListItem.Id.Equals(entry.Id))
                .Select(pluckListItem =>
                {
                    var item = itemRepository.ReadEntry(item => item.ProductID.Equals(pluckListItem.ProductID));
                    if (item == null) return null;
                    return new Item
                    {
                        ProductID = pluckListItem.ProductID,
                        Title = item.Title,
                        Type = item.Type,
                        Amount = pluckListItem.Amount
                    };
                }).Where(item => item != null).ToList();
            var json = JsonConvert.SerializeObject(new FullPluckList()
            {
                Id = entry.Id,
                Name = entry.Name,
                Shipment = entry.Shipment,
                Address = entry.Address,
                Items = items!,
                Archived = entry.Archived
            });
            var result = Results.Bytes(Encoding.UTF8.GetBytes(json), "application/json");
            return await Task.FromResult(result);
        });
        pluckListGroup.MapPut("/{id:guid}", async (HttpContext context, Guid id) =>
        {
            var entry = pluckListRepository.ReadEntry(pluckList => pluckList.Id.Equals(id));
            if (entry == null) return Results.NotFound();
            if (entry.Archived) return Results.BadRequest();
            var items = pluckListItemsRepository.ReadEntries(item => item.Id.Equals(id));
            foreach (var reservedItem in items)
            {
                storageRepository.Update(item => item.ProductID.Equals(reservedItem.ProductID), item =>
                {
                    item.Amount -= reservedItem.Amount;
                    return item;
                });
            }
            pluckListRepository.Update(pluckList => pluckList.Id.Equals(id), pluckList =>
            {
                pluckList.Archived = true;
                return pluckList;
            });
            return await Task.FromResult(Results.Ok());
        });
        pluckListGroup.MapPost("/create", (HttpContext context) =>
        {
            List<string> missing = [];
            if (!context.Request.Form.TryGetValue("name", out var name) || name[0]!.Length == 0) missing.Add("name");
            if (!context.Request.Form.TryGetValue("shipment", out var shipment) || shipment[0]!.Length == 0) missing.Add("shipment");
            if (!context.Request.Form.TryGetValue("address", out var address) || address[0]!.Length == 0) missing.Add("address");
            if (!context.Request.Form.TryGetValue("items", out var items) || items[0]!.Length == 0) missing.Add("items");
            if (missing.Count != 0) return Results.BadRequest($"Missing arguments: {string.Join(", ", missing)}");
            List<Item> lines = [];
            List<string> errors = [];
            foreach (var itemString in items.Select(item => item!.Split(";")))
            {
                foreach (var values in itemString.Select(item => item.Split(',')))
                {
                    if (values.Length != 2 || !int.TryParse(values[1], out var amount))
                    {
                        errors.Add($"Invalid item format: {string.Join(",", values)}");
                        continue;
                    }
                    var productId = values[0];
                    var item = itemRepository.ReadEntry(item => item.ProductID.Equals(productId));
                    if (item == null)
                    {
                        errors.Add($"Unrecognized item: {productId}");
                        continue;
                    }
                    lines.Add(new Item()
                    {
                        ProductID = productId,
                        Title = item.Title,
                        Type = item.Type,
                        Amount = amount
                    });
                }
            }
            if (lines.Count == 0) return Results.BadRequest(errors);
            var pluckList = new BasePluckList()
            {
                Id = Guid.NewGuid(),
                Name = name[0]!,
                Shipment = shipment[0]!,
                Address = address[0]!
            };
            pluckListRepository.AddEntry(pluckList);
            pluckListItemsRepository.AddEntries(lines.Select(item => new PluckListItem()
            {
                Id = pluckList.Id,
                ProductID = item.ProductID,
                Amount = item.Amount
            }));
            return Results.Ok(pluckList);
        });

        app.Run();
    }
}
