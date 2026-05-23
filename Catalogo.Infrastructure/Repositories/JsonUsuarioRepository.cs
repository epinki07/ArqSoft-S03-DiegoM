using System.Text.Json;
using Catalogo.Domain.Interfaces;
using Catalogo.Domain.Models;

namespace Catalogo.Infrastructure.Repositories
{
    public class JsonUsuarioRepository : IUsuarioRepository
    {
        private readonly string _filePath;

        public JsonUsuarioRepository(string filePath)
        {
            _filePath = filePath;

            // Si la carpeta no existe, crearla
            var carpeta = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(carpeta))
                Directory.CreateDirectory(carpeta);

            // Si el archivo no existe, crearlo con una lista vacía
            if (!File.Exists(_filePath))
            {
                var usuariosVacios = new List<Usuario>();
                GuardarUsuarios(usuariosVacios);
            }
        }

     
        public Usuario? ObtenerPorNombreUsuario(string nombreUsuario)
        {
            var usuarios = ObtenerTodos();
            return usuarios.FirstOrDefault(u => u.UniqueNombreUsuario == nombreUsuario);
        }

        public Usuario? ObtenerPorEmail(string email)
        {
            var usuarios = ObtenerTodos();
            return usuarios.FirstOrDefault(u => u.UniqueEmail == email);
        }

        public List<Usuario> ObtenerTodos()
        {
            if (!File.Exists(_filePath))
                return new List<Usuario>();

            try
            {
                var json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<Usuario>>(json) 
                    ?? new List<Usuario>();
            }
            catch (Exception)
            {
                return new List<Usuario>();
            }
        }

        public bool Registrar(Usuario usuario)
        {
            try
            {
                var usuarios = ObtenerTodos();

                // Generar nuevo ID (el mayor ID + 1)
                int nuevoId = usuarios.Any() ? usuarios.Max(u => u.Id) + 1 : 1;
                usuario.Id = nuevoId;

                usuarios.Add(usuario);
                GuardarUsuarios(usuarios);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    
        public Usuario? Autenticar(string nombreUsuario, string passwordHash)
        {
            var usuario = ObtenerPorNombreUsuario(nombreUsuario);

            if (usuario == null)
                return null;

            // El UsuarioService hace la verificación de BCrypt
            // Esta clase solo devuelve el usuario si existe
            return usuario;
        }


        public Usuario? ObtenerPorId(int id)
        {
            var usuarios = ObtenerTodos();
            return usuarios.FirstOrDefault(u => u.Id == id);
        }


        public bool Eliminar(int id)
        {
            try
            {
                var usuarios = ObtenerTodos();
                var usuario = usuarios.FirstOrDefault(u => u.Id == id);

                if (usuario == null)
                    return false;

                usuarios.Remove(usuario);
                GuardarUsuarios(usuarios);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void GuardarUsuarios(List<Usuario> usuarios)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(usuarios, options);
                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al guardar usuarios: {ex.Message}", ex);
            }
        }
    }
}
