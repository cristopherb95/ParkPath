using System;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace ParkPathMVC
{
    public static class SD
    {
        public static string APIBaseUrl = "https://localhost:5001/";
        public static string NationalParkAPIPath = APIBaseUrl + "api/v1/nationalparks/";
        public static string TrailAPIPath = APIBaseUrl + "api/v1/trails/";
        public static string AccountAPIPath = APIBaseUrl + "api/v1/users/";

        public static byte[] GetByteArrayFromImage(IFormFile file)
        {
            using (var target = new MemoryStream())
            {
                file.CopyTo(target);
                return target.ToArray();
            }
        }

    }
}
