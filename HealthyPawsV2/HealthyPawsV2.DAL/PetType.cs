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
    [Table("PetTypes")]
    public class PetType
    {
        [Key]
        [DisplayName("Id")]
        public int petTypeId { get; set; }  //This is the Pets ID.                  

        [DisplayName("Tipo de Animal")]
        public string name { get; set; }    

        [DisplayName("Estado")]
        public bool status { get; set; }  

        public ICollection<PetBreed> PetBreeds { get; set; } = new List<PetBreed>();  // Collection related to the PetBreed Entity.

    }
}
