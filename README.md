# LocalStack Lambda

```sh
docker-compose up

dotnet publish src/local-dynamodb-stream

zip -rj function.zip src/local-dynamodb-stream/bin/Debug/netcoreapp2.1/publish

awslocal lambda create-function \
    --function-name local-function \
    --runtime dotnetcore2.1 \
    --zip-file fileb://function.zip \
    --handler local-dynamodb-stream::local_dynamodb_stream.Function::Handler \
    --role local-role

awslocal lambda invoke --function-name local-function local-function.log
```
