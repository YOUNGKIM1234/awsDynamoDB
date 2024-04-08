using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace MoviesScan
{
    class Program
    {
        static void Main(string[] args)
        {
            AmazonDynamoDBClient ddbClient = new AmazonDynamoDBClient();
            int fromYear = 1984;
            int toYear = 1987;
            //without the Limit Parameter, the Scan method will return a maximum of 1MB of data
            //and then apply filters defined in the FilterExpression
            var sResponse = ddbClient.Scan(new ScanRequest
            {
                TableName = "Movies",
                ExpressionAttributeNames = new Dictionary<string, string>
                {
                    { "#yr", "year" }
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    { ":yFromYear", new AttributeValue { N = fromYear.ToString() } },
                    { ":yToYear", new AttributeValue { N = toYear.ToString() } },
                },
                FilterExpression = "#yr between :yFromYear and :yToYear",
                ProjectionExpression = "#yr, title"
            });
            foreach (var ddbItem in sResponse.Items)
            {
                foreach (var sKey in ddbItem.Keys)
                {
                    if (sKey == "year")
                    {
                        Console.WriteLine(string.Format("{0} : {1}", sKey, ddbItem[sKey].N));
                    }
                    else
                    {
                        Console.WriteLine(string.Format("{0} : {1}", sKey, ddbItem[sKey].S));
                    }
                }
            }
        }
    }
}
