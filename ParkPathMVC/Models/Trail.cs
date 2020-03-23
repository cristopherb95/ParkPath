using System.ComponentModel.DataAnnotations;

namespace ParkPathMVC.Models
{
    public enum DifficultyType
    {
        Easy,
        Moderate,
        Difficult,
        Expert
    }
    
    public class Trail
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public double Elevation { get; set; }
        
        [Required]
        public double Distance { get; set; }

        public DifficultyType Difficulty { get; set; }

        [Required]
        public int NationalParkId { get; set; }

        public NationalPark NationalPark { get; set; }
    }
}