using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HealthyPawsV2.DAL;
using Microsoft.AspNetCore.Identity;
using HealthyPawsV2.Utils;
using System.Security.Claims;
using System.Drawing.Imaging;

namespace HealthyPawsV2.Controllers
{
    public class PetFilesController : Controller
    {
        private readonly HPContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public PetFilesController(HPContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: PetFiles
        [HttpGet]
        public async Task<IActionResult> Index(string searchPetFile, string statusFilterPet = "active") 
        {
            //Get logged user
            var userIdentity = User.Identity as ClaimsIdentity;
            var loggedUserTask = RolesUtils.GetLoggedUser(_userManager, new ClaimsPrincipal(userIdentity));
            loggedUserTask.Wait();
            var loggedUser = loggedUserTask.Result;

            //list pets
            var pets = _context.PetFiles
                .Include(p => p.Owner)
                .Include(p => p.PetBreed)
                .ThenInclude(b => b.PetType)
                .AsQueryable();

            //list pets ONLY of logged user
            if (User.IsInRole("User"))
            {
                pets = pets
                    .Where(m => m.ownerId == loggedUser.Id)
                    .AsQueryable();
            }

            //This is the Search Bar
            if (!string.IsNullOrWhiteSpace(searchPetFile))
            {
                string normalizedSearch = searchPetFile.Trim().ToLower(); // Normalizar entrada

                int.TryParse(normalizedSearch, out int parsedPetFileId);

                pets = pets.Where(p => p.name.ToLower().Contains(normalizedSearch) ||
                                       p.Owner.name.ToLower().Contains(normalizedSearch) ||
                                       p.petFileId == parsedPetFileId);
            }


            // Filter by status
            if (statusFilterPet == "active")
            {
                pets = pets.Where(u => u.status == true);
            }
            else if (statusFilterPet == "inactive")
            {
                pets = pets.Where(u => u.status == false);
            }

            var hpContext = await pets.ToListAsync();

            ViewBag.NoResultados = hpContext.Count == 0;
            ViewData["statusFilterPet"] = statusFilterPet;

            //Properties for the Modal
            //Get users with "User" role for Dueño dropdown in create page
            var ownersTask = RolesUtils.GetUsersPerRole(_roleManager, _userManager, "User");
            ownersTask.Wait();
            var owners = ownersTask.Result;
            var ownerList = owners.Select(user => new
            {
                Id = user.Id, // AppUser "Id" Relation
                DisplayName = $"{user.name} {user.surnames} - {user.idNumber}" // idNumber to display personal id of the user
            }).ToList();
            ViewData["Owners"] = new SelectList(ownerList, "Id", "DisplayName");

            ViewBag.PetTypes = new SelectList(await _context.PetTypes.ToListAsync(), "petTypeId", "name");
            ViewBag.PetBreeds = new SelectList(await _context.PetBreeds.ToListAsync(), "petBreedId", "name");

            return View(hpContext);
        }

        // GET: PetFiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var ownersTask = RolesUtils.GetUsersPerRole(_roleManager, _userManager, "User");
            ownersTask.Wait();
            var owners = ownersTask.Result;
            var ownerList = owners.Select(user => new
            {
                Id = user.Id, // AppUser "Id" Relation
                DisplayName = $"{user.name} {user.surnames}"}).ToList();
            ViewData["Owners"] = new SelectList(ownerList, "Id", "DisplayName");

            if (id == null)
            {
                return NotFound();
            }

            var petFile = await _context.PetFiles
                .Include(p => p.PetBreed)
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(m => m.petFileId == id);

            if (petFile == null)
            {
                return NotFound();
            }

            return View(petFile);
        }

        // GET: PetFiles/Create
        public IActionResult Create()
        {
            ViewData["petBreedId"] = new SelectList(_context.PetBreeds, "petBreedId", "name");

            //Here we are inlcuding the name -Surnames and Idnumber form the user 
            ViewData["Users"] = new SelectList(_context.ApplicationUser
                .Select(u => new
                {
                    Id = u.Id,
                    DisplayName = $"{u.name} {u.surnames} - {u.idNumber}"
                }), "Id", "DisplayName");

            ViewData["petTypeId"] = new SelectList(_context.PetTypes, "petTypeId", "name");

            

            return View();
        }

        // POST: PetFiles/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("petFileId,petBreedId,ownerId,name,petTypeId,gender,age,weight,creationDate,vaccineHistory,castration,description,status")] PetFile petFile)
        {
            petFile.creationDate = DateTime.Now.Date + DateTime.Now.TimeOfDay;
            petFile.status = true;

            if (ModelState.IsValid)
            {
                _context.Add(petFile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Users"] = new SelectList(_context.ApplicationUser, "Id", "name", petFile.Owner);
            ViewData["petTypeId"] = new SelectList(_context.PetTypes, "petTypeId", "name", petFile.petTypeId);
            ViewData["petBreedId"] = new SelectList(new List<SelectListItem>());


            return View(petFile);
        }

        //GET Raza y luego se transforma a JSON
        public JsonResult GetBreedsByType(int petTypeId)
        {
            var breeds = _context.PetBreeds
                .Where(b => b.petTypeId == petTypeId)
                .Select(b => new { b.petBreedId, b.name })
                .ToList();

            return Json(breeds);
        }


        // GET: PetFiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var ownersTask = RolesUtils.GetUsersPerRole(_roleManager, _userManager, "User");
            ownersTask.Wait();
            var owners = ownersTask.Result;
            var ownerList = owners.Select(user => new
            {
                Id = user.Id, // AppUser "Id" Relation
                DisplayName = $"{user.name} {user.surnames} - {user.idNumber}" // idNumber to display personal id of the user
            }).ToList();
            ViewData["Owners"] = new SelectList(ownerList, "Id", "DisplayName");

            if (id == null)
            {
                return NotFound();
            }

            var petFile = await _context.PetFiles.FindAsync(id);
            if (petFile == null)
            {
                return NotFound();
            }
            ViewData["petBreedId"] = new SelectList(_context.PetBreeds, "petBreedId", "name", petFile.petBreedId);
            ViewData["Users"] = new SelectList(_context.ApplicationUser, "Id", "name", petFile.Owner);
            ViewData["petTypeId"] = new SelectList(_context.PetTypes, "petTypeId", "name", petFile.petTypeId);
            return View(petFile);
        }

        // POST: PetFiles/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("petFileId,petBreedId,ownerId,name,petTypeId,gender,age,weight,creationDate,vaccineHistory,castration,description,status")] PetFile petFile, bool reactivePet)
        {
            if (id != petFile.petFileId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var originalPetfile = await _context.PetFiles.AsNoTracking().FirstOrDefaultAsync(m => m.petFileId == id);

                    if (originalPetfile == null)
                    {
                        return NotFound();
                    }

                    // Mantener la fecha de creación
                    petFile.creationDate = originalPetfile.creationDate;

                    // Mantener el dueño original si no se está modificando en la vista
                    petFile.ownerId = originalPetfile.ownerId;

                    // Reactivar la mascota si es necesario
                    if (reactivePet)
                    {
                        petFile.status = true; // Cambiar el estado a "reactivado"
                    }
                    else
                    {
                        petFile.status = originalPetfile.status; // Mantener el estado original si no se marca el checkbox
                    }


                    _context.Update(petFile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }
            ViewData["petBreedId"] = new SelectList(_context.PetBreeds.Where(b => b.petTypeId == petFile.petTypeId), "petBreedId", "name", petFile.petBreedId);
            ViewData["Users"] = new SelectList(_context.ApplicationUser, "Id", "name", petFile.Owner);
            ViewData["petTypeId"] = new SelectList(_context.PetTypes, "petTypeId", "name", petFile.petTypeId);
            return View(petFile);
        }
      
        public async Task<IActionResult> Delete(int? id)
        {
                if (id == null)
                {
                    return NotFound();
                }

                var petfile = await _context.PetFiles.FindAsync(id);
                if (petfile == null)
                {
                    return NotFound();
                }

                petfile.status = false;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
        }

        // POST: PetFiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var petfile = await _context.PetFiles.FindAsync(id);
            if (petfile != null)
            {
                petfile.status = false;
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
