from couchlib import CouchLib

configs = {}
configs['username'] = 'Administrator'
configs['password'] = 'aabdelm4'
configs['rest_endpoint'] = 'http://localhost:8091/'
configs['queryendpoint'] = 'http://localhost:8093/query/service'

class CONS:
    def __init__(self):
        self.username = 'Administrator'
        self.password = 'aabdelm4'
        self.rest_endpoint = 'http://localhost:8091/'
        self.queryendpoint = 'http://localhost:8093/query/service'

configs2 = CONS()

jsonobj = {
  "Companies": [
    "Pepsi Co"
  ],
  "ExpectedPay": 10000,
  "FurtherReadingLinks": [
    "https://www.usnews.com/education/best-colleges/accounting-major-overview"
  ],
  "Jobs": [
    "Accountant"
  ],
  "ProgramDescription": "you work in finances with numbers and calulcate invoices",
  "ProgramName": "Samoor",
  "Skills": [
    "Organization"
  ],
  "VideoLink": "https://www.youtube.com/watch?v=06DH-8QM6D0"
}

jsonobj2 = {
  "Companies": [
    "Samra Co", "GCo"
  ],
  "ExpectedPay": 50000,
  "VideoLink": "Samoora ana wenta walla"
}

mydb_instance = CouchLib(configs2)
res = mydb_instance.insert_json_doc(jsonobj["ProgramName"], "Programs", jsonobj)
res = mydb_instance.get_json_doc(jsonobj["ProgramName"], "Programs")
print(res)
res = mydb_instance.update_json_doc_direct(jsonobj["ProgramName"], "Programs", jsonobj2)
res = mydb_instance.get_json_doc(jsonobj["ProgramName"], "Programs")
print(res)
res = mydb_instance.query_db_with_field("Companies", ["Vodafone","Amazon"], "Programs")
print(res)
statement = 'FROM Programs AS BN WHERE BN.Companies = ["Vodafone","Amazon"] SELECT BN;'
res = mydb_instance.query_db(statement)
print(res)
res = mydb_instance.delete_json_doc(jsonobj["ProgramName"], "Programs")
res = mydb_instance.get_json_doc(jsonobj["ProgramName"], "Programs")
print(res)
