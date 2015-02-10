/*
v0.9c 2015/2/10  Add "setting_dog.txt"、"Auto_Test_dog"、"debug_dog" for Auto-test by Parker.
v0.9c 2011/9/1   Disables send in loopback mode
v0.9b 2011/8/17  Skip 0x12 and 0x14 also under Xon/Xoff mode
                 Will not send anymore if port is closed
v0.9a 2011/3/24  Now uses MicroTimer to improve send resolution to 3ms
v0.9  2011/2/25  Fix a bug when changing Flow control does not work
                 Added COM Escape Commands
                 Use 2-D array to add controls
v0.8c 2011/2/21  Improve COM error handling
                 Increase buffer to 100K
v0.8b 2011/1/27  Add Loopback Delay function
v0.8a 2010/12/8  Add ErrorReceived Event Handler for MSCom
                 Does not calculate latency when not in WaitnSend
v0.8  2010/11/18 Send Timer now uses System.Timers.Timer instead of WinForm.Timer to prevent GUI hang
                 Fix bug where sendinterval is not assigned to Send_Timer.Interval.
                 Now program starts according to the screen resolution
                 Now use CustomMSCom to add a tag
v0.7h 2010/9/28  Fix Xon/Off logic issue with 5,6,7 bits
v0.7g 2010/8/11  Improved Serial Timeout handling and stop sending if CTS pull low
v0.7f 2010/7/20  Fix crash issue when Serial->USB unplugged and program closes in app.config
                 Ignores 11 and 13 in XonXoff mode (untested)
v0.7e 2010/7/16  Fix duplicate COM name will cause Serial_DataReceived to find wrong port
v0.7d 2010/6/29  Disable latency calculation in loopback. When use "check", it will not affect opened COMs
v0.7c 2010/6/28  Fix no tx count bug in loopback. 
v0.7b 2010/6/8   Stop Send timer when close port. Add support for 5,6,7 Data bits.
v0.7a 2010/6/5   Changed debug to hex output. Size for 7 port is 1090,390, 16 port is 1240, 690
v0.7  2010/6/2   Added Custom Send string function
v0.6c 2010/4/26  Added duration function
v0.6b 2010/4/13  Added on/off funtion for RTS and DTS button
v0.6a 2010/4/12  function added in v0.6 will take wait_n_send into consideration
v0.6  2010/4/9   In loopback, program will wait for all bytes to arrive before returning
v0.5b 2010/4/2   Disable error check when wait_n_send is disabled
v0.5a 2010/2/22  Fix bugs related to debug output
v0.5  2010/1/14  Changed send data logic to resemble Muti-Port testing 
v0.4c 2010/1/6   Add Stop on Error
v0.4a 2009/12/27 Add resend in wait_n_send mode and changed wait_n_send logic
v0.4  2009/12/15 Add Total Port   
v0.3  2009/12/11 Add wait_n_send function
                 Split Send and Button
v0.2  2009/12/10 Update Debugbox to show more items in one row
                 Add Total Average Latency
v0.1  2009/12/09 First release
 */


//Send_Buffer[n] = Send_Buffer[n] + char.ConvertFromUtf32(k);   //send ascii code
//using System.Runtime.InteropServices; //dllimport

//public void SetRx(string text)
//{
//    if (this.Rx.InvokeRequired)
//    {
//        this.Label_Status.BeginInvoke(
//            new MethodInvoker(
//            delegate() { SetStatus(text); }));
//    }
//    else
//    {
//        this.Label_Status.Text = text;
//    }
//}

//public int GetPort(string tport)
//{
//    int j = 0;

//    if (this.txPort[j].InvokeRequired)
//    {
//        this.txPort[j].BeginInvoke(new MethodInvoker(delegate() { GetPort(tport); }));
//    }
//    else
//    {
//        for (j = 0; j < max; j++)                   //find which port in GUI
//        {
//            if (txPort[j].Text == ttxPort[1])
//                break;
//        }
//    }
//    return j;

//}

//private void UpdateRx(int nport, int readlength)
//{
//    if (this.Rx[i].InvokeRequired)
//        this.Rx[i].BeginInvoke(new MethodInvoker(delegate() { UpdateRx(nport, readlength); }));
//    else
//        Rx[nport].Text = (Convert.ToInt32(Rx[nport].Text) + readlength).ToString();

//}

//string hexValues = "48 65 6C 6C 6F 20 57 6F 72 6C 64 21";
//string[] hexValuesSplit = hexValues.Split(' ');
//foreach (String hex in hexValuesSplit)
//{
//    // Convert the number expressed in base-16 to an integer.
//    int value = Convert.ToInt32(hex, 16);
//    // Get the character corresponding to the integral value.
//    string stringValue = Char.ConvertFromUtf32(value);
//    char charValue = (char)value;
//    Console.WriteLine("hexadecimal value = {0}, int value = {1}, char value = {2} or {3}",
//                        hex, value, stringValue, charValue);
//}
/* Output:
    hexadecimal value = 48, int value = 72, char value = H or H
    hexadecimal value = 65, int value = 101, char value = e or e
    hexadecimal value = 6C, int value = 108, char value = l or l
    hexadecimal value = 6C, int value = 108, char value = l or l
    hexadecimal value = 6F, int value = 111, char value = o or o
    hexadecimal value = 20, int value = 32, char value =   or
    hexadecimal value = 57, int value = 87, char value = W or W
    hexadecimal value = 6F, int value = 111, char value = o or o
    hexadecimal value = 72, int value = 114, char value = r or r
    hexadecimal value = 6C, int value = 108, char value = l or l
    hexadecimal value = 64, int value = 100, char value = d or d
    hexadecimal value = 21, int value = 33, char value = ! or !
*/
//SerialPort Crashes after disconnect of USB COM port
//http://connect.microsoft.com/VisualStudio/feedback/details/140018/serialport-crashes-after-disconnect-of-usb-com-port