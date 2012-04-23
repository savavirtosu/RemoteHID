
import socket
from threading import Thread
from configs import SERVER

import win32api
import win32com.client

class UDPServer(Thread):

	def __init__(self, port = SERVER.UDP_PORT):
		Thread.__init__(self)
		self.running = True
		self.UDPSocket = socket.socket(socket.AF_INET,socket.SOCK_DGRAM)
		try:
			self.UDPSocket.bind(("", SERVER.UDP_PORT))
			self.shell = win32com.client.Dispatch("WScript.Shell")
		except: 
			print 'Error 44: Port are in use. Fail to run server.'	

	def stop_server(self):
		self.running = False
		# kick new socket connection to past accept step in main loop.
		temp_socket = socket.socket ( socket.AF_INET, socket.SOCK_DGRAM )
		temp_socket.connect( (SERVER.UDP_HOST, SERVER.UDP_PORT) )

	def run (self):
		print "udp server waiting for incoming connection..."
		while self.running:
			#print "udp server waiting for incoming connection..."
			data, address = self.UDPSocket.recvfrom(SERVER.MAX_BUFFER_SIZE)
			print "( " ,address[0], " " , address[1] , " ) said : ", data

			#TODO: eliminate all testing code.
			try:
				x,y,z = data.split(':')
				if float(z) > -0.5:
					self.shell.SendKeys("{UP}")
				else:
					self.shell.SendKeys("{DOWN}")

				if float(y) > 0:
					self.shell.SendKeys("{LEFT}")
				else:
					self.shell.SendKeys("{RIGHT}")
 			except ValueError:
				print data


			#self.UDPSocket.sendto( "accept", (address[0], address[1]) )
			#print "message accept was sent"