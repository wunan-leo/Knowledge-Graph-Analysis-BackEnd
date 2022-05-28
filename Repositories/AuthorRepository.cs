using Knowledge_Graph_Analysis_BackEnd.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Neo4j.Driver;
using System.Text;

namespace Knowledge_Graph_Analysis_BackEnd.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly IDriver _driver;

        public AuthorRepository(IDriver driver)
        {
            _driver = driver;
        }

        public async Task<List<string>> GetAvailableAuthors(string contains)
        {
            IResultCursor cursor;
            var authors = new List<string>();
            IAsyncSession session = _driver.AsyncSession();
            try
            {
                var statement = new StringBuilder();
                statement.Append($"MATCH (a:Author) where a.name starts with '{contains}' return a.name as name limit 10");
                cursor = await session.RunAsync(statement.ToString());
                authors = await cursor.ToListAsync(record => record["name"].As<string>());
            }
            finally
            {
                await session.CloseAsync(); 
            }
            return authors;

        }
    }
}
