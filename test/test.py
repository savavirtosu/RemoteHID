import win32com.client
def find_process(name):
    objWMIService = win32com.client.Dispatch("WbemScripting.SWbemLocator")
    objSWbemServices = objWMIService.ConnectServer(".", "root\cimv2")
    colItems = objSWbemServices.ExecQuery(
         "Select * from Win32_Process where Caption = '{0}'".format(name))
    return len(colItems)

print find_process("FlatOut2.exe")