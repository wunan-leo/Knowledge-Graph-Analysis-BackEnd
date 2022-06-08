using Knowledge_Graph_Analysis_BackEnd.Models;

namespace Knowledge_Graph_Analysis_BackEnd.Dtos
{
    public class ImportantDepartment
    {
        public string departmentName { get; set; } = null!;
        public float avgHi { get; set; }
        public float avgPi { get; set; }
        public float avgUpi { get; set; }
        public int authorCount { get; set; }
        public List<Author> authors { get; set; }

    }
}
