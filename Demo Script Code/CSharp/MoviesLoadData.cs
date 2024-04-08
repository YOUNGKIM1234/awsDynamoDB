using Amazon.DynamoDBv2;
using System.Text.Json;
using System.IO;
using Amazon.DynamoDBv2.DocumentModel;

namespace MoviesLoadData
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new JsonDocumentOptions
            {
                AllowTrailingCommas = true,
                CommentHandling = JsonCommentHandling.Skip
            };

            AmazonDynamoDBClient ddbclient = new AmazonDynamoDBClient();

            var sr = new FileStream(@"moviedata.txt", FileMode.Open, FileAccess.Read);
            var jsonDocument = JsonDocument.Parse(sr, options);
            var table = Table.LoadTable(ddbclient, "Movies");
            foreach (JsonElement je in jsonDocument.RootElement.EnumerateArray())
            {
                var item = new Document();
                foreach (JsonProperty je2 in je.EnumerateObject())
                {
                    if (je2.Name == "year")
                    {
                        item[je2.Name] = je2.Value.GetInt32();
                    }
                    else
                    {
                        item[je2.Name] = je2.Value.ToString(); ;
                    }

                }
                table.PutItem(item);
            }

        }
    }
}
