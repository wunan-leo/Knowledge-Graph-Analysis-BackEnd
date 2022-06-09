using Knowledge_Graph_Analysis_BackEnd.IRepositories;
using Knowledge_Graph_Analysis_BackEnd.Services;
using Knowledge_Graph_Analysis_BackEnd.Services.Implements;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Knowledge_Graph_Analysis_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private IUploadService uploadService;

        public UploadController(IUploadRepository uploadRepository)
        {
            this.uploadService = new UploadServiceImpl(uploadRepository);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file, string tableName)
        {
            var uploadResult = await uploadService.uploadFile(file);
            if(uploadResult.flag == false)
            {
                return BadRequest(new {msg = uploadResult.msg});
            }
            
            return Ok(await uploadService.MergeTable(file.FileName, tableName));
        }
    }
}
