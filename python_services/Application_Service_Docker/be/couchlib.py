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


    def generate_set_clause(self, json_obj, parent_key):
        set_clauses = []
        for key, value in json_obj.items():
            if isinstance(value, dict):
                nested_set_clause = self.generate_set_clause(value, parent_key+key+'.')
                set_clauses.append(nested_set_clause)
            else:
                set_clauses.append(f"{parent_key}{key}={json.dumps(value)}")
        return ", ".join(set_clauses)
    
    
    def build_update_query(self, bucket_name, conditions, update_props):
        query = "UPDATE `" + bucket_name + "` SET "
        set_clause = ', '.join([f'{key}={json.dumps(value)}' for key, value in update_props.items()])
        query += set_clause
        query += " WHERE "
        where_clause = ' AND '.join([f'{key}={json.dumps(value)}' for key, value in conditions.items()])
        query += where_clause
        return query
    
    def build_select_query(self, bucket_name, conditions):
        query = "SELECT * FROM `" + bucket_name + "`"
        query += " WHERE "
        where_clause = ' AND '.join([f'{key}={json.dumps(value)}' for key, value in conditions.items()])
        query += where_clause
        return query

    def insert_json_doc(self, key, bucketname, json_obj):
        statement = f"INSERT INTO {bucketname} (KEY, VALUE) VALUES ('{key}', {json.dumps(json_obj)})"   
        json_res = self.query_db(statement)
        return {"status": json_res["status"], "result": json_res["result"], "key": key}
        
    def update_json_doc_direct(self, key, bucketname, json_obj):
        set_clause = ', '.join([f'{key}={json.dumps(value)}' for key, value in json_obj.items()])
        statement = f'UPDATE `{bucketname}` USE KEYS "{key}" SET {set_clause}'
        json_res = self.query_db(statement)
        return {"status": json_res["status"], "result": json_res["result"], "key": key}
         
    def add_to_json_array(self, key, bucketname, json_obj, array_key):
        statement = f'UPDATE `{bucketname}` USE KEYS "{key}" SET {array_key} = ARRAY_APPEND({array_key}, {json.dumps(json_obj)})'
        json_res = self.query_db(statement)
        print(statement)
        return {"status": json_res["status"], "result": json_res["result"], "key": key}
    
    def update_json_doc_nested(self, key, bucketname, json_obj):
        set_clause = self.generate_set_clause(json_obj, "")
        statement = f'UPDATE `{bucketname}` USE KEYS "{key}" SET {set_clause}'
        json_res = self.query_db(statement)
        return {"status": json_res["status"], "result": json_res["result"], "key": key}
    
    def update_docs_satisfying_conditions(self, bucketname, conditions, new_properties_values):
        statement = self.build_update_query(bucketname, conditions, new_properties_values)
        json_res = self.query_db(statement)
        return {"status": json_res["status"], "result": json_res["result"],}
        
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
        result =''
        if response.status_code == 200:
            document = 'success'
        else:
            document = 'fail'
            result = 'Error: {}'.format(response.content)
        return {"status": document, "result": result}
    
    def get_json_doc_detailed(self, key, bucketname):
        url = '{}pools/default/buckets/{}/docs/{}'.format(self.rest_endpoint, bucketname, key)
        response = requests.get(url, headers=self.headers, auth=(self.username, self.password))
        document = {}
        if response.status_code == 200:
            document = response.json()
        else:
            document = {'Error':'{}'.format(response.content)}
        return document
    def get_json_doc(self, key, bucketname):
        obj = self.get_json_doc_detailed(key, bucketname).get('json')
        if obj is None:
            return {"status": "failure", "result": "get failed"}
        else:
            pyobj = json.loads(obj)
            if 'Error' in pyobj:
                return {"status": "failure", "result": pyobj}
            else:
                return {"status": "success", "result":pyobj}
    
    def query_db(self, querystring):
        body = {
            'statement': querystring
        }
        response = requests.post(self.query_endpoint, headers=self.headers, 
                                 auth=(self.username, self.password), data=json.dumps(body))
        if response.status_code == 200:
            document = response.json()
            result = {"status": document["status"], "result": document["results"]}
        else:
            result = {"status": "failure", "result": {"Error": 'Error {}'.format(response.content)}}
        return result
    
    
    def query_db_with_field(self, known_property, known_propvalue, bucketname):
        statement = f"FROM {bucketname} AS BN WHERE BN.{known_property} = {json.dumps(known_propvalue)} SELECT BN;"
        return self.query_db(statement)
    
    def check_element_in_array(self, key, bucketname, json_obj, array_key, id_property):
        statement = f'FROM {bucketname} AS BN WHERE {json.dumps(json_obj)} IN BN.{array_key} AND BN.{id_property} = "{key}" SELECT BN;'
        print(statement)
        return self.query_db(statement)
    
    def select_docs_satisfying_conditions(self, bucketname, conditions):
        statement = self.build_select_query(bucketname, conditions)
        json_res = self.query_db(statement)
        return {"status": json_res["status"], "result": json_res["result"],}
    
    def get_all_json_docs(self, bucketname):
        statement = f"FROM {bucketname} SELECT *;"
        return self.query_db(statement)


