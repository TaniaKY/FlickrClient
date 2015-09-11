using Caliburn.Micro;
using FlickrClient.ApiClient;
using FlickrClient.Models;
using FlickrClient.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace FlickrClient.ViewModels
{
    public class MainPageViewModel : Screen
    {
        private readonly INavigationService navigationService;
        private readonly IClient client;
        private readonly ICache<string, ObservableCollection<Photo>> photoCache;
        private readonly ICache<string, string> photoIdCache;
        private CancellationTokenSource cts;
        private int page = 1;

        public MainPageViewModel(INavigationService navigationService, IClient client, ICache<string, ObservableCollection<Photo>> photoCache,
            ICache<string, string> photoIdCache)
        {
            this.navigationService = navigationService;
            this.client = client;
            this.photoCache = photoCache;
            this.photoIdCache = photoIdCache;
        }

        public ObservableCollection<Photo> Photos { get; set; }

        private string searchText;
        public string SearchText
        {
            get
            {
                return searchText;
            }
            set
            {
                searchText = value;
                if (string.IsNullOrWhiteSpace(searchText))
                    return;
                Search();
            }
        }

        private bool isInProgress;
        public bool IsInProgress
        {
            get
            {
                return isInProgress;
            }
            set
            {
                isInProgress = value;
                NotifyOfPropertyChange(() => IsInProgress);
            }
        }

        public bool IsSearch { get; set; }

        private async void Search()
        {
            try
            {
                if (cts != null)
                {
                    cts.Cancel();
                    cts = null;
                }
                cts = new CancellationTokenSource();

                page = 1;
                var searchTask = client.Search(SearchText, cts.Token, page);
                var response = await searchTask;
                if (response.IsSuccess)
                {
                    Photos = new ObservableCollection<Photo>(response.Photos.Photo);
                    NotifyOfPropertyChange(() => Photos);
                }
            }
            catch (OperationCanceledException)
            {
                //search was cancelled
            }
        }

        protected async override void OnInitialize()
        {
            base.OnInitialize();
            var photosTask = client.GetPhotos();
            var response = await photosTask;
            if (response.IsSuccess)
            {
                Photos = new ObservableCollection<Photo>(response.Photos.Photo);
                NotifyOfPropertyChange(() => Photos);
            }
        }

        public void NavigateToPhoto(Photo photo)
        {
            photoCache.TryPut("photos", Photos);
            photoIdCache.TryPut("photoId", photo.Id);
            navigationService.UriFor<PivotPageViewModel>().Navigate();
        }

        public async void LoadMorePhotos()
        {
            if (IsInProgress)
                return;
            IsInProgress = true;
            page++;
            var photosTask = IsSearch ? client.Search(SearchText, CancellationToken.None, page) : client.GetPhotos(page);
            var response = await photosTask;
            if (response.IsSuccess)
            {
                foreach (var photo in response.Photos.Photo)
                {
                    Photos.Add(photo);
                }
            }
            IsInProgress = false;
        }
    }
}
