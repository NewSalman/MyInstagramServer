using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyInstagramApi.InstagramModel
{
    public class HomeFeedModel
    {
        public string ID { get; set; }
        public string Filename { get; set; }
        public string Username { get; set; }
        public string UserID { get; set; }
        public DateTime PostedAt { get; set; }
        public string Caption { get; set; }
        public int Likes { get; set; }
        public int CommentsCount { get; set; }
        public int Share { get; set; }
    }
}
