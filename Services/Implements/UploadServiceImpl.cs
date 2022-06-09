using Knowledge_Graph_Analysis_BackEnd.IRepositories;
using static Knowledge_Graph_Analysis_BackEnd.Services.IUploadService;

namespace Knowledge_Graph_Analysis_BackEnd.Services.Implements
{
    public class UploadServiceImpl : IUploadService
    {
        private IUploadRepository uploadRepository;
        
        public UploadServiceImpl(IUploadRepository uploadRepository)
        {
            this.uploadRepository = uploadRepository;
        }
        public async Task<UploadReply> uploadFile(IFormFile file)
        {
            if (file == null)
            {
                return new UploadReply { flag = false, msg = "There isn't a file." };
            }
            string[] limitFileType = { ".csv" };
            string currentFileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!limitFileType.Contains(currentFileExtension))
            {
                return new UploadReply { flag = false, msg = "File must be csv file." };
            }
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "imports/");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            var path = Path.Combine(filePath, file.FileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return new UploadReply { flag = true, msg = "successfully upload the file!" };
        }
        public async Task<object> MergeTable(string fileName, string tableName)
        {
            if(tableName == "Author")
            {
                int count = await uploadRepository.UploadAuthor(fileName);
                return new { flag = true, msg = "sucessfully insert the author in to neo4j.", count = count };
            }
            return new {flag = false, msg = "fail to insert the data in to neo4j.", count = 0};
        }
    }
}
