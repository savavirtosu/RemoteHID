#############################################################################
# Copyright 2012 Virtosu Sava                                               #
# Licensed under the Apache License, Version 2.0 (the "License");           #
# you may not use this file except in compliance with the License.          #
# You may obtain a copy of the License at                                   #
#                                                                           #
# http://www.apache.org/licenses/LICENSE-2.0                                #
#                                                                           #
# Unless required by applicable law or agreed to in writing, software       #
# distributed under the License is distributed on an "AS IS" BASIS,         #
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  #
# See the License for the specific language governing permissions and       #
# limitations under the License.                                            #
#############################################################################

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