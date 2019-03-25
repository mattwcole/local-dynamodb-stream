# LocalStack DynamoDB Stream to Lambda

An example .NET Core Lambda consuming a DynamoDB Stream. Runs in [LocalStack](https://github.com/localstack/localstack) on Docker.

## Usage

Requires .NET Core 2.1, Docker, Docker Compose, the aws cli (or awslocal) and [7Zip](https://www.7-zip.org/download.html) on the path if using Windows.

### Build and Zip the Lambda

#### Linux

```sh
dotnet publish src/LocalDynamoDbStream
zip -rj function.zip src/LocalDynamoDbStream/bin/Debug/netcoreapp2.1/publish
```

#### Windows

```sh
dotnet publish src/LocalDynamoDbStream
7z a -tzip function.zip ./src/LocalDynamoDbStream/bin/Debug/netcoreapp2.1/publish/*
```

### Create the Local Infrastructure

Start LocalStack and wait for the provisioning to complete.

```sh
docker-compose up
```

### Put an Item into the DynamoDB Table

```sh
aws --endpoint-url=http://localhost:4569 dynamodb put-item \
    --table-name local-table \
    --item Id={S="key1"},Value={S="value1"}
```

### Get the CloudWatch Logs for the Lamda Invocation

_Note that CloudWatch Logs do not appear to be working from Windows hosts (see [here](https://github.com/localstack/localstack/issues/1211))._

```sh
aws --endpoint-url=http://localhost:4586 logs filter-log-events \
    --log-group-name /aws/lambda/local-function
```

### Update the Lambda Code

If you have made changes to the Lambda and want to update the existing version, first use the commands above to re-publish and zip, then replace the function using the following.

```sh
aws --endpoint-url=http://localhost:4574 lambda update-function-code \
    --function-name local-function \
    --zip-file fileb://function.zip
```
