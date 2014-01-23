using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.IsolatedStorage;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;

namespace Zhihu.Library.ZhihuAPI
{
    class ZhihuAPI
    {
        protected static string accessToken;
        protected static string tokenType;
        protected static string refreshToken;
        protected static int expiresIn;
        protected static IsolatedStorageSettings iss;

        public ZhihuAPI()
        {
            iss = IsolatedStorageSettings.ApplicationSettings;
            if (iss.Contains("accessToken"))
                accessToken = (string)iss["accessToken"];
            if (iss.Contains("tokenType"))
                tokenType = (string)iss["tokenType"];
            if (iss.Contains("expiresIn"))
                expiresIn = (int)iss["expiresIn"];
            if (iss.Contains("refreshToken"))
                refreshToken = (string)iss["refreshToken"];
        }

        // async post request
        protected static async Task<string> PostAsync(string RequestUrl, string Context)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(RequestUrl);
                httpWebRequest.Method = "POST";
                httpWebRequest.Accept = "application/json";
                httpWebRequest.UserAgent   = "ZhihuAppIrisRelease/96 (iPhone; iOS 6.1.2; Scale/2.00)";
                httpWebRequest.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
                httpWebRequest.Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
                httpWebRequest.Headers[HttpRequestHeader.Connection]     = "Keep-Alive";
                httpWebRequest.Headers[HttpRequestHeader.Authorization]  = "Bearer " + accessToken;
                httpWebRequest.Headers["XAPIVERSION"] = "3.0";
                httpWebRequest.Headers["XAPPVERSION"] = "2.1.0";
                using (Stream stream = await httpWebRequest.GetRequestStreamAsync())
                {
                    byte[] entryBytes = Encoding.UTF8.GetBytes(Context);
                    stream.Write(entryBytes, 0, entryBytes.Length);
                    stream.Close();
                }

                WebResponse response = await httpWebRequest.GetResponseAsync();
                StreamReader streamReader = new StreamReader(response.GetResponseStream());
                return streamReader.ReadToEnd();
            }
            catch
            {
                throw;
            }
        }

        // async get request
        protected static async Task<string> GetAsync(string RequestUrl)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(new Uri(RequestUrl, UriKind.Absolute));
                httpWebRequest.Method    = "GET";
                httpWebRequest.UserAgent = "ZhihuAppIrisRelease/96 (iPhone; iOS 6.1.2; Scale/2.00)";
                httpWebRequest.Headers[HttpRequestHeader.Authorization] = "Bearer " + accessToken;
                WebResponse response = await httpWebRequest.GetResponseAsync();
                Stream streamResult = response.GetResponseStream();
                StreamReader sr = new StreamReader(streamResult, Encoding.UTF8);
                string returnValue = sr.ReadToEnd();
                return DeleteLines(UnicodeToGB(returnValue));
            }
            catch
            {
                throw;
            }
        }

        // delete double extra lines
        protected static string DeleteLines(string text)
        {
            int index = text.IndexOf('\\');
            while (index != -1)
            {
                if (text[index + 1] == '/')
                {
                    text = text.Remove(index, 1);
                }
                index = text.IndexOf('\\', index + 1);
            }
            return text;
        }

        // unicode to GB encoding
        public static string UnicodeToGB(string text)
        {
            System.Text.RegularExpressions.MatchCollection mc = System.Text.RegularExpressions.Regex.Matches(text, "\\\\u([\\w]{4})");
            if (mc != null && mc.Count > 0)
            {
                foreach (System.Text.RegularExpressions.Match m2 in mc)
                {
                    string v = m2.Value;
                    string word = v.Substring(2);
                    byte[] codes = new byte[2];
                    int code = Convert.ToInt32(word.Substring(0, 2), 16);
                    int code2 = Convert.ToInt32(word.Substring(2), 16);
                    codes[0] = (byte)code2;
                    codes[1] = (byte)code;
                    text = text.Replace(v, Encoding.Unicode.GetString(codes, 0, codes.Length));
                }
            }
            return text;
        }
    }
}
