using BibliotecaMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaMVC.Data
{
    public class BibliotecaDbContext : IdentityDbContext<IdentityUser>
    {
        public BibliotecaDbContext(DbContextOptions<BibliotecaDbContext> options)
            : base(options)
        {
        }

        public DbSet<Libro> Libros { get; set; }
    }
}