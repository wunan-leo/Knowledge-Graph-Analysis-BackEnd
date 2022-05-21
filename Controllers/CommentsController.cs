using Knowledge_Graph_Analysis_BackEnd.Models;
using Knowledge_Graph_Analysis_BackEnd.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Knowledge_Graph_Analysis_BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentRepository commentRepository;

        public CommentsController(ICommentRepository commentRepository)
        {
            this.commentRepository = commentRepository;
        }
        [HttpGet(Name = "GetComments")]
        [ActionName("GetComments")]
        public async Task<ActionResult> GetComments()
        {
            try
            {
                return Ok(await commentRepository.GetComments());
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database.");
            }
        }

        [HttpGet("{commentId:int}", Name = "GetComment")]
        [ActionName(nameof(GetComment))]
        public async Task<ActionResult<Comment>> GetComment(int commentId)
        {
            try
            {
                var comment = await commentRepository.GetComment(commentId);

                if(comment == null)
                {
                   return NotFound();
                }   
                return Ok(comment);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Comment>> CreateComment([FromBody]Comment comment)
        {
            try
            {
                if(comment == null)
                {
                    return BadRequest();
                }

                var createdComment = await commentRepository.AddComment(comment);

                var action = CreatedAtAction(nameof(GetComment), new { commentId = createdComment.CommentId }, createdComment);
                return action;
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error creating new comment record");
            }
        }

    }
}
