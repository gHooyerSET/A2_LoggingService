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
TAG_ERROR_TYPE = "errorType"
TAG_MESSAGE = "msg"
TAG_WHITESPACE = "\s"
#Log Levels
LOGLEVEL_DEBUG_FLOOR    = 0
LOGLEVEL_INFO_FLOOR     = 10000
LOGLEVEL_WARNING_FLOOR  = 20000
LOGLEVEL_ERROR_FLOOR    = 30000
LOGLEVEL_FATAL_FLOOR    = 40000
LOGLEVEL_CEILING        = 50000
LOGLEVEL_DEBUG = "debug"
LOGLEVEL_INFO = "info"
LOGLEVEL_WARNING = "warning"
LOGLEVEL_ERROR = "error"
LOGLEVEL_FATAL = "fatal"
LOGLEVEL_UNK = "unknown"

# FUNCTION : resolveOutput
# DESCRIPTION : Determines which output type to use based on
#               the command line arguments
# PARAMETERS : parser - the ArgParser object for this server instance
#              request - The client's request and related data
#              msg - The client's original JSON message as a string
# RETURNS : N/A
def resolveOutput(parser,request,msg):
    # Check the mode and set msg accordingly
    if(parser.mode == argParser.MODE_JSON):
        pass
    if(parser.mode == argParser.MODE_SYSLOG):
        msg = createSyslogOutput(request)
    else:
        msg = createOutput(parser,request)
    # Then write msg to the file
    write(parser.fileName,msg)

# FUNCTION : getErrorType
# DESCRIPTION : Uses an error level to return the matching error type
# PARAMETERS : errorLvl - an integer representing the error type and priority
# RETURNS : retValue - a string with the matching error type     
def getErrorType(errorLvl):
    retValue = ""
    # Read the error level by range checking
    # and set the retValue to the corresponding string
    if(errorLvl >= LOGLEVEL_DEBUG_FLOOR and errorLvl < LOGLEVEL_INFO_FLOOR):
        retValue = LOGLEVEL_DEBUG
    elif(errorLvl >= LOGLEVEL_INFO_FLOOR and errorLvl < LOGLEVEL_WARNING_FLOOR):
        retValue = LOGLEVEL_INFO
    elif(errorLvl >= LOGLEVEL_WARNING_FLOOR and errorLvl < LOGLEVEL_ERROR_FLOOR):
        retValue = LOGLEVEL_WARNING
    elif(errorLvl >= LOGLEVEL_ERROR_FLOOR and errorLvl < LOGLEVEL_FATAL_FLOOR):
        retValue = LOGLEVEL_ERROR
    elif(errorLvl >= LOGLEVEL_FATAL_FLOOR and errorLvl < LOGLEVEL_CEILING):
        retValue = LOGLEVEL_FATAL
    else:
        retValue = LOGLEVEL_UNK
    
    return retValue

# FUNCTION : createOutput
# DESCRIPTION : Creates an output string using the custom user format
# PARAMETERS : parser - the ArgParser object for this server instance
#              request - The client's request and related data# RETURNS : retValue - a string with the matching error type 
# RETURNS : retValue - a string of the desired output format with data inserted
def createOutput(parser,request):
    retValue = ""
    # Iterate through each tag in the format
    for tag in parser.outFormat:
        # Append the matching value to the tag
        if(tag == TAG_CLIENT_ID):
            retValue += str(request.clientId)
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
        elif(tag == TAG_ERROR_TYPE):
            retValue += getErrorType(request.errorLvl)
        elif(tag == TAG_WHITESPACE):
            retValue += " "
        # If the 'tag' doesn't match anything, simply add it as text
        else:
            retValue += tag
        
    return retValue

# FUNCTION : createSyslogOutput()
# DESCRIPTION : Creates a log string based on data in the client request
#               in a syslog-esque format
# PARAMETERS : 
#       request - Request - an object containing information related to the client request
# RETURNS : N/A
def createSyslogOutput(request):
    # Build the syslog-style output
    retValue = "<{errorLvl}> {date}T{time} ".format(errorLvl=request.errorLvl,date=request.date,time=request.time)
    retValue += "{devName} {appName} {pId} ".format(devName=request.devName,appName=request.appName,pId=request.pId)
    retValue += "{clientId} - BOM {msg}".format(clientId=request.clientId,msg=request.msg)
    # Return the resultant string
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