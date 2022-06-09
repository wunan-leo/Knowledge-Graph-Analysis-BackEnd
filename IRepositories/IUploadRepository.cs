namespace Knowledge_Graph_Analysis_BackEnd.IRepositories
{
    public interface IUploadRepository
    {
        Task<int> UploadAuthor(string fileName);
        Task<int> UploadPaper(string fileName);
        Task<int> UploadCompany(string fileName);
        Task<int> UploadArea(string fileName);
        Task<int> UploadAuthorPaper(string fileName);
        Task<int> UploadPaperReference(string fileName);
        Task<int> UploadAuthorCooperate(string fileName);
        Task<int> UploadAuthorCompany(string fileName);
        Task<int> UploadPaperCompany(string fileName);
        Task<int> UploadAuthorArea(string fileName);

    }
}
