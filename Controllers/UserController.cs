using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shiro.Models;
using Shiro.Services;

namespace Shiro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly SystemContext _context;
        private readonly IUserRepository _userRepository;

        public UserController(SystemContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            try
            {
                return Ok(await _userRepository.GetAllAsync(ct));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> Get(Guid id, CancellationToken ct)
        {
            try
            {
                return Ok(await _userRepository.GetByIdAsync(id, ct));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create(SignupModel newUser, CancellationToken ct)
        {
            try
            {
                return Ok(await _userRepository.AddAsync(newUser, ct));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
