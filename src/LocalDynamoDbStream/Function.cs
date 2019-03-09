using System;
using System.IO;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;
using Amazon.Lambda.DynamoDBEvents;
using Newtonsoft.Json;

[assembly: LambdaSerializer(typeof(JsonSerializer))]

namespace LocalDynamoDbStream
{
    public class Function
    {
        private static readonly JsonSerializer JsonSerializer = new JsonSerializer();

        public void FunctionHandler(DynamoDBEvent dynamoEvent, ILambdaContext context)
        {
            context.Logger.LogLine($"Beginning to process {dynamoEvent.Records.Count} records...");

            foreach (var record in dynamoEvent.Records)
            {
                context.Logger.LogLine($"Event ID: {record.EventID}");
                context.Logger.LogLine($"Event Name: {record.EventName}");

                var streamRecordJson = SerializeStreamRecord(record.Dynamodb);
                context.Logger.LogLine("DynamoDB Record:");
                context.Logger.LogLine(streamRecordJson);
            }

            context.Logger.LogLine("Stream processing complete.");
        }

        public void Handler()
        {
            Console.WriteLine("HELLO FROM LAMBDA");
        }

        private static string SerializeStreamRecord(StreamRecord streamRecord)
        {
            using (var writer = new StringWriter())
            {
                JsonSerializer.Serialize(writer, streamRecord);
                return writer.ToString();
            }
        }
    }
}
