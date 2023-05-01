# please note I only recieve json data. From front end I recieve essays, certificates either 
# as text in text boxes or files and images in base64 string format. Thus no need for file system
from flask import Flask, request, jsonify
import requests
from flask_cors import CORS
import json
import os
from couchlib import CouchLib

app = Flask(__name__)
CORS(app, origins='*')

class CONS:
    def __init__(self):
        self.username = 'Administrator'
        self.password = 'aabdelm4'
        self.rest_endpoint = 'http://localhost:8091/'
        self.queryendpoint = 'http://localhost:8093/query/service'


configs = CONS()
db = CouchLib(configs)

@app.route('/api/post', methods=['POST'])
def post_endpoint():
    data = request.get_json() # get the JSON data from the request
    result = db.insert_json_doc(data["key"], data["bucketname"], data["jsonobj"])
    return result
    # # do something with the data, e.g. convert it to a Python object
    # result = {"message": "Data received successfully"}
    # return jsonify(result), 200 # return a JSON response with HTTP status code 200

@app.route('/api/get', methods=['GET'])
def get_endpoint():
    # generate some data to return as JSON
    data = {"name": "John Doe", "age": 30, "email": "john.doe@example.com"}
    return jsonify(data), 200 # return the data as a JSON response with HTTP status code 200



@app.route('/api/create_profile', methods=['POST'])
def create_profile():
    data = request.get_json() # get the JSON data from the request
    result = db.insert_json_doc(data["profile_id"], 'profiles', data["profile_data"])
    return result

@app.route('/api/get_profile_by_id', methods=['GET'])
def get_profile_by_id():
    data = request.get_json() # get the JSON data from the request
    jsonobj = db.get_json_doc_only(data["profile_id"], 'profiles')
    # jsonobj = db.get_json_doc_only(profile_id,'profiles')
    return jsonobj

@app.route('/api/get_profile_role', methods=['GET'])   
def get_profile_role():
    data = request.get_json() # get the JSON data from the request
    jsonobj = db.get_json_doc_only(data["profile_id"], 'profiles')
    if jsonobj is None:
        return None
    return jsonobj['Role']

@app.route('/api/get_profile_fields', methods=['GET'])
def get_profile_fields(fields):
    data = request.get_json() # get the JSON data from the request
    fields = data["fields"]
    jsonobj = db.get_json_doc_only(data["profile_id"], 'profiles')
    if jsonobj is None:
        return None
    result = {}
    for field in fields:
        if field in jsonobj:
            result[field] = jsonobj[field]
    return result

@app.route('/api/update_profile', methods=['POST'])
def update_profile():
    data = request.get_json() # get the JSON data from the request
    result = db.update_json_doc_fields(data["profile_id"], 'profiles', data["field_updates"])
    return result

@app.route('/api/delete_profile', methods=['DELETE'])
def delete_profile():
    data = request.get_json() # get the JSON data from the request
    jsonobj = db.delete_json_doc(data["profile_id"], 'profiles')
    return jsonobj

@app.route('/api/Check_University_And_Program_Accessibility', methods=['GET'])
def Check_University_And_Program_Accessibility():
    data = request.get_json() # get the JSON data from the request
    profile = db.get_json_doc_only(data["profile_id"], 'profiles')
    datareq = data['type']
    query_entity = data['query_entity']
    if query_entity in profile[datareq]:
        return jsonify({"allowed": True}), 200
        return {"allowed": True}
    else:
        return jsonify({"allowed": False}), 200

if __name__ == '__main__':
    app.run(debug=True, port=9500)

