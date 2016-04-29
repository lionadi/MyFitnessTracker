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
	[Register ("MainViewController")]
	partial class MainViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField excerciseRecordTextField { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton SaveButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UISwitch singleRecordSwitch { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UISwitch timerSwitch { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIPickerView userExcercisePickerView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIPickerView userSetsPickerView { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (excerciseRecordTextField != null) {
				excerciseRecordTextField.Dispose ();
				excerciseRecordTextField = null;
			}
			if (SaveButton != null) {
				SaveButton.Dispose ();
				SaveButton = null;
			}
			if (singleRecordSwitch != null) {
				singleRecordSwitch.Dispose ();
				singleRecordSwitch = null;
			}
			if (timerSwitch != null) {
				timerSwitch.Dispose ();
				timerSwitch = null;
			}
			if (userExcercisePickerView != null) {
				userExcercisePickerView.Dispose ();
				userExcercisePickerView = null;
			}
			if (userSetsPickerView != null) {
				userSetsPickerView.Dispose ();
				userSetsPickerView = null;
			}
		}
	}
}
