using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Postulate.Data;
using Postulate.Models;

namespace Postulate.Controllers
{
    public class ServiciosController : Controller
    {
        private readonly ILogger<ServiciosController> _logger;
        private readonly ApplicationDbContext _context;

        private readonly UserManager<IdentityUser> _userManager; // correo servicio

        public ServiciosController(ILogger<ServiciosController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {

            // recuperar el correo

            var usuarioLogueado = _userManager.GetUserAsync(HttpContext.User).Result;
            var correoUsuarioLogueado = usuarioLogueado?.Email;


            // Obtener la persona asociada al correo del usuario logueado

            var personaLogueada = _context.Personas.FirstOrDefault(p => p.Email == correoUsuarioLogueado);
            var personaIDLogueada = personaLogueada?.PersonaID;
            var nombrePersonaLogueada = personaLogueada?.Nombre;

        if (personaLogueada == null)
    {


        
        return RedirectToAction("Error", "Home"); 
    }

    // Si la persona está desactivada (Eliminado == true)
    if (personaLogueada.Eliminado)
    {
        // con esto redirigimos a una vista que todavia no esta realizada 
        ViewBag.Mensaje = "Tu cuenta está desactivada. No tienes acceso a los servicios.";
        return View("CuentaDesactivada");
    }

            var profesiones = _context.Profesiones.ToList();
            profesiones.Add(new Profesion { ProfesionID = 0, Nombre = "[SELECCIONE...]" });

            ViewBag.ProfesionID = new SelectList(profesiones.OrderBy(c => c.Nombre), "ProfesionID", "Nombre");
            ViewBag.ProfesionBuscarID = new SelectList(profesiones.OrderBy(c => c.Nombre), "ProfesionID", "Nombre");

            var personas = _context.Personas.ToList();
            personas.Add(new Persona { PersonaID = 0, Nombre = "[SELECCIONE...]" });

            ViewBag.PersonaID = new SelectList(personas.OrderBy(c => c.Nombre), "PersonaID", "Nombre");
            ViewBag.BuscarPersonaID = new SelectList(personas.OrderBy(c => c.Nombre), "PersonaID", "Nombre");

            // Pasar el ID y nombre de la persona logueada a la vista
            ViewBag.PersonaIDLogueada = personaIDLogueada;
            ViewBag.NombrePersonaLogueada = nombrePersonaLogueada;



            return View("Servicio");
        }



        public JsonResult CardServicios(int? servicioID, string NombreProfesion)
        {
            var usuarioLogueado = _userManager.GetUserAsync(HttpContext.User).Result;
            var correoUsuarioLogueado = usuarioLogueado?.Email;

            List<VistaTipoProfesion> tiposProfesionMostrar = new List<VistaTipoProfesion>();

            var servicios = _context.Servicios
                .Include(t => t.Persona)
                .Include(t => t.Profesion)
                .Where(s => s.Persona.Email == correoUsuarioLogueado)
                .ToList();

            if (NombreProfesion != null)
            {
                servicios = servicios.Where(s => s.Profesion.Nombre == NombreProfesion).ToList();
            }

            foreach (var servicio in servicios)
            {
                var tipoProfesionMostrar = tiposProfesionMostrar.SingleOrDefault(t => t.ProfesionID == servicio.ProfesionID);
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

                var vistaPersonasServicios = new VistaPersonasServicios
                {
                    NombrePersona = servicio.Persona.Nombre,
                    ApellidoPersona = servicio.Persona.Apellido,
                    TelefonoPersona = servicio.Persona.Telefono,
                    ServicioID = servicio.ServicioID
                };

                tipoProfesionMostrar.ListadoPersonas.Add(vistaPersonasServicios);
            }

            return Json(tiposProfesionMostrar);
        }




        public JsonResult AgregarServicio(int ServicioID, int PersonaID, int ProfesionID, bool herramienta, string? descripcion, string? titulo, string? Institucion)
        {
            var servicioExistente = _context.Servicios.FirstOrDefault(s => s.PersonaID == PersonaID && s.ProfesionID == ProfesionID);

            if (servicioExistente != null)
            {
                if (ServicioID == 0 || servicioExistente.ServicioID != ServicioID)
                {
                    return Json(new { success = false, message = "La combinación de persona y profesión ya existe." });
                }
            }

            string resultado = "";

            if (ServicioID == 0)
            {
                var servicio = new Servicio
                {
                    PersonaID = PersonaID,
                    ProfesionID = ProfesionID,
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

                // Actualizar un servicio existente
                var servicioEditar = _context.Servicios.FirstOrDefault(e => e.ServicioID == ServicioID);
                if (servicioEditar != null)
                {
                    servicioEditar.Herramienta = herramienta;
                    servicioEditar.Descripcion = descripcion;
                    servicioEditar.Titulo = titulo;
                    servicioEditar.Institucion = Institucion;
                    _context.SaveChanges();

                    return Json(new { success = true, message = "Servicio actualizado exitosamente." });
                }
                else
                {
                    return Json(new { success = false, message = "Servicio no encontrado." });
                }
            }

        }



        public JsonResult EliminarServicio(int servicioID)
        {



            var ServicioEliminar = _context.Servicios.Find(servicioID);
            _context.Remove(ServicioEliminar);
            _context.SaveChanges();
            return Json(ServicioEliminar);



        }




        public IActionResult VistaServicio(int id)
        {
            ViewBag.ServicioID = id;
            return View("VistaServicio");
        }




        public JsonResult RecuperarPerfilServicio(int id)
        {

            var servicios = _context.Servicios
                .Include(s => s.Persona)
                .Include(s => s.Profesion)
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