using Knowledge_Graph_Analysis_BackEnd.Dtos;
using Knowledge_Graph_Analysis_BackEnd.Models;

namespace Knowledge_Graph_Analysis_BackEnd.IRepositories
{
    public interface IAuthorRepository
    {
        Task<List<AvailableAuthor>> GetAvailableAuthors(string contains);

        Task<List<Author>> GetAreaedAuthors(string area);

        Task<List<Author>> GetCooperateAuthors(string authorIndex); 

        Task<List<string>> GetAvailableAreas(string contains);
    }
}
