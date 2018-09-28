using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SecureNotesWeb1.Models;

//using Angelo.Identity;
//using Angelo.Identity.Models;
//using Angelo.Aegis.Client.Models;
//using AutoMapper.Extensions;

namespace SecureNotesWeb1.Controllers
{
    [Route("/api")]
    public class TestApiController : ApiController
    {
        //private UserManager _userManager;

        public TestApiController()
        {
        }

        /// <summary>
        /// Returns the user's identity by either unique id, username, or email
        /// </summary>
        /// <param name="userId">Optional user id in route path</param>
        /// <param name="username">Optional username in route query</param>
        /// <param name="email">Optional user email in route query</param>
        [HttpGet, Route("time/{id?}")]
        public async Task<IActionResult> GetCurrentTime(string id = null, [FromQuery]string username = null, [FromQuery]string email = null)
        {
            if (id == null)
                return BadRequest("Invalid api call. No pameters specified.");
            //            return NotFound($"User does not exist");

            var model = new TimeApiModel();
            model.TimeStr = DateTime.Now.ToString("h:mm:ss");
            model.DateStr = DateTime.Now.ToString("M/dd/yyyy");
            model.Id = id;

            return Ok(model);
        }

        [Authorize, HttpGet, Route("stime/{id?}")]
        public async Task<IActionResult> GetSecureTime(string id = null, [FromQuery]string username = null, [FromQuery]string email = null)
        {
            if (id == null)
                return BadRequest("Invalid api call. No pameters specified.");
            //            return NotFound($"User does not exist");

            var model = new TimeApiModel();
            model.TimeStr = DateTime.Now.ToString("h:mm:ss");
            model.DateStr = DateTime.Now.ToString("M/dd/yyyy");
            model.Id = id;

            return Ok(model);
        }

        [HttpPost, Route("ptime")]
        public async Task<JsonResult> ProcessTime(TimeApiModel time)
        {
            var model = new TimeApiModel();
            model.TimeStr = DateTime.Now.ToString("h:mm:ss");
            model.DateStr = DateTime.Now.ToString("M/dd/yyyy");
            model.Id = time.Id;

            return Json(model);
        }


    }
}
