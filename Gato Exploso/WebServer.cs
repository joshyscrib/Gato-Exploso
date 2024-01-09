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
        public delegate void PlayerRegisterHandler(object sender, RegisterPlayerArgs args);
        public event PlayerActionHandler PlayerAction;
        public event PlayerRegisterHandler PlayerRegister;
        private void ReceiveData()
        {
            listener.BeginGetContext(new AsyncCallback(Callback), listener);
        }
        public void HandleMove(string direct, string name)
        {
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
                ReportPlayerMoved(dir, name);
            }
        }
        public void HandleJoin(string name)
        {
            if(PlayerRegister != null)
            {
                RegisterPlayerArgs registerPlayerArgs = new RegisterPlayerArgs();
                registerPlayerArgs.Name = name;
                PlayerRegister(this, registerPlayerArgs);
            }
        }
        private void Callback(IAsyncResult result)
        {
            if (listener.IsListening)
            {
                var context = listener.EndGetContext(result);
                var request = context.Request;
                var response = context.Response;

                Console.WriteLine("data received.");
                string html = "ok";
                if (request.Url.PathAndQuery.Contains("home"))
                {
                    html = File.ReadAllText("Content/GatoControl.html");
                }
                if (request.Url.PathAndQuery.Contains("action"))
                {
                    string command = request.QueryString["command"];
                    string name = request.QueryString["name"];
                    if (command != null)
                    {
                        switch (command)
                        {
                            case "move":
                                HandleMove(request.QueryString["direction"], request.QueryString["name"]);
                                break;
                            case"join":
                                HandleJoin(name);
                                break;

                        }
                    }
                }
                
                
                
                
                String direct = request.QueryString["action"];

               

                String url = request.Url.PathAndQuery;
                byte[] bytes = Encoding.ASCII.GetBytes(html);
                response.OutputStream.Write(bytes, 0, bytes.Length);
                response.OutputStream.Close();
                ReceiveData();
            }

        }
            private void ReportPlayerMoved(MoveDirection direction, string name)
        {
            if(PlayerAction != null)
            {
                PlayerActionArgs actionArgs = new PlayerActionArgs();
                actionArgs.name = name;
                actionArgs.direction = direction;
                PlayerAction(this, actionArgs);
            }
        }
    }
}
// kool kid koment kfor kgithub