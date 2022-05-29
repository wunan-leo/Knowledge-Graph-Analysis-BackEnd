using Knowledge_Graph_Analysis_BackEnd.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Knowledge_Graph_Analysis_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaperController : ControllerBase
    {
        private readonly IPaperRepository paperRepository;
        public PaperController(IPaperRepository paperRepository)
        {
            this.paperRepository = paperRepository;
        }

        [HttpGet]
        [Route("/api/availablePapers")]
        public async Task<ActionResult> GetAvailablePapers(string paperBody = "", int page = 0, int pageSize = 10)
        {
            try
            {
                return Ok(await paperRepository.GetAvailablePapers(paperBody, page, pageSize));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the neo4j database.");
            }
        }

        [HttpGet]
        [Route("/api/writtenPapers")]
        public async Task<ActionResult> GetWrittenPapers(string authorIndex)
        {
            try
            {
                return Ok(await paperRepository.GetWrittenPapers(authorIndex));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the neo4j database.");
            }
        }

        [HttpGet]
        [Route("/api/cooperatePapers")]
        public async Task<ActionResult> GetCooperatePapers(string oneAuthorIndex, string anotherAuthorIndex)
        {
            try
            {
                return Ok(await paperRepository.GetCooperatePapers(oneAuthorIndex, anotherAuthorIndex));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the neo4j database.");
            }
        }
    }
}
