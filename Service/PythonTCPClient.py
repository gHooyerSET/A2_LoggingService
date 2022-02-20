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
    sock.send(("{\"clientID\":\"CFCD208495D565EF66E7DFF9F98764DA\",\"date\":\"18-24-2022\",\"time\":\"02:24:32\",\"devName\":\"DESKTOP-3C8HQUC\",\"appName\":\"A2_TestClient\",\"pId\":38452,\"errorLvl\":1,\"msg\":\"hello\"}").encode())
    #sock.send(("A Bunch of junk!!!!").encode())
    # Get the host's response, no more than, say, 1,024 bytes
    response_data = sock.recv(1024)

    response_msg = response_data.decode()

    print(response_msg)
    
    # Terminate
    sock.close(  )

sendMessage()

    
