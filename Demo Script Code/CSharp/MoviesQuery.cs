using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace MoviesQuery
{
    class Program
    {
        static void Main(string[] args)
        {
            AmazonDynamoDBClient ddbClient = new AmazonDynamoDBClient();
            int year = 1985;

            var qResponse = ddbClient.Query(new QueryRequest
            {
                TableName = "Movies",
                ExpressionAttributeNames = new Dictionary<string, string>
                {
                    { "#yr","year" }
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {":qYr", new AttributeValue {N = year.ToString()} }
                },
                KeyConditionExpression = "#yr = :qYr",
                ProjectionExpression = "#yr, title"
            });

            foreach (var ddbItem in qResponse.Items)
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
