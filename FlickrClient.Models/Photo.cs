using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlickrClient.Models
{
    public class Photo
    {
        public string Id { get; set; }
        public string Owner { get; set; }
        public string Secret { get; set; }
        public string Server { get; set; }
        public int Farm { get; set; }
        public string Title { get; set; }
        public int Ispublic { get; set; }
        public int Isfriend { get; set; }
        public int Isfamily { get; set; }
        public Location Location { get; set; }
        public string UrlSmall
        {
            get
            {
                return string.Format("https://farm{0}.staticflickr.com/{1}/{2}_{3}_q.jpg", Farm, Server, Id, Secret);
            }
        }

        public string UrlBig
        {
            get
            {
                return string.Format("https://farm{0}.staticflickr.com/{1}/{2}_{3}_m.jpg", Farm, Server, Id, Secret);
            }
        }
    }
}
