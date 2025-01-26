using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechStore.Data;  // Sigurohuni që të keni shtuar këtë përdorim nëse përdorni Data nga një projekt tjetër
using TechStore.Constants;

namespace TechStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Index page for Admin to see all users
        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        // GET: API - Get user by ID
        [HttpGet]
        [Route("api/v1/user/{id}")]
        public async Task<IActionResult> GetUserByIdV1(string id)
        {
            var user = await _userManager.Users
                                         .Where(u => u.Id == id)
                                         .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var userData = new
            {
                user.Id,
                user.Name,
                user.UserName,
                user.Email,
                user.PhoneNumber,
                Role = roles.FirstOrDefault() ?? "No Role"
            };

            return Ok(userData);
        }

        // GET: API - Get all users with roles
        [HttpGet]
        [Authorize]
        [Route("api/user")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var userList = new List<object>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userList.Add(new
                {
                    user.Id,
                    user.Name,
                    user.UserName,
                    user.Email,
                    user.PhoneNumber,
                    Role = roles.FirstOrDefault() ?? "No Role"
                });
            }

            return Ok(userList);
        }

        // POST: API - Create a new user
        [HttpPost]
        [Route("api/admin/create-user")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserModel model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest("Email and Password are required.");
            }

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                return BadRequest("A user with this email already exists.");
            }

            var user = new ApplicationUser
            {
                Name = model.Name,
                Email = model.Email,
                UserName = model.Email,
                PhoneNumber = model.PhoneNumber,
                ProfilePicture = model.ProfilePicture
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            if (!string.IsNullOrEmpty(model.Role))
            {
                if (await _roleManager.RoleExistsAsync(model.Role))
                {
                    await _userManager.AddToRoleAsync(user, model.Role);
                }
                else
                {
                    return BadRequest("Role does not exist.");
                }
            }

            return Ok("User created successfully.");
        }

        // PUT: API - Update user information
        [HttpPut]
        [Route("api/admin/update-user")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.Name = model.Name ?? user.Name;
            user.Email = model.Email ?? user.Email;
            user.PhoneNumber = model.PhoneNumber ?? user.PhoneNumber;
            user.ProfilePicture = model.ProfilePicture ?? user.ProfilePicture;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            if (!string.IsNullOrEmpty(model.Role))
            {
                if (await _roleManager.RoleExistsAsync(model.Role))
                {
                    var currentRoles = await _userManager.GetRolesAsync(user);
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    await _userManager.AddToRoleAsync(user, model.Role);
                }
                else
                {
                    return BadRequest("Role does not exist.");
                }
            }

            return Ok("User updated successfully.");
        }
        // POST: Assign Role
        [HttpPost]
        [Route("api/admin/assign-role")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> AssignRole([FromBody] RoleAssignmentModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var validRoles = new[] { Roles.User.ToString(), Roles.Manager.ToString(), Roles.Admin.ToString() };
            if (!validRoles.Contains(model.Role))
            {
                return BadRequest("Invalid role selected");
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRoleAsync(user, model.Role);

            return Ok("Role assigned successfully");
        }


        // DELETE: API - Delete user
        [HttpDelete]
        [Route("api/admin/users/{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok("User deleted successfully");
            }

            return BadRequest(result.Errors);
        }

        // Model for creating a user
        public class CreateUserModel
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string ProfilePicture { get; set; }
            public string Password { get; set; }
            public string Role { get; set; }
        }

        // Model for updating a user
        public class UpdateUserModel
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string ProfilePicture { get; set; }
            public string Role { get; set; }
        }
        public class RoleAssignmentModel
        {
            public string UserId { get; set; }
            public string Role { get; set; }
        }
    }
}

//using Microsoft.AspNetCore.Mvc;

//namespace TechStore.Areas.Admin.Controllers
//{
//    [Area("Admin")]
//    public class AdminController : Controller
//    {
//        public IActionResult Index()
//        {
//            return Content("Admin Area is Working!");
//        }
//    }
//}
