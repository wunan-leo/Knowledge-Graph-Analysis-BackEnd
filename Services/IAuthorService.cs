using Knowledge_Graph_Analysis_BackEnd.Dtos;

namespace Knowledge_Graph_Analysis_BackEnd.Services
{
    public interface IAuthorService
    {
        Task<List<BriefAuthor>> GetAuthorsBriefInfoByName(string name);

        Task<ImportantAuthorsDept> GetImportantAuthorAndDepartmentByArea(string area, string indicator, int authorLimit, int departmentLimit);
    }

}
