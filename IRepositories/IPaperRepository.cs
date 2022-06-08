using Knowledge_Graph_Analysis_BackEnd.Dtos;
using Knowledge_Graph_Analysis_BackEnd.Models;

namespace Knowledge_Graph_Analysis_BackEnd.IRepositories
{
    public interface IPaperRepository
    {
        Task<List<Paper>> GetAvailablePapers(string contains, int page, int pageSize);
        Task<List<Paper>> GetWrittenPapers(string authorIndex);

        Task<List<Paper>> GetCooperatePapers(string oneAuthorIndex, string anotherAuthorIndex);

        Task<List<ImportantVenue>> GetImportantVenue(string area, int limit);
    }
}
