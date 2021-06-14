using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyInstagramApi.ViewModels
{
    public class PostModel
    {
        public string Sender { get; set; }
        public DateTime PostedAt { get; set; }
        public string MimeType { get; set; }
        public string Caption { get; set; }
        public string Message { get; set; }
        public bool Succeded { get; set; }
    }
}
