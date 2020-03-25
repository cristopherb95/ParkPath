using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ParkPathMVC.Repository.IRepository;

namespace ParkPathMVC.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public Repository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        
        public async Task<T> GetAsync(string url, int id, string token="")
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url+id);

            var client = _httpClientFactory.CreateClient();
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(jsonString); 
            }
            return null;
        }

        public async Task<IEnumerable<T>> GetAllAsync(string url, string token="")
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var client = _httpClientFactory.CreateClient();
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString); 
            }
            return null;
        }

        public async Task<bool> CreateAsync(string url, T objToCreate, string token="")
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (objToCreate != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(objToCreate), Encoding.UTF8, "application/json");
            }
            else
            {
                return false;
            }
            
            var client = _httpClientFactory.CreateClient();
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            HttpResponseMessage response = await client.SendAsync(request);
            
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
                return true;
            return false;
        }

        public async Task<bool> UpdateAsync(string url, T objToUpdate, string token="")
        {
            var request = new HttpRequestMessage(HttpMethod.Patch, url);
            if (objToUpdate != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(objToUpdate), Encoding.UTF8, "application/json");
            }
            else
            {
                return false;
            }
            
            var client = _httpClientFactory.CreateClient();
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            
            HttpResponseMessage response = await client.SendAsync(request);
            
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                return true;
            return false;
        }

        public async Task<bool> DeleteAsync(string url, int id, string token="")
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url+id);
            
            var client = _httpClientFactory.CreateClient();
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                return true;
            return false;
        }
    }
}