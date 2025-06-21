using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using spark2.Models.DTOs.Account;
using spark2.Models.Entities;
using spark2.Services.Account;

namespace spark2.Controllers
{

    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<Person> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IDashboardService _dashboardService;

        public UserController(UserManager<Person> userManager, RoleManager<IdentityRole> roleManager, IDashboardService dashboardService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dashboardService = dashboardService;
        }


        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            ViewBag.UserCount = users.Count;
            var allRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            var userDtos = new List<UserDTO>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                userDtos.Add(new UserDTO
                {
                    UserId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    ProfilePicture = user.ProfilePicture,
                    Role = roles.FirstOrDefault(),
                    AvailableRoles = allRoles
                });
            }

            return View(userDtos);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);

            if (user == null)
                return NotFound();

            await _userManager.DeleteAsync(user);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRole(string userId, string newRole)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || string.IsNullOrWhiteSpace(newRole))
                return NotFound();

            var currentRoles = await _userManager.GetRolesAsync(user);
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
                return BadRequest("Failed to remove current roles");

            var addResult = await _userManager.AddToRoleAsync(user, newRole);
            if (!addResult.Succeeded)
                return BadRequest("Failed to add new role");

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            var newUser = new AddUserDTO
            {
                AvailableRoles = roles
            };

            return View(newUser);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddUserDTO newUser)
        {
            if (!ModelState.IsValid)
            {
                newUser.AvailableRoles = await _roleManager.Roles.Select(role => role.Name).ToListAsync();
                return View(newUser);
            }

            var userExists = await _userManager.FindByEmailAsync(newUser.Email);
            if (userExists != null)
            {
                ModelState.AddModelError("Email", "A user with this email already exists.");
                newUser.AvailableRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
                return View(newUser);
            }

            var user = new Person
            {
                UserName = newUser.Email,
                Email = newUser.Email,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName
            };

            var result = await _userManager.CreateAsync(user, newUser.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                newUser.AvailableRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
                return View(newUser);
            }

            await _userManager.AddToRoleAsync(user, newUser.SelectedRole);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            var dto = new UserDTO
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = roles.FirstOrDefault(),
                AvailableRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync()
            };

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
                return NotFound();

             
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Failed to update user");
                return View(model);
            }

            // Update role if changed
            var currentRoles = await _userManager.GetRolesAsync(user);
            if (!currentRoles.Contains(model.Role))
            {
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRoleAsync(user, model.Role);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ViewProfile(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            var model = new UserDTO
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                ProfilePicture = user.ProfilePicture,
                Role = roles.FirstOrDefault()
            };

            return View(model);
        }

        public async Task<IActionResult> Dashboard()
        {
            var countsDto = await _dashboardService.GetDashboardCountsAsync();
            return View(countsDto);
        }

    }
}


