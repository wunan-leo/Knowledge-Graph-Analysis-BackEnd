using Knowledge_Graph_Analysis_BackEnd.IRepositories;
using Knowledge_Graph_Analysis_BackEnd.Models;
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
        public async Task<List<Author>> GetAreaedAuthors(string area)
        {
            IResultCursor cursor;
            var authors = new List<Author>();
            IAsyncSession session = _driver.AsyncSession();
            try
            {
                var statement = new StringBuilder();
                statement.Append($"MATCH p=(a:Author)-[r:Search]->(n:Area) where n.name = '{area}' RETURN a as author");
                cursor = await session.RunAsync(statement.ToString());
                authors = await cursor.ToListAsync(record =>
                {
                    var node = record["author"].As<INode>();
                    var author = new Author
                    {
                        index = node.Properties["index"].As<string>(),
                        name = node.Properties["name"].As<string>(),
                        hi = node.Properties["hi"].As<string>(),
                        pc = node.Properties["pc"].As<string>(),
                        pi = node.Properties["pi"].As<string>(),
                        cn = node.Properties["cn"].As<string>(),
                        upi = node.Properties["upi"].As<string>()
                    };
                    return author;
                });
                
            }
            finally 
            {
                await session.CloseAsync(); 
            }
            return authors;
        }
    }
}
