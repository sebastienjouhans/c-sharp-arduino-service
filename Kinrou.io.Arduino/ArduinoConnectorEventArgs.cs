using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinrou.io.Arduino
{
     public class ArduinoConnectorEventArgs : EventArgs
    {
         public string data { get; private set; }

         public ArduinoConnectorEventArgs(string data)
         {
             this.data = data;
         }
    }
}
