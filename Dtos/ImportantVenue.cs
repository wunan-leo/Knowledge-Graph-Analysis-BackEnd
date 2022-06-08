using Knowledge_Graph_Analysis_BackEnd.Models;

namespace Knowledge_Graph_Analysis_BackEnd.Dtos
{
    public class ImportantVenue
    {
        public string venueName { get; set; }
        public int paperCount { get; set; }
        public int referenceCount { get; set; }
        public List<Paper> papers { get; set; }
    }
}
