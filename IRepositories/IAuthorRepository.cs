namespace Knowledge_Graph_Analysis_BackEnd.IRepositories
{
    public interface IAuthorRepository
    {
        Task<List<string>> GetAvailableAuthors(string contains);
    }
}
