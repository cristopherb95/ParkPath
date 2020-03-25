using System;
using System.Collections.Generic;

namespace ParkPathMVC.Models.ViewModels
{
    public class IndexVM
    {
        public IEnumerable<NationalPark> NationalParks { get; set; }

        public IEnumerable<Trail> Trails { get; set; }
    }
}
