using System.Diagnostics;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Web.Models;

namespace Web.Controllers;

public class HomeController : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var httpClient = new HttpClient();
        var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/plucklists"));
        var json = await response.Content.ReadAsStringAsync();
        ViewData["Page"] = 0;
        ViewData["Pages"] = JsonConvert.DeserializeObject<List<FullPluckList>>(json)?.Count ?? 0;
        return View();
    }

    [HttpGet]
    public async Task<PartialViewResult> GetPluckList(int index)
    {
        var httpClient = new HttpClient();
        var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/plucklists"));
        var json = await response.Content.ReadAsStringAsync();
        var pluckLists = JsonConvert.DeserializeObject<List<FullPluckList>>(json)?.Select(pluckList => new Pluklist()
        {
            Name = pluckList.Name,
            Forsendelse = pluckList.Shipment,
            Adresse = pluckList.Address,
            Lines = pluckList.Items
        }).ToList() ?? [];
        return PartialView("_PluckList", pluckLists[index]);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
