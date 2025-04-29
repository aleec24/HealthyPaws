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
    public class LogReportsController : Controller
    {
        private readonly HPContext _context;

        public LogReportsController(HPContext context)
        {
            _context = context;
        }

        // GET: LogReports
        public async Task<IActionResult> Index()
        {
            return View(await _context.LogReports.ToListAsync());
        }

        // GET: LogReports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logReport = await _context.LogReports
                .FirstOrDefaultAsync(m => m.LogReportId == id);
            if (logReport == null)
            {
                return NotFound();
            }

            return View(logReport);
        }

        // GET: LogReports/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LogReports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LogReportId,creator,name,type,creationDate")] LogReport logReport)
        {
            if (ModelState.IsValid)
            {
                _context.Add(logReport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(logReport);
        }

        // GET: LogReports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logReport = await _context.LogReports.FindAsync(id);
            if (logReport == null)
            {
                return NotFound();
            }
            return View(logReport);
        }

        // POST: LogReports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LogReportId,creator,name,type,creationDate")] LogReport logReport)
        {
            if (id != logReport.LogReportId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(logReport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LogReportExists(logReport.LogReportId))
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
            return View(logReport);
        }

        // GET: LogReports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logReport = await _context.LogReports
                .FirstOrDefaultAsync(m => m.LogReportId == id);
            if (logReport == null)
            {
                return NotFound();
            }

            return View(logReport);
        }

        // POST: LogReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var logReport = await _context.LogReports.FindAsync(id);
            if (logReport != null)
            {
                _context.LogReports.Remove(logReport);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LogReportExists(int id)
        {
            return _context.LogReports.Any(e => e.LogReportId == id);
        }
    }
}
