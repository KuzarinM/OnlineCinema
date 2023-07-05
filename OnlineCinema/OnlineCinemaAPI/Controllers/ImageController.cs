using Microsoft.AspNetCore.Mvc;

namespace OnlineCinemaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : Controller
    {
        private readonly string bazePath = "E:\\SERVICES\\Картинки\\";

        [HttpGet("object/{name}")]
        public IActionResult GetObj(string name)
        {
            string[] options = Directory.GetFiles($"{bazePath}/Плитки", $"{name}.*");
            string filename;
            if (options.Length > 0)
            {
                filename = options[0];
            }
            else
            {
                filename = $"{bazePath}/default.png";
            }
            return File(System.IO.File.OpenRead(filename), $"image/{Path.GetExtension(filename).Substring(1)}");
        }

        [HttpGet("background/{name}")]
        public IActionResult GetBackg(string name)
        {
            string[] options = Directory.GetFiles($"{bazePath}/Фоны", $"{name}.*");
            string filename;
            if (options.Length > 0)
            {
                filename = options[0];
            }
            else
            {
                filename = $"{bazePath}/default.png";
            }
            return File(System.IO.File.OpenRead(filename), $"image/{Path.GetExtension(filename).Substring(1)}");
        }
    }
}
