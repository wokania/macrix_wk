using System.Net;
using macrix_wk_backend.Models;
using Newtonsoft.Json;

namespace macrix_wk_UI.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    public ApiService()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("http://localhost:5242/api/");
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
    }

    public async Task<List<PersonModel>> GetAllPersonsAsync()
    {
        var response = await _httpClient.GetAsync("home");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<List<PersonModel>>(content);//todo
    }
    public async Task<PersonModel> GetPersonById(long id){
        var response = await _httpClient.GetAsync("home");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<PersonModel>(id);
    }
}