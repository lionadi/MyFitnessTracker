// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace MyFitnessXamarinApps.iOS
{
	[Register ("LoginViewController")]
	partial class LoginViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton LoginButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel PasswordLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField PasswordTextField { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel UserIDLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField UserIDTextField { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (LoginButton != null) {
				LoginButton.Dispose ();
				LoginButton = null;
			}
			if (PasswordLabel != null) {
				PasswordLabel.Dispose ();
				PasswordLabel = null;
			}
			if (PasswordTextField != null) {
				PasswordTextField.Dispose ();
				PasswordTextField = null;
			}
			if (UserIDLabel != null) {
				UserIDLabel.Dispose ();
				UserIDLabel = null;
			}
			if (UserIDTextField != null) {
				UserIDTextField.Dispose ();
				UserIDTextField = null;
			}
		}
	}
}
