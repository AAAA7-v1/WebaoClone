using System;
using System.Collections.Generic;

namespace Webao.Test
{
    public abstract class MockRequest : IRequest
    {
        private readonly Dictionary<string, object> responses = new Dictionary<string, object>();
        private readonly Dictionary<string, string> queryParameters = new Dictionary<string, string>();
        protected HttpRequest req = new HttpRequest();
        private string host;
        protected string Path { get; set; }
        protected Type Dest { get; set; }

        protected void AddResponses(string[] validRequests)
        {
            foreach (String request in validRequests)
            {
                responses.Add(req.Url(Path+request), req.Get(Path + request, Dest)); // Loads response dictionary from doing the actual HttpRequests
            }
        }
        public IRequest AddParameter(string arg, string val)
        {
            queryParameters.Add(arg, val);
            return this;
        }
        public IRequest BaseUrl(string host)
        {
            this.host = host;
            return this;
        }
        public string Url(string path)
        {
            string url = host + path;
            if (queryParameters.Count != 0 && !url.Contains("?"))
                url += "?";
            else
                url += "&";
            foreach (var pair in queryParameters)
            {
                url += pair.Key + "=" + pair.Value + "&";
            }
            return url;
        }
        public object Get(string path, Type targetType)
        {
            if (!responses.TryGetValue(Url(path), out object toReturn)) throw new ArgumentException();
            return toReturn;
        }
    }
}
