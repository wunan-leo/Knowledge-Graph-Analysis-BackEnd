using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Knowledge_Graph_Analysis_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
        {
            if (file == null)
            {
                return BadRequest("There isn't a file.");
            }
            string[] limitFileType = { ".csv" };
            string currentFileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!limitFileType.Contains(currentFileExtension))
            {
                return new JsonResult(new { code = "-1", msg = "File must be csv file." });
            }
            var filePath = Path.Combine("imports/", file.FileName);
            var path = Path.Combine(Directory.GetCurrentDirectory(), filePath);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return Ok(new { size = file.Length, msg = "successfully upload the file!" });
        }
    }
}
