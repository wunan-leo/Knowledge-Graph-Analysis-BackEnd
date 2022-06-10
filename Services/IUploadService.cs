namespace Knowledge_Graph_Analysis_BackEnd.Services
{
    public interface IUploadService
    {
        public class UploadReply
        {
            public bool flag { get; set; } = false;
            public string msg { get; set; } = null!;
        }
        Task<UploadReply> uploadFile(IFormFile file);

        Task<object> MergeTable(string fileName, string tableName, string method);
    }
}
