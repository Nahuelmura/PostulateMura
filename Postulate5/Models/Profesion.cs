using System.ComponentModel.DataAnnotations;

namespace Postulate.Models;

public class Profesion 
{
    [Key]
    public int ProfesionID { get; set; }
    public string? Nombre { get; set; }
    public string? Matricula { get; set; }

    public virtual  ICollection <Servicio> Servicios  { get; set; }

    public virtual ICollection <Trabajo> Trabajos { get; set; }


}