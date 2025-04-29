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
    public class PetBreedsController : Controller
    {
        private readonly HPContext _context;

        public PetBreedsController(HPContext context)
        {
            _context = context;
        }

        // GET: PetBreeds
        public async Task<IActionResult> Index(string searchPetBreed, string statusFilterPetBreed = "active")
        {
            var petBreed = _context.PetBreeds.AsQueryable();

            if (!string.IsNullOrEmpty(searchPetBreed))
            {
                petBreed = petBreed.Where(m => m.name.Contains(searchPetBreed));
            }

            // Filter by status
            if (statusFilterPetBreed == "active")
            {
                petBreed = petBreed.Where(u => u.status == true);
            }
            else if (statusFilterPetBreed == "inactive")
            {
                petBreed = petBreed.Where(u => u.status == false);
            }

            var hpContext = await petBreed.ToListAsync();
            //Enlistar solo tipos de animales que se encuentren activos
            ViewBag.PetTypes = await _context.PetTypes.Where(pt => pt.status == true).ToListAsync();
            ViewData["searchPetBreed"] = searchPetBreed;
            ViewData["statusFilterPetBreed"] = statusFilterPetBreed;

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

        // GET: PetBreeds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var petBreed = await _context.PetBreeds
                .Include(p => p.PetType)
                .FirstOrDefaultAsync(m => m.petBreedId == id);
            if (petBreed == null)
            {
                return NotFound();
            }

            return View(petBreed);
        }

        // GET: PetBreeds/Create
        public IActionResult Create()
        {
            ViewData["petTypeId"] = new SelectList(_context.PetTypes, "petTypeId", "name");
            return View();
        }

        // POST: PetBreeds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("petBreedId,petTypeId,name,status")] PetBreed petBreed)
        {
            petBreed.status = true;
            if (ModelState.IsValid)
            {
                _context.Add(petBreed);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            ViewBag.PetTypes = new SelectList(await _context.PetTypes.ToListAsync(), "petTypeId", "name", petBreed.petTypeId);
            return View(petBreed);
        }

        // GET: PetBreeds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var petBreed = await _context.PetBreeds.FindAsync(id);
            if (petBreed == null)
            {
                return NotFound();
            }
            ViewData["petTypeId"] = new SelectList(_context.PetTypes, "petTypeId", "name", petBreed.petTypeId);
            return View(petBreed);
        }

        // POST: PetBreeds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("petBreedId,petTypeId,name,status")] PetBreed petBreed)
        {
            if (id != petBreed.petBreedId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(petBreed);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PetBreedExists(petBreed.petBreedId))
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
            ViewData["petTypeId"] = new SelectList(_context.PetTypes, "petTypeId", "name", petBreed.petTypeId);
            return View(petBreed);
        }

        // GET: PetBreeds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var petBreed = await _context.PetBreeds
                .Include(p => p.PetType)
                .FirstOrDefaultAsync(m => m.petBreedId == id);
            if (petBreed == null)
            {
                return NotFound();
            }

            petBreed.status = false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: PetBreeds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var petBreed = await _context.PetBreeds.FindAsync(id);
            if (petBreed != null)
            {
                petBreed.status = false;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PetBreedExists(int id)
        {
            return _context.PetBreeds.Any(e => e.petBreedId == id);
        }
    }
}
