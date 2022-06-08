﻿using Knowledge_Graph_Analysis_BackEnd.Dtos;
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

        private async Task<List<string>> GetPropsByStatement(string statement, string resultProps)
        {
            var results = new List<string>();
            IResultCursor cursor;
            IAsyncSession session = _driver.AsyncSession();
            try
            {
                cursor = await session.RunAsync(statement);
                results = await cursor.ToListAsync(record => record[resultProps].As<string>());     
            }
            finally
            {
                await session.CloseAsync();
            }
            return results;
        }
        public async Task<List<string>> GetAuthorIndexByName(string name)
        {
            string returnProps = "result";
            var statement = new StringBuilder();
            statement.Append($"match (a:Author) where a.name = '{name}' return a.index as {returnProps}");
            return await GetPropsByStatement(statement.ToString(), returnProps);
        }

        public async Task<List<string>> GetAuthorDepartment(string authorIndex)
        {
            string returnProps = "result";
            var statement = new StringBuilder();
            statement.Append($"MATCH (a: Author)-[:Work]->(c: Company) where a.index = '{authorIndex}' return c.name as {returnProps}");
            return await GetPropsByStatement(statement.ToString(), returnProps);
        }

        public async Task<List<string>> GetAuthorPaperTitle(string authorIndex)
        {
            string returnProps = "result";
            var statement = new StringBuilder();
            statement.Append($"MATCH (a: Author)-[:Write]->(p: Paper) where a.index = '{authorIndex}' return p.paper_title as {returnProps}");
            return await GetPropsByStatement(statement.ToString(), returnProps);
        }

        public async Task<List<string>> GetAuthorAreas(string authorIndex)
        {
            string returnProps = "result";
            var statement = new StringBuilder();
            statement.Append($"MATCH (a:Author)-[:Search]->(e:Area) where a.index = '{authorIndex}' return e.name as {returnProps}");
            return await GetPropsByStatement(statement.ToString(), returnProps);
        }

        public async Task<string> GetAuthorNameByIndex(string authorIndex)
        {
            string returnProps = "result";
            var statement = new StringBuilder();
            statement.Append($"MATCH (a:Author) where a.index = '{authorIndex}' return a.name as {returnProps}");
            var resultList = await GetPropsByStatement(statement.ToString(), returnProps);
            if(resultList.Count != 0)
            {
                return resultList[0];
            }
            return "";
        }
    }
}
