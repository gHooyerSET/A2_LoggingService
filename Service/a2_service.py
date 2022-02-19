# FILE : a2_service.py
# PROJECT : A2 Logging Service
# PROGRAMMER : Gerritt Hooyer
# FIRST VERSION : 2022-02-15
# DESCRIPTION : The logging service for our A2
#               Logging Service assignment.

#IMPORT LIBRARIES 
import sys
import socket
import threading
import fileHandler
import argParser
import clientHandler
import time



# FUNCTION : startServer(parser)
# DESCRIPTION : Starts an instance of the logger server.
# PARAMETERS : parser - argParser.ArgParser - an instance of an argParser.ArgParser object
# RETURNS : N/A
def startServer(parser):
    # TITLE : Python TCP Server
    # AUTHOR : O'Reilly Textbook
    # DATE : Unspecified
    # VERSION : Unknown
    # AVAILABIILTY : https://learning.oreilly.com/library/view/python-cookbook/0596001673/ch10s03.html#pythoncook-CHP-10-SECT-3.2
    
    print("Selected Port : " + str(parser.port))
    print("Max Clients : "+ str(parser.maxClients))

    cleanupStarted = False

    # Create a socket
    sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    # Ensure that the server can be restarted quickly if it terminates
    sock.setsockopt(socket.SOL_SOCKET,socket.SO_REUSEADDR,1)
    # Bind the server to the port
    sock.bind(('',parser.port))
    # Set the max number of clients
    sock.listen(parser.maxClients)    
    # Print a 'ready' message
    print("Server listening...")
    # loop waiting for connections
    try:
        while True:
            # Tickle the cleanup watchdog
            clientHandler.cleanClients()
            # Accept the connection
            cSock, address = sock.accept( )
            # Display a connected message
            print(str(address) + "connected\n")
            # Create the new client thread
            clientThread = threading.Thread(target = clientHandler.handleClient, args = (cSock, parser))
            # Start the client thread
            clientThread.start()
    except Exception as e:
        print(e)
    finally:
        # Close the server socket on exit
        sock.close()


# FUNCTION : main
# DESCRIPTION : Calls functions and methods to run the program
# PARAMETERS : N/A
# RETURNS : N/A
def main():
    # Create our ArgParser object
    parser = argParser.ArgParser()
    # Then parse our arguments
    parser.parseArgs(sys.argv)
    # Check if they displayed the help menu
    # (the server doesn't start if the help menu was displayed)
    if (parser.displayedHelp == False):
        # Start the server
        startServer(parser)

if __name__ == "__main__":
    main()