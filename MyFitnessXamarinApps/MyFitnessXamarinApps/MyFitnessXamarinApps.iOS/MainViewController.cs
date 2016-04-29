using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using MyFitnessXamarinApps.Common;
using MyFitnessXamarinApps.Core;
using MyFitnessXamarinApps.Core;

namespace MyFitnessXamarinApps.iOS
{
	partial class MainViewController : UIViewController
	{
		public MainViewController (IntPtr handle) : base (handle)
		{
		}
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ServiceOperator.GetInstance().LoadUserData();
            this.userSetsPickerView.Model = new SetsUIPickerViewModel(this.userExcercisePickerView);
            this.userExcercisePickerView.Model = new ExcercisesUIPickerViewController(0);

            //ServiceOperator.GetInstance().GetUserSets()
            
        }
    }
}
