using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace blog_ug_api.Models
{
    public class Comentario
    {
        public int Id { get; set; }

        [Required]
        public string Contenido { get; set; }

        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }
        public User Usuario { get; set; }

        [ForeignKey("Post")]
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
