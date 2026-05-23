using CatalogoApp.Application.Services;
using CatalogoApp.Domain.Models;
using Catalogo.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CatalogoApp.Presentation.Controllers
{
    public class CatalogoController : Controller
    {
        private readonly ItemService _service;
        private readonly ComentarioService _comentarioService;

        // Los servicios llegan por inyección de dependencias
        public CatalogoController(ItemService service, ComentarioService comentarioService)
        {
            _service = service;
            _comentarioService = comentarioService;
        }

        // Lista con filtro opcional por género
        public IActionResult Index(string? genero)
        {
            var items = string.IsNullOrEmpty(genero)
                ? _service.ObtenerTodos()
                : _service.ObtenerPorGenero(genero);

            ViewBag.Generos = _service.ObtenerGeneros();
            ViewBag.GeneroActual = genero;

            return View(items);
        }

        // Detalle de un item
        public IActionResult Detalle(int id)
        {
            var item = _service.ObtenerPorId(id);
            if (item == null)
                return NotFound();

            // Obtener comentarios del item
            item.Comentarios = _comentarioService.ObtenerComentariosDelItem(id);

            // Obtener promedio de rating
            ViewBag.PromedioRating = _comentarioService.ObtenerPromedioRating(id);
            ViewBag.CantidadComentarios = _comentarioService.ObtenerCantidadComentarios(id);

            return View(item);
        }

        // Formulario — GET
        public IActionResult Agregar()
        {
            return View();
        }

        // Formulario — POST
        [HttpPost]
        public IActionResult Agregar(Item item)
        {
            _service.Agregar(item);
            return RedirectToAction("Index");
        }

        // Eliminar
        public IActionResult Eliminar(int id)
        {
            _service.Eliminar(id);
            return RedirectToAction("Index");
        }

        // POST: Agregar comentario
        [HttpPost]
        public IActionResult AgregarComentario(int itemId, string texto, int rating)
        {
            // Verificar si usuario está logueado
            var usuarioNombre = HttpContext.Session.GetString("usuarioNombre");

            if (string.IsNullOrEmpty(usuarioNombre))
            {
                TempData["Error"] = "Debes estar logueado para comentar.";
                return RedirectToAction("Detalle", new { id = itemId });
            }

            // Validar rating
            if (rating < 1 || rating > 5)
            {
                TempData["Error"] = "El rating debe estar entre 1 y 5 estrellas.";
                return RedirectToAction("Detalle", new { id = itemId });
            }

            // Agregar comentario
            var (exito, mensaje) = _comentarioService.AgregarComentario(itemId, usuarioNombre, texto, rating);

            if (exito)
            {
                TempData["Exito"] = "Comentario agregado correctamente.";
            }
            else
            {
                TempData["Error"] = mensaje;
            }

            return RedirectToAction("Detalle", new { id = itemId });
        }
    }
}