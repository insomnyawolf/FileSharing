using System;
using System.Web;

namespace FileSharing.Models
{
    public class SharedFileInfo
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public long Size { get; set; }
        public string ContentType { get; set; }

        public string Url
        {
            get
            {
                return HttpUtility.UrlEncode(Name);
            }
        }
    }
}
