using Microsoft.AspNetCore.Http;
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
    [Table("Document")]
    public class Document
    {
        [Key]
        [Required]
        public int documentId { get; set; }

        [ForeignKey("PetFile")]
        [Required]
        [DisplayName("ID de la Mascota")]
        public int petFileId { get; set; }

        [ForeignKey("Appointment")]
        [Required]
        [DisplayName("ID de la Cita")]
        public int AppointmentId { get; set; }

        [Required]
        [DisplayName("Nombre del Documento/Examen")]
        public string name { get; set; }

        [Required]
        [DisplayName("Categoria")]
        public string category { get; set; }

        [DisplayName("Tipo de Archivo")]
        public byte[] fileType { get; set; }

        public string FileTypeMime { get; set; }


        [DisplayName("Fecha de Creación")]
        public DateTime FechaCreacion { get; set; }

        [DisplayName("Estado")]
        public bool status { get; set; }

        //Relacion con entidad PetFile
        public PetFile? PetFile { get; set; }

        //Relacion con entidad Appointment
        public Appointment? Appointment { get; set; }

    }
}
