using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HouseMates.Attributes;
using HouseMates.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using HouseMates.Models;
using System.Threading.Tasks;
using System.Linq;

namespace HouseMates.Controllers
{
    [AdminOnly] // Restricts access to admin users only
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public AdminController(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        // GET: /Admin
        public IActionResult Index()
        {
            // Retrieve and pass all houses to the view
            var allHouses = _dbContext.Houses.ToList();
            return View(allHouses);
        }

        // GET: /Admin/ManageUsers
        [AdminOnly]
        public IActionResult ManageUsers()
        {
            // Retrieve all users for the admin to manage
            var users = _userManager.Users.ToList();
            return View(users);
        }

        // POST: /Admin/ToggleAdminStatus
        [HttpPost]
        [AdminOnly]
        public async Task<IActionResult> ToggleAdminStatus(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // Toggle the IsAdmin property and update the user record
                user.IsAdmin = !user.IsAdmin;
                await _userManager.UpdateAsync(user);
            }
            return RedirectToAction("ManageUsers");
        }

        // POST: /Admin/DeleteUser
        [HttpPost]
        [AdminOnly]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null && user.UserName != User.Identity.Name)
            {
                // Delete user, ensuring the admin doesn't delete themselves
                await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("ManageUsers");
        }

        // GET: /Admin/EditUser
        [AdminOnly]
        public async Task<IActionResult> EditUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return RedirectToAction("ManageUsers");

            // Create an EditUserViewModel with the current user data
            var model = new EditUserViewModel
            {
                UserId = user.Id,
                FirstName = user.firstname,
                LastName = user.lastname,
                UserName = user.UserName,
                Email = user.Email,
                IsAdmin = user.IsAdmin
            };

            return View(model);
        }

        // POST: /Admin/EditUser
        [HttpPost]
        [AdminOnly]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
                return RedirectToAction("ManageUsers");

            // Update user details
            user.firstname = model.FirstName;
            user.lastname = model.LastName;
            user.UserName = model.UserName;
            user.Email = model.Email;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                ModelState.AddModelError("", "Error updating user.");
                return View(model);
            }

            // Update password if it's provided and not empty
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                await _userManager.ResetPasswordAsync(user, token, model.Password);
            }

            return RedirectToAction("ManageUsers");
        }

        // GET: /Admin/AddUser
        [AdminOnly]
        public IActionResult AddUser()
        {
            // Pass a new, empty model to the view
            var model = new AddUserViewModel();
            return View(model);
        }


        [HttpPost]
        [AdminOnly]
        public async Task<IActionResult> AddUser(AddUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var newUser = new ApplicationUser
            {
                firstname = model.FirstName,
                lastname = model.LastName,
                UserName = model.Email,
                Email = model.Email
            };

            var creationResult = await _userManager.CreateAsync(newUser, model.Password);
            if (!creationResult.Succeeded)
            {
                ModelState.AddModelError("", "Error creating new user.");
                return View(model);
            }

            return RedirectToAction("ManageUsers");
        }

    }
}
