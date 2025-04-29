using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthyPawsV2.DAL
{
    [Table("PetBreeds")]
    public class PetBreed
    {
        [Key]
        [DisplayName("Id")]
        public int petBreedId { get; set; }

        [ForeignKey("PetType")]
        [DisplayName("Tipo de Mascota")]
        public int petTypeId { get; set; }

        [DisplayName("Raza de la Mascota")]
        public string name { get; set; }

        [DisplayName("Estado")]
        public bool status { get; set; }

        public PetType? PetType { get; set; }

        public ICollection<PetFile> PetFiles { get; set; } = new List<PetFile>();


    }
}
