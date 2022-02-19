# FILE : fileHandler.py
# PROJECT : A2 Logging Service
# PROGRAMMER : Gerritt Hooyer
# FIRST VERSION : 2022-02-15
# DESCRIPTION : A python file handler module.

# IMPORTS
import threading
import argParser
from os import path


# Declare the thread lock
lock = threading.Lock()

# Custom Format I/O Tags
TAG_CLIENT_ID = "clientId"
TAG_DATE = "date"
TAG_TIME = "time"
TAG_DEVICE_NAME = "devName"
TAG_APP_NAME = "appName"
TAG_PID = "pId"
TAG_ERROR_LEVEL = "errorLvl"
TAG_MESSAGE = "msg"

def resolveOutput(parser,request,msg):
    if(parser.mode == argParser.MODE_JSON):
        write(parser.fileName,msg)
    else:
        msg = createOutput(parser,request)
        write(parser.fileName,msg)

def createOutput(parser,request):
    retValue = ""
    for tag in parser.outFormat:
        if(tag == TAG_CLIENT_ID):
            retValue += str(request.clientID)
        elif(tag == TAG_DATE):
            retValue += str(request.date)
        elif(tag == TAG_TIME):
            retValue += str(request.time)
        elif(tag == TAG_DEVICE_NAME):
            retValue += str(request.devName)
        elif(tag == TAG_APP_NAME):
            retValue += str(request.appName)
        elif(tag == TAG_PID):
            retValue += str(request.pId)
        elif(tag == TAG_ERROR_LEVEL):
            retValue += str(request.errorLvl)
        elif(tag == TAG_MESSAGE):
            retValue += str(request.msg)
        else:
            retValue += tag
        retValue += " "
    return retValue

# FUNCTION : write
# DESCRIPTION : Writes a message to the specified file. Uses locks to be thread-safe
#               and will append to existing files, while creating a file if one doesn't
#               exist
# PARAMETERS : 
#       filename - string - the name of the file to write to
#       msg      - string - the message to be written
# RETURNS : N/A
def write(fileName,msg):
    # TITLE : Errors & Exceptions
    # AUTHOR : Python Official Documentation
    # DATE : Unspecified
    # VERSION : Unknown
    # AVAILABIILTY : https://docs.python.org/3/tutorial/errors.html
    
    # Acquire the lock to ensure thread-safe access to the file
    lock.acquire()
    try:
        # TITLE : Python File I/O
        # AUTHOR : TutorialsPoint
        # DATE : Unspecified
        # VERSION : Unknown
        # AVAILABIILTY : https://www.tutorialspoint.com/python/python_files_io.htm        
        # Open the file and write to it
        if path.exists(fileName) == False:
            #If the file doesn't exist, open in write mode
            fo = open(fileName,"w")
        else:
            #Otherwise, we append to the file
            fo = open(fileName,"a")
        fo.write(msg + "\n")
        # Then close it
        fo.close()
    except OSError:
        print("OS Error: {0}".format(err))
    except BaseException as err:
        print(f"Unexpected {err=},{type(err)=}")
        raise
    # Release the lock
    lock.release()