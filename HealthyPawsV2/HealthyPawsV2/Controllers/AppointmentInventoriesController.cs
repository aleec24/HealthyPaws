using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HealthyPawsV2.DAL;
using System.Drawing;
using System.Drawing.Imaging;
using HealthyPawsV2.Utils;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace HealthyPawsV2.Controllers
{
    public class AppointmentInventoriesController : Controller
    {
        private readonly HPContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AppointmentInventoriesController(HPContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: AppointmentInventories
        [HttpGet]
        public async Task<IActionResult> Index(string searchInvApp, string statusFilterInvApp = "active")
        {
            //Get logged user
            var userIdentity = User.Identity as ClaimsIdentity;
            var loggedUserTask = RolesUtils.GetLoggedUser(_userManager, new ClaimsPrincipal(userIdentity));
            loggedUserTask.Wait();
            var loggedUser = loggedUserTask.Result;

            var invApp = _context.AppointmentInventories
              .Include(p => p.Appointment)
              .Include(p => p.Inventory).AsQueryable();

            //list medications ONLY of logged user
            if (User.IsInRole("User"))
            {
                invApp = invApp
                    .Where(m => m.Appointment.ownerId == loggedUser.Id)
                    .AsQueryable();
            }

            if (!string.IsNullOrEmpty(searchInvApp))
            {
                int.TryParse(searchInvApp, out int parsedInvAppId);
                invApp = invApp.Where(p => p.appointmentId == parsedInvAppId || p.inventoryID == parsedInvAppId || p.appointmentInventoryId == parsedInvAppId);
            }

            // Filter by status
            if (statusFilterInvApp == "active")
            {
                invApp = invApp.Where(u => u.status == true);
            }
            else if (statusFilterInvApp == "inactive")
            {
                invApp = invApp.Where(u => u.status == false);
            }

            var hpContext = await invApp.ToListAsync();
            ViewData["statusFilterInvApp"] = statusFilterInvApp;

            if (hpContext.Count == 0)
            {
                ViewBag.NoResultados = true;
            }
            else
            {
                ViewBag.NoResultados = false;
            }

            // Cargar opciones de medicamentos
            ViewData["Medicamentos"] = new SelectList(_context.Inventories
                .Where(i => i.category == "Medicamento"), "inventoryId", "name");

            ViewData["Citas"] = new SelectList(_context.Appointments.Where(a => a.status != "Cancelada"), "AppointmentId", "AppointmentId");

            return View(hpContext);
        }

        // GET: AppointmentInventories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentInventory = await _context.AppointmentInventories
                .Include(a => a.Appointment)
                .Include(a => a.Inventory)
                .FirstOrDefaultAsync(m => m.appointmentInventoryId == id);
            if (appointmentInventory == null)
            {
                return NotFound();
            }

            return View(appointmentInventory);
        }

        // GET: AppointmentInventories/Create
        public IActionResult Create()
        {
            // ViewData["inventoryID"] = new SelectList(_context.Inventories, "inventoryId", "name");
            //ViewData["appointmentId"] = new SelectList(_context.Appointments, "AppointmentId", "AppointmentId");            
            ViewData["inventoryID"] = new SelectList(_context.Inventories.Where(i => i.category == "Medicamento"), "inventoryId", "name");

            ViewData["appointmentId"] = new SelectList(_context.Appointments.Where(a => a.status != "Cancelada")
        .Select(a => new
        {
            AppointmentId = a.AppointmentId,
            DisplayName = $"{a.AppointmentId} - {a.PetFile.name} - {a.PetFile.ownerId}"
        }), "AppointmentId", "DisplayName");
            return View();
        }

        // POST: AppointmentInventories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("appointmentInventoryId,appointmentId,inventoryID,dose,measuredose,status")] AppointmentInventory appointmentInventory)
        {
            appointmentInventory.status = true;

            if (ModelState.IsValid)
            {
                _context.Add(appointmentInventory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // ViewData["inventoryID"] = new SelectList(_context.Inventories, "inventoryId", "name", appointmentInventory.inventoryID);
            ViewData["inventoryID"] = new SelectList(_context.Inventories.Where(i => i.category == "Medicamento"),"inventoryId", "name", appointmentInventory.inventoryID);
            ViewData["appointmentId"] = new SelectList(_context.Appointments.Where(a => a.status != "Cancelada"), "AppointmentId", "AppointmentId", appointmentInventory.appointmentId);

            return View();
        }

        // GET: AppointmentInventories/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentInventory = await _context.AppointmentInventories.FindAsync(id);
            if (appointmentInventory == null)
            {
                return NotFound();
            }
            ViewData["appointmentId"] = new SelectList(_context.Appointments.Where(a => a.status != "Cancelada"), "AppointmentId", "AppointmentId", appointmentInventory.appointmentId);
            ViewData["inventoryID"] = new SelectList(_context.Inventories, "inventoryId", "brand", appointmentInventory.inventoryID);

            return View(appointmentInventory);
        }

        // POST: AppointmentInventories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("appointmentInventoryId,appointmentId,inventoryID,dose,measuredose,status")] AppointmentInventory appointmentInventory, bool reactiveInvApp)
        {
            if (id != appointmentInventory.appointmentInventoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var originalInvApp = await _context.AppointmentInventories.AsNoTracking().FirstOrDefaultAsync(i => i.appointmentInventoryId == id);

                    if(originalInvApp == null)
                    {
                        return NotFound();
                    }

                    if(reactiveInvApp && !originalInvApp.status)
                    {
                        appointmentInventory.status = true;
                    }

                    _context.Update(appointmentInventory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["appointmentId"] = new SelectList(_context.Appointments.Where(a => a.status != "Cancelada"), "AppointmentId", "AppointmentId", appointmentInventory.appointmentId);
            ViewData["inventoryID"] = new SelectList(_context.Inventories, "inventoryId", "brand", appointmentInventory.inventoryID);
            return View(appointmentInventory);
        }

        // GET: AppointmentInventories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentInventory = await _context.AppointmentInventories
                .Include(a => a.Appointment)
                .Include(a => a.Inventory)
                .FirstOrDefaultAsync(m => m.appointmentInventoryId == id);

            if (appointmentInventory == null)
            {
                return NotFound();
            }

            appointmentInventory.status = false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: AppointmentInventories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var appointmentInventory = await _context.AppointmentInventories.FindAsync(id);
            if (appointmentInventory != null)
            {
                appointmentInventory.status = false;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
