using Catalogo.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Catalogo.Application.Services;

namespace Catalogo.Presentation.Controllers
{
    public class AuthController : Controller
    {
        private readonly UsuarioService _usuarioService;

        // El servicio llega por inyección de dependencias
        public AuthController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // GET: Mostrar formulario de Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Procesar Login
        [HttpPost]
        public IActionResult Login(string usuario, string password)
        {
            // Validar que los campos no estén vacíos
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "El usuario y contraseña son requeridos.");
                return View();
            }

            // Autenticar usuario
            var (usuarioAutenticado, mensaje) = _usuarioService.Autenticar(usuario, password);

            if (usuarioAutenticado != null)
            {
                // Guardar en sesión
                HttpContext.Session.SetString("usuarioNombre", usuarioAutenticado.UniqueNombreUsuario);
                HttpContext.Session.SetString("usuarioId", usuarioAutenticado.Id.ToString());

                // Redirigir al catálogo
                return RedirectToAction("Index", "Catalogo");
            }
            else
            {
                ModelState.AddModelError("", mensaje);
                return View();
            }
        }

        // GET: Mostrar formulario de Registro
        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        // POST: Procesar Registro
        [HttpPost]
        public IActionResult Registro(string usuario, string email, string password, string passwordConfirm)
        {
            // Validar que las contraseñas coincidan
            if (password != passwordConfirm)
            {
                ModelState.AddModelError("", "Las contraseñas no coinciden.");
                return View();
            }

            // Registrar usuario
            var (exito, mensaje) = _usuarioService.Registrar(usuario, email, password);

            if (exito)
            {
                // Redirigir a login con mensaje de éxito
                TempData["Mensaje"] = "Usuario registrado correctamente. Por favor, inicia sesión.";
                return RedirectToAction("Login");
            }
            else
            {
                ModelState.AddModelError("", mensaje);
                return View();
            }
        }

        // GET: Cerrar sesión
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
