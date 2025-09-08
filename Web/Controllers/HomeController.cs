using System.Diagnostics;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Web.Models;
using Item = Web.Models.Item;
using PluckListItem = Web.Models.PluckListItem;

namespace Web.Controllers;

public class HomeController : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var httpClient = new HttpClient();
        var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/plucklists"));
        var json = await response.Content.ReadAsStringAsync();
        var itemsResponse = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/items"));
        var items = JsonConvert.DeserializeObject<List<Item>>(await itemsResponse.Content.ReadAsStringAsync());
        var pluckLists = JsonConvert.DeserializeObject<List<FullPluckList>>(json)?.Select(pluckList => new PluckList()
        {
            Id = pluckList.Id,
            Name = pluckList.Name,
            Shipment = pluckList.Shipment,
            Address = pluckList.Address,
            Items = pluckList.Items.Select(item => new PluckListItem()
            {
                ProductID = item.ProductID!,
                Title = item.Title!,
                Type = item.Type,
                Amount = item.Amount,
                Total = items!.Where(i => i.ProductID.Equals(item.ProductID)).Sum(i => i.Amount),
                Reserved = items!.Where(i => i.ProductID.Equals(item.ProductID)).Sum(i => i.Reserved)
            }).ToList()
        }).ToList() ?? [];
        ViewData["Page"] = 1;
        ViewData["Pages"] = pluckLists.Count;
        ViewData["PluckLists"] = pluckLists;
        return View();
    }

    [HttpGet]
    public async Task<PartialViewResult> GetPluckList(int index)
    {
        var httpClient = new HttpClient();
        var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/plucklists"));
        var json = await response.Content.ReadAsStringAsync();
        var itemsResponse = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/items"));
        var items = JsonConvert.DeserializeObject<List<Item>>(await itemsResponse.Content.ReadAsStringAsync());
        var pluckLists = JsonConvert.DeserializeObject<List<FullPluckList>>(json)?.Select(pluckList => new PluckList()
        {
            Id = pluckList.Id,
            Name = pluckList.Name,
            Shipment = pluckList.Shipment,
            Address = pluckList.Address,
            Items = pluckList.Items.Select(item => new PluckListItem()
            {
                ProductID = item.ProductID!,
                Title = item.Title!,
                Type = item.Type,
                Amount = item.Amount,
                Total = items!.Where(i => i.ProductID.Equals(item.ProductID)).Sum(i => i.Amount),
                Reserved = items!.Where(i => i.ProductID.Equals(item.ProductID)).Sum(i => i.Reserved)
            }).ToList()
        }).ToList() ?? [];
        return PartialView("_PluckList", pluckLists[index]);
    }

    [HttpPut]
    public async Task<PartialViewResult> FinishPluckList(string id)
    {
        var httpClient = new HttpClient();
        await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Put, $"http://localhost:5000/plucklists/{id}"));
        var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, $"http://localhost:5000/plucklists/{id}"));
        var pluckList = JsonConvert.DeserializeObject<FullPluckList>(await response.Content.ReadAsStringAsync());
        var itemsResponse = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/items"));
        var items = JsonConvert.DeserializeObject<List<Item>>(await itemsResponse.Content.ReadAsStringAsync());
        return PartialView("_PluckList", new PluckList()
        {
            Id = pluckList!.Id,
            Name = pluckList.Name,
            Shipment = pluckList.Shipment,
            Address = pluckList.Address,
            Items = pluckList.Items.Select(item => new PluckListItem()
            {
                ProductID = item.ProductID!,
                Title = item.Title!,
                Type = item.Type,
                Amount = item.Amount,
                Total = items!.Where(i => i.ProductID.Equals(item.ProductID)).Sum(i => i.Amount),
                Reserved = items!.Where(i => i.ProductID.Equals(item.ProductID)).Sum(i => i.Reserved)
            }).ToList()
        });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
