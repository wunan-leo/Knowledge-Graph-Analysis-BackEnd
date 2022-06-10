namespace Knowledge_Graph_Analysis_BackEnd.IRepositories
{
    public interface IUploadRepository
    {
        Task<int> UploadAuthor(string fileName, string method);
        Task<int> UploadPaper(string fileName, string method);
        Task<int> UploadCompany(string fileName, string method);
        Task<int> UploadArea(string fileName, string method);
        Task<int> UploadAuthorPaper(string fileName, string method);
        Task<int> UploadPaperReference(string fileName, string method);
        Task<int> UploadAuthorCooperate(string fileName, string method);
        Task<int> UploadAuthorCompany(string fileName, string method);
        Task<int> UploadPaperCompany(string fileName, string method);
        Task<int> UploadAuthorArea(string fileName, string method);

    }
}
