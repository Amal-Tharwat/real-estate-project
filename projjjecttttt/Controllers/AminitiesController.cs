using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using projjjecttttt.Data;
using projjjecttttt.Models;

namespace projjjecttttt.Controllers
{
    public class AminitiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AminitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Aminities
        public async Task<IActionResult> Index()
        {
              return View(await _context.Aminity.ToListAsync());
        }

        // GET: Aminities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Aminity == null)
            {
                return NotFound();
            }

            var aminity = await _context.Aminity
                .FirstOrDefaultAsync(m => m.ID == id);
            if (aminity == null)
            {
                return NotFound();
            }

            return View(aminity);
        }

        // GET: Aminities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Aminities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Wifi,AC,TV,Fan,Parking,Landline")] Aminity aminity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aminity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aminity);
        }

        // GET: Aminities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Aminity == null)
            {
                return NotFound();
            }

            var aminity = await _context.Aminity.FindAsync(id);
            if (aminity == null)
            {
                return NotFound();
            }
            return View(aminity);
        }

        // POST: Aminities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Wifi,AC,TV,Fan,Parking,Landline")] Aminity aminity)
        {
            if (id != aminity.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aminity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AminityExists(aminity.ID))
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
            return View(aminity);
        }

        // GET: Aminities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Aminity == null)
            {
                return NotFound();
            }

            var aminity = await _context.Aminity
                .FirstOrDefaultAsync(m => m.ID == id);
            if (aminity == null)
            {
                return NotFound();
            }

            return View(aminity);
        }

        // POST: Aminities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Aminity == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Aminity'  is null.");
            }
            var aminity = await _context.Aminity.FindAsync(id);
            if (aminity != null)
            {
                _context.Aminity.Remove(aminity);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AminityExists(int id)
        {
          return _context.Aminity.Any(e => e.ID == id);
        }
    }
}
