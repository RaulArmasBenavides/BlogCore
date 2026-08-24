using BlogCore.AccesoDatos.Data.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace BlogCore.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class UsuariosController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
       
        public UsuariosController(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;
            
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var usuarioActual = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var usuarios = await _contenedorTrabajo.Usuario.GetAllAsync(u => u.Id != usuarioActual.Value);
            return View(usuarios);
        }

        [HttpGet]
        public async Task<IActionResult> Bloquear(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            await _contenedorTrabajo.Usuario.BloquearUsuarioAsync(id);
            await _contenedorTrabajo.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Desbloquear(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            await _contenedorTrabajo.Usuario.DesbloquearUsuarioAsync(id);
            await _contenedorTrabajo.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
