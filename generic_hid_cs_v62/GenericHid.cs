
using System.Windows.Forms;

namespace GenericHid
{
	/// <summary>
	///  Runs the application and provides access to the instance of the form.
	/// </summary> 
	  
	public class GenericHid  
	{ 
		internal static FrmMain FrmMy; 
		
		/// <summary>
		///  Displays the application's main form.
		/// </summary> 
		
		public static void Main() 
		{ 
			FrmMy = new FrmMain(); 
			Application.Run(FrmMy); 
		} 
	} 
} 
