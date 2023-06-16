from flask import Flask,jsonify,request

app = Flask(__name__)

statusLamp = False

statusLed = False
RLed = 0
GLed = 0
BLed = 0

TempDHT = 33.3
HumDHT = 15


@app.route('/lamp', methods = ['GET','POST'])
def lamp():
    global statusLamp

    if request.method == 'GET':
        data = {
            "status" : statusLamp
        }
  
        return jsonify(data)

    elif request.method == 'POST':
        statusLamp = request.json["status"]
  
        return "Ok"

@app.route('/led', methods = ['GET','POST'])
def led():
    global statusLed
    global RLed
    global GLed
    global BLed

    if request.method == 'GET':
        data = {
            "status" : statusLed,
            "R" : RLed,
            "G" : GLed,
            "B" : BLed
        }
  
        return jsonify(data)

    elif request.method == 'POST':
        statusLed = request.json["status"]
        RLed = request.json["R"]
        GLed = request.json["G"]
        BLed = request.json["B"]
  
        return "Ok"

@app.route('/dht', methods = ['GET'])
def dht():
    global TempDHT
    global HumDHT

    if request.method == 'GET':
        data = {
            "Temp" : TempDHT,
            "Hum" : HumDHT
        }
  
        return jsonify(data)

if __name__=='__main__':
    app.run(host="0.0.0.0",port="8080",debug=True)