using Azure;
using NetProject.ViewModels;
using Newtonsoft.Json;
using System.Text;

namespace NetProject.Services
{
    public class AuthService
    {

        private static readonly HttpClient client = new HttpClient();
        private IConfiguration configuration;
        private readonly ApiResponse response = new();
        public string ROUTE_API = "";

        public AuthService(IConfiguration _configuration)
        {
            configuration = _configuration;
            ROUTE_API = configuration["RouteApi"] ?? throw new ArgumentNullException(nameof(ROUTE_API), "RouteApi is missing in configuration");
        }
        

        public async Task<ApiResponse> CheckLogin(string email, string password)
        {
            string json = JsonConvert.SerializeObject(new { Email = email, Password = password });

            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = await client.PostAsync($"{ROUTE_API}apiAuth/CheckLogin", content);

            string apiResponse = await request.Content.ReadAsStringAsync();
            ApiResponse data = JsonConvert.DeserializeObject<ApiResponse>(apiResponse)!;

            return data;
        }
    }

    
}
