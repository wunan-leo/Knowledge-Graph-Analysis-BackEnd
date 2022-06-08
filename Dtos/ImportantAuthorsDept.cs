using Knowledge_Graph_Analysis_BackEnd.Models;

namespace Knowledge_Graph_Analysis_BackEnd.Dtos
{
    public class ImportantAuthorsDept
    {
        public List<Author> importAuthors { get; set; }
        public List<ImportantDepartment> importantDepartments { get; set; }
    }
}
