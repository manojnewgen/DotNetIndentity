using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace RoleClaimsApp
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly UserManager<IdentityUser> _userManager;


        public UsersController(ILogger<UsersController> logger, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }


        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterViewModel model)
        {
            var user = new IdentityUser
            {
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
                return Ok(new { Message = "User created successfully!" });

            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SetClaim()
        {
            var user = await _userManager.FindByNameAsync("TestUser");
            var claim = new Claim("Department", "IT");
            await _userManager.AddClaimAsync(user, claim);
            return Ok(new { Message = "Claim added successfully!" });
        }

        [HttpGet]

        public async Task<IActionResult> GetClaim()
        {
            var user = await _userManager.FindByNameAsync("TestUser");
            var claims = await _userManager.GetClaimsAsync(user);
            bool hasClaim = await _userManager.GetClaimsAsync(user).ContinueWith(task => task.Result.Any(c => c.Type == "Department" && c.Value == "IT"));
            if (hasClaim)
            {
                return Ok(new { Message = "Access granted for IT department." });
            }
            else
            {
                return Forbid();
            }
            //return Ok(claims);
        }

        [HttpGet("role-based")]
        public async Task<IActionResult> GetUser()
        {
            // Simulate manual identity injection
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
               {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.Role, "Admin") // Simulating an Admin role
            }, "mock"));

            HttpContext.User = user;

            // Perform role-based authorization manually
            if (user.IsInRole("Admin"))
            {
                return Ok(new { Message = "Access granted for Admin role." });
            }
            else
            {
                return Forbid();
            }
        }

        [HttpGet("claims-based")]
        public IActionResult GetUserByClaim()
        {
            // Simulate a logged-in user with a claim
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim("Department", "IT") // Simulating an IT Department claim
            }, "mock"));

            HttpContext.User = user;

            // Perform claim-based authorization manually
            var hasClaim = user.HasClaim(c => c.Type == "Department" && c.Value == "IT");

            if (hasClaim)
            {
                return Ok(new { Message = "Access granted for IT department." });
            }
            else
            {
                return Forbid();
            }
        }
    }


}