using Knowledge_Graph_Analysis_BackEnd.Models;

namespace Knowledge_Graph_Analysis_BackEnd.Repositories
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetComments();
        Task<Comment?> GetComment(int commentId);
        Task<Comment> AddComment(Comment comment);
        Task<Comment?> UpdateComment(Comment comment);
        Task<Comment?> DeleteComment(int commentId);

    }
}
