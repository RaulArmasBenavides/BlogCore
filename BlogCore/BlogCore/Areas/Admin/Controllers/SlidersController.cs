using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BlogCore.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class SlidersController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public SlidersController(IContenedorTrabajo contenedorTrabajo, IWebHostEnvironment hostingEnvironment)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _hostingEnvironment = hostingEnvironment;
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
        public async Task<IActionResult> Create(Slider slider)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostingEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;

                //Nuevo slider
                string nombreArchivo = Guid.NewGuid().ToString();
                var subidas = Path.Combine(rutaPrincipal, @"imagenes\sliders");
                var extension = Path.GetExtension(archivos[0].FileName);

                using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                {
                    archivos[0].CopyTo(fileStreams);
                }

                slider.UrlImagen = @"\imagenes\sliders\" + nombreArchivo + extension;

                await _contenedorTrabajo.Slider.AddAsync(slider);
                await _contenedorTrabajo.SaveAsync();

                return RedirectToAction(nameof(Index));
            }
            return View();
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                var slider = await _contenedorTrabajo.Slider.GetAsync(id.GetValueOrDefault());
                return View(slider);
            }

            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Slider slider)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostingEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;

                var sliderDesdeDb = await _contenedorTrabajo.Slider.GetAsync(slider.Id);

                if (archivos.Count() > 0)
                {
                    //Nueva imagen para el slider
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\sliders");
                    var extension = Path.GetExtension(archivos[0].FileName);

                    var rutaImagen = Path.Combine(rutaPrincipal, sliderDesdeDb.UrlImagen.TrimStart('\\'));

                    if (System.IO.File.Exists(rutaImagen))
                    {
                        System.IO.File.Delete(rutaImagen);
                    }

                    //Nuevamente subimos el archivo
                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }

                    slider.UrlImagen = @"\imagenes\sliders\" + nombreArchivo + extension;

                    await _contenedorTrabajo.Slider.UpdateAsync(slider);
                    await _contenedorTrabajo.SaveAsync();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    //Aquí sería cuando la imagen ya existe y se conserva
                    slider.UrlImagen = sliderDesdeDb.UrlImagen;
                }

                await _contenedorTrabajo.Slider.UpdateAsync(slider);
                await _contenedorTrabajo.SaveAsync();

                return RedirectToAction(nameof(Index));
            }

            return View();
        }




        #region Llamadas a la API
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _contenedorTrabajo.Slider.GetAllAsync();
            return Json(new { data = data });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var sliderDesdeDb = await _contenedorTrabajo.Slider.GetAsync(id);

            if (sliderDesdeDb == null)
            {
                return Json(new { success = false, message = "Error borrando slider" });
            }

            await _contenedorTrabajo.Slider.RemoveAsync(sliderDesdeDb);
            await _contenedorTrabajo.SaveAsync();
            return Json(new { success = true, message = "Slider borrado correctamente" });
        }

        #endregion

    }
}
