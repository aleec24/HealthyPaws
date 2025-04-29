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
    [Table("Contacts")]
    public class Contact
    {
        [Key]
        public int contactId { get; set; }

        [DisplayName("Correo Electrónico")]
        [EmailAddress(ErrorMessage = "Ingrese un correo electrónico válido.")]
        public string Email { get; set; }
        [DisplayName("Número de Teléfono")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "El número de teléfono debe ser de 8 dígitos.")]
        public string Phone{ get; set; }
        public bool WhatsApp { get; set; }
    }
}
