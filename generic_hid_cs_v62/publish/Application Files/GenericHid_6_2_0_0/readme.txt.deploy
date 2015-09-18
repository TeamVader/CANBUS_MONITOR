	///<summary>
	/// Project: GenericHid
	/// 
	/// ***********************************************************************
	/// Software License Agreement
	///
	/// Licensor grants any person obtaining a copy of this software ("You") 
	/// a worldwide, royalty-free, non-exclusive license, for the duration of 
	/// the copyright, free of charge, to store and execute the Software in a 
	/// computer system and to incorporate the Software or any portion of it 
	/// in computer programs You write.   
	/// 
	/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
	/// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
	/// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
	/// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
	/// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
	/// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
	/// THE SOFTWARE.
	/// ***********************************************************************
	/// 
	/// Author             
	/// Jan Axelson        
	/// 
	/// This software was written using Visual Studio Express 2012 for Windows
	/// Desktop building for the .NET Framework v4.5.
	/// 
	/// Purpose: 
	/// Demonstrates USB communications with a generic HID-class device
	/// 
	/// Requirements:
	/// Windows Vista or later and an attached USB generic Human Interface Device (HID).
	/// (Does not run on Windows XP or earlier because .NET Framework 4.5 will not install on these OSes.) 
	/// 
	/// Description:
	/// Finds an attached device that matches the vendor and product IDs in the form's 
	/// text boxes.
	/// 
	/// Retrieves the device's capabilities.
	/// Sends and requests HID reports.
	/// 
	/// Uses the System.Management class and Windows Management Instrumentation (WMI) to detect 
	/// when a device is attached or removed.
	/// 
	/// A list box displays the data sent and received along with error and status messages.
	/// You can select data to send and 1-time or periodic transfers.
	/// 
	/// You can change the size of the host's Input report buffer and request to use control
	/// transfers only to exchange Input and Output reports.
	/// 
	/// To view additional debugging messages, in the Visual Studio development environment,
	/// from the main menu, select Build > Configuration Manager > Active Solution Configuration 
	/// and select Configuration > Debug and from the main menu, select View > Output.
	/// 
	/// The application uses asynchronous FileStreams to read Input reports and write Output 
	/// reports so the application's main thread doesn't have to wait for the device to retrieve a 
	/// report when the HID driver's buffer is empty or send a report when the device's endpoint is busy. 
	/// 
	/// For code that finds a device and opens handles to it, see the FindTheHid routine in frmMain.cs.
	/// For code that reads from the device, see GetInputReportViaInterruptTransfer, 
	/// GetInputReportViaControlTransfer, and GetFeatureReport in Hid.cs.
	/// For code that writes to the device, see SendInputReportViaInterruptTransfer, 
	/// SendInputReportViaControlTransfer, and SendFeatureReport in Hid.cs.
	/// 
	/// This project includes the following modules:
	/// 
	/// GenericHid.cs - runs the application.
	/// FrmMain.cs - routines specific to the form.
	/// Hid.cs - routines specific to HID communications.
	/// DeviceManagement.cs - routine for obtaining a handle to a device from its GUID.
	/// Debugging.cs - contains a routine for displaying API error messages.
	/// HidDeclarations.cs - Declarations for API functions used by Hid.cs.
	/// FileIODeclarations.cs - Declarations for file-related API functions.
	/// DeviceManagementDeclarations.cs - Declarations for API functions used by DeviceManagement.cs.
	/// DebuggingDeclarations.cs - Declarations for API functions used by Debugging.cs.
	/// 
	/// Companion device firmware for several device CPUs is available from www.Lvr.com/hidpage.htm
	/// You can use any generic HID (not a system mouse or keyboard) that sends and receives reports.
	/// This application will not detect or communicate with non-HID-class devices.
	/// 
	/// For more information about HIDs and USB, and additional example device firmware to use
	/// with this application, visit Lakeview Research at http://Lvr.com 
	/// Send comments, bug reports, etc. to jan@Lvr.com or post on my PORTS forum: http://www.lvr.com/forum 
	/// 
	/// V6.2
	/// 11/12/13
	/// Disabled form buttons when a transfer is in progress.
	/// Other minor edits for clarity and readability.
	/// Will NOT run on Windows XP or earlier, see below.
	/// 
	/// V6.1
	/// 10/28/13
	/// Uses the .NET System.Management class to detect device arrival and removal with WMI instead of Win32 RegisterDeviceNotification.
	/// Other minor edits.
	/// Will NOT run on Windows XP or earlier, see below.
	///  
	/// V6.0
	/// 2/8/13
	/// This version will NOT run on Windows XP or earlier because the code uses .NET Framework 4.5 to support asynchronous FileStreams.
	/// The .NET Framework 4.5 redistributable is compatible with Windows 8, Windows 7 SP1, Windows Server 2008 R2 SP1, 
	/// Windows Server 2008 SP2, Windows Vista SP2, and Windows Vista SP3.
	/// For compatibility, replaced ToInt32 with ToInt64 here:
	/// IntPtr pDevicePathName = new IntPtr(detailDataBuffer.ToInt64() + 4);
	/// and here:
	/// if ((deviceNotificationHandle.ToInt64() == IntPtr.Zero.ToInt64()))
	/// For compatibility if the charset isn't English, added System.Globalization.CultureInfo.InvariantCulture here:
	/// if ((String.Compare(DeviceNameString, mydevicePathName, true, System.Globalization.CultureInfo.InvariantCulture) == 0))
	/// Replaced all Microsoft.VisualBasic namespace code with other .NET equivalents.
	/// Revised user interface for more flexibility.
	/// Moved interrupt-transfer and other HID-specific code to Hid.cs.
	/// Used JetBrains ReSharper to clean up the code: http://www.jetbrains.com/resharper/
	/// 
	/// V5.0
	/// 3/30/11
	/// Replaced ReadFile and WriteFile with FileStreams. Thanks to Joe Dunne and John on my Ports forum for tips on this.
	/// Simplified Hid.cs.
	/// Replaced the form timer with a system timer.
	/// 
	/// V4.6
	/// 1/12/10
	/// Supports Vendor IDs and Product IDs up to FFFFh.
	///
	/// V4.52
	/// 11/10/09
	/// Changed HIDD_ATTRIBUTES to use UInt16
	/// 
	/// V4.51
	/// 2/11/09
	/// Moved Free_ and similar to Finally blocks to ensure they execute.
	/// 
	/// V4.5
	/// 2/9/09
	/// Changes to support 64-bit systems, memory management, and other corrections. 
	/// Big thanks to Peter Nielsen.
	///  
	/// </summary>
