using System.ComponentModel.DataAnnotations.Schema;

namespace blog_ug_api.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Imagen {  get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Contenido { get; set; }
        public DateTime FechaPublicacion { get; set; }
        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }
        public User Usuario { get; set; }
        public List<Comentario>? Comentarios { get; set; }
        public string? Categoria { get; set; }
    }
}
