using AutoMapper;
using ParkPathAPI.Models;

namespace ParkPathAPI.Mapper
{
    public class ParkPathMappings : Profile
    {
        public ParkPathMappings()
        {
            CreateMap<NationalPark, NationalParkDto>().ReverseMap(); // Map in both ways
            CreateMap<Trail, TrailDto>().ReverseMap(); 
            CreateMap<Trail, TrailCreateDto>().ReverseMap();
            CreateMap<Trail, TrailUpdateDto>().ReverseMap(); 
            CreateMap<User, UserToAuthenticateDto>().ReverseMap();
            CreateMap<User, UserToReturnDto>().ReverseMap();
        }
    }
}