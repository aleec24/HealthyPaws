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
    [Table("AppointmentInventories")]
    public class AppointmentInventory
    {
        [Key]
        [Required]
        [DisplayName("Id")]
        public int appointmentInventoryId { get; set; }

        [ForeignKey("Appointment")]
        [Required]
        [DisplayName("Número de Cita")]
        public int appointmentId { get; set; }

        [ForeignKey("Inventory")]
        [Required]
        [DisplayName("Medicamento")]
        public int inventoryID { get; set; }

        [DisplayName("Dosis")]
        public string dose {  get; set; }

        [DisplayName("Medida de Dosis")]
        public string  measuredose { get; set; }

        [DisplayName("Estado")]
        public bool status { get; set; }

        public Inventory? Inventory { get; set; }

        public Appointment? Appointment { get; set; }









    }
}
