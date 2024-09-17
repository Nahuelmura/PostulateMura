using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Postulate.Data;
using Postulate.Models;

namespace Postulate.Controllers;

public class PersonaController : Controller
{
    private readonly ILogger<PersonaController> _logger;

    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager; // obtener correo

    public PersonaController(ILogger<PersonaController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _logger = logger;
        _context = context;

        _userManager = userManager; //tiene que ver con el correo
    }



 public IActionResult Index()
{
    // Obtener el correo del usuario logueado
    var usuarioLogueado = _userManager.GetUserAsync(HttpContext.User).Result;
    var correoUsuarioLogueado = usuarioLogueado?.Email;

    // Buscamos la persona con el correo del usuario logueado
    var persona = _context.Personas.FirstOrDefault(p => p.Email == correoUsuarioLogueado);

  
    if (persona != null)
    {
        return RedirectToAction("VistaPersonaPerfil"); 
    }

   


   
    var provincias = _context.Provincias.ToList();
    var provinciasBuscar = provincias.ToList();

    provincias.Add(new Provincia { ProvinciaID = 0, Nombre = "[SELECCIONE...]" });
    provinciasBuscar.Add(new Provincia { ProvinciaID = 0, Nombre = "[TODAS LAS PROVINCIAS]" });

    ViewBag.ProvinciaID = new SelectList(provincias.OrderBy(c => c.Nombre), "ProvinciaID", "Nombre");
    ViewBag.ProvinciaBuscarID = new SelectList(provinciasBuscar.OrderBy(c => c.Nombre), "ProvinciaID", "Nombre");

    var localidades = _context.Localidades.ToList();
    var localidadesBuscar = localidades.ToList();

    localidades.Add(new Localidad { LocalidadID = 0, Nombre = "[SELECCIONE...]" });
    localidadesBuscar.Add(new Localidad { LocalidadID = 0, Nombre = "[TODAS LAS LOCALIDADES]" });

    ViewBag.LocalidadID = new SelectList(localidades.OrderBy(c => c.Nombre), "LocalidadID", "Nombre");
    ViewBag.LocalidadBuscarID = new SelectList(localidadesBuscar.OrderBy(c => c.Nombre), "LocalidadID", "Nombre");

    ViewBag.CorreoUsuarioLogueado = correoUsuarioLogueado;

    return View("Persona");
}


    



    // en guardar se guardar los datos del formulario de ingreso de la persona 
    [HttpPost]
    public ActionResult Guardar(int localidadId, int? personaID, string nombre, string apellido, int edad, int telefono, int documento, string? correo)
    {

      
        string resultado = "Error al guardar el formulario";

        // declaro variables para  obtener el correo del usuario logueado 
        var usuarioLogueado = _userManager.GetUserAsync(HttpContext.User).Result;
        var correoUsuarioLogueado = usuarioLogueado?.Email;

        // Validar que el correo del formulario sea el mismo que el del usuario logueado
        if (correo != correoUsuarioLogueado)
        {
            resultado = "El correo debe coincidir con el correo del usuario logueado.";
            return Json(resultado);
        }

        // validacion para comprobar correo ya existe en la base de datos, excepto para la persona actual si se está editando
    var existeCorreo = _context.Personas.Any(p => p.Email == correo && (!personaID.HasValue || p.PersonaID != personaID.Value));

    if (existeCorreo)
    {
        resultado = "El correo ya está registrado en la base de datos.";
        return Json(resultado);
    }
        

        

        if (personaID == 0 || !personaID.HasValue)
        {
            var existeFormulario = _context.Personas.Any(p => p.PersonaID == personaID);
            if (!existeFormulario)
            {
                var Nuevousuario = new Persona
                {
                    LocalidadID = localidadId,
                    Nombre = nombre,
                    Apellido = apellido,
                    Edad = edad,
                    Telefono = telefono,
                    Documento = documento,
                    Email = correo
                };
                _context.Personas.Add(Nuevousuario);
                _context.SaveChanges();
                resultado = "Usuario guardado correctamente";
            }
            else
            {
                resultado = "El formulario ya existe";
            }
        }
       

        return Json(resultado);
    }


    // en esta vista se recupera los datos para crear el listado y luego poder editarlos

    public IActionResult VistaPersona()
    {
        
        var localidades = _context.Localidades.ToList();
        var localidadesBuscar = localidades.ToList();

       
        localidades.Add(new Localidad { LocalidadID = 0, Nombre = "[SELECCIONE...]" });
        localidadesBuscar.Add(new Localidad { LocalidadID = 0, Nombre = "[TODAS LAS LOCALIDADES]" });

    
        ViewBag.LocalidadID = new SelectList(localidades.OrderBy(c => c.Nombre), "LocalidadID", "Nombre");
        ViewBag.LocalidadBuscarID = new SelectList(localidadesBuscar.OrderBy(c => c.Nombre), "LocalidadID", "Nombre");

        return View("VistaPersona");



    }

    // recuperar perfil es para recuperar los datos y posterior mente editarlos en GuardarPerfil
    public JsonResult RecuperarPerfilPersona(int id)
    {
        var personas = _context.Personas
            .Include(p => p.Localidad)
            .ToList();

        if (id > 0)
        {
            personas = personas.Where(p => p.PersonaID == id).ToList();
        }

        var personaMostrar = personas.Select(p => new VistaTraerDatosPersonal
        {
            PersonaID = p.PersonaID,
            NombreLocalidad = p.Localidad.Nombre,
            Nombre = p.Nombre,
            Apellido = p.Apellido,
            Telefono = p.Telefono,
            Edad = p.Edad,
            Documento = p.Documento,
            Email = p.Email,
        }).ToList();

        return Json(personaMostrar);
    }


    // en GuardarPerfil en esta vista esta elaborado para editar desde la vista persona

    public ActionResult GuardarPerfil(int localidadId, int? personaID, string nombre, string apellido, int edad, int telefono, int documento, string? correo)
    {
        string resultado = "Error al guardar el formulario";

        if (personaID == 0 || !personaID.HasValue)
        {
            var Nuevousuario = new Persona
            {
                LocalidadID = localidadId,
                Nombre = nombre,
                Apellido = apellido,
                Edad = edad,
                Telefono = telefono,
                Documento = documento,
                Email = correo
            };
            _context.Personas.Add(Nuevousuario);
            _context.SaveChanges();
            resultado = "Usuario guardado correctamente";
        }
        else
        {
            var personaEditar = _context.Personas.Where(e => e.PersonaID == personaID).SingleOrDefault();
            if (personaEditar != null)
            {
                // personaEditar.LocalidadID = localidadId;
                personaEditar.Nombre = nombre;
                personaEditar.Apellido = apellido;
                personaEditar.Edad = edad;
                personaEditar.Telefono = telefono;
                personaEditar.Documento = documento;

                _context.SaveChanges();
                resultado = "Usuario actualizado correctamente";
            }
        }

        return Json(resultado);
    }



 






    // Vista persona Perfil, es la vista persona Logeada

    public IActionResult VistaPersonaPerfil()
    {

        var usuarioLogueado = _userManager.GetUserAsync(HttpContext.User).Result;
        var correoUsuarioLogueado = usuarioLogueado?.Email;

        // Obtener la persona asociada al correo del usuario logueado
        var personaLogueada = _context.Personas.FirstOrDefault(p => p.Email == correoUsuarioLogueado);
        var personaIDLogueada = personaLogueada?.PersonaID;
        var nombrePersonaLogueada = personaLogueada?.Nombre;




        // Cargar localidades de la base de datos
        var localidades = _context.Localidades.ToList();
        var localidadesBuscar = localidades.ToList();

        // Agregar opciones de selección predeterminadas
        localidades.Add(new Localidad { LocalidadID = 0, Nombre = "[SELECCIONE...]" });
        localidadesBuscar.Add(new Localidad { LocalidadID = 0, Nombre = "[TODAS LAS LOCALIDADES]" });

        // Asignar las listas de selección al ViewBag con las claves correctas
        ViewBag.LocalidadID = new SelectList(localidades.OrderBy(c => c.Nombre), "LocalidadID", "Nombre");
        ViewBag.LocalidadBuscarID = new SelectList(localidadesBuscar.OrderBy(c => c.Nombre), "LocalidadID", "Nombre");

        return View("VistaPersonaPerfil");



    }


    public JsonResult RecuperarPerfilPersonaLogeada(int id)
    {

         var usuarioLogueado = _userManager.GetUserAsync(HttpContext.User).Result;
        var correoUsuarioLogueado = usuarioLogueado?.Email;


        var personas = _context.Personas
            .Include(p => p.Localidad).Where(p => p.Email == correoUsuarioLogueado)
            .ToList();

        if (id > 0)
        {
            personas = personas.Where(p => p.PersonaID == id).ToList();
        }

        var personaMostrar = personas.Select(p => new VistaTraerDatosPersonal
        {
            PersonaID = p.PersonaID,
            NombreLocalidad = p.Localidad.Nombre,
            Nombre = p.Nombre,
            Apellido = p.Apellido,
            Telefono = p.Telefono,
            Edad = p.Edad,
            Documento = p.Documento,
            Email = p.Email,
        }).ToList();

        return Json(personaMostrar);
    }


    // en GuardarPerfil en esta vista esta elaborado para editar desde la vista persona

    public ActionResult GuardarPerfilLogeada(int localidadId, int? personaID, string nombre, string apellido, int edad, int telefono, int documento, string? correo)
    {
        string resultado = "Error al guardar el formulario";

        if (personaID == 0 || !personaID.HasValue)
        {
            var Nuevousuario = new Persona
            {
                LocalidadID = localidadId,
                Nombre = nombre,
                Apellido = apellido,
                Edad = edad,
                Telefono = telefono,
                Documento = documento,
                Email = correo
            };
            _context.Personas.Add(Nuevousuario);
            _context.SaveChanges();
            resultado = "Usuario guardado correctamente";
        }
        else
        {
            var personaEditar = _context.Personas.Where(e => e.PersonaID == personaID).SingleOrDefault();
            if (personaEditar != null)
            {
                personaEditar.LocalidadID = localidadId;
                personaEditar.Nombre = nombre;
                personaEditar.Apellido = apellido;
                personaEditar.Edad = edad;
                personaEditar.Telefono = telefono;
                personaEditar.Documento = documento;
                

                _context.SaveChanges();
                resultado = "Usuario actualizado correctamente";
            }
        }

        return Json(resultado);
    }




}
















