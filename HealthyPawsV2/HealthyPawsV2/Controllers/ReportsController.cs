using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthyPawsV2.DAL;

namespace HealthyPawsV2.Controllers
{
    public class ReportsController : Controller
    {
        private readonly HPContext _context;

        public ReportsController(HPContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var datosProvincias = await ObtenerDatosPorProvincia();
            var datosTiposDeMascotas = await ObtenerDatosPorTipoDeMascota();
            var citasPorDia = await ObtenerCitasPorDia();

            var cantidadExpedientesStatusTrue = await ObtenerCantidadExpedientesStatusTrue();
            var cantidadUsuarios = await ObtenerCantidadUsuarios();
            var cantidadCitasAgendadas = await ObtenerCantidadCitasAgendadas();

            var viewModel = new ReporteViewModel
            {
                ReporteProvincias = datosProvincias,
                ReporteTiposDeMascotas = datosTiposDeMascotas,
                CitasPorDia = citasPorDia,
                CantidadExpedientesStatusTrue = cantidadExpedientesStatusTrue,
                CantidadUsuarios = cantidadUsuarios,
                CantidadCitasAgendadas = cantidadCitasAgendadas
            };

            return View(viewModel);
        }

        //private async Task<List<ProvinciaReporte>> ObtenerDatosPorProvincia()
        //{
        //    var reporte = await _context.ApplicationUser
        //        .Include(a => a.addressId)
        //        .GroupBy(c => c.province)
        //        .Select(g => new ProvinciaReporte
        //        {
        //            Provincia = g.Key,
        //            Cantidad = g.Count()
        //        })
        //        .ToListAsync();

        //    return reporte;
        //}

        private async Task<List<ProvinciaReporte>> ObtenerDatosPorProvincia()
        {
            var reporte = await _context.ApplicationUser
                .Join(_context.Addresses,
                      user => user.addressId,
                      address => address.AddressId,
                      (user, address) => new { user, address }) 
                .GroupBy(ua => ua.address.province) 
                .Select(g => new ProvinciaReporte
                {
                    Provincia = g.Key,
                    Cantidad = g.Count()
                })
                .ToListAsync();

            return reporte;
        }

        private async Task<List<TipoDeMascotaReporte>> ObtenerDatosPorTipoDeMascota()
        {
            var reporte = await _context.PetFiles
                .GroupBy(p => p.petTypeId)
                .Select(g => new TipoDeMascotaReporte
                {
                    TipoDeMascota = _context.PetTypes.Where(pt => pt.petTypeId == g.Key).Select(pt => pt.name).FirstOrDefault(),
                    Cantidad = g.Count()
                })
                .ToListAsync();

            return reporte;
        }

        private async Task<List<CitasPorDiaReporte>> ObtenerCitasPorDia()
        {
            var reporte = await _context.Appointments
                .GroupBy(a => a.Date.Date)
                .Select(g => new CitasPorDiaReporte
                {
                    Fecha = g.Key,
                    Cantidad = g.Count()
                })
                .OrderBy(g => g.Fecha)
                .ToListAsync();

            return reporte;
        }

        private async Task<int> ObtenerCantidadExpedientesStatusTrue()
        {
            var cantidad = await _context.PetFiles
                .Where(p => p.status == true)
                .CountAsync();

            return cantidad;
        }

        private async Task<int> ObtenerCantidadUsuarios()
        {
            var cantidad = await _context.ApplicationUser.CountAsync();
            return cantidad;
        }

        private async Task<int> ObtenerCantidadCitasAgendadas()
        {
            var cantidad = await _context.Appointments
                .Where(a => a.status == "Agendada")
                .CountAsync();

            return cantidad;
        }
    }

    public class ProvinciaReporte
    {
        public string Provincia { get; set; }
        public int Cantidad { get; set; }
    }

    public class TipoDeMascotaReporte
    {
        public string TipoDeMascota { get; set; }
        public int Cantidad { get; set; }
    }

    public class CitasPorDiaReporte
    {
        public DateTime Fecha { get; set; }
        public int Cantidad { get; set; }
    }

    public class ReporteViewModel
    {
        public List<ProvinciaReporte> ReporteProvincias { get; set; }
        public List<TipoDeMascotaReporte> ReporteTiposDeMascotas { get; set; }
        public List<CitasPorDiaReporte> CitasPorDia { get; set; }
        public int CantidadExpedientesStatusTrue { get; set; }
        public int CantidadUsuarios { get; set; }
        public int CantidadCitasAgendadas { get; set; }
    }
}






