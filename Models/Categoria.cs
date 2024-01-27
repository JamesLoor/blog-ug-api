using System.ComponentModel.DataAnnotations;

namespace blog_ug_api.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<Post>? Posts { get; set; }
    }
}
