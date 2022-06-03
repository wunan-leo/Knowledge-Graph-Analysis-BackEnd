namespace Knowledge_Graph_Analysis_BackEnd.Dtos
{
    public class BriefAuthor
    {
        public BriefAuthor(string authorIndex, List<string> authorDepartments, List<string> articleTitles, List<string> areas)
        {
            this.authorIndex = authorIndex;
            this.authorDepartments = authorDepartments;
            this.articleTitles = articleTitles;
            this.areas = areas;
        }

        public BriefAuthor()
        {
            this.authorDepartments = new List<string>();
            this.articleTitles = new List<string>();
            this.areas = new List<string>();
        }

        public string authorIndex { get; set; } = null!;
        public List<string> authorDepartments { get; set;}
        public List<string> articleTitles { get; set; }
        public List<string> areas { get; set; }

    }
}
