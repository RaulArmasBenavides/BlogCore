using BlogCore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Web;

namespace BlogCore.AccesoDatos.Data.Repository.IRepository
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        Task<IEnumerable<SelectListItem>> GetListaCategoriasAsync();

        Task UpdateAsync(Categoria categoria);
    }
}
