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
        [HttpGet]
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

        [HttpGet("{commentId:int}")]
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
        public async Task<ActionResult<Comment>> CreateComment(Comment comment)
        {
            try
            {
                if(comment == null)
                {
                    return BadRequest();
                }

                var createdComment = await commentRepository.AddComment(comment);

                return CreatedAtAction(nameof(GetComment), new {id = createdComment.CommentId }, createdComment);
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error creating new comment record");
            }
        }

    }
}
