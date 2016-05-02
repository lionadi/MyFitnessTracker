using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using MyFitnessXamarinApps.Common;
using MyFitnessXamarinApps.Core;
using System.Timers;

namespace MyFitnessXamarinApps.iOS
{
	partial class MainViewController : UIViewController
	{
		private Timer timer;
        public double RecordValue
        {
            get
            {
                double recordValue = 0;
                if (this.singleRecordSwitch.On)
                {
                    if (Double.TryParse(this.excerciseRecordTextField.Text, out recordValue))
                    {
                        
                    }
                    else
                    {
                        // TODO: Error message to the user
                    }
                }
                else if (this.timerSwitch.On)
                {

                }

                return recordValue;
            }
        }
		public MainViewController (IntPtr handle) : base (handle)
		{
		}
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ServiceOperator.GetInstance().LoadUserData();
            this.userSetsPickerView.Model = new SetsUIPickerViewModel(this.userExcercisePickerView);
            this.userExcercisePickerView.Model = new ExcercisesUIPickerViewController(0);
            this.timerSwitch.ValueChanged += TimerSwitch_ValueChanged;
            this.singleRecordSwitch.ValueChanged += SingleRecordSwitch_ValueChanged;
            this.SaveButton.TouchUpInside += SaveButton_TouchUpInside;

            //ServiceOperator.GetInstance().GetUserSets()

			this.timer = new Timer ();
			this.timer.Enabled = false;
			this.timer.Interval = 1000;
			NavigationController.BeginInvokeOnMainThread (delegate {
				this.timer.Elapsed += (object sender, ElapsedEventArgs e) => { 
					this.timerInfoLabel.Text = e.SignalTime.ToShortTimeString();

				};
			});

        }

        private void SaveButton_TouchUpInside(object sender, EventArgs e)
        {
            double recordValue = 0;
            if (this.singleRecordSwitch.On)
            {
                if (Double.TryParse(this.excerciseRecordTextField.Text, out recordValue))
                {
                    ServiceOperator.GetInstance().SaveUserActivityRecord((int)this.userSetsPickerView.SelectedRowInComponent(0), (int)this.userExcercisePickerView.SelectedRowInComponent(0), recordValue);
					this.PerformSegue ("mainToResultsSegue", this);
                }
                else
                {
                    // TODO: Error message to the user
                }
            } else if (this.timerSwitch.On)
            {
				if(!this.timer.Enabled)
				{
					this.timer.Enabled = true;
					this.timer.Start ();
				} else
				{
					this.timer.Stop ();
					this.PerformSegue ("mainToResultsSegue", this);
				}
            }
        }

        private void SingleRecordSwitch_ValueChanged(object sender, EventArgs e)
        {
            if (this.singleRecordSwitch.On)
            {
                this.timerSwitch.SetState(false, true);
                this.SaveButton.Hidden = false;
				this.SaveButton.SetTitle("Save activity", UIControlState.Normal);
				this.timerInfoLabel.Hidden = true;
            }
        }

        private void TimerSwitch_ValueChanged(object sender, EventArgs e)
        {
            if (this.timerSwitch.On)
            {
                this.singleRecordSwitch.SetState(false, true);
                //this.SaveButton.Hidden = true;
				this.SaveButton.SetTitle("Start timer", UIControlState.Normal);
				this.timerInfoLabel.Hidden = false;
            }
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);

            var detailViewController = segue.DestinationViewController as ResultsViewController;
            if (detailViewController != null)
            {
                detailViewController.RecordValue = this.RecordValue;
            }
        }


    }
}
