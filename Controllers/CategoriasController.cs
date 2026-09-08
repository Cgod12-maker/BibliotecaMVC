using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using BibliotecaMVC.Models;

namespace BibliotecaMVC.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly IConfiguration _configuration;
        private string connectionString => _configuration.GetConnectionString("DefaultConnection");

        public CategoriasController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // MOSTRAR (Actividad previa de clases)
        public IActionResult Index()
        {
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

        // AGREGAR - GET (Actividad previa de clases)
        public IActionResult Create()
        {
            return View();
        }

        // AGREGAR - POST
        [HttpPost]
        public IActionResult Create(Categoria categoria)
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
            return RedirectToAction("Index");
        }

        // EDITAR - GET (Actividad 1)
        public IActionResult Edit(int id)
        {
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
                return NotFound();
            return View(categoria);
        }

        // EDITAR - POST (Actividad 1)
        [HttpPost]
        public IActionResult Edit(int id, Categoria categoria)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE Categorias SET Nombre = @Nombre, Descripcion = @Descripcion WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Nombre", categoria.Nombre);
                cmd.Parameters.AddWithValue("@Descripcion", categoria.Descripcion);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        // ELIMINAR - GET (Actividad 2 - muestra confirmación)
        public IActionResult Delete(int id)
        {
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
                return NotFound();
            return View(categoria);
        }

        // ELIMINAR - POST (Actividad 2 - ejecuta DELETE)
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Categorias WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }
    }
}
