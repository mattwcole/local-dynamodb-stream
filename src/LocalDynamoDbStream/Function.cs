using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;
using Amazon.Lambda.DynamoDBEvents;
using Newtonsoft.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace LocalDynamoDbStream
{
    public class Function
    {
        private static readonly JsonSerializer JsonSerializer = new JsonSerializer();

        public Task FunctionHandler(DynamoDBEvent dynamoEvent, ILambdaContext context)
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

            return Task.CompletedTask;
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
