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

					CLLocationCoordinate2D coords = new CLLocationCoordinate2D(this.userGPSLocationData[0].Latitude, this.userGPSLocationData[0].Longitude);

					MKCoordinateSpan span = new MKCoordinateSpan(KilometresToLatitudeDegrees(20), KilometresToLongitudeDegrees(20, coords.Latitude));

					mapView.Region = new MKCoordinateRegion(coords, span);
				}
			} else {
				this.recordValueLabel.Text = this.RecordValue.ToString ();
				this.mapView.Hidden = true;
			}
        }

		public double KilometresToLatitudeDegrees(double kms)
		{
			double earthRadius = 6371.0; // in kms
			double radiansToDegrees = 180.0/Math.PI;
			return (kms/earthRadius) * radiansToDegrees;
		}

		/// <summary>Converts kilometres to longitudinal degrees at a specified latitude</summary>
		public double KilometresToLongitudeDegrees(double kms, double atLatitude)
		{
			double earthRadius = 6371.0; // in kms
			double degreesToRadians = Math.PI/180.0;
			double radiansToDegrees = 180.0/Math.PI;
			// derive the earth's radius at that point in latitude
			double radiusAtLatitude = earthRadius * Math.Cos(atLatitude * degreesToRadians);
			return (kms / radiusAtLatitude) * radiansToDegrees;
		}
    }
}
