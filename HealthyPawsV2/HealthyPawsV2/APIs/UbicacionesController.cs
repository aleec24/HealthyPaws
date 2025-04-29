using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class UbicacionesController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public UbicacionesController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet("provincias")]
    public async Task<IActionResult> GetProvincias()
    {
        var apiUrl = "https://ubicaciones.paginasweb.cr/provincias.json";
        var response = await _httpClient.GetAsync(apiUrl);

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            return Ok(JsonConvert.DeserializeObject<Dictionary<int, string>>(json));
        }

        return NotFound();
    }

    [HttpGet("provincias/{id}/cantones")]
    public async Task<IActionResult> GetCantones(int id)
    {
        var apiUrl = $"https://ubicaciones.paginasweb.cr/provincia/{id}/cantones.json";
        var response = await _httpClient.GetAsync(apiUrl);

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            return Ok(JsonConvert.DeserializeObject<Dictionary<int, string>>(json));
        }

        return NotFound();
    }

    [HttpGet("provincias/{id_provincia}/canton/{id_canton}/distritos")]
    public async Task<IActionResult> GetDistritos(int id_provincia, int id_canton)
    {
        var apiUrl = $"https://ubicaciones.paginasweb.cr/provincia/{id_provincia}/canton/{id_canton}/distritos.json";
        var response = await _httpClient.GetAsync(apiUrl);

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            return Ok(JsonConvert.DeserializeObject<Dictionary<int, string>>(json));
        }

        return NotFound();
    }
}
