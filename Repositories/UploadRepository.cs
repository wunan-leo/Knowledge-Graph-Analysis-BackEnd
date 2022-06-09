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

        public async Task<int> UploadAuthor(string fileName)
        {
            var statement = new StringBuilder();
            statement.Append($"using periodic commit 5000 " +
                $"LOAD CSV WITH HEADERS FROM \"file:///{fileName}\" as line" +
                " MERGE(p: Author{ index: line['index'],name: line['name']," +
                "pc: line['pc'],cn: line['cn'],hi: line['hi'],pi: line['pi'],upi: line['upi']})");
            var count = await UploadDataByStatement(statement.ToString());
            return count;
        }

    }
}
