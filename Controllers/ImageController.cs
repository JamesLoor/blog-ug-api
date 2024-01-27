using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace blog_ug_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ImageController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpPost("upload-image")]
        [Authorize]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile image)
        {
            try
            {
                string randomString = Guid.NewGuid().ToString("N").Substring(0, 8);
                long timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                string filename = $"{randomString}_{timestamp}.jpg";
                string imageUrl = await UploadFile(image.OpenReadStream(), filename);

                return Ok(new { imageUrl });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private async Task<string> UploadFile(Stream file, string filename)
        {
            string ApiKey = _configuration["Firebase:ApiKey"]!;
            string AuthEmail = _configuration["Firebase:AuthEmail"]!;
            string AuthPassword = _configuration["Firebase:AuthPassword"]!;
            string StorageUrl = _configuration["Firebase:StorageUrl"]!;
            string ProjectId = _configuration["Firebase:ProjectId"]!;

            var config = new FirebaseAuthConfig
            {
                ApiKey = ApiKey,
                AuthDomain = $"{ProjectId}.firebaseapp.com",
                Providers = new FirebaseAuthProvider[]{
                new EmailProvider()
            }
            };

            var client = new FirebaseAuthClient(config);
            var auth = await client.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
            var cancellation = new CancellationTokenSource();

            var task = new FirebaseStorage(
                StorageUrl,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = async () => await auth.User.GetIdTokenAsync(),
                    ThrowOnCancel = true
                })
                .Child("imagenes")
                .Child(filename)
                .PutAsync(file, cancellation.Token);

            var downloadURL = await task;
            return downloadURL;
        }
    }
}

