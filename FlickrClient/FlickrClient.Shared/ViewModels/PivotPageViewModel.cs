using Caliburn.Micro;
using FlickrClient.ApiClient;
using FlickrClient.Models;
using FlickrClient.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using System.Threading;

namespace FlickrClient.ViewModels
{
    public class PivotPageViewModel:Screen
    {
        private readonly INavigationService navigationService;
        private readonly IClient client;
        private readonly ICache<string, ObservableCollection<Photo>> photoCache;
        private readonly ICache<string, string> photoIdCache;
        private CancellationTokenSource cts;

        public PivotPageViewModel(INavigationService navigationService, IClient client, ICache<string, ObservableCollection<Photo>> photoCache,
            ICache<string, string> photoIdCache)
        {
            this.navigationService = navigationService;
            this.client = client;
            this.photoCache = photoCache;
            this.photoIdCache = photoIdCache;
        }

        public ObservableCollection<Photo> Photos { get; set; }

        private Photo selectedPhoto;
        public Photo SelectedPhoto 
        { 
            get
            {
                return selectedPhoto;
            }
            set
            {
                selectedPhoto = value;
                if (selectedPhoto.Location == null)
                    GetLocation();
                else
                    IsLocationExist = true;
            }
        }

        private bool isLocationExist;
        public bool IsLocationExist 
        {
            get
            {
                return isLocationExist;
            }
            set
            {
                isLocationExist = value;
                NotifyOfPropertyChange(() => IsLocationExist);
            }
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnInitialize();
            var photoId = string.Empty;
            photoIdCache.TryGet("photoId", out photoId);
            ObservableCollection<Photo> photosCache;
            var isSuccess = photoCache.TryGet("photos", out photosCache);

            if (isSuccess)
            {
                Photos = photosCache;
                NotifyOfPropertyChange(() => Photos);
                SelectedPhoto = Photos.FirstOrDefault(x => x.Id.Equals(photoId));
                NotifyOfPropertyChange(() => SelectedPhoto);
            }
        }

        public void NavigateToMap()
        {
            photoIdCache.TryPut("photoId", SelectedPhoto.Id);
            navigationService.UriFor<MapPageViewModel>().WithParam<string>(x=>x.Latitude, SelectedPhoto.Location.Latitude)
                .WithParam<string>(x=>x.Longitude, SelectedPhoto.Location.Longitude)
                .WithParam<string>(x=>x.Title, SelectedPhoto.Title)
                .Navigate();
        }

        private async void GetLocation()
        {
            try
            {
                cts = new CancellationTokenSource();
                var response = await client.GetPhotoLocation(SelectedPhoto.Id, cts.Token);
                IsLocationExist = response.IsSuccess;
                if (response.IsSuccess)
                {
                    SelectedPhoto.Location = response.Photo.Location;                  
                }
            }
            catch(OperationCanceledException)
            {

            }
        }
    }
}
