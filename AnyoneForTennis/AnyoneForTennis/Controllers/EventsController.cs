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

namespace AnyoneForTennis.Controllers
{
    public class EventsController : Controller
    {
        private readonly AppDBContext _context;

        public EventsController(AppDBContext context)
        {
            _context = context;
        }

        // GET: Events
        [Authorize]
        public async Task<IActionResult> Index()
        {
            if (this.User.IsInRole("Member"))
            {
                var Events = await _context.Events.ToListAsync();
                var Coaches = await _context.Coaches.ToListAsync();
                var EventMembers = await _context.EventMembers.ToListAsync();

                var retList = new List<Event>();
                var user = new Member();

                foreach (var item in _context.Members)
                {
                    if (item.Username.CompareTo(this.User.Identity.Name) == 0)
                    {
                        user = item;
                        break;
                    }
                }

                foreach (var item in Events)
                {
                    var temp = new EventMember {
                        EventId = item.EventId,
                        Event = item,
                        MemberId = user.MemberId,
                        Member = user
                    };
                    if (item.EventMembers != null)
                    {
                        foreach (var @event in item.EventMembers)
                        {
                            if (@event.EventId == item.EventId && @event.MemberId == user.MemberId)
                            {
                                retList.Add(item);
                            }
                        }
                    }
                }

                ViewData["Role"] = "Member";
                return View(retList);
            }
            //-------------------------------------------------------------------------------
            else if (this.User.IsInRole("Coach"))
            {
                var Events = await _context.Events.ToListAsync();
                var coaches = await _context.Coaches.ToListAsync();
                var retList = new List<Event>();
                var user = new Coach();

                foreach (var item in _context.Coaches)
                {
                    if (item.Username.CompareTo(this.User.Identity.Name) == 0)
                    {
                        user = item;
                        break;
                    }
                }

                foreach (var item in Events)
                {
                    if (item.RunningCoach.CoachId == user.CoachId)
                    {
                        retList.Add(item);
                    }
                }

                ViewData["Role"] = "Coach";
                return View(retList);
            }
            //----------------------------------------------------------------------------
            else
            {
                ViewData["Role"] = "Admin";
                return View(await _context.Events.ToListAsync());
            }
        }

        // GET: Events/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (@event == null)
            {
                return NotFound();
            }

            var members = new List<string>();

            @event.EventMembers = new List<EventMember>();

            foreach (var item in _context.EventMembers)
            {
                if (item.EventId == @event.EventId)
                {
                    members.Add(_context.Members.Find(item.MemberId).Username);
                }
            }

            if (@event.EventMembers.Count == 0)
            {
                members.Add("No one is enrolled in this event yet.");
            }

            var retEvent = new EventDetailsViewModel
            {
                Id = (int)id,
                Date = @event.Date,
                Location = @event.Location,
                Name = @event.Name,
                RunningCoach = _context.Coaches.Find(@event.CoachId).Name,
                CoachId = @event.CoachId,
                Members = members
            };

            if (this.User.IsInRole("Coach"))
            {
                ViewData["Role"] = "Coach";
            }
            else if (this.User.IsInRole("Admin"))
            {
                ViewData["Role"] = "Admin";
            }
            else
            {
                ViewData["Role"] = "Member";
            }
            return View(retEvent);
        }

        // GET: Events/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["CoachId"] = new SelectList(_context.Coaches, "CoachId", "Name");
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("EventId,Name,Date,Location")] Event @event, int RunningCoach)
        {
            if (ModelState.IsValid)
            {
                @event.RunningCoach = _context.Coaches.Find(RunningCoach);
                @event.CoachId = RunningCoach;

                _context.Add(@event);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["CoachId"] = new SelectList(_context.Coaches, "CoachId", "Name", @event.RunningCoach.CoachId);
            return View(@event);
        }

        // GET: Events/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            var retEvent = new EventEditViewModel
            {
                EventId = @event.EventId,
                Date = @event.Date,
                Location = @event.Location,
                Name = @event.Name,
                RunningCoach = _context.Coaches.Find(@event.CoachId)
            };

            //Todo figure out why this keeps selecting the wrong value on page load
            ViewData["CoachId"] = new SelectList(_context.Coaches, "CoachId", "Name", @event.CoachId);
            return View(retEvent);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("EventId,Name,Date,Location")] EventEditViewModel @event, int RunningCoach)
        {
            if (id != @event.EventId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var efEvent = _context.Events.Find(@event.EventId);

                efEvent.RunningCoach = _context.Coaches.Find(RunningCoach);
                efEvent.CoachId = RunningCoach;

                _context.Update(efEvent);
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }

            ViewData["CoachId"] = new SelectList(_context.Coaches, "CoachId", "Name", @event.RunningCoach.CoachId);
            return View(@event);
        }

        // GET: Events/Enroll/5
        [Authorize(Roles = "Member,Admin")]
        public async Task<IActionResult> Enroll(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            var retEvent = new EventEnrollViewModel
            {
                Id = (int)id,
                Date = @event.Date,
                Location = @event.Location,
                Name = @event.Name,
                RunningCoach = _context.Coaches.Find(@event.CoachId).Name
            };

            return View(retEvent);
        }

        // POST: Events/Enroll/5
        [HttpPost, ActionName("Enroll")]
        [Authorize(Roles = "Member,Admin")]
        public async Task<IActionResult> EnrollConfirm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var efEvent = await _context.Events.FindAsync(id);
            if (efEvent == null)
            {
                return NotFound();
            }

            var user = new Member();

            foreach (var item in _context.Members)
            {
                if (this.User.Identity.Name.CompareTo(item.Username) == 0)
                {
                    user = item;
                    break;
                }
            }

            if (efEvent.EventMembers != null)
            {
                efEvent.EventMembers.Add(new EventMember {
                EventId = efEvent.EventId,
                MemberId = user.MemberId
            });
            }
            else
            {
                efEvent.EventMembers = new List<EventMember>();
                efEvent.EventMembers.Add(new EventMember
                {
                    EventId = efEvent.EventId,
                    MemberId = user.MemberId
                });
            }
            
            _context.Events.Update(efEvent);
            await _context.SaveChangesAsync();

            return RedirectToAction("");
        }

        // GET: Events/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Events
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            _context.Events.Remove(@event);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Events
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> EventList()
        {
            var Events = await _context.Events.ToListAsync();
            var Coaches = await _context.Coaches.ToListAsync();
            var EventMembers = await _context.EventMembers.ToListAsync();

            var retList = new List<Event>();
            var user = new Member();

            foreach (var item in _context.Members)
            {
                if (item.Username.CompareTo(this.User.Identity.Name) == 0)
                {
                    user = item;
                    break;
                }
            }

            foreach (var item in Events)
            {
                if (item.EventMembers != null)
                {
                    foreach (var @event in item.EventMembers)
                    {
                        if (@event.EventId == item.EventId && @event.MemberId == user.MemberId)
                        {
                            
                        }
                        else
                        {
                            retList.Add(item);
                        }
                    }
                }
                else
                {
                    retList.Add(item);
                }
            }

            ViewData["Role"] = "Member";
            return View(retList);
        }
    }
}
