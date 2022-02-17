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

def clientHandler():
    fileName = ""
    msg = ""    
    # Code to read the server message and parse to the input
    # As well as setting msg and filename will go here
    
    # Create file write thread
    writeThread = threading.Thread(target = fileHandler.write, args = (fileName, msg))
    # Start the thread
    writeThread.start()
    # There's no need to join as the thread will terminate on its own
    # and we don't want to wait for it to finish

def main():
    