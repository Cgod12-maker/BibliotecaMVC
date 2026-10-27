using BibliotecaMVC.Data;
using BibliotecaMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaMVC.Controllers
{
    [Authorize]
    public class LibrosController : Controller
    {
        private readonly BibliotecaDbContext _context;
        private readonly ILogger<LibrosController> _logger;

        public LibrosController(BibliotecaDbContext context, ILogger<LibrosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // CONSULTAR
        public IActionResult Index()
        {
            _logger.LogInformation("Consultando el listado de libros.");
            var libros = _context.Libros.ToList();
            return View(libros);
        }

        // AGREGAR - GET
        [Authorize(Roles = "Admin,Bibliotecario")]
        public IActionResult Create()
        {
            _logger.LogInformation("Accediendo al formulario de creación de libro.");
            return View();
        }

        // AGREGAR - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Bibliotecario")]
        public IActionResult Create(Libro libro)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Libros.Add(libro);
                    _context.SaveChanges();
                    _logger.LogInformation($"Libro creado exitosamente: {libro.Titulo}");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al intentar crear un libro.");
                    ModelState.AddModelError("", "Ocurrió un error al guardar el libro.");
                }
            }
            return View(libro);
        }

        // EDITAR - GET
        [Authorize(Roles = "Admin,Bibliotecario")]
        public IActionResult Edit(int id)
        {
            _logger.LogInformation($"Buscando libro para editar con ID: {id}");
            var libro = _context.Libros.Find(id);
            if (libro == null)
            {
                _logger.LogWarning($"Libro no encontrado con ID: {id}");
                return NotFound();
            }
            return View(libro);
        }

        // EDITAR - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Bibliotecario")]
        public IActionResult Edit(int id, Libro libro)
        {
            if (id != libro.Id)
            {
                _logger.LogWarning("El ID del libro no coincide en la edición.");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Libros.Update(libro);
                    _context.SaveChanges();
                    _logger.LogInformation($"Libro actualizado exitosamente: {libro.Titulo}");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al intentar actualizar un libro.");
                    ModelState.AddModelError("", "Ocurrió un error al actualizar el libro.");
                }
            }
            return View(libro);
        }

        // ELIMINAR - GET
        [Authorize(Roles = "Admin,Bibliotecario")]
        public IActionResult Delete(int id)
        {
            _logger.LogInformation($"Buscando libro para eliminar con ID: {id}");
            var libro = _context.Libros.Find(id);
            if (libro == null)
            {
                _logger.LogWarning($"Libro no encontrado para eliminar con ID: {id}");
                return NotFound();
            }
            return View(libro);
        }

        // ELIMINAR - POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Bibliotecario")]
        public IActionResult DeleteConfirmed(int id)
        {
            var libro = _context.Libros.Find(id);
            if (libro != null)
            {
                try
                {
                    _context.Libros.Remove(libro);
                    _context.SaveChanges();
                    _logger.LogInformation($"Libro eliminado exitosamente: {libro.Titulo}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al intentar eliminar un libro.");
                }
            }
            else
            {
                _logger.LogWarning($"Intento de eliminar un libro inexistente con ID: {id}");
            }
            return RedirectToAction(nameof(Index));
        }
    }
}