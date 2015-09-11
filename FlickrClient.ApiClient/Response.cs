using FlickrClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlickrClient.ApiClient
{
    public class PhotosResponse
    {
        public Photos Photos { get; set; }
        public string Stat { get; set; }
        public bool IsSuccess
        {
            get
            {
                return Stat.Equals("ok");
            }
        }
    }

    public class LocationResponse
    {
        public string Stat { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
        public Photo Photo { get; set; }
        public bool IsSuccess
        {
            get
            {
                return Stat.Equals("ok");
            }
        }
    }
}
