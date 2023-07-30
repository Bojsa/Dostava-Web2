using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pusgs_be.Models
{
    public class MailSettings
    {
        public string From { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public bool UseDefaultCredentials { get; set; }
    }
}
