using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BlogCore.AccesoDatos.Data.Repository
{
    internal class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoriaRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<IEnumerable<SelectListItem>> GetListaCategoriasAsync()
        {
            return await _db.Categoria.Select(i => new SelectListItem()
                {
                    Text = i.Nombre,
                    Value = i.Id.ToString()
                }
            ).ToListAsync();
        }

        public async Task UpdateAsync(Categoria categoria)
        {
            var objDesdeDb = await _db.Categoria.FirstOrDefaultAsync(s => s.Id == categoria.Id);
            if (objDesdeDb != null)
            {
                objDesdeDb.Nombre = categoria.Nombre;
                objDesdeDb.Orden = categoria.Orden;
            }
        }
    }
}
