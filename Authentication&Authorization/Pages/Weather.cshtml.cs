using Authentication_Authorization.Models;
using Authentication_Authorization.Services;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Authentication_Authorization.Pages
{
    [Authorize]
    public class WeatherModel : PageModel
    {
        public List<WeatherData> WeatherData { get; set; }
        private readonly ITokenService _tokenService;

        public WeatherModel(ITokenService tokenService) 
        {
            _tokenService = tokenService;        
        }

        public async Task<IActionResult> OnGet()
        {

            using var client = new HttpClient();
            //var token = await _tokenService.GetTokenAsync("weatherapi.read");
            var token = await HttpContext.GetTokenAsync("access_token");
            client.SetBearerToken(token);

            var result = await client.GetAsync("https://localhost:5003/weather");
            if(result.IsSuccessStatusCode)
            {
                var text = await result.Content.ReadAsStringAsync();
                WeatherData = JsonConvert.DeserializeObject<List<WeatherData>>(text);
                if (WeatherData == null)
                    throw new Exception("Parsing json failed!");
                return Page();
            }
            throw new Exception("Unable to contact api!");
        }
    }
}
