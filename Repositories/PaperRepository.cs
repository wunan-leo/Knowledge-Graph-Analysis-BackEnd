using Knowledge_Graph_Analysis_BackEnd.IRepositories;
using Knowledge_Graph_Analysis_BackEnd.Models;
using Neo4j.Driver;
using System.Text;

namespace Knowledge_Graph_Analysis_BackEnd.Repositories
{
    public class PaperRepository : IPaperRepository
    {
        private readonly IDriver _driver;
        public PaperRepository(IDriver driver)
        {
            _driver = driver;
        }

        private async Task<List<Paper>> GetPapersByStatement(string statement)
        {
            IResultCursor cursor;
            var papers = new List<Paper>();
            IAsyncSession session = _driver.AsyncSession();
            try
            {
                cursor = await session.RunAsync(statement);
                papers = await cursor.ToListAsync(record =>
                {
                    var node = record["paper"].As<INode>();
                    var paper = new Paper
                    {
                        index = node.Properties["index"].As<string>(),
                        abstracts = node.Properties["abstract"].As<string>(),
                        publication_venue = node.Properties["publication_venue"].As<string>(),
                        paper_title = node.Properties["paper_title"].As<string>(),
                        year = node.Properties["year"].As<string>()
                    };
                    return paper;
                });
            }
            finally
            {
                await session.CloseAsync();
            }
            return papers;
        }
        public async Task<List<Paper>> GetAvailablePapers(string contains, int page, int pageSize)
        {
            var statement = new StringBuilder();
            statement.Append($"MATCH (p:Paper) where p.paper_title Contains '{contains}' " +
                $"return p as paper skip {page * pageSize} limit {pageSize}");
            return await GetPapersByStatement(statement.ToString());
        }

        public async Task<List<Paper>> GetWrittenPapers(string authorIndex)
        {
            var statement = new StringBuilder();
            statement.Append($"MATCH (a:Author)-[w:Write]->(p:Paper) where a.index = '{authorIndex}' return p as paper");
            return await GetPapersByStatement(statement.ToString());
        }

        public async Task<List<Paper>> GetCooperatePapers(string oneAuthorIndex, string anotherAuthorIndex)
        {
            var statement = new StringBuilder();
            statement.Append($"match(a:Author)-[:Write]->(p:Paper)<-[:Write]-(b:Author) " +
                $"where a.index = '{oneAuthorIndex}' and b.index = '{anotherAuthorIndex}' return p as paper");
            return await GetPapersByStatement(statement.ToString());
        }
    }
}
