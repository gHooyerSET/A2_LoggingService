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


def clientHandler(sock, parser):
    fileName = parser.fileName
    msg = ""
    # Code to read the server message and parse to the input
    # As well as setting msg and filename will go here    
    # Receive the client's log message
    msg = sock.recv(1024)
    # Send a response message
    sock.send("Log entry created.")
    # Then close the socket
    sock.close()
    # Print a disconnect message
    print("Disconnected from client.")
    # Create file write thread
    writeThread = threading.Thread(target = fileHandler.write, args = (fileName, msg))
    # Start the thread
    writeThread.start()
    # There's no need to join as the thread will terminate on its own
    # and we don't want to wait for it to finish

def startServer(parser):
    # TITLE : Python TCP Server
    # AUTHOR : O'Reilly Textbook
    # DATE : Unspecified
    # VERSION : Unknown
    # AVAILABIILTY : https://learning.oreilly.com/library/view/python-cookbook/0596001673/ch10s03.html#pythoncook-CHP-10-SECT-3.2
    
    print("Selected Port : " + str(parser.port))
    print("Max Clients : "+ str(parser.maxClients))

    # Create a socket
    sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    # Ensure that the server can be restarted quickly if it terminates
    sock.setsockopt(socket.SOL_SOCKET,socket.SO_REUSEADDR,1)
    # Bind the server to the port
    sock.bind(('',parser.port))
    # Set the max number of clients
    sock.listen(parser.maxClients)
    # loop waiting for connections
    try:
        while True:
            print("Listening...")
            # Accept the connection
            cSock, address = sock.accept( )
            # Display a connected message
            print(str(address) + "connected\n")
            # Create the new client thread
            clientThread = threading.thread(target = clientHandler, args = (cSock, parser))
            # Start the client thread
            clientThread.start()
    except:
        printf("Server error.")
    finally:
        # Close the server socket on exit
        sock.close()



def main():
    # Create our ArgParser object
    parser = argParser.ArgParser()
    # Then parse our arguments
    parser.parseArgs(sys.argv)
    # Start the server
    startServer(parser)

if __name__ == "__main__":
    main()