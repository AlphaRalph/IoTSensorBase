import time
import board # library for pins on board
import adafruit_dht # library for temperature and humidity sensor

dhtDevice = adafruit_dht.DHT22(board.D17, use_pulseio=False) # read data on pin 17

while True:
    try:
        dataTemp = dhtDevice.temperature # read temperature
        dataHum = dhtDevice.humidity # read humidity
        print(dataTemp, dataHum)
    except RuntimeError as error:
        # Errors happen fairly often, DHT's are hard to read, just keep going
        print(error.args[0])
        time.sleep(5.0)
        continue
    except Exception as error:
        dhtDevice.exit()
        raise error
    time.sleep(5.0) # maximum time for DHT22 = 2.0