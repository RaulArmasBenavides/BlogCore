using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BlogCore.AccesoDatos.Data.Repository
{
    internal class ArticuloRepository : Repository<Articulo>, IArticuloRepository
    {
        private readonly ApplicationDbContext _db;

        public ArticuloRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(Articulo articulo)
        {
            var objDesdeDb = await _db.Articulo.FirstOrDefaultAsync(s => s.Id == articulo.Id);
            if (objDesdeDb != null)
            {
                objDesdeDb.Nombre = articulo.Nombre;
                objDesdeDb.Descripcion = articulo.Descripcion;
                objDesdeDb.UrlImagen = articulo.UrlImagen;
                objDesdeDb.CategoriaId = articulo.CategoriaId;
            }
        }
    }
}
