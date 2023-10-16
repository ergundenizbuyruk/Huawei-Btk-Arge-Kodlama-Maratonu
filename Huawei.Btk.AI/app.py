from flask import Flask, jsonify, request
import main 

app = Flask(__name__)

@app.route("/api/ai", methods=["POST"])
def api():
   
    # return jsonify(main(request))
    data = request.get_json()
    return main(data)

if __name__ == '__main__':
    app.run(debug=True)
    
