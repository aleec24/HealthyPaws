using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class CedulasController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public CedulasController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet("{cedula}")]
    public async Task<IActionResult> GetCedulaData(string cedula)
    {
        var apiUrl = $"https://apis.gometa.org/cedulas/{cedula}";

        var response = await _httpClient.GetAsync(apiUrl);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var dataResponse = JsonConvert.DeserializeObject<DataResponse>(json);
            return Ok(new
            {
                nombre = dataResponse.Results[0].Firstname,
                apellidos = $"{dataResponse.Results[0].Lastname1} {dataResponse.Results[0].Lastname2}"
            });
        }
        return NotFound();
    }

    public class DataResponse
    {
        public List<Result> Results { get; set; }

        public class Result
        {
            public string Firstname { get; set; }
            public string Lastname1 { get; set; }
            public string Lastname2 { get; set; }
        }
    }
}