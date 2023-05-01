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

# create profile
@app.route('/Profile/<id>', methods=['POST'])
def create_profile(id):
    data = request.get_json()
    key = id    
    bucketname = 'profiles'    
    result = couch.insert_json_doc(key, bucketname, data)
    return jsonify(result)

# update profile
@app.route('/Profile/<id>', methods=['PATCH'])
def update_profile(id):
    data = request.get_json()
    key = id    
    bucketname = 'profiles'   
    result = couch.update_json_doc_direct(key, bucketname, data)
    return jsonify(result)

# get profile
@app.route('/Profile/<id>', methods=['GET'])
def get_profile(id):
    key = id    
    bucketname = 'profiles'    
    result = couch.get_json_doc(key, bucketname)    
    return jsonify(result)

# get profile
@app.route('/Profile/<id>/role', methods=['GET'])
def get_profile_role(id):
    key = id    
    bucketname = 'profiles'    
    result = couch.get_json_doc(key, bucketname)
    return_obj = {"status":"failure", "result":""}
    if result["status"] == "success":
        return_obj["status"] = "success"
        return_obj["result"] = result["result"]["role"]
    return jsonify(return_obj)

@app.route('/Profile/<id>/<field>', methods=['GET'])
def get_profile_field(id, field):
    key = id    
    bucketname = 'profiles'    
    result = couch.get_json_doc(key, bucketname)
    return_obj = {"status":"failure", "result":""}
    if result["status"] == "success":
        return_obj["status"] = "success"
        return_obj["result"] = result["result"][field]
    return jsonify(return_obj)

# get profile
@app.route('/Profile/<id>/check_accessibilty/<entity_type>/<entity>', methods=['GET'])
def check_accessibility(id, entity_type, entity):
    key = id    
    bucketname = 'profiles'
    result = couch.check_element_in_array(key, bucketname, entity, entity_type, "profile_id") 
    return_obj = {"status":"failure", "result":""}
    if result["status"] == "success":
        access_allowed = True
        if result['result'] == []:
            access_allowed = False
        return_obj = {"status":"success", "result":access_allowed}
    return jsonify(return_obj)

@app.route('/Profile/<id>/check_field/<entity_type>/<entity>', methods=['GET'])
def check_field(id, entity_type, entity):
    key = id    
    bucketname = 'profiles'
    result = couch.get_json_doc(key, bucketname) 
    return_obj = {"status":"failure", "result":""}
    if result["status"] == "success":
        access_allowed = False
        if result['result'][entity_type] == entity:
            access_allowed = True
        return_obj = {"status":"success", "result":access_allowed}
    return jsonify(return_obj)


# add new application
@app.route('/add_new_application/<id>', methods=['POST'])
def add_new_applications(id):
    data = request.get_json()
    key = id    
    bucketname = 'profiles'    
    result = couch.add_to_json_array(key, bucketname, data, "applications")
    return jsonify(result)


# Delete profile
@app.route('/Profile/<id>', methods=['DELETE'])
def delete_profile(id):
    key = id    
    bucketname = 'profiles'    
    result = couch.delete_json_doc(key, bucketname)    
    return jsonify(result)

@app.route('/Profile/set_quiz_result/<id>', methods=['PATCH'])
def update_profile_quiz_result(id):
    data = request.get_json()
    key = id    
    bucketname = 'profiles'   
    result = couch.update_json_doc_direct(key, bucketname, data)
    return jsonify(result)

    

if __name__ == "__main__":
    app.run(host='0.0.0.0')