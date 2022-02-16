# FILE : fileHandler.py
# PROJECT : A2 Logging Service
# PROGRAMMER : Gerritt Hooyer
# FIRST VERSION : 2022-02-15
# DESCRIPTION : A python file handler module.

# IMPORTS
import threading
from os import path

# Declare the thread lock
lock = threading.Lock()

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
        fo.write(msg)
        # Then close it
        fo.close()
    except OSError:
        print("OS Error: {0}".format(err))
    except BaseException as err:
        print(f"Unexpected {err=},{type(err)=}")
        raise
    # Release the lock
    lock.release()