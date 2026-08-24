using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BlogCore.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class CategoriasController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        //private readonly ApplicationDbContext _context;

        public CategoriasController(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;
            //_context = context;
        }

        
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                await _contenedorTrabajo.Categoria.AddAsync(categoria);
                await _contenedorTrabajo.SaveAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(categoria);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Categoria categoria = new Categoria();
            categoria = await _contenedorTrabajo.Categoria.GetAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                await _contenedorTrabajo.Categoria.UpdateAsync(categoria);
                await _contenedorTrabajo.SaveAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(categoria);
        }




        #region Llamadas a la API
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _contenedorTrabajo.Categoria.GetAllAsync();
            return Json(new { data = data });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var objFromDb = await _contenedorTrabajo.Categoria.GetAsync(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error borrando categoría" });
            }

            await _contenedorTrabajo.Categoria.RemoveAsync(objFromDb);
            await _contenedorTrabajo.SaveAsync();
            return Json(new { success = true, message = "Categoría borrada correctamente" });
        }
        #endregion
    }
}
