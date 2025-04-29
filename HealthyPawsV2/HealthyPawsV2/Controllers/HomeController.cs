using HealthyPawsV2.DAL;
using HealthyPawsV2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HealthyPawsV2.Controllers
{
    public class SearchViewModel
    {
        public List<PetType> PetTypes { get; set; }
        public List<ApplicationUser> Users { get; set; }
    }
    public class HomeController : Controller
    {
        private readonly HPContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(HPContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Search(string GlobalSearch)
        {
            //Agregar los contextos 
            var petTypeQuery = _context.PetTypes.AsQueryable();
            var usersQuery = _userManager.Users.AsQueryable();

            var petBreedsQuery = _context.PetBreeds
                .Include(r => r.PetType)
                .Where(p => p.status)
                .AsQueryable();

            var appointments = _context.Appointments
                .Include(a => a.PetFile)
                .Include(a => a.owner)
                .Include(a => a.vet)
                .AsQueryable();

            var pets = _context.PetFiles
               .Include(p => p.Owner)
               .Include(p => p.PetBreed)
               .ThenInclude(b => b.PetType)
               .Where(p => p.status)
               .AsQueryable();

            var documents = _context.Documents
                .Include(a => a.PetFile)
                .AsQueryable();


            //Condicional de busqueda 
            if (!string.IsNullOrEmpty(GlobalSearch))
            {
                petTypeQuery = petTypeQuery.Where(m => m.name.Contains(GlobalSearch));
                usersQuery = usersQuery.Where(p => p.name.Contains(GlobalSearch));
                petBreedsQuery = petBreedsQuery.Where(p => p.name.Contains(GlobalSearch) || p.PetType.name.Contains(GlobalSearch));
                appointments = appointments.Where(a => a.owner.name.Contains(GlobalSearch) || a.vet.name.Contains(GlobalSearch) || a.PetFile.name.Contains(GlobalSearch));
                pets = pets.Where(e => e.name.Contains(GlobalSearch) || e.PetBreed.name.Contains(GlobalSearch) || e.Owner.name.Contains(GlobalSearch) || e.gender.Contains(GlobalSearch) || e.castration.Contains(GlobalSearch));
                documents = documents.Where(u => u.name.Contains(GlobalSearch) || u.category.Contains(GlobalSearch));
            }

            //Lista de los datos encontrados 
            var petTypeResult = await petTypeQuery.ToListAsync();
            var usersResult = await usersQuery.ToListAsync();
            var petBreedResult = await petBreedsQuery.ToListAsync();
            var appointmentsResult = await appointments.ToListAsync();
            var petsResult = await pets.ToListAsync();
            var documentResult = await documents.ToListAsync();


            //Clasificar los ViewBag y los No result
            ViewBag.PetTypes = petTypeResult;
            ViewBag.Users = usersResult;
            ViewBag.PetBreeds = petBreedsQuery;
            ViewBag.Appointment = appointments;
            ViewBag.PetFiles = pets;
            ViewBag.Documents = documents;

            // los No result
            ViewBag.NoResultadosPetTypes = petTypeResult.Count == 0;
            ViewBag.NoResultadosUsers = usersResult.Count == 0;
            ViewBag.NoResultadosPetBreeds = petBreedResult.Count == 0;
            ViewBag.NoResultadosAppointment = appointmentsResult.Count == 0;
            ViewBag.NoResultadosPetFiles = petsResult.Count == 0;
            ViewBag.NoResultadosDocuments = documentResult.Count == 0;

            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

    }
}