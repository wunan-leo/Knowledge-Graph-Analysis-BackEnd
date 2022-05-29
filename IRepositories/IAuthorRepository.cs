using Knowledge_Graph_Analysis_BackEnd.Models;

namespace Knowledge_Graph_Analysis_BackEnd.IRepositories
{
    public interface IAuthorRepository
    {
        Task<List<string>> GetAvailableAuthors(string contains);

        Task<List<Author>> GetAreaedAuthors(string area);
    }
}
