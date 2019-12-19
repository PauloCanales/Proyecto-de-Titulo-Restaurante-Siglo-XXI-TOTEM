using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;


namespace SigloXXITotem
{
    public enum httpVerb
    {
        GET,
        POST,
        PUT,
        DELETE
    }

    class ClienteREST
    {
        public string uri { get; set; }
        public httpVerb httpMethod { get; set; }
        public string putJSON { get; set; }

        public ClienteREST()
        {
            uri = string.Empty;
            
        }

        public string MakeRequest()
        {
            string strResponseValue = string.Empty;

            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = httpMethod.ToString();

            if (request.Method == "PUT")
            {
                request.ContentType = "application/json";                            
            }
            


            HttpWebResponse response = null;

            

            try
            {
                response = (HttpWebResponse)request.GetResponse();

                
                //Proecess the resppnse stream... (could be JSON, XML or HTML etc..._

                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            strResponseValue = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //We catch non Http 200 responses here.
                strResponseValue = "{\"Mensaje de error\":[\"" + ex.Message.ToString() + "\"]}";
            }
            finally
            {
                if (response != null)
                {
                    ((IDisposable)response).Dispose();
                }
            }

            return strResponseValue;

        }

           
        
    }
}
