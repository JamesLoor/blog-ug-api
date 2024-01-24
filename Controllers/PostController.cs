using blog_ug_api.Models;
using blog_ug_api.Repositories;
using Microsoft.AspNetCore.Mvc;
namespace blog_ug_api.Controllers
{
    public class PostController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> Get(int id)
        {
            try
            {
                PostRepository postRepository = new PostRepository();
                Post post = await postRepository.consultarPost(id);
                return Ok(post);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        
        [HttpPost]

        public async Task <ActionResult> Post(Post post)
        {
            try
            {
                PostRepository postRepository = new PostRepository();
                postRepository.publicarPost(post);
                return CreatedAtAction("Get", new {Id = post.Id}, post);

            }catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
