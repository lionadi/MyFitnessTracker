using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using System.Collections.Generic;
using MapKit;
using CoreLocation;

namespace MyFitnessXamarinApps.iOS
{
	partial class ResultsViewController : UIViewController
	{
        public double RecordValue { get; set; }
		public int TimerSeconds { get; set; }
		public List<GPSLocationData> userGPSLocationData { get; set;}
		public ResultsViewController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
			if (this.TimerSeconds != 0) {
				this.mapView.Hidden = false;
				int minutes = this.TimerSeconds / 60;
				int hours = minutes / 60;

				this.recordValueLabel.Text = hours + ":" + minutes + ":" + this.TimerSeconds % 60;
				if (this.userGPSLocationData != null) {

					foreach (var userGPSLocation in this.userGPSLocationData) {
						this.mapView.AddAnnotations (new MKPointAnnotation () {
							Title = userGPSLocation.Speed.ToString (),
							Coordinate = new CLLocationCoordinate2D (userGPSLocation.Latitude, userGPSLocation.Longitude),
						});
					}
				}
			} else {
				this.recordValueLabel.Text = this.RecordValue.ToString ();
				this.mapView.Hidden = true;
			}
        }
    }
}
