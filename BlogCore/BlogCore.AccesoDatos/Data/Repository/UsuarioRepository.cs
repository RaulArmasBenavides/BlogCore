using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;
using Microsoft.EntityFrameworkCore;


namespace BlogCore.AccesoDatos.Data.Repository
{
    internal class UsuarioRepository : Repository<ApplicationUser>, IUsuarioRepository
    {
        private readonly ApplicationDbContext _db;

        public UsuarioRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task BloquearUsuarioAsync(string IdUsuario)
        {
            var usuarioDesdeDb = await _db.ApplicationUser.FirstOrDefaultAsync(u => u.Id == IdUsuario);
            if (usuarioDesdeDb != null)
            {
                usuarioDesdeDb.LockoutEnd = DateTime.Now.AddYears(1000);
            }
        }

        public async Task DesbloquearUsuarioAsync(string IdUsuario)
        {
            var usuarioDesdeDb = await _db.ApplicationUser.FirstOrDefaultAsync(u => u.Id == IdUsuario);
            if (usuarioDesdeDb != null)
            {
                usuarioDesdeDb.LockoutEnd = DateTime.Now;
            }
        }
    }
}
