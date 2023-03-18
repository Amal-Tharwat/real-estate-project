using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projjjecttttt.Models;
using System.Data;

namespace projjjecttttt.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        //i need object of roleManager to control the all roles
        private readonly RoleManager<IdentityRole> _roleManager;

        //then i make injection ctor to take obj of roleManger from DIC to initialize my _roleManager obj 
        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            //store all roles objs from _roleManager as a list into roles
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //to prevent cross-site request forgery attacks.
        public async Task<IActionResult> Add(RoleFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                //return view (index)
                return View("Index", await _roleManager.Roles.ToListAsync());
            }
            var roleExist = await _roleManager.RoleExistsAsync(model.Name);
            if (roleExist)
            {
                ModelState.AddModelError("Name", "Role is Exist..!");
                return View("Index", await _roleManager.Roles.ToListAsync());
            }
            //trim to remove the spaces
            await _roleManager.CreateAsync(new IdentityRole(model.Name.Trim()));
            //return to action (index)
            return RedirectToAction("Index");
        }

    }
}

