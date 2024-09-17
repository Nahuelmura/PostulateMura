using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Postulate.Data;
using Postulate.Models;

namespace Postulate.Controllers
{
    public class VistaServicioController : Controller
    {
        private readonly ILogger<VistaServicioController> _logger;
        private readonly ApplicationDbContext _context;

        public VistaServicioController(ILogger<VistaServicioController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var profesiones = _context.Profesiones.ToList();
            profesiones.Add(new Profesion { ProfesionID = 0, Nombre = "[SELECCIONE...]" });

            ViewBag.ProfesionID = new SelectList(profesiones.OrderBy(c => c.Nombre), "ProfesionID", "Nombre");
            ViewBag.ProfesionBuscarID = new SelectList(profesiones.OrderBy(c => c.Nombre), "ProfesionID", "Nombre");

          var personas = _context.Personas.ToList();
            personas.Add(new Persona { PersonaID = 0, Nombre = "[SELECCIONE...]" });

            ViewBag.PersonaID = new SelectList(personas.OrderBy(c => c.Nombre), "PersonaID", "Nombre");
            ViewBag.BuscarPersonaID = new SelectList(personas.OrderBy(c => c.Nombre), "PersonaID", "Nombre");


            // filtro 

            profesiones.Add(new Profesion { ProfesionID = 0, Nombre = "[Busca la profesion interesada ...]" });
            ViewBag.profesionBuscarID = new SelectList(profesiones.OrderBy(c => c.Nombre), "ProfesionID", "Nombre");



          
            return View("VistaServicio");
        }

        
public JsonResult CardServicios(int? servicioID, int? ProfesionID)
{
    // Lista para almacenar las profesiones y sus servicios correspondientes
    List<VistaTipoProfesion> tiposProfesionMostrar = new List<VistaTipoProfesion>();

    // Obtener todos los servicios incluyendo la información de la persona y la profesión asociada
    var servicios = _context.Servicios
        .Include(t => t.Persona)
        .Include(t => t.Profesion)
        .ToList();

    // Filtrar por profesión 
 if(ProfesionID != null && ProfesionID != 0) 
    {
        servicios = servicios.Where(s => s.ProfesionID == ProfesionID).ToList();
    }
    // Organizar los servicios por profesión
    foreach (var servicio in servicios)
    {
        // Buscar si la profesión ya está en la lista
        var tipoProfesionMostrar = tiposProfesionMostrar.SingleOrDefault(t => t.ProfesionID == servicio.ProfesionID);
        
        // Si no está, agregarla a la lista
        if (tipoProfesionMostrar == null)
        {
            tipoProfesionMostrar = new VistaTipoProfesion
            {
                ProfesionID = servicio.ProfesionID,
                Nombre = servicio.Profesion.Nombre,
                ListadoPersonas = new List<VistaPersonasServicios>()
            };
            tiposProfesionMostrar.Add(tipoProfesionMostrar);
        }

        // Crear la entrada para la persona y agregarla a la profesión correspondiente
        var vistaPersonasServicios = new VistaPersonasServicios
        {
            NombrePersona = servicio.Persona.Nombre,
            ApellidoPersona = servicio.Persona.Apellido,
            TelefonoPersona = servicio.Persona.Telefono,
            ServicioID = servicio.ServicioID
        };

        tipoProfesionMostrar.ListadoPersonas.Add(vistaPersonasServicios);
    }

    // Retornar la lista de profesiones con sus respectivos servicios/personas
    return Json(tiposProfesionMostrar);
}

        

       

    
   public JsonResult AgregarServicio(int id,int ServicioID, int PersonaID, int ProfesionID, int? ImagenID, string? nombrePersona, string? nombreProfesion, int? telefonoPersona, bool herramienta, string? descripcion, string? titulo, string? Institucion)
        {
               var servicioExistente = _context.Servicios.FirstOrDefault(s => s.PersonaID == PersonaID && s.ProfesionID == ProfesionID);

    if (servicioExistente != null)
    {
   
        if (ServicioID == 0 || servicioExistente.ServicioID != ServicioID)
        {
            return Json(new { success = false, message = "La combinación de persona y profesión ya existe." });
        }
    }



           string resultado ="";

           if ( ServicioID ==0 )
           {

           
            var servicio = new Servicio
            {
                ServicioID = ServicioID,
                PersonaID = PersonaID,
                ProfesionID = ProfesionID,
                //  ImagenID = ImagenID,
                
                Herramienta = herramienta,
                Descripcion = descripcion,
                Titulo = titulo,
                Institucion = Institucion
            };

            _context.Add(servicio);
            _context.SaveChanges();

            return Json(new { success = true, message = "Servicio guardado exitosamente." });
            }
            else
            {
                
        // Actualizar un servicio
        var servicioEditar = _context.Servicios.FirstOrDefault(e => e.ServicioID == ServicioID);
             if (servicioEditar != null)
        {
            servicioEditar.PersonaID = PersonaID;
            servicioEditar.ProfesionID = ProfesionID;
            // servicioEditar.ImagenID = ImagenID;
            servicioEditar.Herramienta = herramienta;
            servicioEditar.Descripcion = descripcion;
            servicioEditar.Titulo = titulo;
            servicioEditar.Institucion = Institucion;

            _context.Update(servicioEditar);
            _context.SaveChanges();

            return Json(new { success = true, message = "Servicio actualizado exitosamente." });
        }
        else
        {
            return Json(new { success = false, message = "Servicio no encontrado." });
        }
        }
        

        
        
    }
    
//     public JsonResult ObtenerServicio(int id)
// {
//     var servicio = _context.Servicios.FirstOrDefault(s => s.ServicioID == id);
//     if (servicio == null)
//     {
//         return Json(new { success = false, message = "Servicio no encontrado." });
//     }

//     return Json(new { success = true, servicio });
// }
// }


public JsonResult  EliminarServicio (int servicioID)
 {

      

     var ServicioEliminar = _context.Servicios.Find(servicioID);
    _context.Remove(ServicioEliminar);
    _context.SaveChanges();
     return Json(ServicioEliminar);


   
 }
 
 
 

  


public JsonResult RecuperarPerfilServicio(int id)
{
    
    var servicios = _context.Servicios
        .Include(s => s.Persona)
        .Include(s => s.Profesion) 
        // .Include(s => s.Imagen)
        .ToList();

    
    if (id > 0)
    {
        servicios = servicios.Where(p => p.ServicioID == id).ToList();
    }

    var perfilMostrar = servicios.Select(p => new VistaServicio
    {
        ServicioID = p.ServicioID,
        PersonaID = p.PersonaID,
        ProfesionID = p.ProfesionID,
        // ImagenID = p.ImagenID,
        NombrePersona = p.Persona.Nombre,
        ApellidoPersona = p.Persona.Apellido,
        TelefonoPersona = p.Persona.Telefono,
        EdadPersona = p.Persona.Edad,
        DocumentoPersona = p.Persona.Documento,
        NombreProfesion = p.Profesion.Nombre,
        Herramienta = p.Herramienta,
        Descripcion = p.Descripcion,
        Titulo = p.Titulo,
        Institucion = p.Institucion,
        EmailPersona = p.Persona.Email,
    }).ToList();

    return Json(perfilMostrar);
}


    

    
}

}