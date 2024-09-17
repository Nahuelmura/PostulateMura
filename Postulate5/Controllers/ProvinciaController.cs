using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Postulate.Data;
using Postulate.Models;

namespace Postulate.Controllers;

public class ProvinciaController : Controller
{
    private readonly ILogger<ProvinciaController> _logger;
    
        private readonly ApplicationDbContext _context;

    public ProvinciaController(ILogger<ProvinciaController> logger,ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View("Provincia");
    }

       public JsonResult  ListadoProvincia( int? id)
        {
            var provincia = _context.Provincias.ToList();

            if (id != null){
                provincia = provincia.Where(t => t.ProvinciaID == id).ToList();
            }

            return Json(provincia);
            
        }



           public IActionResult GuardarProvincia(int provinciaID, string nombre)
{
    string resultado = "";
    // if (nombre != null)
    if (!String.IsNullOrEmpty(nombre))
    {
        if(provinciaID == 0)
        {
            var existeProvincia = _context.Provincias.Where(t => t.Nombre == nombre).Count();
            if (existeProvincia == 0)
            {

                var nuevaProvincia = new Provincia
        {
            Nombre = nombre.ToUpper()
        }; 
         _context.Provincias.Add(nuevaProvincia);
        _context.SaveChanges();

            
            }
            else
            {
                resultado = "ya existe un registro con ese nombre ";

            }
    }
 
    else
    {
        var ProvinciaEditar = _context.Provincias.Where(t => t.ProvinciaID == provinciaID).SingleOrDefault();
        if (ProvinciaEditar != null)
        {
            var existeProvincia = _context.Provincias.Where( t => t.Nombre == nombre && t.ProvinciaID != provinciaID ).Count();

            if (existeProvincia == 0)
            {
                 ProvinciaEditar.Nombre = nombre.ToUpper();
            _context.SaveChanges();
            }
            else
            {
                resultado = "ya existe ";
            }
           
        

        }
        else
        {
            resultado ="DEBE INGRESAR UNA NOMBRE";
        }
    }
}
return Json(resultado);

}


 public JsonResult  EliminarProvincia (int provinciaID)
 {

      

     var ProvinciaEliminar = _context.Provincias.Find(provinciaID);
    _context.Remove(ProvinciaEliminar);
    _context.SaveChanges();
     return Json(true);


   
 }

    

 }