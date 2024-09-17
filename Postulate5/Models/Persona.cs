using System.ComponentModel.DataAnnotations;
using Azure.Identity;

namespace Postulate.Models;

public class Persona
{
    [Key]
    public int PersonaID { get; set; }
    public int LocalidadID { get; set; }

    public string? Nombre { get; set; }
    public string? Apellido { get; set; }

    
    public int Edad { get; set; }
    public int Telefono { get; set; }
    public int Documento { get; set; }
    public string? Email { get; set; }
    public virtual Localidad Localidad { get; set;}
    public virtual ICollection <Servicio> Servicios { get; set; }
  
}

public class VistaTraerDatosPersonal 
{
    
    public int PersonaID { get; set; }
    public int LocalidadID { get; set; }

     public int ProvinciaID { get; set; }


   public string? NombreLocalidad { get; set; }


    public string? NombreProvincia { get; set; }
    public string? Nombre { get; set; }

    

    
    public string? Apellido { get; set; }

    public int Edad { get; set; }
    public int Telefono { get; set; }
    public int Documento { get; set; }
    public string? Email { get; set; }
  
  


}
