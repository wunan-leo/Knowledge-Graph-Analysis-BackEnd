using Knowledge_Graph_Analysis_BackEnd.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Knowledge_Graph_Analysis_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorRepository authorRepository;

        public AuthorController(IAuthorRepository authorRepository)
        {
            this.authorRepository = authorRepository;
        }

        [HttpGet]
        [Route("/api/availableAuthors/{authorName}")]
        public async Task<ActionResult> GetAvailableAuthors(string authorName)
        {
            try
            {
                return Ok(await authorRepository.GetAvailableAuthors(authorName));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the neo4j database.");
            }
        }

        [HttpGet]
        [Route("/api/areaedAuthors/{area}")]
        public async Task<ActionResult> GetAreaedAuthors(string area)
        {
            try
            {
                return Ok(await authorRepository.GetAreaedAuthors(area));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the neo4j database.");
            }
        }
    }
}
