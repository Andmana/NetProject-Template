using Azure;
using NetProject.ViewModels;
using Newtonsoft.Json;
using System.Text;

namespace NetProject.Services
{
    public class RoleService
    {

        private static readonly HttpClient client = new HttpClient();
        public IConfiguration configuration;
        private ApiResponse response = new();
        public string ROUTE_API = "";

        public RoleService(IConfiguration _configuration)
        {
            configuration = _configuration;
            ROUTE_API = configuration["RouteApi"] ?? throw new ArgumentNullException(nameof(ROUTE_API), "RouteApi is missing in configuration");
        }

        public async Task<EntityPagination<ViewRole>> GetAll(SearchQuery queries)
        {
            string queryParam = queries.generateQuery();
            string apiResponse = await client.GetStringAsync(ROUTE_API + $"apiRole/GetAll?" + queryParam);

            ApiResponse? response = JsonConvert.DeserializeObject<ApiResponse>(apiResponse);
            EntityPagination<ViewRole> data = JsonConvert.DeserializeObject<EntityPagination<ViewRole>>(response.Entity.ToString());
            return data;
        }

        public async Task<ApiResponse> Add(ViewRole dataForm)
        {
            //Proses convert dari objext ke string
            string json = JsonConvert.SerializeObject(dataForm);

            //proses ubah string ke json
            StringContent content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var request = await client.PostAsync(ROUTE_API + "apiRole/Add", content);

            if (request.IsSuccessStatusCode)
            {
                var apiResponse = await request.Content.ReadAsStringAsync();

                response = JsonConvert.DeserializeObject<ApiResponse>(apiResponse);
            }
            else
            {
                response.Success = false;
                response.Message = $"{request.StatusCode} : {request.ReasonPhrase}";
            }
            return response;
        }

        public async Task<ViewRole> GetById(long id)
        {

            string request = await client.GetStringAsync(ROUTE_API + $"apiRole/GetById/{id}");
            response = JsonConvert.DeserializeObject<ApiResponse>(request);
            ViewRole entity = JsonConvert.DeserializeObject<ViewRole>(response.Entity.ToString());

            return entity;
        }

        public async Task<ApiResponse> Edit(ViewRole dataForm)
        {
            //Proses convert dari objext ke string
            string json = JsonConvert.SerializeObject(dataForm);

            //proses ubah string ke json
            StringContent content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var request = await client.PutAsync(ROUTE_API + "apiRole/Edit", content);

            if (request.IsSuccessStatusCode)
            {
                var apiResponse = await request.Content.ReadAsStringAsync();

                response = JsonConvert.DeserializeObject<ApiResponse>(apiResponse);
            }
            else
            {
                response.Success = false;
                response.Message = $"{request.StatusCode} : {request.ReasonPhrase}";
            }

            return response;
        }

        public async Task<ApiResponse> Delete(long id, long userId)
        {
            var request = await client.DeleteAsync(ROUTE_API + $"apiRole/Delete/{id}/{userId}");

            if (request.IsSuccessStatusCode)
            {
                var apiResponse = await request.Content.ReadAsStringAsync();

                response = JsonConvert.DeserializeObject<ApiResponse>(apiResponse);
            }
            else
            {
                response.Success = false;
                response.Message = $"{request.StatusCode} : {request.ReasonPhrase}";
            }

            return response;
        }
    }
}
