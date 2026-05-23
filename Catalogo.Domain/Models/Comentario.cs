using System;

namespace Catalogo.Domain.Models
{
    public class Comentario
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string Texto { get; set; } = string.Empty;
        public int Rating { get; set; } // 1-5 estrellas
        public DateTime Fecha { get; set; } = DateTime.Now;
    }
}
