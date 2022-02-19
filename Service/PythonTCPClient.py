import socket
import time
import sys
from random import seed
from random import random

def sendMessage():
    # Create a socket
    sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

    # Connect to the remote host and port
    sock.connect(("127.0.0.1", 8500))
    # Send a request to the host
    sock.send((sys.argv[1] + "\n").encode())

    # Get the host's response, no more than, say, 1,024 bytes
    response_data = sock.recv(1024)

    response_msg = response_data.decode()

    print(response_msg)
    
    # Terminate
    sock.close(  )

seed(int(sys.argv[4]))

for i in range(int(sys.argv[5])):
    
    short = float(sys.argv[2])
    long = float(sys.argv[3])
    
    if(random() > 0.5):
        for i in range(int(sys.argv[4])):
            sendMessage()
            time.sleep(short)
    else:
        for i in range(int(sys.argv[4])):
            sendMessage()
            time.sleep(long)

    
