using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace HealthyPawsV2.DAL;

[Table("AspNetUsers")]
public class ApplicationUser : IdentityUser
{

    [Required]
    [MaxLength(100)]
    [DisplayName("Nombre de Usuario")]
    public string name { get; set; }  

    [Required]
    [MaxLength(100)]
    [DisplayName("Apellidos")]
    public string  surnames { get; set; }


    [DisplayName("Ultima Fecha de Conexion")]
    public DateTime lastConnection { get; set; }

    [Required]
    [DisplayName("Tipo de Cedula")]
    public string idType { get; set; }  

    [Required]
    [MaxLength(12)]
    [DisplayName("Numero de Cedula")]
    public string idNumber { get; set; }

    [Required]
    [MaxLength(8, ErrorMessage = "El teléfono debe tener 8 caracteres.")]
    [DisplayName("Telefono 1")]
    public string phone1 { get; set; }


    [MaxLength(8, ErrorMessage = "El teléfono debe tener 8 caracteres.")]
    [DisplayName("Telefono 2")]
    public string phone2 { get; set; }

    [MaxLength(8, ErrorMessage = "El teléfono debe tener 8 caracteres.")]
    [DisplayName("Telefono 3")]
    public string phone3 { get; set; }

    [ForeignKey("Address")]
    [DisplayName("Dirección")]
    public int addressId { get; set; }   //Foreign Key para jalar las direcciones

    [Required]
    [DefaultValue(true)]
    public bool status { get; set; }

    
    public string province { get; set; }

    
    public string canton { get; set; }

   
    public string district { get; set; }

    //public Direccion? Direccion { get; set; }  // Jalar direccion del usuario




}

