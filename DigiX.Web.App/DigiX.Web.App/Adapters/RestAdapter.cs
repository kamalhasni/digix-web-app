using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace DigiX.Web.App.Adapters
{
    public sealed class RestAdapter : IDisposable
    {
        public const string URL_FORMAT = "http://localhost:53026/api/{0}";

        private HttpWebRequest _webRequest;
        private string _url;
        private string _responseString;

        public string Url { get { return _url; } }
        public string ResponseString { get { return _responseString; } }
        public string Accepts { get; set; }

        public RestAdapter(string url)
        {
            _url = url;
            _responseString = string.Empty;
        }

        public void ProceedToGet()
        {
            _webRequest = (HttpWebRequest)WebRequest.Create(_url);
            _webRequest.Timeout = 120 * 60000;
            _webRequest.ReadWriteTimeout = 120 * 60000;
            if (!string.IsNullOrEmpty(Accepts))
                _webRequest.Accept = "text/xml";
            _webRequest.Headers["Authorization"] = "Basic ZGlnaXgtd2ViLWFwcC0xOmRpZ2l4";

            using(var webResponse = _webRequest.GetResponse())
            {
                using (var reader = new StreamReader(webResponse.GetResponseStream()))
                {
                    _responseString = reader.ReadToEnd();
                }
            }
        }

        public void Dispose()
        {
            _webRequest = null;
            _url = null;
            _responseString = null;
        }
    }
}