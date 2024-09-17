// using System.ComponentModel.DataAnnotations;
// using System.ComponentModel.DataAnnotations.Schema;

// namespace Postulate.Models;

// public class Resenia
// {
//     [Key]
//     public int ReseniaID { get; set; }
//     public int ServicioID { get; set; }
//     public int TrabajoID { get; set; }

//     [Range(1, 5, ErrorMessage = "La calificación debe estar entre 1 y 5 estrellas.")]
//     public int Valoracion { get; set; }
//     public string? Descripcion { get; set; }
//     public virtual Servicio Servicio   { get; set; }
//     public virtual Trabajo Trabajo { get; set; }

      
// }


// se crea una tabla donde se valida que el prestador de servicio acepta el trabajo y habilita a puntuar // 