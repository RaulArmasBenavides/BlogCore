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
    internal class SliderRepository : Repository<Slider>, ISliderRepository
    {
        private readonly ApplicationDbContext _db;

        public SliderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(Slider slider)
        {
            var objDesdeDb = await _db.Slider.FirstOrDefaultAsync(s => s.Id == slider.Id);
            if (objDesdeDb != null)
            {
                objDesdeDb.Nombre = slider.Nombre;
                objDesdeDb.Estado = slider.Estado;
                objDesdeDb.UrlImagen = slider.UrlImagen;
            }
        }
    }
}
