using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BlogCore.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    public class HomeController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;

        public HomeController(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;
        }

        public async Task<IActionResult> Index()
        {
            var sliders = await _contenedorTrabajo.Slider.GetAllAsync();
            var articulos = await _contenedorTrabajo.Articulo.GetAllAsync();

            HomeVM homeVm = new HomeVM()
            {
                Slider = sliders,
                ListaArticulos = articulos
            };

            ViewBag.IsHome = true;

            return View(homeVm);
        }

        public async Task<IActionResult> Details(int id)
        {
            var articuloDesdeDb = await _contenedorTrabajo.Articulo.GetAsync(id);
            return View(articuloDesdeDb);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}