using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ParkPathAPI.Data;
using ParkPathAPI.Models;
using ParkPathAPI.Repository.IRepository;

namespace ParkPathAPI.Repository
{
    public class TrailRepository : ITrailRepository
    {
        private readonly ApplicationDbContext _db;

        public TrailRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        
        public ICollection<Trail> GetTrails()
        {
            return _db.Trails.Include(np => np.NationalPark).OrderBy(t => t.Name).ToList();
        }

        public Trail GetTrail(int trailId)
        {
            return _db.Trails.Include(np => np.NationalPark).FirstOrDefault(t => t.Id == trailId);
        }

        public bool TrailExists(string name)
        {
            return _db.Trails.Any(t => t.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public bool TrailExists(int id)
        {
            return _db.Trails.Any(t => t.Id == id);
        }

        public bool CreateTrail(Trail trail)
        {
            _db.Trails.Add(trail);
            return Save();
        }

        public bool UpdateTrail(Trail trail)
        {
            _db.Trails.Update(trail);
            return Save();
        }

        public bool DeleteTrail(Trail trail)
        {
            _db.Trails.Remove(trail);
            return Save();
        }

        public bool Save()
        {
            return _db.SaveChanges() > 0 ? true : false;
        }

        public ICollection<Trail> GetTrailsInNationalPark(int nationalParkId)
        {
            return _db.Trails.Include(np => np.NationalPark).Where(t => t.NationalParkId == nationalParkId).ToList();
        }
    }
}