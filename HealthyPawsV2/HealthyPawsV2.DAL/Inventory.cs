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
    [Table("Inventories")]
    public class Inventory
    {
        [Key]
        [Required]
        [DisplayName("Id")]
        public int inventoryId { get; set; }

        [Required]
        [DisplayName("Nombre")]
        public string name { get; set; }

        [DisplayName("Categoria")]
        public string category { get; set; }

        [DisplayName("Marca")]
        public string brand { get; set; }

        [DisplayName("Cantidad Disponible")]
        public int availableStock { get; set; }

        [DisplayName("Descripción")]
        public string description { get; set; }

        [Required]
        [DisplayName("Precio")]
        public double price { get; set; }

        [DisplayName("Proveedor")]
        public string provider { get; set; }

        [DisplayName("Contacto de Proveedor")]
        public string providerInfo { get; set; }

        [DisplayName("Estado")]
        public bool State { get; set; }

        public ICollection<AppointmentInventory> AppointmentInventories { get; set; } = new List<AppointmentInventory>();

    }
}
