using AutoMapper;
using ParkPathAPI.Models;

namespace ParkPathAPI.Mapper
{
    public class ParkPathMappings : Profile
    {
        public ParkPathMappings()
        {
            CreateMap<NationalPark, NationalParkDto>().ReverseMap(); // Map in both ways
            
        }
    }
}