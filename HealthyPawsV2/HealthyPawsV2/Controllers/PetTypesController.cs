using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HealthyPawsV2.DAL;

namespace HealthyPawsV2.Controllers
{
    public class PetTypesController : Controller
    {
        private readonly HPContext _context;

        public PetTypesController(HPContext context)
        {
            _context = context;
        }

        // GET: PetTypes
        [HttpGet]
        public async Task<IActionResult> Index(string searchPetType, string statusFilterPetType = "active")
        {
            var petType = _context.PetTypes.AsQueryable();

            if (!string.IsNullOrEmpty(searchPetType))
            {
                petType = petType.Where(m => m.name.Contains(searchPetType));
            }

            // Filter by status
            if (statusFilterPetType == "active")
            {
                petType = petType.Where(u => u.status == true);
            }
            else if (statusFilterPetType == "inactive")
            {
                petType = petType.Where(u => u.status == false);
            }

            var hpContext = await petType.ToListAsync();
            ViewData["searchPetType"] = searchPetType;
            ViewData["statusFilterPetType"] = statusFilterPetType;

            if (hpContext.Count == 0)
            {
                ViewBag.NoResultados = true;
            }
            else
            {
                ViewBag.NoResultados = false;
            }

            return View(hpContext);
        }



        // GET: PetTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var petType = await _context.PetTypes
                .FirstOrDefaultAsync(m => m.petTypeId == id);
            if (petType == null)
            {
                return NotFound();
            }

            return View(petType);
        }

        // GET: PetTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PetTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("petTypeId,name")] PetType petType)
        {
            petType.status = true;
            if (ModelState.IsValid)
            {
                _context.Add(petType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(petType);
        }

        // GET: PetTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var petType = await _context.PetTypes.FindAsync(id);
            if (petType == null)
            {
                return NotFound();
            }
            return View(petType);
        }

        // POST: PetTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("petTypeId,name,status")] PetType petType)
        {
            if (id != petType.petTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(petType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PetTypeExists(petType.petTypeId))
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
            return View(petType);
        }

        // GET: PetTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var PetType = await _context.PetTypes.FindAsync(id);
            if (PetType == null)
            {
                return NotFound();
            }

            PetType.status = false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // POST: PetTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var petType = await _context.PetTypes.FindAsync(id);
            if (petType != null)
            {
                petType.status = false;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PetTypeExists(int id)
        {
            return _context.PetTypes.Any(e => e.petTypeId == id);
        }
    }
}
