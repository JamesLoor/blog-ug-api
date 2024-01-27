using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace blog_ug_api.Models
{
    public class Comentario
    {
        public int Id { get; set; }
        public string Contenido { get; set; }
        public int UsuarioId { get; set; }
        public User Usuario { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
