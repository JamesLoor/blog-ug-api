using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace blog_ug_api.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required]
        public string Imagen {  get; set; }

        [Required]
        [StringLength(200)]
        public string Titulo { get; set; }

        [Required]
        public string Descripcion { get; set; }

        [Required]
        public string Contenido { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaPublicacion { get; set; }

        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }
        public User Usuario { get; set; }

        public List<Comentario> Comentarios { get; set; }
        public List<Categoria> Categorias { get; set; }
    }
}
