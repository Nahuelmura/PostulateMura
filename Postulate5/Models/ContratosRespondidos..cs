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
   

          public bool Respuesta { get; set; }

    
    }
}