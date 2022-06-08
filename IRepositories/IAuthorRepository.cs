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
        Task<int> GetCooperateCounts(string oneAuthorIndex, string anotherAuthorIndex);

        Task<List<string>> GetAuthorIndexByName(string name);

        Task<List<string>> GetAuthorDepartment(string authorIndex);

        Task<List<string>> GetAuthorPaperTitle(string authorIndex);

        Task<List<string>>GetAuthorAreas(string authorIndex);

        Task<string>GetAuthorNameByIndex(string authorIndex);

    }
}   
