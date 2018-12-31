using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SecureNotesWebAPI.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AspNetCore.Identity.Dapper;
using System;
using System.Collections.Generic;

namespace SecureNotesWebAPI.Controllers
{
    //   [Authorize(Policy = "ApiUser")]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class DashboardController : Controller
    {
        private readonly ClaimsPrincipal _caller;
        //private readonly ApplicationDbContext _appDbContext;

        public DashboardController(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor) //, ApplicationDbContext appDbContext)
        {
            _caller = httpContextAccessor.HttpContext.User;
            //_appDbContext = appDbContext;
        }

        // GET api/dashboard/home
        [HttpGet]
        public async Task<IActionResult> Home()
        {
            // retrieve the user info
            //HttpContext.User
            var userId = _caller.Claims.Single(c => c.Type == "id");
            //var customer = await _appDbContext.Customers.Include(c => c.Identity).SingleAsync(c => c.Identity.Id == userId.Value);

            var claims = new List<string>();
            foreach (var claim in _caller.Claims)
                claims.Add(claim.Type + ":" + claim.Value.ToString());

            return new OkObjectResult(new
            {
                Message = "This is secure API and user data!",
                Id = userId.Value,
                FirstName = User.Identity.Name,
                TimeStamp = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString(),
                UserClaims = claims
            });

            //return new OkObjectResult(new
            //{
            //    Message = "This is secure API and user data!",
            //    customer.Identity.FirstName,
            //    customer.Identity.LastName,
            //    customer.Identity.PictureUrl,
            //    customer.Identity.FacebookId,
            //    customer.Location,
            //    customer.Locale,
            //    customer.Gender
            //});
        }
    }
}