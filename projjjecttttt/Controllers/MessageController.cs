using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using projjjecttttt.Data;
using projjjecttttt.Models;

namespace projjjecttttt.Controllers
{
    public class MessageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MessageController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Message
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Message.Include(m => m.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Message/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Message == null)
            {
                return NotFound();
            }

            var message = await _context.Message
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }
        [Authorize]
        // GET: Message/Create
        public IActionResult Create()
        {
            var UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.UserID = UserID;
            //ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Message/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,UserID,Messeage")] Message message)
        {
            var UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.UserID = UserID;
            if (ModelState.IsValid)
            {
                _context.Add(message);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", message.UserID);
            return View(message);
        }

        // GET: Message/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Message == null)
            {
                return NotFound();
            }

            var message = await _context.Message.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", message.UserID);
            return View(message);
        }

        // POST: Message/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID,UserID,Messeage,Email")] Message message)
        {
            if (id != message.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(message);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MessageExists(message.UserID))
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
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", message.UserID);
            return View(message);
        }

        // GET: Message/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Message == null)
            {
                return NotFound();
            }

            var message = await _context.Message
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // POST: Message/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Message == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Message'  is null.");
            }
            var message = await _context.Message.FindAsync(id);
            if (message != null)
            {
                _context.Message.Remove(message);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MessageExists(string id)
        {
          return _context.Message.Any(e => e.UserID == id);
        }
    }
}
