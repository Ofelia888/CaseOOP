using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Web.Controllers;

public class PluckListController : Controller
{
    public async Task<IActionResult> Index()
    {
        var httpClient = new HttpClient();
        var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/plucklists"));
        var json = await response.Content.ReadAsStringAsync();
        var pluckLists = JsonConvert.DeserializeObject<List<BasePluckList>>(json);
        ViewData["PluckLists"] = pluckLists;
        return View();
    }
}
