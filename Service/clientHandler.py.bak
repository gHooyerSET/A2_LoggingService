import socket
import threading
import fileHandler
import argParser
from datetime import datetime

SECONDS_PER_HOUR = 3600
SECONDS_PER_MINUTE = 60
DEFAULT_RATE = 5.0
DEFAULT_PERIOD = 10.0
CLIENT_NOT_FOUND = -1
ALLOWANCE_CUTTOFF_SECONDS = 1.0

clientList = []

class Client():

    
    rate = DEFAULT_RATE
    period = DEFAULT_PERIOD
    
    # FUNCTION : __init__ (Constructor)
    # DESCRIPTION : Initializes an instance of the Client object
    # PARAMETERS : self - the instance of Client
    #              clientID - an ID for the client instance
    # RETURNS : N/A
    def __init__(self,clientID,allowance,lastCheck):
        self.clientID = clientID
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
    # DESCRIPTION: Finds an instance of a client with a matching clientID
    #              and retuerns it to the user.
    # PARAMETERS: clientID - String - The ID of the client's session
    # RETURNS: retValue - Client - an instance of the client object with the matching ID
    @staticmethod
    def findClient(clientID):
        # Create our variables for the item index
        # and the retValue
        index = CLIENT_NOT_FOUND
        retValue = None
        # Iterate through the list, searching for an item
        print(clientList)
        if clientList:
            for client in clientList:
                # If it is found, set index to 'i'
                if client.clientID == clientID:
                    retValue = client
        # If a client was found (i.e. index != -1)
        if retValue == None:
            # Otherwise, create a new client and append it to the list
            retValue = Client(clientID,Client.rate,datetime.now())
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



# FUNCTION : clientHandler
# DESCRIPTION : Handles a client request.
# PARAMETERS : sock - socket.socket -  The client socket
#              parser - argParser.ArgParser - The argParser.ArgParser object
# RETURNS : N/A
def handleClient(sock, parser):
    fileName = parser.fileName
    response = None
    # Code to read the server message and parse to the input
    # As well as setting msg and filename will go here    
    # Receive the client's log message
    request = sock.recv(1024)
    # Decode it
    msg = request.decode()
    # Then print it
    print(msg)
    # Parse the client ID here
    clientID = msg
    # Get our client instance
    client = Client.findClient(clientID)

    print(client.allowance)
    print(client.lastCheck)

    # If the rate limiter returns true, write to the log
    if Client.rateLimiter(client) == True:
        # Then create our response
        response = "Log entry created".encode()
        
        writeThread = threading.Thread(target = fileHandler.write, args = (fileName, msg))
        # Start the thread
        writeThread.start()
        # There's no need to join as the thread will terminate on its own
        # and we don't want to wait for it to finish
    else:
        response = "Rate limited for client".encode()
        print("Rate limited for client " + client.clientID)
    # Send a response message
    sock.send(response)
    # Then close the socket
    sock.close()
    # Print a disconnect message
    print("Disconnected from client.")
    # Create file write thread
    