using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkPathAPI.Models;
using ParkPathAPI.Repository.IRepository;

namespace ParkPathAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class NationalParksController : ControllerBase
    {
        private readonly INationalParkRepository _npRepository;
        private readonly IMapper _mapper;

        public NationalParksController(INationalParkRepository npRepository, IMapper mapper)
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
            var parks = _npRepository.GetNationalParks();
            var mappedParks = _mapper.Map<ICollection<NationalParkDto>>(parks);
            return Ok(mappedParks);
        }

        /// <summary>
        /// Get individual national park
        /// </summary>
        /// <param name="nationalParkId">Id of national park</param>
        /// <returns></returns>
        [HttpGet("{nationalParkId}", Name="GetNationalPark")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NationalParkDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult GetNationalPark(int nationalParkId)
        {
            var park = _npRepository.GetNationalPark(nationalParkId);
            if (park == null)
                return NotFound();
            var mappedPark = _mapper.Map<NationalParkDto>(park);
            return Ok(mappedPark);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(NationalParkDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateNationalPark([FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_npRepository.NationalParkExists(nationalParkDto.Name))
            {
                ModelState.AddModelError("","National Park already exists in database!");
                return StatusCode(404, ModelState);
            }

            var nationalParkToAdd = _mapper.Map<NationalPark>(nationalParkDto);
            if (!_npRepository.CreateNationalPark(nationalParkToAdd))
            {
                ModelState.AddModelError("",$"Something went wrong when saving the record {nationalParkToAdd.Name}");
                return StatusCode(500, ModelState);
            }
            
            return CreatedAtRoute("GetNationalPark", new { nationalParkId = nationalParkToAdd.Id}, nationalParkToAdd);
        }

        [HttpPatch("{nationalParkId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateNationalPark(int nationalParkId, [FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null || nationalParkId!= nationalParkDto.Id)
            {
                return BadRequest(ModelState);
            }
            
            var nationalParkToUpdate = _mapper.Map<NationalPark>(nationalParkDto);
            if (!_npRepository.UpdateNationalPark(nationalParkToUpdate))
            {
                ModelState.AddModelError("",$"Something went wrong when updating the record {nationalParkToUpdate.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
        
        [HttpDelete("{nationalParkId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteNationalPark(int nationalParkId)
        {
            if (!_npRepository.NationalParkExists(nationalParkId))
            {
                return NotFound();
            }

            var nationalParkToDelete = _npRepository.GetNationalPark(nationalParkId);
            if (!_npRepository.DeleteNationalPark(nationalParkToDelete))
            {
                ModelState.AddModelError("",$"Something went wrong when deleting the record {nationalParkToDelete.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
        
        
    }
}