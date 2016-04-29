using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using MyFitnessXamarinApps.Common;
using MyFitnessXamarinApps.Core;

namespace MyFitnessXamarinApps.iOS
{
	partial class LoginViewController : UIViewController
	{
		public LoginViewController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.LoginButton.TouchUpInside += LoginButton_TouchUpInside;
            this.PasswordTextField.SecureTextEntry = true;
        }

        private void LoginButton_TouchUpInside(object sender, EventArgs e)
        {
            ServiceOperator.GetInstance().LoginUser(this.UserIDTextField.Text, PasswordTextField.Text);

        }
    }
}
