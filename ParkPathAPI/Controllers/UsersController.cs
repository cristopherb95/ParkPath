using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ParkPathAPI.Models;
using ParkPathAPI.Repository.IRepository;

namespace ParkPathAPI.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserToAuthenticateDto userModel)
        {
            var userFromDb = _userRepository.Authenticate(userModel.Username, userModel.Password);
            if (userFromDb == null)
            {
                return BadRequest(new {message = "Username or password is incorrect"});
            }
            var userToReturn = _mapper.Map<UserToReturnDto>(userFromDb);
            return Ok(userToReturn);
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserToAuthenticateDto userModel)
        {
            var isUsernameUnique = _userRepository.IsUniqueUser(userModel.Username);
            if (!isUsernameUnique)
            {
                return BadRequest(new { message = "Username already exists!" });
            }
            var userToRegister = _userRepository.Register(userModel.Username, userModel.Password);
            return Ok();
        }
    }
}