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
    public class ProfileUnitController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfileUnitController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProfileUnit
        public async Task<IActionResult> Index()
        {
            List<Area> areas = new List<Area>();

            List<Area> areanum = _context.Area.ToList();
            ViewBag.areanum = areanum;
            List<Category> categories = new List<Category>();
            List<Category> type = _context.Category.ToList();
            ViewBag.type = type;
            var UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.UserID = UserID;

            IList<Image> img = _context.Image.ToList();
            ViewBag.img = img;
            var applicationDbContext = _context.Unit.Include(u => u.Aminities).Include(u => u.Area).Include(u => u.Category).Include(u=>u.Images).Include(u => u.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ProfileUnit/Details/5
        public async Task<IActionResult> Details(int? id)
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
            .Include(u => u.Images)
            .Include(u => u.Reviews)
            .FirstOrDefaultAsync(m => m.ID == id);
            if (unit == null)
            {
                return NotFound();
            }
            IList<Image> img = _context.Image.Where(a => a.UnitID == id).ToList();
            ViewBag.img = img; IList<Review> review = _context.Review.Where(a => a.UnitID == id).ToList();
            ViewBag.review = review; IList<Review> re = _context.Review.ToList();
            ViewBag.review = re; IList<Unit> unittt = _context.Unit.Where(a => a.ID == id).ToList();
            ViewBag.unittt = unittt; IList<ApplicationUser> user = _context.Users.ToList();
            ViewBag.user = user.ToList();
            //IList<Aminity> aminities = _context.Aminity.Where(a=>a.ID==unittt.a);
            //ViewBag.aminities = aminities;       
            return View(unit);
        }

        // GET: ProfileUnit/Create
        public IActionResult Create()
        {
            ViewData["AminitiesID"] = new SelectList(_context.Set<Aminity>(), "ID", "ID");
            ViewData["AreaID"] = new SelectList(_context.Area, "ID", "ID");
            ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "ID");
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: ProfileUnit/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,guestNo,Price,Address,RoomNum,PathRoomNum,UserID,CategoryID,AminitiesID,AreaID,Description")] Unit unit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(unit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AminitiesID"] = new SelectList(_context.Set<Aminity>(), "ID", "ID", unit.AminitiesID);
            ViewData["AreaID"] = new SelectList(_context.Area, "ID", "ID", unit.AreaID);
            ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "ID", unit.CategoryID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", unit.UserID);
            return View(unit);
        }

        // GET: ProfileUnit/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
            ViewData["AreaID"] = new SelectList(_context.Area, "ID", "ID", unit.AreaID);
            ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "ID", unit.CategoryID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", unit.UserID);
            return View(unit);
        }

        // POST: ProfileUnit/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int amnintyID, [Bind("ID,guestNo,Price,Address,RoomNum,PathRoomNum,UserID,CategoryID,AminitiesID,AreaID,Description")] Unit unit, IFormFile[] imgsfile)
        {





            if (ModelState.IsValid)
            {

                try
                {


                    if (id != unit.ID)
                    {
                        return NotFound();
                    }

                    _context.SaveChanges();


                    for (int i = 0, count = 0; i < imgsfile.Length && count < imgsfile.Length; i++, count++)
                    {
                        string filename;
                        Image unitImage = new Image();
                        Image images = _context.Image.OrderByDescending(a => a.UnitID).FirstOrDefault();
                        if (images != null)
                        {
                            filename = (images.UnitID + count).ToString() + "." + imgsfile[i].FileName.Split(".").Last();
                            unitImage.ImgSrc = filename;
                            unitImage.UnitID = unit.ID;
                            _context.Image.Add(unitImage);
                            await _context.SaveChangesAsync();

                            using (var fs = System.IO.File.Create("wwwroot/images/" + filename))
                            {
                                imgsfile[i].CopyTo(fs);
                            }
                        }
                        else
                        {
                            filename = i.ToString() + "." + imgsfile[i].FileName.Split(".").Last();
                            unitImage.ImgSrc = filename;
                            unitImage.UnitID = unit.ID;
                            _context.Image.Add(unitImage);
                            await _context.SaveChangesAsync();

                            using (var fs = System.IO.File.Create("wwwroot/images" + filename))
                            {
                                imgsfile[i].CopyTo(fs);
                            }
                        }

                        _context.Update(unit);



                        await _context.SaveChangesAsync();

                    }

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
                return RedirectToAction(nameof(Index));

            }
            ViewData["AminitiesID"] = new SelectList(_context.Set<Aminity>(), "ID", "ID", unit.AminitiesID);
            ViewData["AreaID"] = new SelectList(_context.Area, "ID", "ID", unit.AreaID);
            ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "ID", unit.CategoryID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", unit.UserID);
            ViewData["name"] = new SelectList(_context.Set<Area>(), "name", "name");
            ViewData["Type"] = new SelectList(_context.Set<Category>(), "Type", "Type");
            return View(unit);

        }

        private bool UnitExists(int iD)
        {
            throw new NotImplementedException();
        }
       

        // GET: ProfileUnit/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: ProfileUnit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> DeleteConfirmed(int id, int AminitiesID, int CategoryID, int AreaID, string UserID)
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
                IList<Aminity> amn = _context.Aminity.Where(a => a.ID == AminitiesID).ToList();
                IList<Category> ctg = _context.Category.Where(a => a.ID == CategoryID).ToList();
                IList<Area> ar = _context.Area.Where(a => a.ID == AreaID).ToList();
                IList<ApplicationUser> user = _context.Users.Where(a => a.Id == UserID).ToList();

                foreach (var item in ar)
                {
                    _context.Area.Remove(item);
                }
                foreach (var item in user)
                {
                    _context.Users.Remove(item);
                }
                foreach (var item in ctg)
                {
                    _context.Category.Remove(item);
                }
                foreach (var item in amn)
                {
                    _context.Aminity.Remove(item);
                }
                await _context.SaveChangesAsync();



                _context.Unit.Remove(unit);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
