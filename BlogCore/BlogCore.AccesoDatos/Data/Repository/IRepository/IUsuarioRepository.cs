using BlogCore.Models;

namespace BlogCore.AccesoDatos.Data.Repository.IRepository
{
    public interface IUsuarioRepository : IRepository<ApplicationUser>
    {
        Task BloquearUsuarioAsync(string IdUsuario);
        Task DesbloquearUsuarioAsync(string IdUsuario);
    }
}
