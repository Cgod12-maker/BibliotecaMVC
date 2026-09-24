using System.ComponentModel.DataAnnotations;

namespace BibliotecaMVC.Models
{
    public class Libro
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El titulo es obligatorio")]
        [StringLength(200)]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El autor es obligatorio")]
        [StringLength(150)]
        public string Autor { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Genero { get; set; }

        [Range(1400, 2100, ErrorMessage = "Anio invalido")]
        public int AnioPublicacion { get; set; }

        [Range(0, 100000)]
        public int NumeroPaginas { get; set; }
    }
}