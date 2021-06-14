using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyInstagramApi
{
    public class JWT
    { 
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public string Key { get; set; }
        public double DurationInDays { get; set; }
    }
}
