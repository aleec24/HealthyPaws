using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthyPawsV2.DAL
{
    public class LogReport
    {
        [Key]
        public int LogReportId { get; set; }

        [DisplayName("Creador")]
        [Required]
        public string creator { get; set; }

        [DisplayName("Nombre")]
        [Required]
        public string name { get; set; }

        [DisplayName("Tipo")]
        public string type { get; set; }

        [DisplayName("Fecha de Creación")]
        public DateTime creationDate { get; set; }
        [DisplayName("Fecha de Creación")]
        public string formattedCreationDate
        {
            get
            {
                return creationDate.ToString("dd/MM/yyyy HH:mm tt");
            }
        }

        public ApplicationUser? CreatorId { get; set; }
    }

}
