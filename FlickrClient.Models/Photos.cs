﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlickrClient.Models
{
    public class Photos
    {
        public int Page { get; set; }
        public int Pages { get; set; }
        public int Perpage { get; set; }
        public int Total { get; set; }
        public List<Photo> Photo { get; set; }
    }
}
