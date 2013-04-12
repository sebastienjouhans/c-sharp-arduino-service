using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinrou.io.Arduino
{
     public class ArduinoServiceEventArgs : EventArgs
    {
         public string data { get; private set; }

         public ArduinoServiceEventArgs(string data)
         {
             this.data = data;
         }
    }
}
