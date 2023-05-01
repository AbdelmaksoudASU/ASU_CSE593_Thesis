from flask import Flask, jsonify, request
from flask_cors import CORS
from couchlib import CouchLib

app = Flask(__name__)
CORS(app, origins='*')

# class CONS:
#     def __init__(self):
#         self.username = 'Administrator'
#         self.password = 'aabdelm4'
#         self.rest_endpoint = 'http://localhost:48091/'
#         self.queryendpoint = 'http://localhost:48093/query/service'

# configs = {
#     "username": "Administrator",
#     "password": "aabdelm4",
#     "rest_endpoint": "http://localhost:48091/",
#     "queryendpoint": "http://localhost:48093/query/service"
# }

class CONS:
    def __init__(self):
        self.username = 'Administrator'
        self.password = 'password'
        self.rest_endpoint = 'http://localhost:38091/'
        self.queryendpoint = 'http://localhost:38093/query/service'


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
@app.route('/apply', methods=['POST'])
def apply():
    data = request.get_json()
    uni = data['uni']
    program = data['program']
    semester_year = data['semester_year']
    formresponse = data['formresponse']
    
    bucketname = "applications"
    key = f"{uni}_{program}_{semester_year}_{formresponse['id']}"
    #key = uuid , semester = datetime.now
    result = couch.insert_json_doc(key, bucketname, formresponse)
    
    
    return jsonify(result)# uuid app id

# filter with policy
# @app.route('/filter_with_policy', methods=['POST'])
# def filter_with_policy():
#     data = request.get_json()
#     uni = data['uni']
#     program = data['program']
#     semester_year = data['semester_year']
#     formresponse_array = data['formresponse_array']
    
#     bucketname = "applications"
    
#     # perform custom filtering based on the formresponse_array
#     # and return the filtered results as a list of dictionaries
    
#     return jsonify(filtered_results)

# get applicant
@app.route('/getapplicant', methods=['POST'])
def get_applicant():
    data = request.get_json()
    id = data['id']
    
    bucketname = "applications"
    result = couch.get_json_doc(id, bucketname)
    
    return jsonify(result)

# update app id status
@app.route('/update_appid_status', methods=['POST'])
def update_appid_status():
    data = request.get_json()
    id = data['id']
    status = data['status']
    
    bucketname = "applications"
    result = couch.update_json_doc_direct(id, bucketname, {"status": status})
    
    return jsonify(result)

    
# update app id list status
@app.route('/update_appid_list_status', methods=['POST'])
def update_appid_list_status():
    data = request.get_json()
    id_list = data['id_list']
    status = data['status']
    
    bucketname = "applications"
    for id in id_list:
        couch.update_json_doc_direct(id, bucketname, {"status": status})
    
    return jsonify({"message": "Status updated for all IDs"})

# get app id status
@app.route('/get_appid_status', methods=['POST'])
def get_appid_status():
    data = request.get_json()
    id = data['id']
    
    bucketname = "applications"
    result = couch.get_json_doc(id, bucketname)
    
    if result is None:
        return jsonify({"message": "No application found with that ID"})
    
    return jsonify({"status": result.get("status", "Unknown")})


if __name__ == '__main__':
    app.run(debug=True, port=9700)

