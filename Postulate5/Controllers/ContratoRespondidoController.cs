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
            
            
             var usuarioLogueado = _userManager.GetUserAsync(HttpContext.User).Result;
            var correoUsuarioLogueado = usuarioLogueado?.Email;


            // Obtener la persona asociada al correo del usuario logueado

            var personaLogueada = _context.Personas.FirstOrDefault(p => p.Email == correoUsuarioLogueado);
            var personaIDLogueada = personaLogueada?.PersonaID;
            var nombrePersonaLogueada = personaLogueada?.Nombre;



     ViewBag.PersonaIDLogueada = personaIDLogueada;
            ViewBag.NombrePersonaLogueada = nombrePersonaLogueada;




            

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
            RespuestaDesolicitud = Estado.Pendiente
        };

        _context.ContratoRespondidos.Add(contratoRespondido);
        _context.SaveChanges();

        return Json(new { success = true, mensaje = "Solicitud enviada al servicio de " + servicio.Persona.Nombre });
    }

    return Json(new { success = false, mensaje = "Servicio o trabajo no encontrado" });
}
    

    

public async Task<JsonResult> ListadoServicioSolicitado(int? id)
{
    
    var usuarioLogueado = await _userManager.GetUserAsync(HttpContext.User);

    if (usuarioLogueado == null)
    {
        return Json(new { success = false, mensaje = "Usuario no logueado." });
    }

    var correoUsuarioLogueado = usuarioLogueado.Email;

    
    var personaLogueada = _context.Personas.FirstOrDefault(p => p.Email == correoUsuarioLogueado);

    if (personaLogueada == null)
    {
        return Json(new { success = false, mensaje = "No se encontró una persona asociada al usuario logueado." });
    }

    var personaIDLogueada = personaLogueada.PersonaID;

    // Filtrar los servicios donde otras personas solicitaron un servicio ofrecido por la persona logueada
    var datosServicios = _context.ContratoRespondidos
        .Include(e => e.Persona) 
        .Include(e => e.Trabajo)
        .Include(e => e.Servicio)
        .Where(e => e.Servicio.PersonaID == personaIDLogueada) 
        .ToList();

   
    var DatosServicio = datosServicios.Select(e => new ContratoRespondidoVista
    {
        ContratoRespondidoID = e.ContratoRespondidoID,
        ServicioID = e.ServicioID,
        ProfesionID = e.ProfesionID,
        StringFechaMatch = e.FechaMatch.ToString("dd/MM/yyyy"),
        NombrePersona = e.Persona.Nombre,

        DireccionTrabajo =e.Trabajo.Direccion,
        HoraSolicitadaTrabajo = e.Trabajo.Hora.ToString("HH:mm"),
        FechaSolicitadaTrabajo = e.Trabajo.Fecha.ToString("dd/MM/yyyy"),



        DescripcionTrabajo = e.Trabajo.Descripcion,
        ComentarioTrabajo = e.Trabajo.Comentario,
        Respuesta = e.Respuesta,
        RespuestaDesolicitud = e.RespuestaDesolicitud,
        RespuestaDesolicitudString = e.RespuestaDesolicitud.ToString().ToUpper()

    }).ToList();

    return Json(DatosServicio);
}

    
public JsonResult CambiarRespuesta(int contratoRespondidoID, bool nuevaRespuesta)
{
   

    var contrato = _context.ContratoRespondidos.FirstOrDefault(c => c.ContratoRespondidoID == contratoRespondidoID);

    if (contrato != null)
    {
        contrato.Respuesta = nuevaRespuesta;
        _context.SaveChanges();

        return Json(new { success = true, mensaje = "Respuesta actualizada correctamente" });
    }

    return Json(new { success = false, mensaje = "Contrato no encontrado" });
}






public JsonResult CambiarEstado(int contratoRespondidoID, string estado)
{

    var contrato = _context.ContratoRespondidos.FirstOrDefault(c => c.ContratoRespondidoID == contratoRespondidoID);

    if (contrato != null)
    {
        
        if (estado == "aceptar")
        {
            contrato.RespuestaDesolicitud = Estado.Aceptado;
        }
        else if (estado == "rechazar")
        {
            contrato.RespuestaDesolicitud = Estado.Rechazado;
        }

        _context.SaveChanges();
        return Json(new { success = true, mensaje = "Estado actualizado correctamente." });
    }

    return Json(new { success = false, mensaje = "Contrato no encontrado." });
}


















public JsonResult ListadoTrabajosPostulados()
{
    // Obtener el usuario logueado
    var usuarioLogueado = _userManager.GetUserAsync(HttpContext.User).Result;
    var correoUsuarioLogueado = usuarioLogueado?.Email;

    // Obtener la persona asociada al usuario logueado
    var personaLogueada = _context.Personas.FirstOrDefault(p => p.Email == correoUsuarioLogueado);
    var personaIDLogueada = personaLogueada?.PersonaID;

    if (personaIDLogueada == null)
    {
        return Json(new { success = false, mensaje = "No se encontró una persona asociada al usuario logueado." });
    }

    // Obtener los trabajos donde la persona logueada ha postulado a los servicios de otras personas
    var trabajosPostulados = _context.ContratoRespondidos
        .Include(c => c.Trabajo) // Incluir datos del trabajo
        .Include(c => c.Servicio) // Incluir datos del servicio
        .ThenInclude(s => s.Persona) // Incluir la persona que ofrece el servicio
        .Where(c => c.Trabajo.PersonaID == personaIDLogueada) // Solo los trabajos donde la persona logueada ha postulado a servicios de otros
        .ToList();

    // Crear una lista de objetos anónimos con la información relevante de los trabajos postulados
    var trabajosConRespuestas = trabajosPostulados.Select(c => new 
    {
        TrabajoID = c.TrabajoID,
        DescripcionTrabajo = c.Trabajo.Descripcion,
        ComentarioTrabajo = c.Trabajo.Comentario,
        ServicioID = c.ServicioID,
        NombrePersonaServicio = c.Servicio.Persona.Nombre, // Persona que ofrece el servicio
        ApellidoPersonaServicio = c.Servicio.Persona.Apellido, // Apellido de la persona que ofrece el servicio
        EmailPersonaServicio = c.Servicio.Persona.Email, // Email de la persona que ofrece el servicio
        Respuesta = c.Respuesta ? "Aceptado" : "Rechazado", // Estado de la respuesta
        EstadoSolicitud = c.RespuestaDesolicitud.ToString(), // Estado de la solicitud
        FechaPostulacion = c.FechaMatch.ToString("dd/MM/yyyy") // Fecha de postulación
    }).ToList();

    return Json(trabajosConRespuestas);
}


}


}












   


    
    

    





















