using Knowledge_Graph_Analysis_BackEnd.Dtos;
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

        public async Task<List<AvailableAuthor>> GetAvailableAuthors(string contains)
        {
            IResultCursor cursor;
            var authors = new List<AvailableAuthor>();
            IAsyncSession session = _driver.AsyncSession();
            try
            {
                var statement = new StringBuilder();
                statement.Append($"MATCH (a:Author) with distinct(a.name) as name, a.index as authorIndex " +
                    $"where a.name starts with '{contains}' return name,authorIndex limit 10");
                cursor = await session.RunAsync(statement.ToString());
                authors = await cursor.ToListAsync(record => {
                    var author = new AvailableAuthor
                    {
                        authorIndex = record["authorIndex"].As<string>(),
                        name = record["name"].As<string>()
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
        private async Task<List<Author>> GetAuthorsByStatement(string statement)
        {
            IResultCursor cursor;
            var authors = new List<Author>();
            IAsyncSession session = _driver.AsyncSession();
            try
            {       
                cursor = await session.RunAsync(statement);
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
        public async Task<List<Author>> GetAreaedAuthors(string area)
        {           
            var statement = new StringBuilder();
            statement.Append($"MATCH p=(a:Author)-[r:Search]->(n:Area) where n.name = '{area}' RETURN a as author");
            return await GetAuthorsByStatement(statement.ToString());
        }
        public async Task<List<Author>> GetCooperateAuthors(string authorIndex)
        {
            var statement = new StringBuilder();
            statement.Append($"MATCH (a:Author)-[r:Cooperate]->(b) where a.index = '{authorIndex}' RETURN b as author");
            return await GetAuthorsByStatement(statement.ToString());
        }

        public async Task<List<string>> GetAvailableAreas(string contains)
        {
            IResultCursor cursor;
            var areas = new List<string>();
            IAsyncSession session = _driver.AsyncSession();
            try
            {
                var statement = new StringBuilder();
                statement.Append($"MATCH (a:Area) " +
                    $"where a.name starts with '{contains}' return a.name as area limit 10");
                cursor = await session.RunAsync(statement.ToString());
                areas = await cursor.ToListAsync(record => record["area"].As<string>());
            }
            finally
            {
                await session.CloseAsync();
            }
            return areas;
        }

        public async Task<int> GetCooperateCounts(string oneAuthorIndex, string anotherAuthorIndex)
        {
            IResultCursor cursor;
            var result = 0;
            IAsyncSession session = _driver.AsyncSession();
            try
            {
                var statement = new StringBuilder();
                statement.Append($"Match (a:Author)-[r:Cooperate]->(b:Author)" +
                    $"where (a.index = '{oneAuthorIndex}' and b.index = '{anotherAuthorIndex}') or" +
                    $" (a.index = '{anotherAuthorIndex}' and b.index = '{oneAuthorIndex}') return r.co_num as num");
                cursor = await session.RunAsync(statement.ToString());
                var results = await cursor.ToListAsync(record => record["num"].As<int>());
                result = results.Count == 0? 0 : results[0];
            }
            finally
            {
                await session.CloseAsync();
            }
            return result;
        }
    }
}
