using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Postulate.Data;
using Postulate.Models;

namespace Postulate.Controllers;

public class LocalidadController : Controller
{
    private readonly ILogger<LocalidadController> _logger;

    private readonly ApplicationDbContext _context;

    public LocalidadController(ILogger<LocalidadController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()

    {
        // Obtener la lista de provincias de la base de datos
        var provincias = _context.Provincias.ToList();
        var provinciasBuscar = provincias.ToList();

        // Agregar opciones de selección predeterminadas
        provincias.Add(new Provincia { ProvinciaID = 0, Nombre = "[SELECCIONE...]" });
        provinciasBuscar.Add(new Provincia { ProvinciaID = 0, Nombre = "[TODAS LAS PROVINCIAS]" });

        // Asignar las listas de selección al ViewBag con las claves correctas
        ViewBag.ProvinciaID = new SelectList(provincias.OrderBy(c => c.Nombre), "ProvinciaID", "Nombre");
        ViewBag.ProvinciaBuscarID = new SelectList(provinciasBuscar.OrderBy(c => c.Nombre), "ProvinciaID", "Nombre");


        return View("Localidad");
    }

    public JsonResult ListadoLocalidad(int? id,int? ProvinciaBuscarID)
    {
        var localidad = _context.Localidades.Include(t => t.Provincia).ToList();
          if (id != null)
        {
            localidad = localidad.Where(t => t.LocalidadID == id).ToList();
        }
        if (ProvinciaBuscarID != null && ProvinciaBuscarID !=0 ) 
        {
            localidad = localidad.Where(t => t.ProvinciaID == ProvinciaBuscarID).ToList(); 
        }

        var localidadMostrar = localidad.Select ( e => new VistaLocalidades 
        {
            LocalidadID = e.LocalidadID,
            ProvinciaID = e.ProvinciaID,
            ProvinciaNombre = e.Provincia.Nombre,
            Nombre = e.Nombre,
         
            CodigoPostal = e.CodigoPostal,
            

        }).ToList();

        return Json(localidadMostrar);

    }


    public IActionResult GuardarLocalidad(int provinciaID, int localidadID, string nombre, int codigoPostal)
    {
        string resultado = "";
    
        if (!String.IsNullOrEmpty(nombre))
        {
            if (localidadID == 0)
            {
                var existeLocalidad = _context.Localidades.Where(t => t.Nombre == nombre).Count();
                if (existeLocalidad == 0)
                {

                    var nuevaLocalidad = new Localidad
                    {
                        ProvinciaID = provinciaID,
                        Nombre = nombre.ToUpper(),
                        CodigoPostal = codigoPostal,
                    };
                    _context.Localidades.Add(nuevaLocalidad);
                    _context.SaveChanges();


                }
                else
                {
                    resultado = "ya existe un registro con ese nombre ";

                }
            }

            else
            {
                var LocalidadEditar = _context.Localidades.Where(t => t.LocalidadID == localidadID).SingleOrDefault();
                if (LocalidadEditar != null)
                {
                    var existeLocalidad = _context.Localidades.Where(t => t.Nombre == nombre && t.LocalidadID != localidadID).Count();

                    if (existeLocalidad == 0)
                    {
                        LocalidadEditar.ProvinciaID = provinciaID;
                        LocalidadEditar.Nombre = nombre.ToUpper();
                        LocalidadEditar.CodigoPostal = codigoPostal;
                        _context.SaveChanges();
                    }
                    else
                    {
                        resultado = "ya existe ";
                    }



                }
                else
                {
                    resultado = "DEBE INGRESAR UNA NOMBRE";
                }
            }
        }
        return Json(resultado);

    }

     public JsonResult  EliminarLocalidad (int localidadID)
 {

      

     var localidadEliminar = _context.Localidades.Find(localidadID);
    _context.Remove(localidadEliminar);
    
    _context.SaveChanges();
     return Json(true);


   
 }






}
