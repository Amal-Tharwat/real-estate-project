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
    public class ReserveController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReserveController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reserves
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Reserve.Include(r => r.Unit).Include(r => r.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Reserves/Details/5
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
        [Authorize]
        // GET: Reserves/Create
        public IActionResult Create(int id, int idg)
        {
            //var inq = Convert.ToDateTime("02/20/2023");
            //var checkin = inq.Day;
            //ViewBag.checkin = checkin;
            //var outq = Convert.ToDateTime("02/25/2023");
            //var checkout = outq.Day;
            //ViewBag.checkout = checkout;
            //List<int> days = new List<int>();
            //ViewBag.check = days;
            ViewBag.flag = true;
            ViewData["selectedUnitId"] = id;

            if (TempData.ContainsKey("unitId"))
            {
                ViewBag.flag = false;
                ViewData["selectedUnitId"] = TempData["unitId"];
            }

            ViewBag.id = Url.ActionContext.RouteData.Values["id"];
            ViewBag.idg = Url.ActionContext.RouteData.Values["GuestNum"];
            var UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.UserID = UserID;
            ViewData["UnitID"] = new SelectList(_context.Set<Unit>(), "ID", "ID");
            ViewBag.p = Url.ActionContext.RouteData.Values["Price"];
            //ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }



        // POST: Reserves/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UnitID,UserID,Checkin,Checkout,GuestNum,IdImg,Phone")] Reserve reserve, IFormFile unitimg)
        {
            //bool notAvailable = _context.Reserve.Where(a => a.UnitID == reserve.UnitID).Select(a => a.Checkin <= reserve.Checkin && a.Checkout >= reserve.Checkin).FirstOrDefault();

            var valid = _context.Reserve.ToArray();
            Reserve reserves = null;
            for (int i = 0; i < valid.Length; i++)
            {
                if (reserve.UnitID == valid[i].UnitID && ((reserve.Checkin >= valid[i].Checkin && reserve.Checkout <= valid[i].Checkout) || (reserve.Checkin <= valid[i].Checkin && reserve.Checkout >= valid[i].Checkin && reserve.Checkout <= valid[i].Checkout) || (reserve.Checkin >= valid[i].Checkin && reserve.Checkin <= valid[i].Checkout && reserve.Checkout >= valid[i].Checkout)))
                {
                    reserves = valid[i];
                    break;
                }
            }
            //ViewBag.notAvailable=notAvailable;
            if (ModelState.IsValid)
            {


                if (unitimg == null)
                {
                    ModelState.AddModelError("", "No file chosen,Please upload your img");
                    return View(reserve);
                }
                else
                {

                    string filename = "UnitID" + "-" + reserve.UnitID.ToString() + "UserId" + "-" + reserve.UserID.ToString() + "." + unitimg.FileName.Split(".").Last();
                    reserve.IdImg = filename;
                    using (var fs = System.IO.File.Create("wwwroot/images/" + filename))
                    {
                        unitimg.CopyTo(fs);
                    }
                   if (reserves==null)
                    {
                        _context.Add(reserve);
                        await _context.SaveChangesAsync();
                        IList<Unit> list = _context.Unit.ToList();
                        ViewBag.list = list;


                        //var unit = _context.Unit.Where(a => a.ID == unitId);
                        //// return View(user);

                        ViewBag.id = Url.ActionContext.RouteData.Values["id"];
                        ViewBag.idg = Url.ActionContext.RouteData.Values["GuestNum"];
                        var UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
                        ViewBag.UserID = UserID;
                        return View("ReserveSuccess");

                    }
                    else
                    {
                        int id = reserve.UnitID;
                        TempData["unitId"] = id;
                        return RedirectToAction("Create");


                    }

                }





                //_context.Reserve.Add(reserve);
                //await _context.SaveChangesAsync();
                ////return View(unit);

                //return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewData["UnitID"] = new SelectList(_context.Set<Unit>(), "ID", "ID", reserve.UnitID);

                return View(reserve);
            }

            //ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", reserve.UserID);

        }

        // GET: Reserves/Edit/5
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

        // POST: Reserves/Edit/5
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

        // GET: Reserves/Delete/5
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

        // POST: Reserves/Delete/5
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
