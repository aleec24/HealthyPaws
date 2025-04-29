using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthyPawsV2.DAL
{
    [Table("Appointment")]
    public class Appointment
    {
        [Key]
        [DisplayName("ID Cita")]
        public int AppointmentId { get; set; }

        [ForeignKey("PetFile")]
        [Required]
        [DisplayName("ID Mascota")]
        public int petFileId { get; set; }

        [Required]
        [DisplayName("ID de Veterinario")]
        public string vetId { get; set; }

        [Required]
        [DisplayName("Nombre de Cliente")]
        public string ownerId { get; set; }

        [DisplayName("Fecha de la Cita")]
        public DateTime Date {get; set; }

        [DisplayName("Descripción")]
        public string description { get; set; }

        [DisplayName("Estado de Cita")]
        public string status { get; set; }


        [DisplayName("Diagnóstico")]
        public string diagnostic { get; set; }

        [DisplayName("Observaciones Adicionales")]
        public string Additional { get; set; }


        public PetFile? PetFile { get; set; }   //PetFile

        public ApplicationUser? owner { get; set; }

        public ApplicationUser? vet { get; set; }

        public ICollection<AppointmentInventory> AppointmentInventories { get; set; } = new List<AppointmentInventory>();
        public ICollection<Document> Documents { get; set; } = new List<Document>();

    }
}
