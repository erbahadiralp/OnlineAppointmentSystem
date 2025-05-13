using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace OnlineAppointmentSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly PasswordHasher<object> _passwordHasher;

        public TestController()
        {
            _passwordHasher = new PasswordHasher<object>();
        }

        [HttpGet("generate-password-hash")]
        public IActionResult GeneratePasswordHash(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return BadRequest("Password cannot be empty");
            }

            var dummyUser = new object();
            var hashedPassword = _passwordHasher.HashPassword(dummyUser, password);

            return Ok(new { hashedPassword });
        }
    }
} 