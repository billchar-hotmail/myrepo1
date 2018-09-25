using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SecureNotesWeb1.Models;
using SecureNotesWeb1.Services;

namespace SecureNotesWeb1.Controllers
{
    [Authorize]
    [ApiController]
    //[Route("[controller]")]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]User userParam)
        {
            var user = _userService.Authenticate(userParam.Username, userParam.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        //[AllowAnonymous]
        //[HttpPost]
        //public async Task<IActionResult> GenerateToken([FromBody] LoginViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await _userManager.FindByEmailAsync(model.Email);

        //        if (user != null)
        //        {
        //            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
        //            if (result.Succeeded)
        //            {

        //                var claims = new[]
        //                {
        //  new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        //  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //};

        //                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
        //                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //                var token = new JwtSecurityToken(_config["Tokens:Issuer"],
        //                  _config["Tokens:Issuer"],
        //                  claims,
        //                  expires: DateTime.Now.AddMinutes(30),
        //                  signingCredentials: creds);

        //                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        //            }
        //        }
        //    }

        //    return BadRequest("Could not create token");
        //}

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }
    }

}