using System;
namespace ParkPathMVC
{
    public static class SD
    {
        public static string APIBaseUrl = "https://localhost:5001/";
        public static string NationalParkAPIPath = APIBaseUrl + "api/v1/nationalparks";
        public static string TrailAPIPath = APIBaseUrl + "api/v1/trails";
    }
}
