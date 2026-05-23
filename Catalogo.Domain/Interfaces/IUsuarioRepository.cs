using System;
using System.Collections.Generic;
using System.Text;
using Catalogo.Domain.Models;

namespace Catalogo.Domain.Interfaces
{
    public interface IUsuarioRepository
    {

        Usuario? ObtenerPorNombreUsuario(string nombreUsuario);

        Usuario? ObtenerPorEmail(string email);

        List<Usuario> ObtenerTodos();

        bool Registrar(Usuario usuario);

        Usuario? Autenticar(string nombreUsuario, string passwordHash);

        bool Eliminar(int id);

        Usuario? ObtenerPorId(int id);
    }
}
