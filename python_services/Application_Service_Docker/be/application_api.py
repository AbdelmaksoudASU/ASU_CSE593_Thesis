from flask import Flask, jsonify, request
# from flask_cors import CORS
from couchlib import CouchLib
import uuid 

app = Flask(__name__)
#CORS(app, origins='*')


class CONS:
    def __init__(self):
        self.username = 'Administrator'
        self.password = 'password'
        self.rest_endpoint = 'http://db:8091/'
        self.queryendpoint = 'http://db:8093/query/service'


configs = CONS()
couch = CouchLib(configs)

# create application form
@app.route('/appform', methods=['POST'])
def create_application_form():
    data = request.get_json()
    key = data['university']    
    bucketname = "schemas"    
    result = couch.insert_json_doc(key, bucketname, data)
    return jsonify(result)

# update application form
@app.route('/appform', methods=['PATCH'])
def update_application_form():
    data = request.get_json()
    key = data['university']
    bucketname = "schemas"
    result = couch.update_json_doc_direct(key, bucketname, data)
    return jsonify(result)

# get application form
@app.route('/appform/<id>', methods=['GET'])
def get_application_form(id):
    key = id
    bucketname = "schemas"    
    result = couch.get_json_doc(key, bucketname)    
    return jsonify(result)

@app.route('/appform', methods=['GET'])
def get_all_resources():
    bucketname = "schemas"    
    resources = couch.get_all_json_docs(bucketname)
    return jsonify(resources)



# apply
@app.route('/student_application', methods=['POST'])
def create_student_application():
    data = request.get_json()
    key = str(uuid.uuid4())    
    data["application_id"] = key
    data["status"] = "under review"
    bucketname = "applications"    
    result = couch.insert_json_doc(key, bucketname, data)
    return jsonify(result)

# update application form
@app.route('/student_application/<id>', methods=['PATCH'])
def update_student_application(id):
    data = request.get_json()
    key = id
    bucketname = "applications"
    result = couch.update_json_doc_nested(key, bucketname, data)
    return jsonify(result)
    
# get applicant
@app.route('/student_application/<id>', methods=['GET'])
def get_student_application(id):
    bucketname = "applications"
    result = couch.get_json_doc(id, bucketname)
    return jsonify(result)


@app.route('/student_application_status/<id>', methods=['GET'])
def get_student_application_status(id):
    bucketname = "applications"
    result = couch.get_json_doc(id, bucketname)['status']
    return jsonify(result)

# update app id status
@app.route('/student_application_status/<id>', methods=['PATCH'])
def update_student_application_status(id):
    data = request.get_json()
    status = data['status']
    bucketname = "applications"
    result = couch.update_json_doc_direct(id, bucketname, {"status": status})
    return jsonify(result)



# update app id status
@app.route('/student_application_status', methods=['PATCH'])
def update_student_application_status_bulk():
    data = request.get_json()
    status = data['status']
    conditions = data['conditions']
    bucketname = "applications"
    result = couch.update_docs_satisfying_conditions(bucketname, conditions, {"status": status})
    return jsonify(result)



# filter students with policy
@app.route('/filter_student_applications', methods=['POST'])
def filter():
    data = request.get_json()
    conditions = data
    bucketname = "applications"    
    result = couch.select_docs_satisfying_conditions(bucketname, conditions)
    return jsonify(result)


    

if __name__ == "__main__":
    app.run(host='0.0.0.0')