using System.ComponentModel.DataAnnotations;

namespace blog_ug_api.Models
{
    public class Categoria
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        public List<Post> Posts { get; set; }
    }
}
