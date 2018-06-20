using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shiro.Models;

namespace Shiro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ApplicationContext _context;
        
        public UserController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<UserModel>> GetAll()
        {
            return _context.Users.ToList();
        }

        [HttpGet("{id}", Name = "GetUser")]
        public ActionResult<UserModel> Get(int id)
        {
            UserModel user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Create(UserModel user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();

            return CreatedAtRoute("GetUser", new { id = user.Id }, user);
        }
    }
}
