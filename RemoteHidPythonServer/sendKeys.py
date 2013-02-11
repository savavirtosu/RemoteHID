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
''' 
Part of this code is property of jh45dev@gmail.com
From: http://stackoverflow.com/questions/1823762/sendkeys-for-python-3-1-on-windows
'''

import ctypes as ct
from win32con import SW_MINIMIZE, SW_RESTORE
from win32ui import FindWindow, error as ui_err
from time import sleep

class cls_KeyBdInput(ct.Structure):
    _fields_ = [
        ("wVk", ct.c_ushort),
        ("wScan", ct.c_ushort),
        ("dwFlags", ct.c_ulong),
        ("time", ct.c_ulong),
        ("dwExtraInfo", ct.POINTER(ct.c_ulong) )
    ]

class cls_HardwareInput(ct.Structure):
    _fields_ = [
        ("uMsg", ct.c_ulong),
        ("wParamL", ct.c_short),
        ("wParamH", ct.c_ushort)
    ]

class cls_MouseInput(ct.Structure):
    _fields_ = [
        ("dx", ct.c_long),
        ("dy", ct.c_long),
        ("mouseData", ct.c_ulong),
        ("dwFlags", ct.c_ulong),
        ("time", ct.c_ulong),
        ("dwExtraInfo", ct.POINTER(ct.c_ulong) )
    ]

class cls_Input_I(ct.Union):
    _fields_ = [
        ("ki", cls_KeyBdInput),
        ("mi", cls_MouseInput),
        ("hi", cls_HardwareInput)
    ]

class cls_Input(ct.Structure):
    _fields_ = [
        ("type", ct.c_ulong),
        ("ii", cls_Input_I)
    ]

class Keys():
    KEY_W_PRESS = ( 0x57, 0 )
    KEY_S_PRESS = ( 0x53, 0 )
    KEY_A_PRESS = ( 0x41, 0 )
    KEY_D_PRESS = ( 0x44, 0 )
    KEY_W_RELEASE = ( 0x57, 2 )
    KEY_S_RELEASE = ( 0x53, 2 )
    KEY_A_RELEASE = ( 0x41, 2 )
    KEY_D_RELEASE = ( 0x44, 2 )
    

def find_window( s_app_name ):

    try:
        window1 = FindWindow(  None, s_app_name,)
        return window1
    except ui_err:
        pass
    except:
        raise

    try:
        window1 = FindWindow( s_app_name3, None, )
        return window1
    except ui_err:
        return None
    except:
        raise


def make_input_objects( l_keys ):

    p_ExtraInfo_0 = ct.pointer(ct.c_ulong(0))

    l_inputs = [ ]
    for n_key, n_updown in l_keys:
        ki = cls_KeyBdInput( n_key, 0, n_updown, 0, p_ExtraInfo_0 )
        ii = cls_Input_I()
        ii.ki = ki
        l_inputs.append( ii )

    n_inputs = len(l_inputs)

    l_inputs_2=[]
    for ndx in range( 0, n_inputs ):
        s2 = "(1, l_inputs[%s])" % ndx
        l_inputs_2.append(s2)
    s_inputs = ', '.join(l_inputs_2)


    cls_input_array = cls_Input * n_inputs
    o_input_array = eval( "cls_input_array( %s )" % s_inputs )

    p_input_array = ct.pointer( o_input_array )
    n_size_0 = ct.sizeof( o_input_array[0] )

    # these are the args for user32.SendInput()
    return ( n_inputs, p_input_array, n_size_0 )

    '''It is interesting that o_input_array has gone out of scope
    by the time p_input_array is used, but it works.'''

def send_input(my_input):
    rv = ct.windll.user32.SendInput( *my_input )
    return rv

def send_input_to_window( window1, t_inputs, b_minimize=True ):

    tpl1 = window1.GetWindowPlacement()
    was_min = False
    if tpl1[1] == 2:
        was_min = True
        window1.ShowWindow(SW_RESTORE)
        sleep(0.2)

    window1.SetForegroundWindow()
    sleep(0.2)
    window1.SetFocus()
    sleep(0.2)
    rv = ct.windll.user32.SendInput( *t_inputs )

    if was_min and b_minimize:
        sleep(0.3) # if the last input was Save, it may need time to take effect
        window1.ShowWindow(SW_MINIMIZE)

    return rv



# define some commonly-used key sequences
t_ctrl_s = (  # save in many apps
    ( 0x11, 0 ),
    ( 0x53, 0 ),
    ( 0x11, 2 ),
)
t_ctrl_r = (  # reload in some apps
    ( 0x11, 0 ),
    ( 0x52, 0 ),
    ( 0x11, 2 ),
)


def test():

    # file > open; a non-invasive way to test
    t_ctrl_o = ( ( 0x11, 0 ), ( 0x4F, 0 ), ( 0x11, 2 ), )

    # writes "Hello\n"
    # 0x10 is shift.  note that to repeat a key, as with 4C here, you have to release it after the first press
    t_hello = ( ( 0x10, 0 ), ( 0x48, 0 ), ( 0x10, 2 ), ( 0x45, 0 ), ( 0x4C, 0 ), ( 0x4C, 2 ), ( 0x4C, 0 ), ( 0x4F, 0 ), ( 0x0D, 0 ), )


    l_keys = [ ]
    ## l_keys.extend( t_ctrl_o )
    l_keys.extend( t_hello )
    l_keys.extend( t_ctrl_s )

    ## s_app_name = "SciTE"
    ## s_app_name = "(Untitled) - SciTE"
    s_app_name = "test.txt - SciTE"
    ## s_app_name = "Notepad2"
    ## s_app_name = "Notepad"

    window1 = find_window( s_app_name )
    if window1 == None:
        print( "%r has no window." % s_app_name )
        input( 'press enter to close' )
        exit()

    t_inputs = make_input_objects( l_keys )

    n = send_input( window1, t_inputs )

    ## print( "SendInput returned: %r" % n )
    ## print( "GetLastError: %r" % ct.windll.kernel32.GetLastError() )
    ## input( 'press enter to close' )

def test_2():
    sleep(1)
    send_input(( ( 0x57, 0 ), ( 0x53, 0 ) ))
    sleep(2)
    send_input(Keys.KEY_D_PRESS)


if __name__ == '__main__':
    test_2()