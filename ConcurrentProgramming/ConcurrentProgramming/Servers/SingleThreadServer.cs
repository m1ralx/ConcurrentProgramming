using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentProgramming
{
    class SingleThreadServer
    {
        private HttpListener _listener;
        private int _port;
        private string _suffix;
        private SafetyStruct _structure;
        public SingleThreadServer()
        {
            //var rand = new Random();
            //var baseStrings = new[] { "apple", "banana", "orange" };
            //var strEnum = Enumerable.Range(0, 10000).Select(i => baseStrings[rand.Next(3)]).ToArray();
            //var strings = strEnum.ToArray();

            var strings = File.ReadAllLines(@"C:\Users\Alexey\Desktop\text.txt");
            _structure = new SafetyStruct(strings);
            _port = 16000;
            _listener = new HttpListener();
            _suffix = "index";
        }

        public void Start()
        {
            _listener.Prefixes.Add(string.Format("http://localhost:{0}/{1}/", _port, _suffix));
            _listener.Start();
            while (true)
            {
                HttpListenerContext context = _listener.GetContext();
                var request = context.Request;
                string word = request.QueryString[0];
                if (word == "exit")
                    break;
                string replace = request.QueryString[1];
                var result = _structure.ReplaceFirst(word, replace);
                var response = context.Response;
                string responseString = "<HTML><BODY>Number of string: {0}<br>New string: {1}</BODY></HTML>";
                if (result != null)
                    responseString = String.Format(responseString, result.Item1, result.Item2);
                else
                    responseString = "There is no suitable line";
                byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
            }
            _listener.Stop();
        }
    }
}
