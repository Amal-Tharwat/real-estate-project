using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using projjjecttttt.Data;
using projjjecttttt.Models;

namespace projjjecttttt.Controllers
{
    public class ProfileReserveController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfileReserveController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProfileReserve
        public async Task<IActionResult> Index()
        {
            var UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.UserID = UserID;
            var applicationDbContext = _context.Reserve.Include(r => r.Unit).Include(r => r.User).Where(a => a.UserID == UserID);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> ReservesonUnit(int? ID)
        {
            var applicationDbContext = _context.Reserve.Include(r => r.Unit).Include(r => r.User).Where(a => a.UnitID == ID);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ProfileReserve/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: ProfileReserve/Create
        public IActionResult Create()
        {
            ViewData["UnitID"] = new SelectList(_context.Unit, "ID", "ID");
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: ProfileReserve/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UnitID,UserID,Checkin,Checkout,GuestNum,IdImg,Phone")] Reserve reserve)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reserve);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UnitID"] = new SelectList(_context.Unit, "ID", "ID", reserve.UnitID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", reserve.UserID);
            return View(reserve);
        }

        // GET: ProfileReserve/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
            ViewData["UnitID"] = new SelectList(_context.Unit, "ID", "ID", reserve.UnitID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", reserve.UserID);
            return View(reserve);
        }

        // POST: ProfileReserve/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UnitID,UserID,Checkin,Checkout,GuestNum,IdImg,Phone")] Reserve reserve)
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["UnitID"] = new SelectList(_context.Unit, "ID", "ID", reserve.UnitID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", reserve.UserID);
            return View(reserve);
        }

        // GET: ProfileReserve/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: ProfileReserve/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
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
            return RedirectToAction(nameof(Index));
        }

        private bool ReserveExists(int id)
        {
          return _context.Reserve.Any(e => e.Id == id);
        }
    }
}
