using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlickrClient.ApiClient
{
    public class Client : IClient
    {
        private const string ApiKey = "5db8b35cad09c909492bdf509d416ec1";
        private const string ApiSecret = "4521c140305742cd";

        private HttpClient httpClient;

        public Client()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            httpClient.BaseAddress = new Uri("https://api.flickr.com/services/rest");
        }

        public async Task<PhotosResponse> GetPhotos(int page = 1)
        {
            var photosMethod = "flickr.photos.getRecent";
            var response = await GetResponse<PhotosResponse>(string.Format("{0}&page={1}", photosMethod, page), CancellationToken.None);
            return response;
        }

        public async Task<PhotosResponse> Search(string text, CancellationToken token, int page = 1)
        {
            var searchMethod = "flickr.photos.search";
            var response = await GetResponse<PhotosResponse>(string.Format("{0}&text={1}&page={2}", searchMethod, text, page), token);
            return response;
        }

        public async Task<LocationResponse> GetPhotoLocation(string photo_id, CancellationToken token)
        {
            var locationMethod = "flickr.photos.geo.getLocation";
            var response = await GetResponse<LocationResponse>(string.Format("{0}&photo_id={1}", locationMethod, photo_id), token);
            return response;
        }

        private async Task<T> GetResponse<T>(string uriParameters, CancellationToken token)
        {
            try
            {
                var uri = string.Format("?api_key={0}&format=json&nojsoncallback=1&method={1}", ApiKey, uriParameters);
                using (var responseMessage = await httpClient.GetAsync(uri, token))
                {
                    var response = await HandleResponse<T>(responseMessage);
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<T> HandleResponse<T>(HttpResponseMessage responseMessage)
        {
            T response = default(T);
            var content = await responseMessage.Content.ReadAsStringAsync();
            if (responseMessage.IsSuccessStatusCode)
                response = JsonConvert.DeserializeObject<T>(content);
            return response;
        }
    }
}
