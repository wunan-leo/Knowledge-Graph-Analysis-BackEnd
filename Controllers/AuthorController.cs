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
        [Route("/api/availableAuthors")]
        public async Task<ActionResult> GetAvailableAuthors(string authorName = "")
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
        [Route("/api/areaedAuthors")]
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

        [HttpGet]
        [Route("/api/cooperateAuthors")]
        public async Task<ActionResult> GetCooperateAuthors(string authorIndex)
        {
            try
            {
                return Ok(await authorRepository.GetCooperateAuthors(authorIndex));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the neo4j database.");
            }
        }

        [HttpGet]
        [Route("/api/availableAreas")]
        public async Task<ActionResult> GetAvailableAreas(string areaName = "")
        {
            try
            {
                return Ok(await authorRepository.GetAvailableAreas(areaName));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the neo4j database.");
            }
        }

        [HttpGet]
        [Route("/api/cooperateCounts")]
        public async Task<ActionResult> GetCooperateCounts(string oneAuthorIndex, string anotherAuthorIndex)
        {
            try
            {
                return Ok(await authorRepository.GetCooperateCounts(oneAuthorIndex, anotherAuthorIndex));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the neo4j database.");
            }
        }
    }
}
