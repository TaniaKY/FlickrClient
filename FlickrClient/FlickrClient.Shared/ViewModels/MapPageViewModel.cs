using Caliburn.Micro;
using FlickrClient.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Devices.Geolocation;

namespace FlickrClient.ViewModels
{
    public class MapPageViewModel: Screen
    {
        public Geopoint Location{get;set;}

        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Title { get; set; }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            Location = new Geopoint(new BasicGeoposition()
            {
                Latitude = double.Parse(Latitude),
                Longitude = double.Parse(Longitude)
            });
            NotifyOfPropertyChange(() => Location);
        }
    }
}
