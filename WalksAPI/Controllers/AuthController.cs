using Microsoft.AspNetCore.Mvc;
using WalksAPI.Repositories;

namespace WalksAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly ITokenHandler tokenHandler;

        public AuthController(IUserRepository userRepository,ITokenHandler tokenHandler)
        {
            this.userRepository = userRepository;
            this.tokenHandler = tokenHandler;
        }
        [HttpPost]
        [Route("login")]//open to all users no authorize atrribute
        public async Task<IActionResult> Login(Models.DTO.LoginRequest loginRequest)
        {
            //validate incoming request - fluent validation
            var user = await userRepository.Authenticate(loginRequest.UserName, loginRequest.Password);
 
            //check if user is authenticated 
            if(user!=null)
            {
                //generate JWT token - we using repository pattern to generate token- ITokenHandler
                //controllers need to be clean and minimalistic so create repository

                var token =  await tokenHandler.CreateToken(user);
                return Ok(token);
                
            }
            return BadRequest("Username or Password is incorrect");
        }
    }
}
