using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace blog_ug_api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Contrasena { get; set; }
        public List<Post>? Posts { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Last_login { get; set; }
    }
}
