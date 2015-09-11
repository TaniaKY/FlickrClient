using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using FlickrClient.ApiClient;
using System.Threading.Tasks;
using System.Threading;

namespace FlickrClient.UnitAPITests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestPhotos()
        {
            try
            {
                var client = new Client();
                var photos = await client.GetPhotos();
                Assert.IsNotNull(photos);
            }
            catch (Exception ex)
            {

            }
        }

        [TestMethod]
        public async Task TestSearch()
        {
            try
            {
                var client = new Client();
                var photos = await client.Search("avto", CancellationToken.None);
                Assert.IsNotNull(photos);
            }
            catch (Exception ex)
            {

            }
        }

        [TestMethod]
        public async Task TestLocation()
        {
            try
            {
                var client = new Client();
                var s = await client.GetPhotos();
                var location = await client.GetPhotoLocation("15392348071", CancellationToken.None);//s.photos.photo.FirstOrDefault().id);
                Assert.IsNotNull(location);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
