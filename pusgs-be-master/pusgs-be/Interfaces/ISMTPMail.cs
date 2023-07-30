using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pusgs_be.Interfaces
{
    public interface ISMTPMail
    {
        public void SendMail(string to, string subject, string body);
    }
}
