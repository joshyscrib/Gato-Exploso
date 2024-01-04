using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Gato_Exploso
{
    internal class WebServer
    {
        HttpListener listener = new HttpListener();
        public void Start()
        {
            listener = new HttpListener();
            listener.Prefixes.Add("http://*:8888/");
            listener.Start();
            ReceiveData();
        }

        private void ReceiveData()
        {
            listener.BeginGetContext(new AsyncCallback(Callback), listener);
        }

        private void Callback(IAsyncResult result)
        {
            if (listener.IsListening)
            {
                var context = listener.EndGetContext(result);
                var request = context.Request;
                var response = context.Response;

                Console.WriteLine("data received.");
                string html = "<html>  <h1>hello peeps</h1>  </html>";
                if (request.Url.PathAndQuery.Contains("cat"))
                {
                    html = File.ReadAllText("Content/Cats.html");
                }
                if (request.Url.PathAndQuery.Contains("dog"))
                {
                    html = File.ReadAllText("Content/Dogs.html");
                }

                String name = request.QueryString["name"];
                if (name != null)
                {
                    html = html.Replace("{name}", name);
                }
                String age = request.QueryString["age"];
                if (age != null)
                {
                    html = html.Replace("{age}", age);
                }

                String url = request.Url.PathAndQuery;
                byte[] bytes = Encoding.ASCII.GetBytes(html);
                response.StatusCode = 404;
                response.OutputStream.Write(bytes, 0, bytes.Length);
                response.OutputStream.Close();
                ReceiveData();
            }

        }
    }
}
