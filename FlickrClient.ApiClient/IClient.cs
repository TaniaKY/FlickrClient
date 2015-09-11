using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlickrClient.ApiClient
{
    public interface IClient
    {
        Task<PhotosResponse> GetPhotos(int page = 1);
        Task<PhotosResponse> Search(string text, CancellationToken token, int page = 1);
        Task<LocationResponse> GetPhotoLocation(string photo_id, CancellationToken token);
    }
}
