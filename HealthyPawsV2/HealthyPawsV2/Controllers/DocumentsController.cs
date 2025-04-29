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
using System.Drawing;

namespace HealthyPawsV2.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly HPContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public DocumentsController(HPContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

		// GET: Documents
		[HttpGet]
		public async Task<IActionResult> Index(string documentSearch, string fileTypeFilter, string statusFilterDoc = "active")
        {
            //Get logged user
            var userIdentity = User.Identity as ClaimsIdentity;
            var loggedUserTask = RolesUtils.GetLoggedUser(_userManager, new ClaimsPrincipal(userIdentity));
            loggedUserTask.Wait();
            var loggedUser = loggedUserTask.Result;

            var documents = _context.Documents
                .Include(d => d.PetFile)
                .AsQueryable();

            //list documents ONLY of logged user
            if (User.IsInRole("User"))
            {
                documents = documents
                    .Where(m => m.PetFile.ownerId == loggedUser.Id)
                    .AsQueryable();
            }

         

            if (!string.IsNullOrWhiteSpace(documentSearch))
            {
                documentSearch = documentSearch.Trim();

                if (int.TryParse(documentSearch, out int idSearch))
                {
                    // Búsqueda numérica y por texto
                    documents = documents.Where(d =>
                        d.petFileId == idSearch ||
                        d.AppointmentId == idSearch ||
                        d.name.Contains(documentSearch) ||
                        d.category.Contains(documentSearch));
                }
                else
                {
                    // Búsqueda textual case-insensitive más eficiente
                    documents = documents.Where(d =>
                        EF.Functions.Like(d.name, $"%{documentSearch}%") ||
                        EF.Functions.Like(d.category, $"%{documentSearch}%"));
                }
            }


            // Filter by file type
            if (!string.IsNullOrEmpty(fileTypeFilter))
            {
                documents = documents.Where(d =>
                    (fileTypeFilter == "image" &&
                        (d.fileType[0] == 0xFF && d.fileType[1] == 0xD8 ||
                         d.fileType[0] == 0x89 && d.fileType[1] == 0x50 && d.fileType[2] == 0x4E && d.fileType[3] == 0x47)) ||
                    (fileTypeFilter == "pdf" &&
                        d.fileType[0] == 0x25 && d.fileType[1] == 0x50 && d.fileType[2] == 0x44 && d.fileType[3] == 0x46) ||
                    (fileTypeFilter == "word" &&
                        d.fileType[0] == 0x50 && d.fileType[1] == 0x4B && d.fileType[2] == 0x03 && d.fileType[3] == 0x04)
                );
            }

            // Filter by status
            if (statusFilterDoc == "active")
            {
                documents = documents.Where(u => u.status == true);
            }
            else if (statusFilterDoc == "inactive")
            {
                documents = documents.Where(u => u.status == false);
            }

            var hpContext = await documents.ToListAsync();
          
            ViewData["statusFilterDoc"] = statusFilterDoc;

            if (hpContext.Count == 0)
            {
                ViewBag.NoResultados = true;
            }
            else
            {
                ViewBag.NoResultados = false;
            }

            ViewData["AppointmentId"] = new SelectList(
            from appointment in _context.Appointments
            join petFile in _context.PetFiles on appointment.petFileId equals petFile.petFileId
            join user in _context.ApplicationUser on petFile.ownerId equals user.Id
            where appointment.status != "Cancelada"
            select new
            {
                AppointmentId = appointment.AppointmentId,
                DisplayText = $"{appointment.AppointmentId} - {petFile.name} - {user.name} - {user.idNumber}  "
            },
            "AppointmentId",
            "DisplayText"
            );


            foreach (var appointment in _context.Appointments)
            {
                Console.WriteLine($"AppointmentId: {appointment.AppointmentId}, Status: {appointment.status}");
            }

            ViewData["petFileName"] = new SelectList(
            from pf in _context.PetFiles
            select new SelectListItem
            {
                Value = pf.petFileId.ToString(),
                Text = $"{pf.petFileId} - {pf.name}"
            },
                "Value",
                "Text"
                );


            //ViewBag.petFileId = new SelectList(petFiles, "Value", "Text");
            ViewData["AppointmentId"] = new SelectList(_context.Appointments.Where(a => a.status != "Cancelada"), "AppointmentId", "AppointmentId");
            ViewData["petFileId"] = new SelectList(_context.PetFiles, "petFileId", "petFileId");
            ViewData["Users"] = new SelectList(_context.ApplicationUser, "Id", "UserName");
            ViewData["FileTypeFilter"] = fileTypeFilter;


            return View(hpContext);
        }

        // GET: Documents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .FirstOrDefaultAsync(m => m.documentId == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // GET: Documents/Create
        public IActionResult Create()
        {

            //ViewData["AppointmentId"] = new SelectList(_context.Appointments, "AppointmentId", "AppointmentId");
            //ViewData["AppointmentId"] = new SelectList(_context.Appointments.Where(a => a.status != "Cancelada"), "AppointmentId", "AppointmentId");


            ViewData["petFileId"] = new SelectList(_context.PetFiles, "petFileId", "petFileId");
            ViewData["Users"] = new SelectList(_context.ApplicationUser, "Id", "UserName");
            return View();
        }

        // POST: Documents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppointmentId,documentId,petFileId,name,category,fileType,status,fileType")] Document document, IFormFile File)
        {
            document.status = true;

            if (ModelState.IsValid)
            {
                if (File != null && File.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await File.CopyToAsync(memoryStream);
                        document.fileType = memoryStream.ToArray();
                    }
                }
                _context.Add(document);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Volver a llenar el ViewData en caso de error de validación
            ViewData["AppointmentId"] = new SelectList(_context.Appointments.Where(a => a.status != "Cancelada"), "AppointmentId", "AppointmentId");

            //ViewData["AppointmentId"] = new SelectList(_context.Appointments, "AppointmentId", "AppointmentId");
            ViewData["petFileId"] = new SelectList(_context.PetFiles, "petFileId", "petFileId", document.petFileId);
            ViewData["Users"] = new SelectList(_context.ApplicationUser, "Id", "UserName");

            return View(document);
        }

        // GET: Documents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }

            ViewBag.Categorias = new List<string>
    {
        "Rayos X",
        "Vacunación",
        "Examen de Sangre",
        "Cirugía",
        "Esterilización",
        "Consultas Veterinarias",
        "Otro"
    };

            // Pasar la categoría seleccionada al ViewBag
            ViewBag.CategoriaSeleccionada = document.category;


            //ViewData["AppointmentId"] = new SelectList(_context.Appointments, "AppointmentId", "AppointmentId");

            ViewData["AppointmentId"] = new SelectList( _context.Appointments.Where(a => a.status != "Cancelada"), "AppointmentId", "AppointmentId",document.AppointmentId);

            ViewData["petFileId"] = new SelectList(_context.PetFiles, "petFileId", "petFileId", document.petFileId);
            ViewData["Users"] = new SelectList(_context.ApplicationUser, "Id", "UserName");

            return View(document);
        }

        // POST: Documents/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("documentId,AppointmentId,petFileId,name,category,fileType,status")] Document document,
            IFormFile File, bool? reactiveDocument)
        {
            if (id != document.documentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var originalDocument = await _context.Documents.AsNoTracking().FirstOrDefaultAsync(d => d.documentId == id);

                    if (originalDocument == null)
                    {
                        return NotFound();
                    }

                    // Manejo del archivo
                    if (File != null && File.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await File.CopyToAsync(memoryStream);
                            document.fileType = memoryStream.ToArray();
                        }
                    }
                    else
                    {
                        document.fileType = originalDocument.fileType;
                    }

                    // Lógica especial para reactivación
                    if (reactiveDocument.HasValue && reactiveDocument.Value)
                    {
                        document.status = true; // Forzar activación si se solicita
                    }
                    else if (!document.status && originalDocument.status)
                    {
                        // Si estaba activo y ahora se desactiva
                        document.status = false;
                    }

                    _context.Update(document);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentExists(document.documentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // Recargar ViewData si hay errores
            //ViewData["AppointmentId"] = new SelectList(_context.Appointments, "AppointmentId", "AppointmentId", document.AppointmentId);

            ViewData["AppointmentId"] = new SelectList(_context.Appointments.Where(a => a.status != "Cancelada"),"AppointmentId","AppointmentId", document.AppointmentId);


            ViewData["petFileId"] = new SelectList(_context.PetFiles, "petFileId", "petFileId", document.petFileId);
            return View(document);
        }

        private bool DocumentExists(int id)
        {
            return _context.Documents.Any(e => e.documentId == id);
        }



        // GET: Documents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .FirstOrDefaultAsync(m => m.documentId == id);
            if (document == null)
            {
                return NotFound();
            }

            document.status = false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var document = await _context.Documents.FindAsync(id);
            if (document != null)
            {
                document.status = false; // Cambiar esto en lugar de eliminarlo
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




        //This is to download the files
        public IActionResult Download(int id)
        {
            var document = _context.Documents.FirstOrDefault(d => d.documentId == id);
            if (document == null)
            {
                return NotFound();
            }

            return File(document.fileType, "application/octet-stream", document.name);
        }
    }
}
