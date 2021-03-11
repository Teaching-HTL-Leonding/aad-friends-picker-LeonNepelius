using AadAuthApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AadAuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FriendsController : ControllerBase
    {
        private readonly FriendsDbContext context;

        public FriendsController(FriendsDbContext context) => this.context = context;

        [HttpGet]
        [RequiredScope("read")]
        public IActionResult GetAllFriends()
        {
            Debug.WriteLine($"The user name is {User.Claims.First(c => c.Type == ClaimTypes.Name)}");
            Debug.WriteLine($"The AAD object ID for the user is {User.Claims.First(c => c.Type == ClaimConstants.ObjectId)}");
            return Ok(context.Friends);
        }

        [HttpPost]
        [RequiredScope("write")]
        public async Task<IActionResult> AddFriend([FromBody] Friend friend)
        {
            await context.AddFriend(friend);
            return StatusCode((int)HttpStatusCode.Created);
        }

        [HttpDelete("{aadId}")]
        [RequiredScope("write")]
        public async Task<IActionResult> RemoveFriend(string aadId)
        {
            await context.RemoveFriend(aadId);
            return NoContent();
        }


        [HttpPost("clear")]
        public async Task<IActionResult> ClearFriends()
        {
            await context.DeleteEverything();
            return NoContent();
        }
    }
}
