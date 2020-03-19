using AutoMapper;
using ParkPathAPI.Models;

namespace ParkPathAPI.Mapper
{
    public class ParkPathMappings : Profile
    {
        public ParkPathMappings()
        {
            CreateMap<NationalPark, NationalParkDto>().ReverseMap(); // Map in both ways
            CreateMap<Trail, TrailDto>().ReverseMap(); // Map in both ways
            CreateMap<Trail, TrailCreateDto>().ReverseMap(); // Map in both ways
            CreateMap<Trail, TrailUpdateDto>().ReverseMap(); // Map in both ways
        }
    }
}