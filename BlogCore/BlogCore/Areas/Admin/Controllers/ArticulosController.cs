using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BlogCore.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class ArticulosController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ArticulosController(IContenedorTrabajo contenedorTrabajo, IWebHostEnvironment hostingEnvironment)
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
        public async Task<IActionResult> Create()
        {

            ArticuloVM artivm = new ArticuloVM()
            {
                Articulo = new BlogCore.Models.Articulo(),
                ListaCategorias = await _contenedorTrabajo.Categoria.GetListaCategoriasAsync()
            };

            return View(artivm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ArticuloVM artiVM)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostingEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;
                if (artiVM.Articulo.Id == 0)
                {
                    if (archivos.Count() == 0)
                    {
                        ModelState.AddModelError("", "Debe seleccionar una imagen");
                        artiVM.ListaCategorias = await _contenedorTrabajo.Categoria.GetListaCategoriasAsync();
                        return View(artiVM);
                    }

                    var extensionesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var extension = Path.GetExtension(archivos[0].FileName).ToLower();

                    if (!extensionesPermitidas.Contains(extension))
                    {
                        ModelState.AddModelError("", "Solo se permiten imágenes: JPG, JPEG, PNG, GIF");
                        artiVM.ListaCategorias = await _contenedorTrabajo.Categoria.GetListaCategoriasAsync();
                        return View(artiVM);
                    }

                    //Nuevo artículo
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\articulos");

                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }

                    artiVM.Articulo.UrlImagen = @"\imagenes\articulos\" + nombreArchivo + extension;

                    await _contenedorTrabajo.Articulo.AddAsync(artiVM.Articulo);
                    await _contenedorTrabajo.SaveAsync();

                    return RedirectToAction(nameof(Index));
                }
            }

            artiVM.ListaCategorias = await _contenedorTrabajo.Categoria.GetListaCategoriasAsync();
            return View(artiVM);
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            ArticuloVM artivm = new ArticuloVM()
            {
                Articulo = new BlogCore.Models.Articulo(),
                ListaCategorias = await _contenedorTrabajo.Categoria.GetListaCategoriasAsync()
            };

            if (id != null)
            {
                artivm.Articulo = await _contenedorTrabajo.Articulo.GetAsync(id.GetValueOrDefault());
            }

            return View(artivm);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ArticuloVM artiVM)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostingEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;

                var articuloDesdeDb = await _contenedorTrabajo.Articulo.GetAsync(artiVM.Articulo.Id);

                if (archivos.Count() > 0)
                {
                    var extensionesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var extension = Path.GetExtension(archivos[0].FileName).ToLower();

                    if (!extensionesPermitidas.Contains(extension))
                    {
                        ModelState.AddModelError("", "Solo se permiten imágenes: JPG, JPEG, PNG, GIF");
                        artiVM.ListaCategorias = await _contenedorTrabajo.Categoria.GetListaCategoriasAsync();
                        return View(artiVM);
                    }

                    //Nueva imagen para el artículo
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\articulos");

                    var rutaImagen = Path.Combine(rutaPrincipal, articuloDesdeDb.UrlImagen.TrimStart('\\'));

                    if (System.IO.File.Exists(rutaImagen))
                    {
                        System.IO.File.Delete(rutaImagen);
                    }

                    //Nuevamente subimos el archivo
                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }

                    artiVM.Articulo.UrlImagen = @"\imagenes\articulos\" + nombreArchivo + extension;

                    await _contenedorTrabajo.Articulo.UpdateAsync(artiVM.Articulo);
                    await _contenedorTrabajo.SaveAsync();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    //Aquí sería cuando la imagen ya existe y se conserva
                    artiVM.Articulo.UrlImagen = articuloDesdeDb.UrlImagen;
                }

                await _contenedorTrabajo.Articulo.UpdateAsync(artiVM.Articulo);
                await _contenedorTrabajo.SaveAsync();
                return RedirectToAction(nameof(Index));
            }

            artiVM.ListaCategorias = await _contenedorTrabajo.Categoria.GetListaCategoriasAsync();
            return View(artiVM);
        }




        #region Llamadas a la API
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _contenedorTrabajo.Articulo.GetAllAsync(includeProperties: "Categoria");
            return Json(new { data = data });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {

            var articuloDesdeDb = await _contenedorTrabajo.Articulo.GetAsync(id);
            string rutaDirectorioPrincipal = _hostingEnvironment.WebRootPath;
            var rutaImagen = Path.Combine(rutaDirectorioPrincipal, articuloDesdeDb.UrlImagen.TrimStart('\\'));

            if (System.IO.File.Exists(rutaImagen))
            {
                System.IO.File.Delete(rutaImagen);
            }

            if (articuloDesdeDb == null)
            {
                return Json(new { success = false, message = "Error borrando artículo" });
            }

            await _contenedorTrabajo.Articulo.RemoveAsync(articuloDesdeDb);
            await _contenedorTrabajo.SaveAsync();
            return Json(new { success = true, message = "Artículo borrado correctamente" });
        }


        #endregion

    }
}
