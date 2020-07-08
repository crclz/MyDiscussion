using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyDiscussion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public UsersController()
        {

        }

        // POST example.com/api/Users
        [HttpPost]
        public ActionResult<Guid> CreateUser([FromBody] CreateUserModel model)
        {
            throw new NotImplementedException();
        }

        public class CreateUserModel
        {
            [Required]
            [MinLength(4)]
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
