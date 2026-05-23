using System;
using System.Collections.Generic;
using System.Text;
using Catalogo.Domain.Interfaces;
using Catalogo.Domain.Models;

namespace Catalogo.Application.Services
{
    public class ComentarioService
    {
        private readonly IComentarioRepository _repo;

        // El servicio recibe el repositorio por inyección de dependencias
        public ComentarioService(IComentarioRepository repo)
        {
            _repo = repo;
        }


        public List<Comentario> ObtenerComentariosDelItem(int itemId)
        {
            return _repo.ObtenerPorItemId(itemId);
        }

    
      
        public (bool exito, string mensaje) AgregarComentario(int itemId, string nombreUsuario, string texto, int rating)
        {
            // Validar que los campos no estén vacíos
            if (itemId <= 0)
                return (false, "El ID del álbum es inválido.");

            if (string.IsNullOrWhiteSpace(nombreUsuario))
                return (false, "El nombre de usuario es requerido.");

            if (string.IsNullOrWhiteSpace(texto))
                return (false, "El comentario no puede estar vacío.");

            // Validar longitud del comentario
            if (texto.Length < 5 || texto.Length > 500)
                return (false, "El comentario debe tener entre 5 y 500 caracteres.");

            // Validar rating
            if (rating < 1 || rating > 5)
                return (false, "El rating debe estar entre 1 y 5 estrellas.");

            try
            {
                var nuevoComentario = new Comentario
                {
                    ItemId = itemId,
                    NombreUsuario = nombreUsuario,
                    Texto = texto,
                    Rating = rating,
                    Fecha = DateTime.Now
                };

                bool resultado = _repo.Agregar(nuevoComentario);

                if (resultado)
                    return (true, "Comentario agregado correctamente.");
                else
                    return (false, "Error al guardar el comentario.");
            }
            catch (Exception ex)
            {
                return (false, $"Error en el servidor: {ex.Message}");
            }
        }

        public double ObtenerPromedioRating(int itemId)
        {
            return _repo.ObtenerPromedioRating(itemId);
        }

        public int ObtenerCantidadComentarios(int itemId)
        {
            var comentarios = _repo.ObtenerPorItemId(itemId);
            return comentarios.Count;
        }

        public bool EliminarComentario(int id)
        {
            return _repo.Eliminar(id);
        }


        public Comentario? ObtenerComentario(int id)
        {
            return _repo.ObtenerPorId(id);
        }
    }
}
