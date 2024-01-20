using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace blog_ug_api.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string Contrasena { get; set; }

        public List<Post> Posts { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Last_login { get; set; }
    }
}
