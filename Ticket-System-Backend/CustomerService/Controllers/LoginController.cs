using CustomerApi.Data.Helpers;
using CustomerApi.Data.Repository;
using CustomerApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomerApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IRepository<Customer> _customerRepo;
        private readonly IAuthenticationHelper authenticationHelper;

        public LoginController(IRepository<Customer> userService, IAuthenticationHelper authService)
        {
            _customerRepo = userService;
            authenticationHelper = authService;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginInputModel model)
        {
            var user = _customerRepo.GetAllAsync().Result.FirstOrDefault(u => u.Email == model.Username);

            // check if username exists
            if (user == null)
                return Unauthorized();

            // check if password is correct
            if (!authenticationHelper.VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt))
                return Unauthorized();

            // Authentication successful
            return Ok(new
            {
                username = user.Email,
                token = authenticationHelper.GenerateToken(user)
            });
        }
    }
}
