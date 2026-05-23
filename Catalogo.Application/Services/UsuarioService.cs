using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Catalogo.Domain.Interfaces;
using Catalogo.Domain.Models;

namespace Catalogo.Application.Services
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _repo;

        // El servicio recibe el repositorio por inyección de dependencias
        public UsuarioService(IUsuarioRepository repo)
        {
            _repo = repo;
        }

     
        public (bool exito, string mensaje) Registrar(string nombreUsuario, string email, string password)
        {
            // Validar que no estén vacíos
            if (string.IsNullOrWhiteSpace(nombreUsuario))
                return (false, "El nombre de usuario no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(email))
                return (false, "El email no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(password))
                return (false, "La contraseña no puede estar vacía.");

            // Validar longitud del nombre de usuario
            if (nombreUsuario.Length < 3 || nombreUsuario.Length > 50)
                return (false, "El nombre de usuario debe tener entre 3 y 50 caracteres.");

            // Validar que el nombre de usuario sea alfanumérico
            if (!Regex.IsMatch(nombreUsuario, @"^[a-zA-Z0-9_-]+$"))
                return (false, "El nombre de usuario solo puede contener letras, números, guiones y guiones bajos.");

            // Validar formato de email
            if (!Regex.IsMatch(email, @"^[^\s@]+@[^\s@]+\.[^\s@]+$"))
                return (false, "El formato del email no es válido.");

            // Validar longitud de contraseña
            if (password.Length < 6)
                return (false, "La contraseña debe tener al menos 6 caracteres.");

            // Verificar si el usuario ya existe
            if (_repo.ObtenerPorNombreUsuario(nombreUsuario) != null)
                return (false, "El nombre de usuario ya está registrado.");

            // Verificar si el email ya existe
            if (_repo.ObtenerPorEmail(email) != null)
                return (false, "El email ya está registrado.");

            try
            {
                // Encriptar la contraseña usando BCrypt
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

                // Crear nuevo usuario
                var nuevoUsuario = new Usuario
                {
                    UniqueNombreUsuario = nombreUsuario,
                    UniqueEmail = email,
                    PasswordHash = passwordHash,
                    FechaRegistro = DateTime.Now
                };

                // Registrar en el repositorio
                bool resultado = _repo.Registrar(nuevoUsuario);

                if (resultado)
                    return (true, "Usuario registrado correctamente.");
                else
                    return (false, "Error al registrar el usuario.");
            }
            catch (Exception ex)
            {
                return (false, $"Error en el registro: {ex.Message}");
            }
        }

        public (Usuario? usuario, string mensaje) Autenticar(string nombreUsuario, string password)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario))
                return (null, "El nombre de usuario es requerido.");

            if (string.IsNullOrWhiteSpace(password))
                return (null, "La contraseña es requerida.");

            try
            {
                // Buscar usuario por nombre
                var usuario = _repo.ObtenerPorNombreUsuario(nombreUsuario);

                if (usuario == null)
                    return (null, "Usuario o contraseña incorrectos.");

                // Verificar contraseña usando BCrypt
                bool passwordValida = BCrypt.Net.BCrypt.Verify(password, usuario.PasswordHash);

                if (passwordValida)
                    return (usuario, "Autenticación exitosa.");
                else
                    return (null, "Usuario o contraseña incorrectos.");
            }
            catch (Exception ex)
            {
                return (null, $"Error en la autenticación: {ex.Message}");
            }
        }

        public bool UsuarioExiste(string nombreUsuario)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario))
                return false;

            return _repo.ObtenerPorNombreUsuario(nombreUsuario) != null;
        }


        public bool EmailExiste(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return _repo.ObtenerPorEmail(email) != null;
        }


        public Usuario? ObtenerPorId(int id)
        {
            return _repo.ObtenerPorId(id);
        }


        public List<Usuario> ObtenerTodos()
        {
            return _repo.ObtenerTodos();
        }

        public bool Eliminar(int id)
        {
            return _repo.Eliminar(id);
        }
    }
}
