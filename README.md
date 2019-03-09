# LocalStack DynamoDB Stream to Lambda

## Linux

```sh
docker-compose up

dotnet publish src/LocalDynamoDbStream

zip -rj function.zip src/LocalDynamoDbStream/bin/Debug/netcoreapp2.1/publish

awslocal lambda create-function \
    --function-name local-function \
    --runtime dotnetcore2.1 \
    --zip-file fileb://function.zip \
    --handler local-dynamodb-stream::LocalDynamoDbStream.Function::Handler \
    --role local-role

awslocal lambda invoke --function-name local-function local-function.log
```

## Windows

```sh
docker-compose up

dotnet publish src/LocalDynamoDbStream

7z a -tzip function.zip ./src/LocalDynamoDbStream/bin/Debug/netcoreapp2.1/publish/*

aws --endpoint-url=http://localhost:4574 lambda create-function \
    --function-name local-function \
    --runtime dotnetcore2.1 \
    --zip-file fileb://function.zip \
    --handler local-dynamodb-stream::LocalDynamoDbStream.Function::Handler \
    --role local-role

aws --endpoint-url=http://localhost:4574 lambda invoke --function-name local-function local-function.log
```
