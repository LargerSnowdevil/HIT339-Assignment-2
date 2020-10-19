using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AnyoneForTennis.Data;
using AnyoneForTennis.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AnyoneForTennis.Controllers
{
    public class CoachesController : Controller
    {
        private readonly AppDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CoachesController(AppDBContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Coaches
        [Authorize(Roles = "Admin,Member")]
        public async Task<IActionResult> Index()
        {
            var Coaches = await _context.Coaches.ToListAsync();
            var retList = new List<CoachIndexViewModel>();

            foreach (var item in Coaches)
            {
                retList.Add(new CoachIndexViewModel {
                    CoachId = item.CoachId,
                    Name = item.Name
                });
            }

            if (this.User.IsInRole("Admin"))
            {
                ViewData["Role"] = "Admin";
            }
            else
            {
                ViewData["Role"] = "Member";
            }
            

            return View(retList);
        }

        // GET: Coaches/Details/5
        [Authorize(Roles = "Admin,Member")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coach = await _context.Coaches
                .FirstOrDefaultAsync(m => m.CoachId == id);
            if (coach == null)
            {
                return NotFound();
            }

            if (this.User.IsInRole("Admin"))
            {
                ViewData["Role"] = "Admin";
            }
            else
            {
                ViewData["Role"] = "Member";
            }

            return View(coach);
        }

        // GET: Coaches/Edit/5
        [Authorize(Roles = "Admin,Coach")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coach = await _context.Coaches.FindAsync(id);
            if (coach == null)
            {
                return NotFound();
            }

            var retCoach = new CoachEditViewModel {
                Name = coach.Name,
                CoachId = coach.CoachId,
                Age = coach.Age,
                Biography = coach.Biography
            };

            return View(retCoach);
        }

        // POST: Coaches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Coach")]
        public async Task<IActionResult> Edit(int id, [Bind("CoachId,Name,Age,Biography")] CoachEditViewModel coach)
        {
            if (id != coach.CoachId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var efCoach = _context.Coaches.Find(coach.CoachId);

                efCoach.Biography = coach.Biography;
                efCoach.Age = coach.Age;
                efCoach.Name = coach.Name;

                _context.Update(efCoach);
                await _context.SaveChangesAsync();

                if (this.User.IsInRole("Admin"))
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(coach);
        }

        // GET: Coaches/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coach = await _context.Coaches
                .FirstOrDefaultAsync(m => m.CoachId == id);
            if (coach == null)
            {
                return NotFound();
            }

            return View(coach);
        }

        // POST: Coaches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coach = await _context.Coaches.FindAsync(id);
            _context.Coaches.Remove(coach);

            var coachLogin = await _userManager.FindByNameAsync(coach.Username);
            await _userManager.DeleteAsync(coachLogin);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CoachExists(int id)
        {
            return _context.Coaches.Any(e => e.CoachId == id);
        }
    }
}
