#!/bin/sh

set -e

export AWS_ACCESS_KEY_ID=accesskeyid
export AWS_SECRET_ACCESS_KEY=secretaccesskey
export AWS_DEFAULT_REGION=us-east-1

echo "Creating Lambda function..."

aws --endpoint-url=http://localhost:4574 lambda create-function \
    --function-name local-function \
    --runtime dotnetcore2.1 \
    --zip-file fileb:///tmp/function.zip \
    --handler LocalDynamoDbStream::LocalDynamoDbStream.Function::FunctionHandler \
    --role local-role

echo "Done creating Lambda function"
echo "Creating DynamoDB table..."

streamArn=$(aws --endpoint-url=http://localhost:4569 dynamodb create-table \
    --table-name local-table \
    --attribute-definitions AttributeName=Id,AttributeType=S \
    --key-schema AttributeName=Id,KeyType=HASH \
    --stream-specification StreamEnabled=true,StreamViewType=NEW_AND_OLD_IMAGES \
    --provisioned-throughput ReadCapacityUnits=10,WriteCapacityUnits=10 \
    --query 'TableDescription.LatestStreamArn' \
    --output text)

echo "Done creating DynamoDB table"
echo "Creating Lambda trigger..."

aws --endpoint-url=http://localhost:4574 lambda create-event-source-mapping \
    --function-name local-function \
    --event-source $streamArn \
    --batch-size 1 \
    --starting-position TRIM_HORIZON

echo "Done creating Lambda trigger"
echo "Provisioning complete!"
