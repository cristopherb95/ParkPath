using System.Net.Http;
using ParkPathMVC.Models;
using ParkPathMVC.Repository.IRepository;

namespace ParkPathMVC.Repository
{
    public class TrailRepository : Repository<Trail>, ITrailRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TrailRepository(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
    }
}