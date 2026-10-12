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

        public LibrosController(BibliotecaDbContext context)
        {
            _context = context;
        }

        // MOSTRAR - Cualquier usuario logueado
        public IActionResult Index()
        {
            var libros = _context.Libros.ToList();
            return View(libros);
        }

        // AGREGAR - Solo Administrador
        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            return View();
        }

        // AGREGAR - Solo Administrador
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult Create(Libro libro)
        {
            if (ModelState.IsValid)
            {
                _context.Libros.Add(libro);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(libro);
        }

        // EDITAR - Solo Administrador
        [Authorize(Roles = "Administrador")]
        public IActionResult Edit(int id)
        {
            var libro = _context.Libros.Find(id);
            if (libro == null)
            {
                return NotFound();
            }
            return View(libro);
        }

        // EDITAR - Solo Administrador
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult Edit(int id, Libro libro)
        {
            if (id != libro.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _context.Libros.Update(libro);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(libro);
        }

        // ELIMINAR - Solo Administrador
        [Authorize(Roles = "Administrador")]
        public IActionResult Delete(int id)
        {
            var libro = _context.Libros.Find(id);
            if (libro == null)
            {
                return NotFound();
            }
            return View(libro);
        }

        // ELIMINAR - Solo Administrador
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult DeleteConfirmed(int id)
        {
            var libro = _context.Libros.Find(id);
            if (libro != null)
            {
                _context.Libros.Remove(libro);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}