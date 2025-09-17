using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Messaging.Setting
{
    public interface IMessageBrokerSettings
    {
        string Host { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
    }


}
