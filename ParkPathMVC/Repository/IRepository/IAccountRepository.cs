using System;
using System.Threading.Tasks;
using ParkPathMVC.Models;

namespace ParkPathMVC.Repository.IRepository
{
    public interface IAccountRepository : IRepository<User>
    {
        Task<User> LoginAsync(string url, User user);

        Task<bool> RegisterAsync(string url, User user);
    }
}
