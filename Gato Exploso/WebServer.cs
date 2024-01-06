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

        public delegate void PlayerActionHandler(object sender, PlayerActionArgs args);

        public event PlayerActionHandler PlayerAction;

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
                string html = File.ReadAllText("Content/GatoControl.html");
                

                String direct = request.QueryString["action"];
                if (direct != null)
                {
                    MoveDirection dir = new MoveDirection();
                    switch (direct)
                    {
                        case "up":
                            dir.Up = true;
                            break;
                        case "left":
                            dir.Left = true;
                            break;
                        case "down":
                            dir.Down = true;
                            break;
                        case "right":
                            dir.Right = true;
                            break;
                        case "stop":

                            break;

                    }
                    ReportPlayerMoved(dir);
                }

                String url = request.Url.PathAndQuery;
                byte[] bytes = Encoding.ASCII.GetBytes(html);
                response.OutputStream.Write(bytes, 0, bytes.Length);
                response.OutputStream.Close();
                ReceiveData();
            }

        }
            private void ReportPlayerMoved(MoveDirection direction)
        {
            if(PlayerAction != null)
            {
                PlayerActionArgs actionArgs = new PlayerActionArgs();
                actionArgs.direction = direction;
                PlayerAction(this, actionArgs);
            }
        }
    }
}
