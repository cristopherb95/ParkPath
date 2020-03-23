using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ParkPathMVC.Models.ViewModels
{
    public class TrailsVM
    {
        public IEnumerable<SelectListItem> NationalParks { get; set; }
        
        public Trail Trail { get; set; }
    }
}