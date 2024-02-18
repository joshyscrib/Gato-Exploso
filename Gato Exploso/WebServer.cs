using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Threading;
using Microsoft.Xna.Framework.Graphics.PackedVector;

namespace Gato_Exploso
{
    internal class WebServer
    {
        private bool running = false;
        Thread listenThread;

        HttpListener listener = new HttpListener();
        public void Start()
        {
            running = true;
            listenThread = new Thread(()=> RunThread(this));
            listenThread.Start();

         //   ReceiveData();
        }

        public static void RunThread(WebServer instance)
        {
            instance.StartListening();
        }
        public void Stop()
        {
            running = false;
            listener.Stop();
            listenThread.Join();
        }

        public delegate void PlayerActionHandler(object sender, PlayerActionArgs args);
        public delegate void PlayerRegisterHandler(object sender, RegisterPlayerArgs args);
        public event PlayerActionHandler PlayerAction;
        public event PlayerRegisterHandler PlayerRegister;

        private void StartListening()
        {

            listener = new HttpListener();
            listener.Prefixes.Add("http://*:8888/");
            listener.Start();
            while (running)
            {
                ThreadPool.QueueUserWorkItem(Process, listener.GetContext());
            }
        }
        byte[] GetBinaryFile(string fileName)
        {
            return File.ReadAllBytes("../../../Content/" + fileName);
        }
        void Process(object o)
        {
            try
            {
                var context = o as HttpListenerContext;


                var request = context.Request;
                var response = context.Response;
                // if web should return a string
                bool returnString = true;
                // array of bytes 
                byte[] bytes = null;
                string html = "ok";

                if (request.Url.PathAndQuery.ToLower().Contains("joy.js"))
                {
                    html = File.ReadAllText("../../../Content/Joy.js");
                }
                if (request.Url.PathAndQuery.ToLower().Contains("image"))
                {
                    try
                    {
                        string path = request.Url.LocalPath;
                        // gets the name of the file provided by PATH variable
                        string[] parts = path.Split(new char[] { '/' });
                        bytes = GetBinaryFile(parts[2]);
                        returnString = false;
                    }
                    catch (Exception ex) { }


                }

                if (request.Url.PathAndQuery.Contains("home"))
                {
                    html = File.ReadAllText("../../../Content/GatoControl.html");
                }
                if (request.Url.PathAndQuery.Contains("playerinfo"))
                {
                    string name = request.QueryString["name"];
                    string timeString = request.QueryString["time"];
                    int webTime = int.Parse(timeString);
                    html = HandleGetPlayers(name, webTime);



                }
                if (request.Url.PathAndQuery.Contains("gameworld"))
                {
                    html = Game1.Instance.GetGameWorld();
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
                            case "join":
                                HandleJoin(name);
                                break;
                            case "attack":
                                HandleAttack(name,"peck");
                                break;
                            case "shoot":
                                HandleAttack(name,"shoot");
                                break;
                                

                        }
                    }
                }




                String direct = request.QueryString["action"];



                String url = request.Url.PathAndQuery;
                if (returnString)
                {
                    bytes = Encoding.ASCII.GetBytes(html);
                }
                else
                {
                    response.AddHeader("Content-Type", "image/png");
                }

                response.OutputStream.Write(bytes, 0, bytes.Length);
                response.OutputStream.Close();
            }
            catch (Exception ex) { }
 
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
            if (PlayerRegister != null)
            {
                RegisterPlayerArgs registerPlayerArgs = new RegisterPlayerArgs();
                registerPlayerArgs.Name = name;
                PlayerRegister(this, registerPlayerArgs);
            }
        }
        public void HandleAttack(string name, string type)
        {
            if (PlayerAction != null)
            {
                PlayerActionArgs actionArgs = new PlayerActionArgs();
                actionArgs.name = name;
               if(type == "peck")
                {
                    actionArgs.attack = true;
                }
                else
                {
                    actionArgs.shoot = true;
                }
                PlayerAction(this, actionArgs);
            }
        }


        public string HandleGetPlayers(string name, int time)
        {
            var gameInfo = Game1.Instance.GetGameInfo(name, time);
            string json = JsonSerializer.Serialize(gameInfo);
            return json;
        }
        private void ReportPlayerMoved(MoveDirection direction, string name)
        {
            if (PlayerAction != null)
            {
                PlayerActionArgs actionArgs = new PlayerActionArgs();
                actionArgs.name = name;
                actionArgs.direction = direction;
                PlayerAction(this, actionArgs);
            }
        }
    }
}