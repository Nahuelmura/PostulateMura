using System.ComponentModel.DataAnnotations;

namespace Postulate.Models;

public class Localidad
{
    [Key]
    public int LocalidadID { get; set; }
    public int ProvinciaID  { get; set; }
    public string? Nombre { get; set; }
    public int CodigoPostal { get; set; }

    public virtual Provincia Provincia{ get; set; }

    public virtual ICollection <Persona> Personas { get; set; }
}

public class VistaLocalidades
{
    public int LocalidadID {get; set; }

    public int ProvinciaID{ get; set; }

    public string? ProvinciaNombre { get; set; }

     public string? Nombre { get; set; }

    public int CodigoPostal { get; set; }
}
