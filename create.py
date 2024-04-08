from dynamoClient import DynamoClient

def create_table(name=None):
    if name==None:
        print("error :: table name empty")
        return 
    
    dynamodb = DynamoClient.get_resource()
    response = dynamodb.tables.all
    print(response)
    try:
        # Create the DynamoDB table.
        table = dynamodb.create_table(
            TableName=name,
            KeySchema=[
                {
                    'AttributeName': 'id',
                    'KeyType': 'HASH'    # partition key
                },
                {
                    'AttributeName': 'account_id',
                    'KeyType': 'RANGE'   # sort key
                }
            ],
            AttributeDefinitions=[
                {
                    'AttributeName': 'id',
                    'AttributeType': 'S'
                },
                {
                    'AttributeName': 'account_id',
                    'AttributeType': 'S'
                }
            ],
            ProvisionedThroughput={
                'ReadCapacityUnits': 5,
                'WriteCapacityUnits': 5
            }
        )
        table.wait_until_exists()
        
        return table
    except Exception as e:
        if e.response['Error']['Code'] == 'ResourceInUseException':
            # 이미 동일한 이름의 테이블이 존재하는 경우 에러 처리
            print("이미 동일한 이름의 테이블이 존재합니다.")
            # delete_table(name)
            # create_table(name)
            return dynamodb.Table(name)
        else:
            # 다른 종류의 클라이언트 에러가 발생한 경우 처리
            print("테이블 생성 중에 오류가 발생했습니다:", e)

if __name__ == '__main__':
    table = create_table("carts")
    print(table.meta)
    print("Table Name:", table.table_name)
    print("Table status:", table.table_status)
    print("Table ARN:", table.table_arn)

