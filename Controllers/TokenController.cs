using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shiro.Models;
using Shiro.Services;

namespace Shiro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TokenController : Controller
    {
        private IConfiguration _config;
        private readonly IUserRepository _userRepository;

        public TokenController(IConfiguration config, IUserRepository userRepository)
        {
            _config = config;
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody]LoginModel login, CancellationToken ct)
        {
            IActionResult response = Unauthorized();

            User user = await _userRepository.Authenticate(login, ct);
            if (user != null)
            {
                var tokenString = BuildToken(user);
                response = Ok(new { id = user.Id, token = tokenString });
            }

            return response;
        }

        [HttpGet]
        public IActionResult GetIdentity()
        {
            var identity = (ClaimsIdentity) Request.HttpContext.User.Identity;
            var claims = identity.Claims.ToDictionary(x => x.Type, x => x.Value);

            return Ok(claims);
        }

        private string BuildToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim("userId", user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
