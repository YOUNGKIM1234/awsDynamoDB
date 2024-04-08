using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace MoviesCreateTable
{
    class Program
    {
        static void Main(string[] args)
        {
            AmazonDynamoDBClient ddbClient = new AmazonDynamoDBClient();
            string tableName = "Movies";

            Console.WriteLine("Getting list of tables");
            List<string> currentTables = ddbClient.ListTables().TableNames;
            Console.WriteLine("Number of tables: " + currentTables.Count);
            //Creating the table only of the table does not exist
            if (!currentTables.Contains(tableName))
            {
                try
                {
                    var request = new CreateTableRequest
                    {
                        TableName = tableName,
                        AttributeDefinitions = new List<AttributeDefinition>
                        {
                            new AttributeDefinition
                            {
                                AttributeName = "year",
                                // "S" = string, "N" = number, and so on.
                                AttributeType = ScalarAttributeType.N
                            },
                            new AttributeDefinition
                            {
                                AttributeName = "title",
                                AttributeType = ScalarAttributeType.S
                            }
                        },
                        KeySchema = new List<KeySchemaElement>
                        {
                             new KeySchemaElement
                            {
                              AttributeName = "year",
                              // "HASH" = hash key, "RANGE" = range key.
                              KeyType = KeyType.HASH
                            },
                            new KeySchemaElement
                            {
                              AttributeName = "title",
                              KeyType = KeyType.RANGE
                            }
                        },
                        BillingMode = BillingMode.PROVISIONED,
                        ProvisionedThroughput = new ProvisionedThroughput
                        {
                            ReadCapacityUnits = 10,
                            WriteCapacityUnits = 10
                        }
                    };
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}
