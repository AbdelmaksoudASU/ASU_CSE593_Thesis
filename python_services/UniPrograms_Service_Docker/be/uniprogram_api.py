from flask import Flask, jsonify, request
from couchlib import CouchLib

app = Flask(__name__)

class CONS:
    def __init__(self):
        self.username = 'Administrator'
        self.password = 'password'
        self.rest_endpoint = 'http://db:8091/'
        self.queryendpoint = 'http://db:8093/query/service'


configs = CONS()
couch = CouchLib(configs)

# create Educational Entity
@app.route('/EducationalEntity/<type>/<id>', methods=['POST'])
def create_educational_entity(type, id):
    data = request.get_json()
    key = id    
    bucketname = type    
    result = couch.insert_json_doc(key, bucketname, data)
    return jsonify(result)

# update Educational Entity
@app.route('/EducationalEntity/<type>/<id>', methods=['PATCH'])
def update_educational_entity(type, id):
    data = request.get_json()
    key = id    
    bucketname = type   
    result = couch.update_json_doc_direct(key, bucketname, data)
    return jsonify(result)

# get Educational Entity
@app.route('/EducationalEntity/<type>/<id>', methods=['GET'])
def get_educational_entity(type, id):
    key = id    
    bucketname = type    
    result = couch.get_json_doc(key, bucketname)    
    return jsonify(result)

# get all Educational Entity of type
@app.route('/EducationalEntity/<type>', methods=['GET'])
def get_educational_entities(type):
    bucketname = type    
    result = couch.get_all_json_docs(bucketname)   
    return jsonify(result)

# get all Educational Entity satisfying conditions
@app.route('/EducationalEntityWithFilter/<type>', methods=['POST'])
def filter(type):
    data = request.get_json()
    bucketname = type    
    result = couch.select_docs_satisfying_conditions(bucketname, data)
    return jsonify(result)

# Delete Educational Entity
@app.route('/EducationalEntity/<type>/<id>', methods=['DELETE'])
def delete_educational_entity(type, id):
    key = id    
    bucketname = type    
    result = couch.delete_json_doc(key, bucketname)    
    return jsonify(result)

    

if __name__ == "__main__":
    app.run(host='0.0.0.0')