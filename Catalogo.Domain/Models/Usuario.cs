using System;
using System.Collections.Generic;
using System.Text;

namespace Catalogo.Domain.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string UniqueNombreUsuario { get; set; } = string.Empty;
        public string UniqueEmail { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}
