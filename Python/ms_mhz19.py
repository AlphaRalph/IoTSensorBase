import time
import json # library for json strings
import mh_z19 # library for CO2 sensor

while True:
    try:  
        dataCO2 = json.dumps(mh_z19.read()).split() # sensor returns a dict format, converting to JSON and split it into a list
        dataCO2 = dataCO2[1] # use first element in list
        dataCO2 = float(dataCO2[:-1]) # delete last letter in string, convert it to float
        print(dataCO2)
    except:
        print(error.args[0])
        time.sleep(5.0)
        continue
    time.sleep(5.0)