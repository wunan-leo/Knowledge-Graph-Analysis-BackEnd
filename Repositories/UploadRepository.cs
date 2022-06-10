using Knowledge_Graph_Analysis_BackEnd.IRepositories;
using Neo4j.Driver;
using System.Text;

namespace Knowledge_Graph_Analysis_BackEnd.Repositories
{
    public class UploadRepository : IUploadRepository
    {
        private IDriver _driver;
        public UploadRepository(IDriver driver)
        {
            _driver = driver;
        }

        private async Task<int> UploadDataByStatement(string statement)
        {
            IResultCursor cursor;
            IAsyncSession session = _driver.AsyncSession();
            var lineCount = 0;
            try
            {
                cursor = await session.RunAsync(statement);
                var lineCounts = await cursor.ToListAsync(record => record["num"].As<int>());
                lineCount = lineCounts.Count == 0 ? 0 : lineCounts[0];
            }
            finally
            {
                await session.CloseAsync();
            }
            return lineCount;
        }

        public async Task<int> UploadAuthor(string fileName, string method)
        {
            var statement = new StringBuilder();
            statement.Append($"using periodic commit 5000 " +
                $"LOAD CSV WITH HEADERS FROM \"file:///{fileName}\" as line" +
                $" {method}" +
                "(p: Author{ index: line['index'],name: line['name']," +
                "pc: line['pc'],cn: line['cn'],hi: line['hi'],pi: line['pi'],upi: line['upi']})");
            var count = await UploadDataByStatement(statement.ToString());
            return count;
        }

        public async Task<int> UploadPaper(string fileName, string method)
        {
            var statement = new StringBuilder();
            statement.Append($"using periodic commit 5000 " +
                $"LOAD CSV WITH HEADERS FROM \"file:///{fileName}\" as line" +
                $" {method}" +
                "(p: Paper{ index: line['index'], paper_title: line['paper_title'], year: line['year']," +
                " publication_venue: line['publication_venue'], abstract: line['abstract']})");
            var count = await UploadDataByStatement(statement.ToString());
            return count;
        }

        public async Task<int> UploadCompany(string fileName, string method)
        {
            var statement = new StringBuilder();
            statement.Append($"using periodic commit 5000 " +
                $"LOAD CSV WITH HEADERS FROM \"file:///{fileName}\" as line" +
                $" {method}" +
                " (c:Company{name: line['affiliation']})");
            var count = await UploadDataByStatement(statement.ToString());
            return count;
        }

        public async Task<int> UploadArea(string fileName, string method)
        {
            var statement = new StringBuilder();
            statement.Append($"using periodic commit 5000 " +
                $"LOAD CSV WITH HEADERS FROM \"file:///{fileName}\" as line" +
                $" {method}" +
                " (c:Area{name: line['name']})");
            var count = await UploadDataByStatement(statement.ToString());
            return count;
        }

        public async Task<int> UploadAuthorPaper(string fileName, string method)
        {
            var statement = new StringBuilder();
            statement.Append($"using periodic commit 5000 " +
                $"LOAD CSV WITH HEADERS FROM \"file:///{fileName}\" as line" +
                $" {method} " +
                "(p: Paper{index: toString(line['paper_index'])}), (a: Author{index: toString(line['author_index'])})" +
                " MERGE (a)-[:Write{affiliation: line['affiliation']}]->(p)");
            var count = await UploadDataByStatement(statement.ToString());
            return count;
        }
        public async Task<int> UploadPaperReference(string fileName, string method)
        {
            var statement = new StringBuilder();
            statement.Append($"using periodic commit 5000 " +
                $"LOAD CSV WITH HEADERS FROM \"file:///{fileName}\" as line" +
                $" {method}" +
                " (p1: Paper{index: line['paper_index']}), (p2: Paper{index: line['referenced_index']})" +
                " MERGE (p1)-[:Reference]->(p2)");
            var count = await UploadDataByStatement(statement.ToString());
            return count;
        }
        public async Task<int> UploadAuthorCooperate(string fileName, string method)
        {
            var statement = new StringBuilder();
            statement.Append($"using periodic commit 5000 " +
                $"LOAD CSV WITH HEADERS FROM \"file:///{fileName}\" as line" +
                $" {method}" +
                " (p1: Author{index: line['first_author']}), (p2: Author{index: line['second_author']})" +
                " MERGE (p1)-[:Cooperate{co_num: line['co_num']}]->(p2)");
            var count = await UploadDataByStatement(statement.ToString());
            return count;
        }

        public async Task<int> UploadAuthorCompany(string fileName, string method)
        {
            var statement = new StringBuilder();
            statement.Append($"using periodic commit 5000 " +
                $"LOAD CSV WITH HEADERS FROM \"file:///{fileName}\" as line" +
                $" {method}" +
                " (a:Author{index: line['author_index']}), (p: Company{name: line['affiliation']})" +
                " MERGE (a)-[:Work]->(p)");
            var count = await UploadDataByStatement(statement.ToString());
            return count;
        }

        public async Task<int> UploadPaperCompany(string fileName, string method)
        {
            var statement = new StringBuilder();
            statement.Append($"using periodic commit 5000 " +
                $"LOAD CSV WITH HEADERS FROM \"file:///{fileName}\" as line" +
                $" {method}" +
                " (a:Paper{index: line['paper_index']}), (p: Company{name: line['affiliation']})" +
                " CREATE (a)-[:Belongs]->(p)");
            var count = await UploadDataByStatement(statement.ToString());
            return count;
        }

        public async Task<int> UploadAuthorArea(string fileName, string method)
        {
            var statement = new StringBuilder();
            statement.Append($"using periodic commit 5000 " +
                $"LOAD CSV WITH HEADERS FROM \"file:///{fileName}\" as line" +
                $" {method}" +
                " (a:Author{index: line['author_index']}), (p: Area{name: line['interest']})" +
                " CREATE (a)-[:Search]->(p)");
            var count = await UploadDataByStatement(statement.ToString());
            return count;
        }
    }
}
