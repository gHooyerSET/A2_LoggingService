# FILE : argParser.py
# PROJECT : A2 Logging Service
# PROGRAMMER : Gerritt Hooyer
# FIRST VERSION : 2022-02-15
# DESCRIPTION : A python arg parsing module
#               for the NAD Logging Service
#               assignment.

import string

HELP_FILE_PATH = "help.txt"
DEFAULT_PORT = 8500
DEFAULT_FILENAME = "a2log.txt"
DEFAULT_MAX_CLIENTS = 1024
ARG_JSON = "json"
ARG_SYSLOG = "syslog"
MODE_JSON = 0
MODE_SYSLOG = 1
MODE_CUSTOM = 2
INVALID_PORT = -1
ARGS_ARRAY_OFFSET = 0
ARG_OFFSET = 1

# NAME: ArgParser
# PURPOSE : Parses command line arguments and displays information related to those arguments
class ArgParser():

    displayedHelp = False
    port = DEFAULT_PORT
    maxClients = DEFAULT_MAX_CLIENTS
    fileName = DEFAULT_FILENAME
    mode = MODE_JSON
    outFormat = []

    # FUNCTION : printHelpMessage
    # DESCRIPTION : Prints a help message from the 'help.txt' file
    #               to the screen
    # PARAMETERS : N/A
    # RETURNS : N/A
    staticmethod
    def  printHelpMessage():
        # Open the text file with help information
        fHelp = open(HELP_FILE_PATH,"r")
        # Read the text from the file
        helpMsg = fHelp.read()
        # Display the contents to the screen
        print(helpMsg)
        # Close the file
        fHelp.close()
    
    # FUNCTION : parseArgs
    # DESCRIPTION : Parses the user's command-line arguments and stores
    #               them in the static variables in the ArgParser class
    # PARAMETERS : self - the instance of ArgParser
    #              *args - The list of arguments provided via command line as strings
    # RETURNS : N/A
    classmethod
    def parseArgs(self,*args):
        length = len(args[ARGS_ARRAY_OFFSET])
        for i in range(length):
            switch = args[ARGS_ARRAY_OFFSET][i]
            # Check if we have 2 or more arguments or are at the 2nd last arguments
            if(i < length - ARG_OFFSET and length != ARG_OFFSET):
                arg = args[ARGS_ARRAY_OFFSET][i + ARG_OFFSET]
                # Check for a custom port setting
                if(switch == "-p"):
                    try:
                        # Try to parse the port
                        self.port = int(arg)
                    except :
                        # Display an error on failure
                        print("Failed to parse port.\n")
                # Check for a custom # of max clients
                elif(switch == "-c"):
                    try:
                        # Try to parse the maximum # of clients
                        self.maxClients = int(arg)
                    except:
                        # Display an error on failure
                        print("Failed to parse max clients.\n")
                # See if they desire to change the output filename
                elif(switch == "-o"):
                    # Set the filename to the arg
                    self.fileName = arg
                # Check for a format switch tag
                elif(switch == "-f"):
                    if (arg == ARG_JSON):
                        # Set the output mode to JSON
                        ArgParser.mode = MODE_JSON
                    elif (arg == ARG_SYSLOG):
                        # Set the output mode to Syslog
                        ArgParser.mode = MODE_SYSLOG
                    else:
                        ArgParser.mode = MODE_CUSTOM
                        ArgParser.outFormat = arg.split(' ')
            # Check if the 'help message tag' was specified
            if(switch == "-h"):
                    ArgParser.printHelpMessage()
                    ArgParser.displayedHelp = True