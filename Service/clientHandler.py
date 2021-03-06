# FILE : clientHandler.py
# PROJECT : A2 Logging Service
# PROGRAMMER : Gerritt Hooyer
# FIRST VERSION : 2022-02-15
# DESCRIPTION : Handles client requests.

import socket
import threading
import fileHandler
import argParser
import json
import time
from datetime import datetime

SECONDS_PER_HOUR = 3600
SECONDS_PER_MINUTE = 60
DEFAULT_RATE = 5.0
DEFAULT_PERIOD = 10.0
CLIENT_NOT_FOUND = -1
ALLOWANCE_CUTTOFF_SECONDS = 1.0
CLIENT_SESSION_TIMEOUT = 30.0
TIME_BETWEEN_CLEANUP = 15
PID_CUTOFF = 0
clientList = []

# NAME: Request
# PURPOSE : Stores request data in an easy-to-reference object
class Request():    
    def __init__(self,clientId,date,time,devName,appName,pId,errorLvl,msg):
        self.clientId = clientId
        self.date = date
        self.time = time
        self.devName = devName
        self.appName = appName
        self.pId = pId
        self.errorLvl = errorLvl
        self.msg = msg

# NAME: Client
# PURPOSE : Stores information related to a client's session
class Client():
    rate = DEFAULT_RATE
    period = DEFAULT_PERIOD
    cleanupCheck = datetime.now()

    # FUNCTION : __init__ (Constructor)
    # DESCRIPTION : Initializes an instance of the Client object
    # PARAMETERS : self - the instance of Client
    #              clientId - an ID for the client instance
    # RETURNS : N/A
    def __init__(self,clientId,allowance,lastCheck):
        self.clientId = clientId
        self.allowance = allowance
        self.lastCheck = lastCheck

    
    # FUNCTION: addClient
    # DESCRIPTION: Adds a client instance to the list
    # PARAMETERS: client - Client - The instance of a Client object to be inserted
    # RETURNS: N/A
    @staticmethod
    def addClient(client):
        clientList.append(client)

    

    # FUNCTION: findClient
    # DESCRIPTION: Finds an instance of a client with a matching clientId
    #              and retuerns it to the user.
    # PARAMETERS: clientId - String - The ID of the client's session
    # RETURNS: retValue - Client - an instance of the client object with the matching ID
    @staticmethod
    def findClient(clientId):
        # Create our variables for the item index
        # and the retValue
        index = CLIENT_NOT_FOUND
        retValue = None
        # Iterate through the list, searching for an item
        if clientList:
            for client in clientList:
                # If it is found, set index to 'i'
                if client.clientId == clientId:
                    retValue = client
        # If a client was found (i.e. index != -1)
        if retValue == None:
            # Otherwise, create a new client and append it to the list
            retValue = Client(clientId,Client.rate,datetime.now())
            clientList.append(retValue)
            
        return retValue

    # FUNCTION : rateLimiter
    # DESCRIPTION : Limits the rate of requests for a particular client
    # PARAMETERS : self - the instance of Client
    # RETURNS : N/A
    @staticmethod
    def rateLimiter(client):
        # TITLE : Token Bucket Implementation in Python
        # AUTHOR : cweiske and Antti Huima
        # DATE : Original: Mar 20, 2009. Updated: Aug 5, 2016
        # VERSION : Unknown
        # AVAILABIILTY : https://stackoverflow.com/questions/667508/whats-a-good-rate-limiting-algorithm
        # Set our return value to false
        retValue = False
        # Get the current time
        current = datetime.now()
        # Subtract that from the last check to 
        # get the amount of time passed
        timePassed = current - client.lastCheck
        # Set the lastCheck to the current time.
        client.lastCheck = current
        # Add an amount to the allowance equal to to the # of seconds passed
        # multiplied by the ratio of the rate to the period
        client.allowance += float(timePassed.seconds) * (Client.rate / Client.period)
        # If the allowance is greater than the rate
        if (client.allowance > Client.rate):
            # We reset it to the rate to throttle the connection
            client.allowance = Client.rate
        # If the allowance is greater than the cutoff (1 second)
        if (client.allowance > ALLOWANCE_CUTTOFF_SECONDS):
            # Set the return value to true, the bucket isn't full
            retValue = True
            # Then subtract the cutoff from the allowance
            client.allowance -= ALLOWANCE_CUTTOFF_SECONDS
        # Return our retValue
        return retValue

# FUNCTION : cleanClients()
# DESCRIPTION : Watchdog timer that cleans up clients after a set amount of time
#               since their last request
# PARAMETERS : self - the instance of Client
# RETURNS : N/A
def cleanClients():   
        # Get the current time
        current = datetime.now()
        # Get the time passed
        timePassed = current - Client.cleanupCheck
        # If the time passed is >= the time between cleanups, clean up
        if (float(timePassed.seconds) >= TIME_BETWEEN_CLEANUP):
            # Set the cleanupCheck to the current time
            Client.cleanupCheck = current
            # Iterate through the client list
            for client in clientList:
                # Now set timePassed to the client's value
                timePassed = current - client.lastCheck
                # If the client hasn't made a request in 30 seconds or more,
                # close the session to save memory
                if (float(timePassed.seconds) >= CLIENT_SESSION_TIMEOUT):
                    clientList.remove(client)

# FUNCTION : clientHandler
# DESCRIPTION : Handles a client request.
# PARAMETERS : sock - socket.socket -  The client socket
#              parser - argParser.ArgParser - The argParser.ArgParser object
# RETURNS : N/A
def handleClient(sock, parser, address):
    fileName = parser.fileName
    response = None
    try:
        # Code to read the server message and parse to the input
        # As well as setting msg and filename will go here    
        # Receive the client's log message
        request = sock.recv(1024)
        # Decode it
        msg = request.decode()
        # Create a request object from the received msg
        # by first creating our dict via deserialzing the JSON string
        request_dict = json.loads(msg)
        # Then turn it into a Request object
        request = Request(**request_dict)
        # Get our client instance
        client = Client.findClient(request.clientId)
        # If the rate limiter returns true, write to the log
        if Client.rateLimiter(client) == True:
            # Clean the request
            request = checkRequest(request,address)
            # Reset the dictionary to the new request
            request_dict = request.__dict__            
            # Set the msg to a serialized version of the current request
            # The default tag turns any non-serializable items into strings
            msg = json.dumps(request_dict,default=str)
            # Create file write thread
            writeThread = threading.Thread(target = fileHandler.resolveOutput, args = (parser,request,msg))
            # Start the thread
            writeThread.start()
            # There's no need to join as the thread will terminate on its own
            # and we don't want to wait for it to finish
            # Then create our response
            response = "Log entry created.".encode()
        else:
            response = "Rate limited for client".encode()
            print("Rate limited for Client " + client.clientId + "\n")
        # Send a response message
        sock.send(response)        
        # Print a disconnect message
        print("Disconnected from client.\n")
    except Exception as e:
        # If any operation fails, print our exception message
        print(e)
    finally:
        # Then close the socket
        sock.close()

# FUNCTION : checkRequest
# DESCRIPTION : Cleans up a request that may have default values left in it to provide
#               better information.
# PARAMETERS : request - Request - The client's request
#              address - address - The client's IP information
# RETURNS : N/A   
def checkRequest(request, address):
    # Check if the value is default, and
    # if so, overwrite it
    if request.date == None:
        request.date = datetime.utcnow().date()
    if request.time == None:
        request.time = datetime.utcnow().time()
    if request.devName == None:
        request.devName = "Unknown device @" + str(address)
    if request.appName == None:
        request.appName = "Unknown application @ " + str(address)
    if request.pId <= PID_CUTOFF:
        request.pId = "ERROR NO PID PROVIDED"
    if request.errorLvl < fileHandler.LOGLEVEL_DEBUG_FLOOR or request.errorLvl > fileHandler.LOGLEVEL_CEILING:
        request.errorLvl = fileHandler.LOGLEVEL_DEBUG_FLOOR
    if request.msg == None:
        request.msg = "No message provided."
    return request
