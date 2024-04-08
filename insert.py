from dynamoClient import DynamoClient
from datetime import datetime

def insertItems(name=None):
    if name==None:
        print("error :: table name empty")
        return 
    dynamodb = DynamoClient.get_resource().Table(name)

    now = datetime.now()
    dynamodb.put_item(Item={
        'id': 'testid',
        'account_id':'testaccountid',
        'product_id':'testproductid',
        'options':{
            'option1':'option1',
            'option2':3
        },
        'amount':3,
        'created_on' : str(now),
        'last_updated_on': str(now)
    })
    return dynamodb


if __name__ == '__main__':
    table = insertItems("carts")
    response = table.scan()
    items = response['Items']
    print(items)
    # print(table.get_item(Key={
    #     'id':'testid',
    #     'account_id':'testaccountid'
    # }))

