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

import win32api
import win32com.client
import win32con

import ctypes as ct
from win32con import SW_MINIMIZE, SW_RESTORE
from win32ui import FindWindow, error as ui_err
from time import sleep

import sendKeys

class UDPServer(Thread):

	def __init__(self, port = SERVER.UDP_PORT):
		Thread.__init__(self)
		self.running = True
		self.UDPSocket = socket.socket(socket.AF_INET,socket.SOCK_DGRAM)
		try:
			self.UDPSocket.bind(("", SERVER.UDP_PORT))
		except: 
			print 'Error 44: Port are in use. Fail to run server.'	
		try:
			self.shell = win32com.client.Dispatch("WScript.Shell")
		except Exception as e:
			print e

	def stop_server(self):
		self.running = False
		# kick new socket connection to past accept step in main loop.
		temp_socket = socket.socket ( socket.AF_INET, socket.SOCK_DGRAM )
		temp_socket.connect( (SERVER.UDP_HOST, SERVER.UDP_PORT) )

	def run (self):
		print "udp server waiting for incoming connection..."
		old_x=50
		old_y=50
		
		while self.running:
			#print "udp server waiting for incoming connection..."
			data, address = self.UDPSocket.recvfrom(SERVER.MAX_BUFFER_SIZE)
			print "( " ,address[0], " " , address[1] , " ) said : ", data
			#TODO: eliminate all testing code.
			if "Mode" in data:
				inutil, mode = data.split(':')
				if mode == "PowerPoint":
					print "PowerPoint"
					self.shell.Run("powerpnt D:\SAVA\SkyDrive\Licenta\Presentation.pptx") 
				if mode == "Keyboard":
					print "notepad"
					self.shell.Run("winword D:\SAVA\SkyDrive\Licenta\Test.docx") 
				if mode == "RacerGame":
					print "racer game"
					#self.shell.Run("FlatOut2.exe") 
			elif "PowerPoint" in data:
				if "start" in data:
					self.shell.SendKeys("{F5}")
				if "stop" in data:
					self.shell.SendKeys("{ESC}")
				if "previous" in data:
					self.shell.SendKeys("{LEFT}")
				if "next" in data:
					self.shell.SendKeys("{RIGHT}")
			elif "Key" in data:
				inutil, key, keycode = data.split(':')
				try:				
					if keycode == "32":
						self.shell.SendKeys("{ }")
					elif keycode == "8":
						self.shell.SendKeys("{BACKSPACE}")
					else:
						self.shell.SendKeys("{"+key.lower()+"}")
				except:
					print "No Such key."
			else:
				try:
					x,y,z = data.split(':')
					if float(z) < -0.2:
						#self.shell.SendKeys("{UP}")
						#SendKeysCtypes.SendKeys("{w}", pause = 0)
						#self.send_my_input(up)
						t_goo = ( sendKeys.Keys.KEY_W_PRESS,sendKeys.Keys.KEY_S_RELEASE )
						l_keys = [ ]
						l_keys.extend(t_goo)
						sendKeys.send_input(sendKeys.make_input_objects(l_keys))


						# sendKeys.send_input(
						# 	sendKeys.make_input_objects(
						# 		[sendKeys.Keys.KEY_W_PRESS,sendKeys.Keys.KEY_S_RELEASE]))
						
						#win32api.SendMessage(win32con.HWND_TOP, win32con.WM_KEYDOWN, win32con.VK_UP, 1)
						#win32api.SendMessage(win32con.HWND_TOP, win32con.WM_KEYUP, win32con.VK_UP, 0) 
						#from time import sleep
						#sleep(.25)
						#win32api.SendMessage(win32con.HWND_BROADCAST, win32con.WM_KEYUP, win32con.VK_UP, 1) 
						# win32api.SetCursorPos((old_x,old_y))
						# old_y+=1
					elif float(z) > 0.2:
						#SendKeysCtypes.SendKeys("{s}", pause = 0)
						#self.send_my_input(down)

						t_goo = ( sendKeys.Keys.KEY_S_PRESS,sendKeys.Keys.KEY_W_RELEASE )
						l_keys = [ ]
						l_keys.extend(t_goo)
						sendKeys.send_input(sendKeys.make_input_objects(l_keys))
						# sendKeys.send_input(
						# 	sendKeys.make_input_objects(
						# 		[sendKeys.Keys.KEY_W_PRESS,sendKeys.Keys.KEY_S_RELEASE]))
						#self.shell.SendKeys("{DOWN}")
						# win32api.SetCursorPos((old_x,old_y))
						# old_y-=1
					else:		
						#self.send_my_input(nUpDown)
						t_goo = ( sendKeys.Keys.KEY_W_RELEASE,sendKeys.Keys.KEY_S_RELEASE )
						l_keys = [ ]
						l_keys.extend(t_goo)
						sendKeys.send_input(sendKeys.make_input_objects(l_keys))

						# sendKeys.send_input(
						# 	sendKeys.make_input_objects(
						# 		[sendKeys.Keys.KEY_W_PRESS,sendKeys.Keys.KEY_S_RELEASE]))

					if float(y) > 0.2:			
						#self.send_my_input(left)

						t_goo = ( sendKeys.Keys.KEY_A_PRESS,sendKeys.Keys.KEY_D_RELEASE )
						l_keys = [ ]
						l_keys.extend(t_goo)
						sendKeys.send_input(sendKeys.make_input_objects(l_keys))

						# sendKeys.send_input(
						# 	sendKeys.make_input_objects(
						# 		[sendKeys.Keys.KEY_W_PRESS,sendKeys.Keys.KEY_S_RELEASE]))
						#SendKeysCtypes.SendKeys("{a}", pause = 0)
						#self.shell.SendKeys("{LEFT}")
						# win32api.SetCursorPos((old_x,old_y))
						# old_x-=1
					elif float(y) < -0.2:
						#self.send_my_input(right)

						t_goo = ( sendKeys.Keys.KEY_D_PRESS,sendKeys.Keys.KEY_A_RELEASE )
						l_keys = [ ]
						l_keys.extend(t_goo)
						sendKeys.send_input(sendKeys.make_input_objects(l_keys))

						# sendKeys.send_input(
						# 	sendKeys.make_input_objects(
						# 		[sendKeys.Keys.KEY_W_PRESS,sendKeys.Keys.KEY_S_RELEASE]))
						#SendKeysCtypes.SendKeys("{d}", pause = 0)
						#self.shell.SendKeys("{RIGHT}")
						# win32api.SetCursorPos((old_x,old_y))
						# old_x+=1
					else:					
						#self.send_my_input(nLeftRight)
						t_goo = ( sendKeys.Keys.KEY_A_RELEASE,sendKeys.Keys.KEY_D_RELEASE )
						l_keys = [ ]
						l_keys.extend(t_goo)
						sendKeys.send_input(sendKeys.make_input_objects(l_keys))

						# sendKeys.send_input(
						# 	sendKeys.make_input_objects(
						# 		[sendKeys.Keys.KEY_W_PRESS,sendKeys.Keys.KEY_S_RELEASE]))


				except ValueError:
					print data


			#self.UDPSocket.sendto( "accept", (address[0], address[1]) )
			#print "message accept was sent"



class EventHook(object):

    def __init__(self):
        self.__handlers = []

    def __iadd__(self, handler):
        self.__handlers.append(handler)
        return self

    def __isub__(self, handler):
        self.__handlers.remove(handler)
        return self

    def fire(self, *args, **keywargs):
        for handler in self.__handlers:
            handler(*args, **keywargs)

    def clearObjectHandlers(self, inObject):
        for theHandler in self.__handlers:
            if theHandler.im_self == inObject:
                self -= theHandler