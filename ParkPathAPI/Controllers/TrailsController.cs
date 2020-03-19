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
    public class TrailsController : ControllerBase
    {
        private readonly ITrailRepository _trailRepository;
        private readonly IMapper _mapper;

        public TrailsController(ITrailRepository trailRepository, IMapper mapper)
        {
            _trailRepository = trailRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<TrailDto>))]
        public IActionResult GetTrails()
        {
            var trails = _trailRepository.GetTrails();
            var mappedTrails = _mapper.Map<ICollection<TrailDto>>(trails);
            return Ok(mappedTrails);
        }

        [HttpGet("{trailId}", Name = "GetTrail")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrail(int trailId)
        {
            var trail = _trailRepository.GetTrail(trailId);
            if (trail == null)
                return NotFound();
            var mappedTrail = _mapper.Map<TrailDto>(trail);
            return Ok(mappedTrail);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateTrail([FromBody] TrailCreateDto trailDto)
        {
            if (trailDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_trailRepository.TrailExists(trailDto.Name))
            {
                ModelState.AddModelError("", "Trail already exists in database!");
                return StatusCode(404, ModelState);
            }

            var trailToAdd = _mapper.Map<Trail>(trailDto);
            if (!_trailRepository.CreateTrail(trailToAdd))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {trailToAdd.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetTrail", new { trailId = trailToAdd.Id }, trailToAdd);
        }

        [HttpPatch("{trailId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateTrail(int trailId, [FromBody] TrailUpdateDto trailDto)
        {
            if (trailDto == null || trailId != trailDto.Id)
            {
                return BadRequest(ModelState);
            }

            var trailToUpdate = _mapper.Map<Trail>(trailDto);
            if (!_trailRepository.UpdateTrail(trailToUpdate))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {trailToUpdate.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{trailId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteTrail(int trailId)
        {
            if (!_trailRepository.TrailExists(trailId))
            {
                return NotFound();
            }

            var trailToDelete = _trailRepository.GetTrail(trailId);
            if (!_trailRepository.DeleteTrail(trailToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {trailToDelete.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }




    }
}