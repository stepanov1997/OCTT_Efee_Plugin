using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OCTT_Efee_Plugin
{
    class EfeeRestService
    {
        private readonly string rootEfeeURL;
        private string encoded;

        public EfeeRestService(string rootEfeeURL, string username, string password)
        {
            this.encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
            this.rootEfeeURL = rootEfeeURL;
        }
        public bool insertData(string jsonData)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(rootEfeeURL);
                httpWebRequest.Headers.Add("Authorization", "Basic " + encoded);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = jsonData;

                    streamWriter.Write(json);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }
    }
}
