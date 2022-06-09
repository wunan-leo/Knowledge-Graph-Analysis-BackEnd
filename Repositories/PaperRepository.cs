using Knowledge_Graph_Analysis_BackEnd.Dtos;
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
            statement.Append($"MATCH (p:Paper) where p.paper_title Contains \"{contains}\" " +
                $"return p as paper skip {page * pageSize} limit {pageSize}");
            return await GetPapersByStatement(statement.ToString());
        }

        public async Task<List<Paper>> GetWrittenPapers(string authorIndex)
        {
            var statement = new StringBuilder();
            statement.Append($"MATCH (a:Author)-[w:Write]->(p:Paper) where a.index = \"{authorIndex}\" return p as paper");
            return await GetPapersByStatement(statement.ToString());
        }

        public async Task<List<Paper>> GetCooperatePapers(string oneAuthorIndex, string anotherAuthorIndex)
        {
            var statement = new StringBuilder();
            statement.Append($"match(a:Author)-[:Write]->(p:Paper)<-[:Write]-(b:Author) " +
                $"where a.index = \"{oneAuthorIndex}\" and b.index = \"{anotherAuthorIndex}\" return p as paper");
            return await GetPapersByStatement(statement.ToString());
        }

        public async Task<List<ImportantVenue>> GetImportantVenue(string area, int limit)
        {
            IResultCursor cursor;
            var vunues = new List<ImportantVenue>();
            IAsyncSession session = _driver.AsyncSession();
            var statement = new StringBuilder();
            statement.Append($"MATCH (a:Area)<-[:Search]-(au:Author)-[:Write]->(p:Paper)<-[r:Reference]-(:Paper) " +
                $"where a.name=\"{area}\" " +
                $"return p.publication_venue as venueName, count(distinct(p)) as paperCount, count(distinct(r)) as referenceCount " +
                $"order by referenceCount desc limit {limit}");
            try
            {
                cursor = await session.RunAsync(statement.ToString());
                vunues = await cursor.ToListAsync(record =>
                {                
                    var vunue = new ImportantVenue
                    {
                        venueName = record["venueName"].As<string>(),
                        paperCount = record["paperCount"].As<int>(),
                        referenceCount = record["referenceCount"].As<int>()
                    };
                    return vunue;
                });
                foreach(ImportantVenue vunue in vunues)
                {
                    var paperStatement = new StringBuilder();
                    paperStatement.Append($"MATCH (a:Area)<-[:Search]-(au:Author)-[:Write]->(p:Paper) " +
                        $"where a.name=\"{area}\" and p.publication_venue=\"{vunue.venueName}\" return distinct(p) as paper");
                    vunue.papers = await GetPapersByStatement(paperStatement.ToString());
                }
            }
            finally
            {
                await session.CloseAsync();
            }
            return vunues;
        }
    }
}
