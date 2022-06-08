using Knowledge_Graph_Analysis_BackEnd.IRepositories;
using Knowledge_Graph_Analysis_BackEnd.Services;
using Knowledge_Graph_Analysis_BackEnd.Services.Implements;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Knowledge_Graph_Analysis_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorRepository authorRepository;
        private readonly IAuthorService authorService;

        public AuthorController(IAuthorRepository authorRepository)
        {
            this.authorRepository = authorRepository;
            this.authorService = new AuthorServiceImpl(this.authorRepository);
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

        [HttpGet]
        [Route("/api/briefAuthors")]
        public async Task<ActionResult> GetBriefAuthorsByName(string authorName)
        {
            try
            {
                return Ok(await authorService.GetAuthorsBriefInfoByName(authorName));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the neo4j database.");
            }
        }

        [HttpGet]
        [Route("/api/authorAreas")]
        public async Task<ActionResult> GetAreasByAuthorIndex(string authorIndex)
        {
            try
            {
                return Ok(await authorRepository.GetAuthorAreas(authorIndex));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the neo4j database.");
            }
        }

        [HttpGet]
        [Route("/api/authorDepartment")]
        public async Task<ActionResult> GetDepartmentByAuthorIndex(string authorIndex)
        {
            try
            {
                return Ok(await authorRepository.GetAuthorDepartment(authorIndex));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the neo4j database.");
            }
        }

        [HttpGet]
        [Route("/api/authorName")]
        public async Task<ActionResult> GetAuthorNameByIndex(string authorIndex)
        {
            try
            {
                return Ok(await authorRepository.GetAuthorNameByIndex(authorIndex));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the neo4j database.");
            }
        }

        [HttpGet]
        [Route("/api/importantAuthorsDept")]
        public async Task<ActionResult> GetImportantAuthorsAndDepartment(string area, string indicator, int authorLimit, int departmentLimit)
        {
            try
            {
                return Ok(await authorService.GetImportantAuthorAndDepartmentByArea(area, indicator, authorLimit, departmentLimit));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the neo4j database.");
            }
        }
    }
}
