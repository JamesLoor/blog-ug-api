using blog_ug_api.Models;
using Microsoft.AspNetCore.Mvc;
namespace blog_ug_api.Repositories
{
    public class PostRepository
    {
        RailwayContext _context = new RailwayContext();

        public async Task<Post> consultarPost(int id)
        {
            Post post = await _context.Posts.FindAsync(id);
            _context.SaveChangesAsync();
            return post;
        }

        public async void  publicarPost(Post post)
        {
            var publicacion =  await _context.Posts.AddAsync(post);
            _context.SaveChangesAsync();
        }
    }
}
