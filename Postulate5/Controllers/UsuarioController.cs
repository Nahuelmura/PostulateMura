using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Postulate.Data;
using Postulate.Models;

namespace Postulate.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly ILogger<UsuarioController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _rolManager;

        public UsuarioController(ILogger<UsuarioController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> rolManager)
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

            await InicializarPermisosUsuario();

            return View();
        }

        private async Task InicializarPermisosUsuario()
        {
            // Crear rol "Usuario_Registrado" si no existe
            var nombreRolCrearExiste = _context.Roles.SingleOrDefault(r => r.Name == "Usuario_Registrado");
            if (nombreRolCrearExiste == null)
            {
                var roleResult = await _rolManager.CreateAsync(new IdentityRole("Usuario_Registrado"));
                if (!roleResult.Succeeded)
                {
                    _logger.LogError("Error al crear el rol Usuario_Registrado");
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
