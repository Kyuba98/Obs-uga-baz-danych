using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Obsługa_baz_danych.Data;
using Obsługa_baz_danych.Models;

namespace Obsługa_baz_danych.Controllers
{
    [Authorize]
    public class ExercisesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ExercisesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Exercises
        public async Task<IActionResult> Index()
        {
            IdentityUser user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            var applicationDbContext = _context.Exercise.Where(e => e.UserId == user.Id).Include(e => e.User).Include(e => e.ExerciseType).Include(e => e.Session);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Statistic()
        {
            var applicationDbContext = _context.ExerciseType;
            return View(await applicationDbContext.ToListAsync());
        }

        // Post: Statistic
        public async Task<IActionResult> StatisticResults(int exerciseTypeId)
        {
            DateTime date = DateTime.Now;
            date = date.AddDays(-28);
            IdentityUser user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            var exerciseType = _context.ExerciseType.FindAsync(exerciseTypeId).Result;
            var applicationDbContext = _context.Exercise.Where(e => e.UserId == user.Id).Where(e => e.Session.Start > date).Include(e => e.User).Include(e => e.ExerciseType).Include(e => e.Session).Where(e => e.ExerciseType.Id == exerciseTypeId);
            int record = 0;
            int numberOfSesions = 0;
            foreach (var e in applicationDbContext)
            {
                numberOfSesions++;
                int hold = e.Reps * e.Series * e.Weight;
                if (hold > record) { record = hold; }
            }
            
            ViewBag.ExerciseTypeName = exerciseType.Name;
            ViewBag.Record = record;
            ViewBag.NumberOfSesions = numberOfSesions;
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Exercises/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Exercise == null)
            {
                return NotFound();
            }

            var exercise = await _context.Exercise
                .Include(e => e.ExerciseType)
                .Include(e => e.Session)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exercise == null)
            {
                return NotFound();
            }

            return View(exercise);
        }

        // GET: Exercises/Create
        public IActionResult Create()
        {
            IdentityUser user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseType, "Id", "Name");
            ViewData["SessionId"] = new SelectList(_context.Set<Session>().Where(e => e.UserId == user.Id), "Id", "Start");
            return View();
        }

        // POST: Exercises/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Weight,Reps,Series,SessionId,ExerciseTypeId")] Exercise exercise)
        {
            IdentityUser user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            exercise.UserId = user.Id;

            if (ModelState.IsValid)
            {
                _context.Add(exercise);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseType, "Id", "Name", exercise.ExerciseTypeId);
            ViewData["SessionId"] = new SelectList(_context.Set<Session>(), "Id", "Start", exercise.SessionId);
            return View(exercise);
        }

        // GET: Exercises/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Exercise == null)
            {
                return NotFound();
            }

            var exercise = await _context.Exercise.FindAsync(id);
            if (exercise == null)
            {
                return NotFound();
            }
            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseType, "Id", "Name", exercise.ExerciseTypeId);
            ViewData["SessionId"] = new SelectList(_context.Set<Session>(), "Id", "Start", exercise.SessionId);
            return View(exercise);
        }

        // POST: Exercises/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Weight,Reps,Series,SessionId,ExerciseTypeId")] Exercise exercise)
        {
            if (id != exercise.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exercise);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExerciseExists(exercise.Id))
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
            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseType, "Id", "Name", exercise.ExerciseTypeId);
            ViewData["SessionId"] = new SelectList(_context.Set<Session>(), "Id", "Start", exercise.SessionId);
            return View(exercise);
        }

        // GET: Exercises/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Exercise == null)
            {
                return NotFound();
            }

            var exercise = await _context.Exercise
                .Include(e => e.ExerciseType)
                .Include(e => e.Session)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exercise == null)
            {
                return NotFound();
            }

            return View(exercise);
        }

        // POST: Exercises/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Exercise == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Exercise'  is null.");
            }
            var exercise = await _context.Exercise.FindAsync(id);
            if (exercise != null)
            {
                _context.Exercise.Remove(exercise);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExerciseExists(int id)
        {
          return (_context.Exercise?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
