using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using projjjecttttt.Data;
using projjjecttttt.Models;

namespace projjjecttttt.Controllers
{
    public class UnitController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UnitController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Unit
        public async Task<IActionResult> Index()
        {
            IList<Area> areanam = _context.Area.ToList();

            ViewBag.areanam = areanam;

            IList<Category> type = _context.Category.ToList();

            ViewBag.type = type;

            IList<Unit> idd = _context.Unit.ToList();

            ViewBag.idd = idd;

            IList<Image> img = _context.Image.ToList();
            ViewBag.img = img;




            var applicationDbContext = _context.Unit.Include(u => u.Aminities).Include(u => u.Area).Include(u => u.Category).Include(u => u.User).Include(u => u.Images);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Unit/Details/5
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

        // GET: Unit/Create
        [Authorize]
        public IActionResult Create()
        {
            IList<Area> areas = _context.Area.ToList();
            ViewBag.Areas = areas;
            IList<Category> types = _context.Category.ToList();
            ViewBag.types = types;
            ViewData["AminitiesID"] = new SelectList(_context.Set<Aminity>(), "ID", "ID");
            ViewData["AreaID"] = new SelectList(_context.Set<Area>(), "ID", "name");
            ViewData["CategoryID"] = new SelectList(_context.Set<Category>(), "ID", "Type");
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["name"] = new SelectList(_context.Set<Area>(), "name", "name");
            ViewData["Type"] = new SelectList(_context.Set<Category>(), "Type", "Type");




            return View();
        }

        // POST: Unit/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,guestNo,Price,Address,RoomNum,PathRoomNum,UserID,AminitiesID,CategoryID,AreaID,Description")] Unit unit, IFormFile[] imgsfile, Aminity am, string Wifi, string AC, string Fan, string TV, string Parking, string Landline)
        {

            if (Wifi == "true")
            {
                am.Wifi = true.ToString();
            }
            else
            {
                am.Wifi = false.ToString();
            }
            if (TV == "true")
            {
                am.TV = true.ToString();
            }
            else
            {
                am.TV = false.ToString();
            }
            if (Fan == "true")
            {
                am.Fan = true.ToString();
            }
            else
            {
                am.Fan = false.ToString();
            }


            if (AC == "true")
            {
                am.AC = true.ToString();
            }
            else
            {
                am.AC = false.ToString();
            }
            if (Landline == "true")
            {
                am.Landline = true.ToString();
            }
            else
            {
                am.Landline = false.ToString();
            }
            if (Parking == "true")
            {
                am.Parking = true.ToString();
            }
            else
            {
                am.Parking = false.ToString();
            }
            if (ModelState.IsValid)
            {
                _context.Add(am);
                await _context.SaveChangesAsync();




                IList<Area> areas = _context.Area.ToList();
                ViewBag.Areas = areas;
                IList<Category> types = _context.Category.ToList();

                ViewBag.types = types;
                unit.AminitiesID = am.ID;
                _context.Add(unit);
                await _context.SaveChangesAsync();
            }
            if (imgsfile == null)
            {
                ModelState.AddModelError("", "No Images Uplodead,please Upload Images");
            }
            else
            {
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

                }


                return RedirectToAction(nameof(Index));
            }
            ViewData["AminitiesID"] = new SelectList(_context.Set<Aminity>(), "ID", "ID", unit.AminitiesID);
            ViewData["AreaID"] = new SelectList(_context.Set<Area>(), "ID", "name", unit.AreaID);
            ViewData["CategoryID"] = new SelectList(_context.Set<Category>(), "ID", "Type", unit.CategoryID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", unit.UserID);


            //ViewData["name"] = new SelectList(_context.Set<Area>(), "ID", "ID", unit.Area.ID);
            //ViewData["Type"] = new SelectList(_context.Set<Category>(), "ID", "ID", unit.Category.ID);

            var TypeSTR = _context.Category;
            ViewBag.type = TypeSTR;



            return View(unit);
        }

        // GET: Unit/Edit/5
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

        // POST: Unit/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int amnintyID, [Bind("ID,guestNo,Price,Address,RoomNum,PathRoomNum,UserID,CategoryID,AminitiesID,AreaID,Description")] Unit unit, IFormFile[] imgsfile)
        {

            if (ModelState.IsValid)
            {
                _context.Update(unit);

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

        // GET: Unit/Delete/5
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

        // POST: Unit/Delete/5
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





        public IActionResult SearchByValue(int category, int area, int price)
        {
            IList<Area> areanam = _context.Area.ToList();
            ViewBag.areanam = areanam;

            IList<Category> type = _context.Category.ToList();
            ViewBag.type = type;

            if (area != 0 && category != 0 && price != 0)
            {
                var filter = _context.Unit.Where(a => a.CategoryID == category && a.AreaID == area && a.Price <= price).Include(a => a.Images).ToList();
                return View(filter);

            }
            else if (area == 0 && category != 0 && price != 0)
            {
                var filter = _context.Unit.Where(a => a.CategoryID == category && a.Price <= price).Include(a => a.Images).ToList();
                return View(filter);
            }
            else if (area != 0 && category == 0 && price != 0)
            {
                var filter = _context.Unit.Where(a => a.AreaID == area && a.Price <= price).Include(a => a.Images).ToList();
                return View(filter);
            }
            else if (area != 0 && category != 0 && price == 0)
            {
                var filter = _context.Unit.Where(a => a.CategoryID == category && a.AreaID == area).Include(a => a.Images).ToList();
                return View(filter);
            }
            else if (area != 0 && category == 0 && price == 0)
            {
                var filter = _context.Unit.Where(a => a.AreaID == area).Include(a => a.Images).ToList();
                return View(filter);
            }
            else if (area == 0 && category != 0 && price == 0)
            {
                var filter = _context.Unit.Where(a => a.CategoryID == category).Include(a => a.Images).ToList();
                return View(filter);
            }
            else if (area == 0 && category == 0 && price != 0)
            {
                var filter = _context.Unit.Where(a => a.Price <= price).Include(a => a.Images).ToList();
                return View(filter);
            }
            else
            {
                var filter = _context.Unit.Include(a => a.Images).ToList();
                return View(filter);
            }



        }
    }
}
