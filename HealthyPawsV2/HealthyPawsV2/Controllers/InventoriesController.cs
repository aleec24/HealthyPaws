using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HealthyPawsV2.DAL;
using System.Drawing;

namespace HealthyPawsV2.Controllers
{
    public class InventoriesController : Controller
    {
        private readonly HPContext _context;

        public InventoriesController(HPContext context)
        {
            _context = context;
        }

        // GET: Inventories
        [HttpGet]
        public async Task<IActionResult> Index(string searchInventory, string statusFilterInventory = "active")
        {
            var inventaries =  _context.Inventories.AsQueryable();
            //       ViewData["inventarios"] = inventarios;

            if (!string.IsNullOrEmpty(searchInventory))
            {
                int.TryParse(searchInventory, out int parseditemId);
                inventaries = inventaries.Where(p => p.name.Contains(searchInventory) ||p.provider.Contains(searchInventory) ||
                p.category.Contains(searchInventory) ||p.inventoryId == parseditemId);
            }

            // Filter by status
            if (statusFilterInventory == "active")
            {
                inventaries = inventaries.Where(u => u.State == true);
            }
            else if (statusFilterInventory == "inactive")
            {
                inventaries = inventaries.Where(u => u.State == false);
            }

            var hpContext = await inventaries.ToListAsync();
            ViewData["statusFilterInventory"] = statusFilterInventory;

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
        //--------------------------------------------------------------------------------------

        // GET: Notifications
        public async Task<IActionResult> Notifications( )
        {
            var inventaries = _context.Inventories.AsQueryable();
            var inventariesResult = await inventaries.ToListAsync();

            var appointments = _context.Appointments.AsQueryable();
            var appointmentResult = await appointments.ToListAsync();

            ViewBag.Appointment = appointments;
            ViewBag.Inventory = inventaries;

            return View();
        }

        //--------------------------------------------------------------------------------------
        // GET: Inventories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventories
                .FirstOrDefaultAsync(m => m.inventoryId == id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        // GET: Inventories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Inventories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("inventoryId,name,category,brand,availableStock,description,price,provider,providerInfo,State")] Inventory inventory)
        {
            inventory.State = true; 

            if (ModelState.IsValid)
            {
                _context.Add(inventory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(inventory);
        }

        // GET: Inventories/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventories.FindAsync(id);
            if (inventory == null)
            {
                return NotFound();
            }
            return View(inventory);
        }

        // POST: Inventories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("inventoryId,name,category,brand,availableStock,description,price,provider,providerInfo,State")] Inventory inventory, bool reactiveInventory)
        {
            if (id != inventory.inventoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var originalInventory = await _context.Inventories.AsNoTracking().FirstOrDefaultAsync(i => i.inventoryId == id);

                    if(originalInventory == null)
                    {
                        return NotFound();
                    }

                    // Si el usuario marca la opción para reactivar, cambiamos el estado a activo
                    if (reactiveInventory)
                    {
                        inventory.State = true;
                    }
                    else
                    {
                        // Si no, mantenemos el estado original
                        inventory.State = originalInventory.State;
                    }


                    _context.Update(inventory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                        return NotFound(); 
                }
                return RedirectToAction(nameof(Index));
            }
            return View(inventory);
        }

        // GET: Inventories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventories.FindAsync(id);
            if (inventory == null)
            {
                return NotFound();
            }

            inventory.State = false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Inventories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete (int id)
        {
            {
                var inventory = await _context.Inventories.FindAsync(id);
                if (inventory != null)
                {
                    inventory.State = false;
                }

                return RedirectToAction(nameof(Index));
            }
        }
    }
}
