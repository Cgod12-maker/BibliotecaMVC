using BibliotecaMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace BibliotecaMVC.Controllers
{
    [Authorize]
    public class CategoriasController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CategoriasController> _logger;
        private readonly string connectionString;

        public CategoriasController(IConfiguration configuration, ILogger<CategoriasController> logger)
        {
            _configuration = configuration;
            _logger = logger;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        // MOSTRAR
        public IActionResult Index()
        {
            _logger.LogInformation("Consultando el listado de categorías.");
            var categorias = new List<Categoria>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT Id, Nombre, Descripcion FROM Categorias";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    categorias.Add(new Categoria
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Nombre = reader["Nombre"].ToString(),
                        Descripcion = reader["Descripcion"].ToString()
                    });
                }
            }
            return View(categorias);
        }

        // AGREGAR - GET
        public IActionResult Create()
        {
            _logger.LogInformation("Accediendo al formulario de creación de categoría.");
            return View();
        }

        // AGREGAR - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        string query = "INSERT INTO Categorias (Nombre, Descripcion) VALUES (@Nombre, @Descripcion)";
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@Nombre", categoria.Nombre);
                        cmd.Parameters.AddWithValue("@Descripcion", categoria.Descripcion);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                    _logger.LogInformation($"Categoría creada exitosamente: {categoria.Nombre}");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al intentar crear una categoría.");
                    ModelState.AddModelError("", "Ocurrió un error al guardar la categoría.");
                }
            }
            return View(categoria);
        }

        // EDITAR - GET
        public IActionResult Edit(int id)
        {
            _logger.LogInformation($"Buscando categoría para editar con ID: {id}");
            Categoria categoria = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT Id, Nombre, Descripcion FROM Categorias WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    categoria = new Categoria
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Nombre = reader["Nombre"].ToString(),
                        Descripcion = reader["Descripcion"].ToString()
                    };
                }
            }
            if (categoria == null)
            {
                _logger.LogWarning($"Categoría no encontrada con ID: {id}");
                return NotFound();
            }
            return View(categoria);
        }

        // EDITAR - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Categoria categoria)
        {
            if (id != categoria.Id)
            {
                _logger.LogWarning("El ID de la categoría no coincide en la edición.");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        string query = "UPDATE Categorias SET Nombre = @Nombre, Descripcion = @Descripcion WHERE Id = @Id";
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@Nombre", categoria.Nombre);
                        cmd.Parameters.AddWithValue("@Descripcion", categoria.Descripcion);
                        cmd.Parameters.AddWithValue("@Id", id);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                    _logger.LogInformation($"Categoría actualizada exitosamente: {categoria.Nombre}");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al intentar actualizar una categoría.");
                    ModelState.AddModelError("", "Ocurrió un error al actualizar la categoría.");
                }
            }
            return View(categoria);
        }

        // ELIMINAR - GET
        public IActionResult Delete(int id)
        {
            _logger.LogInformation($"Buscando categoría para eliminar con ID: {id}");
            Categoria categoria = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT Id, Nombre, Descripcion FROM Categorias WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    categoria = new Categoria
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Nombre = reader["Nombre"].ToString(),
                        Descripcion = reader["Descripcion"].ToString()
                    };
                }
            }
            if (categoria == null)
            {
                _logger.LogWarning($"Categoría no encontrada para eliminar con ID: {id}");
                return NotFound();
            }
            return View(categoria);
        }

        // ELIMINAR - POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM Categorias WHERE Id = @Id";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Id", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                _logger.LogInformation($"Categoría eliminada exitosamente con ID: {id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al intentar eliminar una categoría.");
            }
            return RedirectToAction(nameof(Index));
        }
    }
}