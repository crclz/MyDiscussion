using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyDiscussion.Data;
using MyDiscussion.Data.Models;

namespace MyDiscussion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public DiscussionContext Context { get; }

        public UsersController(DiscussionContext context)
        {
            Context = context;
        }


        // POST example.com/api/Users
        [HttpPost]
        public ActionResult CreateUser([FromBody] CreateUserModel model)
        {
            if (!ApiUtils.IsModelValid(model))
                return BadRequest("ModelInvalid");

            var userWithSameUsername = Context.Users.Where(p => p.Username == model.Username).FirstOrDefault();

            if (userWithSameUsername != null)
                return BadRequest("UsernameExist");

            var user = new User
            {
                Username = model.Username,
                Password = model.Password,
                Nickname = model.Nickname,
                Id = Guid.NewGuid()
            };

            Context.Users.Add(user);
            Context.SaveChanges();
            return Ok();
        }

        public class CreateUserModel
        {
            [Required]
            [MinLength(3)]
            [MaxLength(16)]
            public string Username { get; set; }

            [Required]
            [MinLength(6)]
            [MaxLength(32)]
            public string Password { get; set; }

            [Required]
            [MinLength(2)]
            [MaxLength(16)]
            public string Nickname { get; set; }
        }
    }
}
