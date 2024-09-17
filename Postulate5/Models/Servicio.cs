using System.ComponentModel.DataAnnotations;

namespace Postulate.Models;

public class Servicio
{
    [Key]
    public int ServicioID { get; set; }
    public int PersonaID { get; set; }
    public int ProfesionID { get; set; }

    //public int? ImagenID { get; set; }
    public bool Herramienta { get; set; }

    // public byte Imagen { get; set; }
    public string? Descripcion { get; set; }
    public string? Titulo { get; set; }
    public string? Institucion { get; set; }
    public virtual Persona Persona { get; set; }
    public virtual Profesion Profesion { get; set; }

    //   public virtual ICollection <ContratoRespondido> ContratosRespondidos { get; set; } 
}

public class VistaServicio
{
    public int ServicioID { get; set; }
    public int PersonaID { get; set; }
    public int ProfesionID { get; set; }

    public string? NombrePersona { get; set; }
    public string? ApellidoPersona { get; set; }



    public int? TelefonoPersona { get; set; }

    public int EdadPersona { get; set; }
    public int DocumentoPersona { get; set; }

    public string? EmailPersona { get; set; }

    public string? NombreProfesion { get; set; }

    public int? ImagenID { get; set; }
    public bool Herramienta { get; set; }

    // public byte Imagen { get; set; }
    public string? Descripcion { get; set; }
    public string? Titulo { get; set; }
    public string? Institucion { get; set; }



}
public class VistaTipoProfesion
{
    public int ProfesionID { get; set; }


    public string? Nombre { get; set; }

    public List<VistaPersonasServicios> ListadoPersonas { get; set; }
}


public class VistaPersonasServicios
{
    public string? NombrePersona { get; set; }
      public string? ApellidoPersona { get; set; }
    public int? TelefonoPersona { get; set; }
    public int ServicioID { get; set; }

    public string? Telefono { get; set; }
}