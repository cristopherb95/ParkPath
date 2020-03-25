using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ParkPathMVC.Models;
using ParkPathMVC.Repository.IRepository;

namespace ParkPathMVC.Repository
{
    public class AccountRepository : Repository<User>, IAccountRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountRepository(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<User> LoginAsync(string url, User user)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (user != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            }
            else
            {
                return new User();
            }

            var client = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<User>(jsonResponse);
            }
            else
            {
                return new User();
            }
        }

        public async Task<bool> RegisterAsync(string url, User user)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (user != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            }
            else
            {
                return false;
            }

            var client = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return true;
            else
                return false;
        }

    }
}
