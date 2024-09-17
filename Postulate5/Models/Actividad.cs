using System.ComponentModel.DataAnnotations;

namespace Postulate.Models;

public class Actividad 
{
    [Key]
    public int ActividadID { get; set; }
    public string? Nombre { get; set; }
}