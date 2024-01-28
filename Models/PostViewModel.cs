using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace blog_ug_api.Models
{
	public class PostViewModel
	{
        public string Imagen { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Contenido { get; set; }
        public string? Categoria { get; set; }
    }
}