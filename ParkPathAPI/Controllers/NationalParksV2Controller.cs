using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkPathAPI.Models;
using ParkPathAPI.Repository.IRepository;

namespace ParkPathAPI.Controllers
{
    [Route("api/v{version:apiVersion}/nationalparks")]
    [ApiVersion("2.0")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class NationalParksV2Controller : ControllerBase
    {
        private readonly INationalParkRepository _npRepository;
        private readonly IMapper _mapper;

        public NationalParksV2Controller(INationalParkRepository npRepository, IMapper mapper)
        {
            _npRepository = npRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list of all national parks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<NationalParkDto>))]
        public IActionResult GetNationalParks()
        {
            var parks = _npRepository.GetNationalParks().Select(x =>
                                                {
                                                    x.Name += " V2";
                                                    return x;
                                                });
            var mappedParks = _mapper.Map<ICollection<NationalParkDto>>(parks);
            return Ok(mappedParks);
        }

    }
}