using Knowledge_Graph_Analysis_BackEnd.Dtos;
using Knowledge_Graph_Analysis_BackEnd.Helper;
using Knowledge_Graph_Analysis_BackEnd.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Text;

namespace Knowledge_Graph_Analysis_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaperController : ControllerBase
    {
        private readonly IPaperRepository paperRepository;
        private readonly IDatabase _redis;
        public PaperController(IPaperRepository paperRepository, RedisHelper client)
        {
            this.paperRepository = paperRepository;
            this._redis = client.GetDatabase(1);
        }
        [HttpGet]
        [Route("/api/paper")]
        public async Task<ActionResult> GetPaperByIndex(string paperIndex)
        {
            try
            {
                return Ok(await paperRepository.GetPaperByIndex(paperIndex));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the neo4j database.");
            }
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

        [HttpGet]
        [Route("/api/importantVenues")]
        public async Task<ActionResult> GetImportantVenues(string area, int limit)
        {
            try
            {
                var arguement = new StringBuilder();
                arguement.Append($"area:{area}, limit:{limit}");
                var result = await _redis.StringGetAsync(arguement.ToString());
                if (result.IsNullOrEmpty)
                {
                    var value = await paperRepository.GetImportantVenue(area, limit);
                    await _redis.StringSetAsync(arguement.ToString(), JsonConvert.SerializeObject(value));
                    _redis.KeyExpire(arguement.ToString(), TimeSpan.FromMinutes(15));
                    return Ok(value);
                }
                else
                {
                    return Ok(JsonConvert.DeserializeObject<List<ImportantVenue>>(result));
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the neo4j database.");
            }
        }
    }
}
