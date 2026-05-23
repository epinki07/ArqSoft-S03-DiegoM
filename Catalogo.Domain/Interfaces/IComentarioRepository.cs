using System;
using System.Collections.Generic;
using System.Text;
using Catalogo.Domain.Models;

namespace Catalogo.Domain.Interfaces
{
    public interface IComentarioRepository
    {
    
        List<Comentario> ObtenerPorItemId(int itemId);

        List<Comentario> ObtenerTodos();

     
        bool Agregar(Comentario comentario);

     
        bool Eliminar(int id);

     
        Comentario? ObtenerPorId(int id);

    
        double ObtenerPromedioRating(int itemId);
    }
}
