using System.ComponentModel.DataAnnotations;

namespace Postulate.Models;

public class Imagen
{
    [Key]
    public int? ImagenID { get; set; }

    public string? Titulo { get; set; }

    public string? Descripcion { get; set; }

    public byte? Foto { get; set; }

    public int ServicioID { get; set; }


 public virtual ICollection <Trabajo> Trabajo { get; set; }

   
}
