# FILE : argParser.py
# PROJECT : A2 Logging Service
# PROGRAMMER : Gerritt Hooyer
# FIRST VERSION : 2022-02-15
# DESCRIPTION : A python arg parsing module
#               for the NAD Logging Service
#               assignment.

DEFAULT_PORT = 8500
DEFAULT_FILENAME = "a2log.txt"
DEFAULT_MAX_CLIENTS = 1024
INVALID_PORT = -1
ARGS_ARRAY_OFFSET = 0
ARG_OFFSET = 1

class ArgParser():

    port = DEFAULT_PORT
    maxClients = DEFAULT_MAX_CLIENTS
    fileName = DEFAULT_FILENAME
    
    staticmethod
    def  printHelpMessage():
        print("")

    classmethod
    def parseArgs(self,*args):
        length = len(args[ARGS_ARRAY_OFFSET])
        for i in range(length):
            if(i != length - ARG_OFFSET):
                switch = args[ARGS_ARRAY_OFFSET][i]
                arg = args[ARGS_ARRAY_OFFSET][i + ARG_OFFSET]
                if(switch == "-p"):
                    try:
                        # Try to parse the port
                        self.port = int(arg)
                    except :
                        # Display an error on failure
                        print("Failed to parse port.\n")
                elif(switch == "-c"):
                    try:
                        # Try to parse the maximum # of clients
                        self.maxClients = int(arg)
                    except:
                        # Display an error on failure
                        print("Failed to parse max clients.\n")
                elif(switch == "-h"):
                    ArgParser.printHelpMessage()

    