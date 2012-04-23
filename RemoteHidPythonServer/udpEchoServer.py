#############################################################################
# Copyright 2012 Virtosu Sava 												#				
# Licensed under the Apache License, Version 2.0 (the "License");          	#
# you may not use this file except in compliance with the License.         	#
# You may obtain a copy of the License at 									#
# 																			#
# http://www.apache.org/licenses/LICENSE-2.0 								#
# 																			#
# Unless required by applicable law or agreed to in writing, software 		#
# distributed under the License is distributed on an "AS IS" BASIS, 		#
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 	#
# See the License for the specific language governing permissions and		#
# limitations under the License. 											#
#############################################################################

import socket
from threading import Thread
from configs import SERVER

class UDPEchoServer(Thread):
	'''This UDP Server is used only for echo back message from client
	The message with which server response back can be also specified.
	'''

	def __init__(self, port = SERVER.UDP_ECHO_PORT, response_msg = ''):
		Thread.__init__(self)
		self.running = True
		self.UDPSocket = socket.socket(socket.AF_INET,socket.SOCK_DGRAM)
		try:
			self.UDPSocket.bind(("", SERVER.UDP_ECHO_PORT))
		except: 
			print 'Error 16: Port are in use. Fail to run server.'	

	def stop_server(self):
		self.running = False
		# kick new socket connection to past accept step in main loop.
		temp_socket = socket.socket ( socket.AF_INET, socket.SOCK_DGRAM )
		temp_socket.connect( (SERVER.UDP_HOST, SERVER.UDP_ECHO_PORT) )

	def run (self):
		while self.running:
			data, address = self.UDPSocket.recvfrom(SERVER.MAX_BUFFER_SIZE)
			#empty string counts as False
			if response_msg:
				self.UDPSocket.sendto( response_msg , (address[0], address[1]) )
			else:
				self.UDPSocket.sendto( data , (address[0], address[1]) )
			#TODO: manage debug info
			print "( " ,address[0], " " , address[1] , " ) said : ", data