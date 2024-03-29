<div id="top"></div>

<br />

## First Ideas

### In our first meeting we wrote down some keywords.

* soon it was clear to create a Git-Repo for sourcecode management.
* JSON is a handy format to send data like our measurement data
* NB-IoT (NarrowBand-IoT) sould be the technology to send data
* see more topics 

![Topics we discussed][topics]

### Second Meeting with first concept

* Our initial concept was to use a Raspberry to install MicroServices to aquire measurement data
* Measurement data has to be written in database
* Gateway has to read data from Database and write it to MQTT
* Data from MQTT are stored in cloud-DB
* Data from cloud-DB are visualized

![Idea of Architecture][architecture]

### We shared the responsibility:

* Data acquisition  ( written in Python )
* DeviceGateway ( written in C# )
* Data storage in cloud DB
* Data visualisation

For more information follow the details at <a href="IMPLEMENTATION_OV.md">implementation-details</a>

### How to read data from sensor

In the first discussion a decision should be made on how the sensors should be implemented.
The choice was between an implementation directly in the C# gateway or as a microservice outside of the gateway programming.

* Using a C# implementation inside the gateway solution should avoid persistence of measured values on Raspberry-Pi.
* Implementing a micro-service for each sensor would make the solutuion more flexible.
  Micro services can be implemented in any programming language.

We have decided to implement the sensors as MicroServices.
We defined to program the microservices in Python as one team member has experience with this programming language.


### How to transfer data from MicroService to Cloud - Step 1

For the purpose of data security, measured values must be persistently buffered.
This works easiest in a lean database.
At the beginning of the discussion, we wanted to use MariaDB.
It can be used with simple SQL statements. 
Advantages of this database are 
* Easy installation on Raspberry
* Easy to use in C# code as there are many well-documented application examples.

However, it is an SQL database, which does not bring any advantage with the simplicity of our data.
We do not need any relations between sets of data.
For this reason, we went looking for a NO-SQL database.
In combination with Raspberry-Pi we decided to use a MONGO-DB.
While using MONGO-DB we got aware of some difficuties.
* Old Mongo DBs no longer work with the current C# drivers.
* new Mongo DBs cannot be easily installed on the latest Raspberry OS versions
* retrieving data from Mongo-DB does not work with the well known syntax of SQL-Selects.

### How to transfer data from MicroService to Cloud - Step 2

A gateway has to be implemented to establish the connection between sensor implementation and the cloud.
In order to remain as flexible as possible in the future several inbound and outbound channels should be parameterisable.

An Inbound-channel has to be able to do the following:
* connect to a datasource
* read values from datasource
* deliver data to a gateway
* mark data as processed when the gateway has completed the process

An Outbound-channel has to be able to do the following:
* connect to a data-sink
* write data into data-sink
* inform gateway about process-state

[topics]: images/Topics.png
[architecture]: images/Architecture.png