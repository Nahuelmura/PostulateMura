using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Postulate.Data;
using Postulate.Models;

namespace Postulate.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _rolManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> rolManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _rolManager = rolManager;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioLogueadoID = _userManager.GetUserId(HttpContext.User);
            // OBJETO PARA PASARLO A VISTA PARA MOSTRAR QUE FUNCIONA
            ViewBag.UsuarioID = usuarioLogueadoID;

            // Obtener el correo del usuario logueado
            var usuarioLogueado = await _userManager.GetUserAsync(HttpContext.User);
            var correoUsuarioLogueado = usuarioLogueado?.Email;

            // Buscar la persona con el correo del usuario logueado
            var persona = await _context.Personas.FirstOrDefaultAsync(p => p.Email == correoUsuarioLogueado);

            // Obtener el nombre de la persona
            var nombrePersona = persona?.Nombre ?? "Desconocido";

            // Pasar el nombre a ViewBag
            ViewBag.NombrePersona = nombrePersona;

            await InicializarPermisosUsuario();

            return View();
        }

        private async Task InicializarPermisosUsuario()
        {
            // Crear rol "ADMINISTRADOR" si no existe
            var nombreRolCrearExiste = await _rolManager.RoleExistsAsync("ADMINISTRADOR");
            if (!nombreRolCrearExiste)
            {
                var roleResult = await _rolManager.CreateAsync(new IdentityRole("ADMINISTRADOR"));
                if (!roleResult.Succeeded)
                {
                    _logger.LogError("Error al crear el rol ADMINISTRADOR");
                }
            }

            // Crear rol "Usuario_Registrado" si no existe
            var nombreRolExiste = await _rolManager.RoleExistsAsync("Usuario_Registrado");
            if (!nombreRolExiste)
            {
                var roleResult = await _rolManager.CreateAsync(new IdentityRole("Usuario_Registrado"));
                if (!roleResult.Succeeded)
                {
                    _logger.LogError("Error al crear el rol Usuario_Registrado");
                }
            }

            // Crear usuario principal si no existe
            var usuario = await _userManager.FindByEmailAsync("admin@sistema.com");
            if (usuario == null)
            {
                var user = new IdentityUser { UserName = "admin@sistema.com", Email = "admin@sistema.com" };
                var result = await _userManager.CreateAsync(user, "contrasenia");

                if (result.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, "ADMINISTRADOR");
                    if (!roleResult.Succeeded)
                    {
                        _logger.LogError("Error al asignar el rol ADMINISTRADOR al usuario");
                    }
                }
                else
                {
                    _logger.LogError("Error al crear el usuario admin@sistema.com");
                }
            }
        }

        [HttpPost]
        public async Task<JsonResult> GuardarUsuario(string username, string email, string password)
        {
            // Crear el usuario con los datos proporcionados
            var user = new IdentityUser { UserName = username, Email = email };

            // Ejecutar el método para crear el usuario
            var result = await _userManager.CreateAsync(user, password);

            // Si el usuario se crea correctamente, agregarlo al rol "Usuario_Registrado"
            if (result.Succeeded)
            {
                var usuario = await _userManager.FindByEmailAsync(email);
                if (usuario != null)
                {
                    await _userManager.AddToRoleAsync(usuario, "Usuario_Registrado");
                }
            }

            return Json(result.Succeeded);
        }
    }
}
