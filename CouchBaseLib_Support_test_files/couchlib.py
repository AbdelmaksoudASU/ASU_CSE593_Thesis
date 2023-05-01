import requests
import json

class CouchLib:
    def __init__(self, configs):
        self.username = configs.username#'Administrator'
        self.password = configs.password#'aabdelm4'
        self.rest_endpoint = configs.rest_endpoint # 'http://localhost:8091/'
        self.query_endpoint = configs.queryendpoint # 'http://localhost:8093/query/service'
        self.headers = {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        } 
    def insert_json_doc(self, key, bucketname, json_obj):
        statement = f"INSERT INTO {bucketname} (KEY, VALUE) VALUES ('{key}', {json.dumps(json_obj)})"   
        return self.query_db(statement)
    def update_json_doc_direct(self, key, bucketname, json_obj):
        set_clause = ', '.join([f'{key}={json.dumps(value)}' for key, value in json_obj.items()])
        statement = f'UPDATE `{bucketname}` USE KEYS "{key}" SET {set_clause}'
        return self.query_db(statement)
    def update_json_doc_rudc(self, key, bucketname, field_updates):
        jsonobj = self.get_json_doc(key, bucketname)
        for field in field_updates:
            if field in jsonobj:
                jsonobj[field] = field_updates[field]
        self.delete_json_doc(key, bucketname)
        return self.insert_json_doc(key, bucketname, jsonobj)
    def delete_json_doc(self, key, bucketname):
        url = '{}pools/default/buckets/{}/docs/{}'.format(self.rest_endpoint, bucketname, key)
        response = requests.delete(url, headers=self.headers, auth=(self.username, self.password))
        if response.status_code == 200:
            document = {'success:':'document deleted successfully'}
        else:
            document = {'Error:':'{}'.format(response.content)}
        return document
    def get_json_doc_detailed(self, key, bucketname):
        url = '{}pools/default/buckets/{}/docs/{}'.format(self.rest_endpoint, bucketname, key)
        response = requests.get(url, headers=self.headers, auth=(self.username, self.password))
        document = {}
        if response.status_code == 200:
            document = response.json()
        else:
            document = {'Error:':'{}'.format(response.content)}
        return document
    def get_json_doc(self, key, bucketname):
        obj = self.get_json_doc_detailed(key, bucketname).get('json')
        if obj is None:
            return obj
        else:
            return json.loads(obj)
    def query_db(self, querystring):
        body = {
            'statement': querystring
        }
        response = requests.post(self.query_endpoint, headers=self.headers, 
                                 auth=(self.username, self.password), data=json.dumps(body))
        if response.status_code == 200:
            document = response.json()
        else:
            document = {'Error:':'{}'.format(response.content)}
        return document
    def query_db_with_field(self, known_property, known_propvalue, bucketname):
        statement = f"FROM {bucketname} AS BN WHERE BN.{known_property} = {json.dumps(known_propvalue)} SELECT BN;"
        return self.query_db(statement)
    
    def get_all_json_docs(self, bucketname):
        statement = f"FROM {bucketname} SELECT *;"
        return self.query_db(statement)


