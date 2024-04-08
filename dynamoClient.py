import boto3
import threading

class DynamoClient:
    _instance = None
    _lock = threading.Lock()

    def __new__(cls, *args, **kwargs):
        if not cls._instance:
            with cls._lock:
                if not cls._instance:
                    cls._instance = super().__new__(cls)
                    cls._instance._init_client()
        return cls._instance

    def _init_client(self):
        # credential : IAM 사용
        role_arn = 'arn:aws:iam::975050314685:role/DynamoDBManager'
        # Boto3 STS(Security Token Service) 클라이언트 생성
        sts_client = boto3.client('sts')

        # assume_role 메서드를 사용하여 IAM 역할 정보 생성
        assumed_role_object = sts_client.assume_role(
            RoleArn=role_arn,
            RoleSessionName='AssumedRoleSession'
        )

        # DynamoDB 클라이언트를 생성
        # region_name(cloud) or endpoint_url(local) 필수
        self._dynamodb = boto3.client(
            'dynamodb',
            region_name='ap-northeast-2',
            aws_access_key_id=assumed_role_object['Credentials']['AccessKeyId'],
            aws_secret_access_key=assumed_role_object['Credentials']['SecretAccessKey'],
            aws_session_token=assumed_role_object['Credentials']['SessionToken']
        )
        
    @classmethod
    def get_client(cls):
        if not cls._instance:
            cls._instance = cls()
        return cls._instance._dynamodb