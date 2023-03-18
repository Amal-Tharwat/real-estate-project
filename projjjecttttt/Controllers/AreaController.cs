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
   
    public class AreaController : Controller
    {
       /* List<Area> Areas = new List<Area>()
        {
          new Area{ID=1,name="villa",},
          new Area{ID=2,name="Apartment"},
          new Area{ID=3,name="shaleh"}

        };*/
        private readonly ApplicationDbContext context;

        public AreaController(ApplicationDbContext _context)
        {
            context = _context;
        }

        // GET: Areas
        public async Task<IActionResult> Index()
        {
              return View(await context.Area.ToListAsync());
        }

        // GET: Areas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || context.Area == null)
            {
                return NotFound();
            }

            var area = await context.Area
                .FirstOrDefaultAsync(m => m.ID == id);
            if (area == null)
            {
                return NotFound();
            }

            return View(area);
        }

        // GET: Areas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Areas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,name")] Area area)
        {
            if (ModelState.IsValid)
            {
                context.Add(area);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(area);
        }

        // GET: Areas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || context.Area == null)
            {
                return NotFound();
            }

            var area = await context.Area.FindAsync(id);
            if (area == null)
            {
                return NotFound();
            }
            return View(area);
        }

        // POST: Areas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,name")] Area area)
        {
            if (id != area.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(area);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AreaExists(area.ID))
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
            return View(area);
        }

        // GET: Areas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || context.Area == null)
            {
                return NotFound();
            }

            var area = await context.Area
                .FirstOrDefaultAsync(m => m.ID == id);
            if (area == null)
            {
                return NotFound();
            }

            return View(area);
        }

        // POST: Areas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (context.Area == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Area'  is null.");
            }
            var area = await context.Area.FindAsync(id);
            if (area != null)
            {
                context.Area.Remove(area);
            }
            
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AreaExists(int id)
        {
          return context.Area.Any(e => e.ID == id);
        }
    }
}
