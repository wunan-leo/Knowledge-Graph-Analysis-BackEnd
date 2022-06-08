using Knowledge_Graph_Analysis_BackEnd.Dtos;
using Knowledge_Graph_Analysis_BackEnd.IRepositories;
using Knowledge_Graph_Analysis_BackEnd.Models;

namespace Knowledge_Graph_Analysis_BackEnd.Services.Implements
{
    public class AuthorServiceImpl : IAuthorService
    {
        private readonly IAuthorRepository authorRepository;

        public AuthorServiceImpl(IAuthorRepository authorRepository)
        {
            this.authorRepository = authorRepository;
        }

        public async Task<List<BriefAuthor>> GetAuthorsBriefInfoByName(string name)
        {
            List<BriefAuthor> authors = new List<BriefAuthor>();
            List<string> authorIndexs = await authorRepository.GetAuthorIndexByName(name);
            if(authorIndexs != null)
            {
                foreach (string authorIndex in authorIndexs)
                {              
                    List<string> authorDepartments = await authorRepository.GetAuthorDepartment(authorIndex);
                    List<string> authorAreas = await authorRepository.GetAuthorAreas(authorIndex);
                    List<string> authorPapers = await authorRepository.GetAuthorPaperTitle(authorIndex);
                    BriefAuthor briefAuthor = new BriefAuthor(authorIndex, authorDepartments, authorPapers, authorAreas);
                    authors.Add(briefAuthor);
                }
            }
            return authors;
        }

        private async Task<List<Author>> GetImportantAuthorByArea(string area, string indicator, int limit)
        {
            List<Author> originalAuthors = await authorRepository.GetAreaedAuthors(area);
            originalAuthors.Sort((left, right) =>
            {
                var leftProperty = left.GetType().GetProperty(indicator);
                var rightProperty = right.GetType().GetProperty(indicator);
                if (leftProperty == null || rightProperty == null)
                {
                    throw new Exception("please provide the true indicator of author.");
                }

                float leftValue = float.Parse(leftProperty.GetValue(left, null).ToString());
                float rightValue = float.Parse(rightProperty.GetValue(right, null).ToString());
                if(leftValue > rightValue)
                {
                    return 1;
                }
                else if(leftValue < rightValue)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            
            });
            limit = Math.Min(limit, originalAuthors.Count);
            return originalAuthors.GetRange(0, limit);
        }
        private async Task<List<ImportantDepartment>> GetImportantDepartmentByArea(string area, int limit)
        {

        }
    }
}
