using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Postulate.Data;
using Postulate.Models;

namespace Postulate.Controllers

{
    public class ContratoRespondidoController : Controller
    {
        private readonly ILogger<ContratoRespondidoController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _rolManager;

        public ContratoRespondidoController(ILogger<ContratoRespondidoController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> rolManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _rolManager = rolManager;
            
        }

        

        

         public JsonResult BuscarServiciosTrabajo(int trabajoID)
        {

            

            // en base al id del trabajo requerido debemos buscar la profesion asociada a ese trabajo

            var TrabajoProfesionId = _context.Trabajos.Where(t => t.TrabajoID == trabajoID).Select(p => p.ProfesionID).FirstOrDefault(); 


            // una vez que tenemos la profesion debemos buscar los servicios asociados a esa profesion, incluimos a la entidad persona

            var serviciosProfesion = _context.Servicios.Where( s =>s.ProfesionID == TrabajoProfesionId) .Include(s => s.Persona);


             // crea una lista vacio del tipo del objeto que vamos a mostrar 
            List<ContratoRespondidoVista> ContratosRespondidoMostrar = new List<ContratoRespondidoVista>();

            // en base a los serviciosProfecion por cada uno crear un contrato en vista

        foreach (var servicio in serviciosProfesion)
        {

            



            var ContratoRespondidoMostrar = new ContratoRespondidoVista
            {
                ServicioID = servicio.ServicioID,
                NombrePersona = servicio.Persona.Nombre,
                ApellidoPersona = servicio.Persona.Apellido,
                
            };

            ContratosRespondidoMostrar.Add(ContratoRespondidoMostrar);     
            
        }
          
         
       
      return Json(ContratosRespondidoMostrar);

       
        }
        
   
    




public JsonResult SolicitarServicios(int servicioID, int trabajoID)
{
    
    var servicio = _context.Servicios.Include(s => s.Persona).FirstOrDefault(s => s.ServicioID == servicioID);
    var trabajo = _context.Trabajos.FirstOrDefault(t => t.TrabajoID == trabajoID);

    if (servicio != null && trabajo != null)
    {
        
        var contratoRespondido = new ContratoRespondido
        {
            ServicioID = servicio.ServicioID,
            TrabajoID = trabajo.TrabajoID, 
            ProfesionID = servicio.ProfesionID,
            PersonaID = servicio.Persona.PersonaID,
            FechaMatch = DateTime.Now,
            Respuesta = false
        };

        _context.ContratoRespondidos.Add(contratoRespondido);
        _context.SaveChanges();

        return Json(new { success = true, mensaje = "Solicitud enviada al servicio de " + servicio.Persona.Nombre });
    }

    return Json(new { success = false, mensaje = "Servicio o trabajo no encontrado" });
}
    

    


public JsonResult TraerServicioSolicitado(int? id)
{
    var datosServicios = _context.ContratoRespondidos.Include(e => e.Persona).Include(e => e.Trabajo) .ToList();

    var DatosServicio = datosServicios.Select(e => new ContratoRespondidoVista
    {
        ServicioID = e.ServicioID,
        ProfesionID = e.ProfesionID,
        StringFechaMatch = e.FechaMatch.ToString("dd/MM/yyyy"),
        NombrePersona = e.Persona.Nombre, 
        DescripcionTrabajo = e.Trabajo.Descripcion,
        ComentarioTrabajo = e.Trabajo.Comentario,
        
        Respuesta = e.Respuesta,
        
    }).ToList();

    return Json(DatosServicio);
}

    
    

    }

    
    

    }





















