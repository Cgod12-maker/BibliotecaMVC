using BibliotecaMVC.Models;

namespace BibliotecaMVC.Services
{
    public class AutorServiceAlternativo : IAutorService
    {
        private readonly List<Autor> _autores;

        public AutorServiceAlternativo()
        {
            _autores = new List<Autor>
            {
                new Autor { Id = 10, Nombre = "Isabel Allende", Nacionalidad = "Chilena", FechaNacimiento = new DateTime(1942, 8, 2) },
                new Autor { Id = 20, Nombre = "Jorge Luis Borges", Nacionalidad = "Argentina", FechaNacimiento = new DateTime(1899, 8, 24) },
                new Autor { Id = 30, Nombre = "Pablo Neruda", Nacionalidad = "Chilena", FechaNacimiento = new DateTime(1904, 7, 12) }
            };
        }

        public IEnumerable<Autor> ObtenerAutores()
        {
            return _autores.OrderBy(a => a.Nombre);
        }

        public Autor? ObtenerAutorPorId(int id)
        {
            return _autores.FirstOrDefault(a => a.Id == id);
        }
    }
}