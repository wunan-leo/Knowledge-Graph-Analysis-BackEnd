namespace Knowledge_Graph_Analysis_BackEnd.IRepositories
{
    public interface IUploadRepository
    {
        Task<int> UploadAuthor(string fileName);
    }
}
