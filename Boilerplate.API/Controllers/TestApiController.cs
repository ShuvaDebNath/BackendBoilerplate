using Boilerplate.API.Controllers;
//using Boilerplate.Entities.ExceptionHandlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Boilerplate.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestApiController : BaseApiController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<TestApiController> _logger;
        private readonly IConfiguration _config;
        public TestApiController(IWebHostEnvironment webHostEnvironment,
            ILogger<TestApiController> logger,
            IConfiguration config)
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _config = config;
        }

        [HttpGet]
        public IActionResult GetAccounts()
        {
            return Ok(new { Message = "API is working successfully!" });
        }

        [AllowAnonymous]
        [HttpGet()]
        [Route("GetToken")]
        public string GetToken()
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["SecurityJwt:Key"]));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Email, "TestEmail"),
                new Claim("UserId", "1"),
                new Claim("UserName", "Admin"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            JwtSecurityToken token = new JwtSecurityToken(_config["SecurityJwt:Issuer"], _config["SecurityJwt:Issuer"],
                                             claims,
                                             notBefore: DateTime.UtcNow,
                                             expires: DateTime.Now.AddHours(12),
                                             signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
