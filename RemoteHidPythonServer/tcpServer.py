
import socket
from threading import Thread
from configs import SERVER

class TCPServer(Thread):

	def __init__(self, port = SERVER.TCP_PORT):
		Thread.__init__(self)
		self.running = True
		self.TCPSocket = socket.socket(socket.AF_INET,socket.SOCK_STREAM)
		try:
			self.TCPSocket.bind((SERVER.TCP_HOST, SERVER.TCP_PORT))
			self.TCPSocket.listen(SERVER.MAX_NUM_INCOMING_CONNECTION)
		except: 
			print 'Error 37: Port are in use. Fail to run server.'	

	def stop_server(self):
		self.running = False
		# kick new socket connection to past accept step in main loop.
		temp_socket = socket.socket ( socket.AF_INET, socket.SOCK_STREAM )
		temp_socket.connect( (SERVER.TCP_HOST, SERVER.TCP_PORT) )

	def run (self):
		while self.running:
			print "tcp waiting for incoming connection..."
			result = self.TCPSocket.accept()
			print "after accept", result
			newSocket = result[0]
			remotePeerInfo = result[1]

			data = newSocket.recv(SERVER.MAX_BUFFER_SIZE)

			print "( " ,remotePeerInfo[0], " " , remotePeerInfo[1] , " ) said : ", data
			print "message accept was sent"