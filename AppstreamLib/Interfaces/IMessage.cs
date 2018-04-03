using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppstreamLib.Interfaces
{
    interface IMessage
    {
        bool Send(string key, string to, string title, string content, object data);
    }
}
