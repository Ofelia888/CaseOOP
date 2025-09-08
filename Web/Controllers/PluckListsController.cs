using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Web.Models;

namespace Web.Controllers;

public class PluckListsController : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var httpClient = new HttpClient();
        var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/plucklists"));
        var json = await response.Content.ReadAsStringAsync();
        var pluckLists = JsonConvert.DeserializeObject<List<FullPluckList>>(json)?.Select(pluckList => new PluckList
        {
            Id = pluckList.Id,
            Name = pluckList.Name,
            Shipment = pluckList.Shipment,
            Address = pluckList.Address
        }).ToList() ?? [];
        ViewData["PluckLists"] = pluckLists;
        return View();
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreatePluckList()
    {
        List<string> missing = [];
        if (!Request.Form.TryGetValue("name", out var name) || name[0]!.Length == 0) missing.Add("name");
        if (!Request.Form.TryGetValue("shipment", out var shipment) || shipment[0]!.Length == 0) missing.Add("shipment");
        if (!Request.Form.TryGetValue("address", out var address) || address[0]!.Length == 0) missing.Add("address");
        if (!Request.Form.TryGetValue("items", out var items) || items[0]!.Length == 0) missing.Add("items");
        if (missing.Count != 0) return RedirectToAction(nameof(Create));
        var httpClient = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5000/plucklists/create");
        var formData = new Dictionary<string, string>
        {
            { "name", name! },
            { "shipment", shipment! },
            { "address", address! },
            { "items", items! },
        };
        request.Content = new FormUrlEncodedContent(formData);
        var response = await httpClient.SendAsync(request);
        return RedirectToAction(response.IsSuccessStatusCode ? nameof(Index) : nameof(Create));
    }
}
