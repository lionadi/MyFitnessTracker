using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace MyFitnessXamarinApps.iOS
{
	partial class ResultsViewController : UIViewController
	{
        public double RecordValue { get; set; }
		public ResultsViewController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.recordValueLabel.Text = this.RecordValue.ToString();
        }
    }
}
