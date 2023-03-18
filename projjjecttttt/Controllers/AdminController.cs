using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using projjjecttttt.Data;
using projjjecttttt.Models;
using System.Data;
using System.Net.NetworkInformation;

namespace projjjecttttt.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;

            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }

        //Get: users
        public IActionResult UsersIndex()
        {
            var users = _userManager.Users.Select(User => new User
            {
                Id = User.Id,
                FullName = User.FullName,
                Email = User.Email,
                Age = User.Age,
                Roles = _userManager.GetRolesAsync(User).Result
            }).ToList();
            return View(users);
        }


        [HttpGet]
        public async Task<IActionResult> ManageRoles(string User)
        {
            var user = await _userManager.FindByIdAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            var roles = await _roleManager.Roles.ToListAsync();
            var viewModel = new UserRolesViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = roles.Select(role => new RoleViewModel
                {
                    RoleName = role.Name,
                    IsSelected = _userManager.IsInRoleAsync(user, role.Name).Result,
                }).ToList()
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageRoles(UserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in model.Roles)
            {
                //fourth condition => user has this role but he unselected this role => remove role
                if (userRoles.Any(r => r == role.RoleName) && !role.IsSelected)
                {
                    await _userManager.RemoveFromRoleAsync(user, role.RoleName);
                }
                //second condition => user hasn't this role but he selected this role => add role
                if (!userRoles.Any(r => r == role.RoleName) && role.IsSelected)
                {
                    await _userManager.AddToRoleAsync(user, role.RoleName);
                }
            }
            return RedirectToAction(nameof(UsersIndex));
        }


        // GET: Units
        public async Task<IActionResult> UnitsIndex()
        {
            var applicationDbContext = _context.Unit.Include(u => u.Aminities).Include(u => u.Area).Include(u => u.Category).Include(u => u.User);
            return View(await applicationDbContext.ToListAsync());
        }
        // GET: Units/Details/5
        public async Task<IActionResult> UnitsDetails(int? id)
        {
            if (id == null || _context.Unit == null)
            {
                return NotFound();
            }

            var unit = await _context.Unit
                .Include(u => u.Aminities)
                .Include(u => u.Area)
                .Include(u => u.Category)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (unit == null)
            {
                return NotFound();
            }

            return View(unit);
        }
        // GET: Units/Edit/5
        public async Task<IActionResult> UnitsEdit(int? id)
        {
            if (id == null || _context.Unit == null)
            {
                return NotFound();
            }

            var unit = await _context.Unit.FindAsync(id);
            if (unit == null)
            {
                return NotFound();
            }
            ViewData["AminitiesID"] = new SelectList(_context.Set<Aminity>(), "ID", "ID", unit.AminitiesID);
            ViewData["AreaID"] = new SelectList(_context.Set<Area>(), "ID", "ID", unit.AreaID);
            ViewData["CategoryID"] = new SelectList(_context.Set<Category>(), "ID", "ID", unit.CategoryID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", unit.UserID);
            return View(unit);
        }

        // POST: Units/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnitsEdit(int id, [Bind("ID,guestNo,Price,Address,RoomNum,PathRoomNum,UserID,CategoryID,AminitiesID,AreaID,Description")] Unit unit)
        {
            if (id != unit.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(unit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UnitExists(unit.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(UnitsIndex));
            }
            ViewData["AminitiesID"] = new SelectList(_context.Set<Aminity>(), "ID", "ID", unit.AminitiesID);
            ViewData["AreaID"] = new SelectList(_context.Set<Area>(), "ID", "ID", unit.AreaID);
            ViewData["CategoryID"] = new SelectList(_context.Set<Category>(), "ID", "ID", unit.CategoryID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", unit.UserID);
            return View(unit);
        }


        // GET: Units/Delete/5
        public async Task<IActionResult> UnitsDelete(int? id)
        {
            if (id == null || _context.Unit == null)
            {
                return NotFound();
            }

            var unit = await _context.Unit
                .Include(u => u.Aminities)
                .Include(u => u.Area)
                .Include(u => u.Category)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (unit == null)
            {
                return NotFound();
            }

            return View(unit);
        }

        // POST: Units/Delete/5
        [HttpPost, ActionName("UnitsDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Unit == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Unit'  is null.");
            }
            var unit = await _context.Unit.FindAsync(id);
            if (unit != null)
            {

                List<Image> imglist = _context.Image.Where(a => a.UnitID == id).ToList();
                foreach (var item in imglist)
                {
                    System.IO.File.Delete("wwwroot/images/" + item.ImgSrc);
                    _context.Image.Remove(item);
                }
                await _context.SaveChangesAsync();

                _context.Unit.Remove(unit);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(UnitsIndex));
        }

        private bool UnitExists(int id)
        {
            return _context.Unit.Any(e => e.ID == id);
        }
        // GET: Reserves
        public async Task<IActionResult> ReservesIndex()
        {
            var applicationDbContext = _context.Reserve.Include(r => r.Unit).Include(r => r.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Reserves/Details/5
        public async Task<IActionResult> ReservesDetails(int? id)
        {
            if (id == null || _context.Reserve == null)
            {
                return NotFound();
            }

            var reserve = await _context.Reserve
                .Include(r => r.Unit)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reserve == null)
            {
                return NotFound();
            }

            return View(reserve);
        }

        // GET: Reserves/Edit/5
        public async Task<IActionResult> ReservesEdit(int? id)
        {
            if (id == null || _context.Reserve == null)
            {
                return NotFound();
            }

            var reserve = await _context.Reserve.FindAsync(id);
            if (reserve == null)
            {
                return NotFound();
            }
            ViewData["UnitID"] = new SelectList(_context.Set<Unit>(), "ID", "ID", reserve.UnitID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", reserve.UserID);
            return View(reserve);
        }

        // POST: Reserves/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReservesEdit(int id, [Bind("Id,UnitID,UserID,Checkin,Checkout,GuestNum,IdImg,Phone")] Reserve reserve)
        {
            if (id != reserve.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reserve);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReserveExists(reserve.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ReservesIndex));
            }
            ViewData["UnitID"] = new SelectList(_context.Set<Unit>(), "ID", "ID", reserve.UnitID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", reserve.UserID);
            return View(reserve);
        }

        // GET: Reserves/Delete/5
        public async Task<IActionResult> ReservesDelete(int? id)
        {
            if (id == null || _context.Reserve == null)
            {
                return NotFound();
            }

            var reserve = await _context.Reserve
                .Include(r => r.Unit)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reserve == null)
            {
                return NotFound();
            }

            return View(reserve);
        }

        // POST: Reserves/Delete/5
        [HttpPost, ActionName("ReservesDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReserveDeleteConfirmed(int id)
        {
            if (_context.Reserve == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Reserve'  is null.");
            }
            var reserve = await _context.Reserve.FindAsync(id);
            if (reserve != null)
            {
                _context.Reserve.Remove(reserve);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ReservesIndex));
        }

        private bool ReserveExists(int id)
        {
            return _context.Reserve.Any(e => e.Id == id);
        }

    }
}

