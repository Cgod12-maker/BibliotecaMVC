using BibliotecaMVC.Models;

namespace BibliotecaMVC.Services
{
    public interface IAutorService
    {
        IEnumerable<Autor> ObtenerAutores();
        Autor? ObtenerAutorPorId(int id);
    }
}
