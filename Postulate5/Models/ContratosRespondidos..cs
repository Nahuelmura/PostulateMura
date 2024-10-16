using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Postulate.Models
{
    public class ContratoRespondido
    {
        [Key]
        public int ContratoRespondidoID { get; set; }


        public int TrabajoID { get; set; }

        public int ServicioID { get; set; }


        public int ProfesionID { get; set; }


        public int PersonaID { get; set; }

        public DateTime FechaMatch { get; set; }


        public bool Respuesta { get; set; }

        public Estado RespuestaDesolicitud { get; set; }
        public Servicio Servicio { get; set; }

        public Trabajo Trabajo { get; set; }

        public Persona Persona { get; set; }

    }

    public enum Estado
    {
        Pendiente = 1,
        Aceptado,
        Rechazado,


    }



    public class ContratoRespondidoVista
    {
        public int ContratoRespondidoID { get; set; }
        public int TrabajoID { get; set; }

        public int ServicioID { get; set; }

        public int ProfesionID { get; set; }

        public string? NombreProfesion { get; set; }


        public int PersonaID { get; set; }

        public string? NombrePersona { get; set; }

        public string? ApellidoPersona { get; set; }
        public string? DescripcionTrabajo { get; set; }

        public string? ComentarioTrabajo { get; set; }


         public string? DireccionTrabajo { get; set; }

            public  string? HoraSolicitadaTrabajo { get; set; }

               public  string? FechaSolicitadaTrabajo { get; set; }








        public Estado RespuestaDesolicitud { get; set; }
        public string? RespuestaDesolicitudString { get; set; }


        public DateTime FechaMatch { get; set; }

        public string StringFechaMatch { get; set; }


        public bool Respuesta { get; set; }



    }
}