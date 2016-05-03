using System;
using CoreLocation;
using UIKit;

namespace MyFitnessXamarinApps.iOS
{
	public class LocationUpdatedEventArgs : EventArgs
	{
		CLLocation location;


		public LocationUpdatedEventArgs(CLLocation location)
		{
			this.location = location;
		}

		public CLLocation Location
		{
			get { return location; }
		}
	}

	public class LocationManager
	{
		bool isInitialized = false;
		protected CLLocationManager locMgr;
		public event EventHandler<LocationUpdatedEventArgs> LocationUpdated = delegate { };

		public LocationManager (){
			this.locMgr = new CLLocationManager();
			this.locMgr.PausesLocationUpdatesAutomatically = false; 

			// iOS 8 has additional permissions requirements
			/*if (UIDevice.CurrentDevice.CheckSystemVersion (8, 0)) {
				locMgr.RequestAlwaysAuthorization (); // works in background
				//locMgr.RequestWhenInUseAuthorization (); // only in foreground
			}*/

			if (UIDevice.CurrentDevice.CheckSystemVersion (9, 0)) {
				locMgr.AllowsBackgroundLocationUpdates = true;
			}

		}

		public CLLocationManager LocMgr{
			get { return this.locMgr; }
		}

		public void StartLocationUpdates()
		{
			if (CLLocationManager.LocationServicesEnabled) {
				//set the desired accuracy, in meters

				if(!this.isInitialized)
				{
					/*this.LocMgr.UpdatedLocation += (object sender, CLLocationUpdatedEventArgs e) =>
					{
						// fire our custom Location Updated event
						LocationUpdated (this, new LocationUpdatedEventArgs (e.NewLocation));
					};*/
				LocMgr.LocationsUpdated += (object sender, CLLocationsUpdatedEventArgs e) =>
				{
					// fire our custom Location Updated event
					LocationUpdated (this, new LocationUpdatedEventArgs (e.Locations [e.Locations.Length - 1]));
				};
				LocMgr.DesiredAccuracy = 1;
					this.LocMgr.DistanceFilter = 1;
						this.isInitialized = true;

						}

				LocMgr.StartUpdatingLocation();
				//this.LocMgr.StartMonitoringSignificantLocationChanges ();
			}
		}

		public void StopLocationUpdates()
		{
			if (CLLocationManager.LocationServicesEnabled) {
				//set the desired accuracy, in meters

				LocMgr.StopUpdatingLocation ();
			}
		}

		public CLLocation GetCurrentLocation()
		{

			return locMgr.Location;
		}

	}
}

