using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using MyFitnessXamarinApps.Common;
using MyFitnessXamarinApps.Core;
using System.Threading.Tasks;
using System.Timers;
using CoreLocation;
using System.Collections.Generic;

namespace MyFitnessXamarinApps.iOS
{
	partial class MainViewController : UIViewController
	{
		private NSTimer nsTimer;
		private int timerSeconds = 0;
		private bool timerEnabled = false;

		List<GPSLocationData> userGPSLocationData;

		public static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public LocationManager Manager { get; set;}

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
			this.Manager = new LocationManager();
			this.Manager.StopLocationUpdates ();

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
			this.timerInfoLabel.Text = String.Empty;
            //ServiceOperator.GetInstance().GetUserSets()



			Manager.LocationUpdated += HandleLocationChanged;


			/*UIApplication.Notifications.ObserveDidEnterBackground ((sender, args) => {
				Manager.LocationUpdated -= HandleLocationChanged;
			});

			UIApplication.Notifications.ObserveDidBecomeActive ((sender, args) => {
				Manager.LocationUpdated += HandleLocationChanged;
			});*/
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
				this.nsTimer = NSTimer.CreateRepeatingTimer (1, delegate {
					int minutes = this.timerSeconds / 60;
					int hours = minutes / 60;
					this.timerSeconds++;

					this.timerInfoLabel.Text = hours + ":" + minutes + ":" + this.timerSeconds % 60;
				});
				if(!this.timerEnabled)
				{
					this.timerEnabled = true;

					NSRunLoop.Current.AddTimer (this.nsTimer, NSRunLoopMode.Default);

					this.userGPSLocationData = new List<GPSLocationData> ();
					this.Manager.StartLocationUpdates();
					this.SaveButton.SetTitle ("Stop", UIControlState.Normal);
				} else
				{
					this.StoreGPSLocation (this.Manager.GetCurrentLocation ());
					this.Manager.StopLocationUpdates ();
					this.nsTimer.Invalidate ();
					this.nsTimer.Dispose ();
					this.nsTimer = null;
					ServiceOperator.GetInstance().SaveUserActivityRecord((int)this.userSetsPickerView.SelectedRowInComponent(0), (int)this.userExcercisePickerView.SelectedRowInComponent(0), this.timerSeconds * 1000);
					this.userGPSLocationData.Clear ();
					//this.timer.Stop ();
					this.PerformSegue ("mainToResultsSegue", this);
				}
            }
        }

		/*public override void DidEnterBackground (UIApplication application)
		{
			Console.WriteLine ("App entering background state.");
		}

		public override void WillEnterForeground (UIApplication application)
		{
			Console.WriteLine ("App will enter foreground");
		}*/

		public void StoreGPSLocation(CLLocation location)
		{
			if (location != null) {
				GPSLocationData locationData = new GPSLocationData ();
				locationData.Altitude = location.Altitude;
				locationData.Course = location.Course;
				locationData.Speed = location.Speed;
				locationData.Latitude = location.Coordinate.Latitude;
				locationData.Longitude = location.Coordinate.Longitude;
				this.userGPSLocationData.Add (locationData);
			}
		}

		public void HandleLocationChanged (object sender, LocationUpdatedEventArgs e)
		{
			// Handle foreground updates
			this.StoreGPSLocation(e.Location);

			/*LblAltitude.Text = location.Altitude + " meters";
			LblLongitude.Text = location.Coordinate.Longitude.ToString ();
			LblLatitude.Text = location.Coordinate.Latitude.ToString ();
			LblCourse.Text = location.Course.ToString ();
			LblSpeed.Text = location.Speed.ToString ();*/


		}

        private void SingleRecordSwitch_ValueChanged(object sender, EventArgs e)
        {
            if (this.singleRecordSwitch.On)
            {
                this.timerSwitch.SetState(false, true);
                this.SaveButton.Hidden = false;
				this.SaveButton.SetTitle("Save activity", UIControlState.Normal);
				this.timerInfoLabel.Hidden = true;
				this.timerSeconds = 0;
				this.excerciseRecordTextField.Text = String.Empty;
				this.excerciseRecordTextField.Hidden = false;
            }
        }

        private void TimerSwitch_ValueChanged(object sender, EventArgs e)
        {
            if (this.timerSwitch.On)
            {
				this.timerSeconds = 0;
                this.singleRecordSwitch.SetState(false, true);
                //this.SaveButton.Hidden = true;
				this.SaveButton.SetTitle("Start", UIControlState.Normal);
				this.timerInfoLabel.Hidden = false;
				this.excerciseRecordTextField.Text = String.Empty;
				this.excerciseRecordTextField.Hidden = true;

            }
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);

            var detailViewController = segue.DestinationViewController as ResultsViewController;
            if (detailViewController != null)
            {
				if (!this.timerEnabled)
					detailViewController.RecordValue = this.RecordValue;
				else {
					detailViewController.TimerSeconds = this.timerSeconds;
					this.timerEnabled = false;
					detailViewController.userGPSLocationData = this.userGPSLocationData;
					this.timerSeconds = 0;
				}
            }
        }


    }
}
