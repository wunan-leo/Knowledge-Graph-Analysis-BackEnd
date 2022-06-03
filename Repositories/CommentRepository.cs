using Knowledge_Graph_Analysis_BackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace Knowledge_Graph_Analysis_BackEnd.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly KnowledgeGraphContext _graphContext;
        public CommentRepository(KnowledgeGraphContext graphContext)
        {
            _graphContext = graphContext;
        }
        public async Task<IEnumerable<Comment>> GetComments()
        {
            return await _graphContext.Comments.ToListAsync();
        }
        public async Task<Comment?> GetComment(int commentId)
        {
            return await _graphContext.Comments
                .FirstOrDefaultAsync(c => c.CommentId == commentId);
        }

        public async Task<Comment> AddComment(Comment comment)
        {
            var result = await _graphContext.Comments.AddAsync(comment);
            await _graphContext.SaveChangesAsync();
            return result.Entity;
        }
        public async Task<Comment?> UpdateComment(Comment comment)
        {
            var result = await _graphContext.Comments
                .FirstOrDefaultAsync(c => c.CommentId == comment.CommentId);
            if(result != null)
            {
                result.FirstName = comment.FirstName;
                result.LastName = comment.LastName;
                result.CommentContent = comment.CommentContent;
                result.CommentTime = comment.CommentTime;

                await _graphContext.SaveChangesAsync();

                return result;
            }
            return null;
        }
        public async void DeleteComment(int commentId)
        {
            var result = await _graphContext.Comments
                .FirstOrDefaultAsync(c => c.CommentId == commentId);
            if(result != null)
            {
                _graphContext.Comments.Remove(result);
                await _graphContext.SaveChangesAsync();
            }
        }

    }
}
