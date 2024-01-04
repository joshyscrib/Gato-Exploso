using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Gato_Exploso
{
    internal class WebServer
    {
        HttpListener listener = new HttpListener();
        public void Start()
        {
            
            // Add the prefixes.
                
                listener.Prefixes.Add("http://127.0.0.1:80");
            
            listener.Start();
            Console.WriteLine("Listening...");
        }

    }
}
