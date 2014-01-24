using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Zhihu.Library.ZhihuAPI
{
    public class ZHAccount : ZhihuAPI
    {
        public async Task<bool> Login(string username, string password)
        {
            try
            {
                string requestContent =
                    "client_id=4bc899f2695f417eb733f3d21a7be0&" +
                    "client_key=d179a1e11f2347a48c7f928d354621&" +
                    "email=" + username + "&" +
                    "grant_type=password&" +
                    "password=" + password;

                string result = await this.PostAsync("https://oauth.zhihu.com/token", requestContent);
                JObject json  = JObject.Parse(result);
                tokenType     = (string)json["token_type"];
                accessToken   = (string)json["access_token"];
                expiresIn     = (int)json["expires_in"];
                refreshToken  = (string)json["refresh_token"];

                if (iss.Contains("tokenType"))
                    iss.Remove("tokenType");
                if (iss.Contains("accessToken"))
                    iss.Remove("accessToken");
                if (iss.Contains("expiresIn"))
                    iss.Remove("expiresIn");
                if (iss.Contains("refreshToken"))
                    iss.Remove("refreshToken");

                iss.Add("tokenType",    tokenType);
                iss.Add("accessToken",  accessToken);
                iss.Add("expiresIn",    expiresIn);
                iss.Add("refreshToken", refreshToken);
                iss.Save();

                return true;
            }
            catch
            {
                throw;
            }
        }

        void Logout()
        {
            if (iss.Contains("accessToken"))
                iss.Remove("accessToken");
            if (iss.Contains("tokenType"))
                iss.Remove("tokenType");
            if (iss.Contains("expiresIn"))
                iss.Remove("expiresIn");
            if (iss.Contains("refreshToken"))
                iss.Remove("refreshToken");
            iss.Save();

            accessToken  = null;
            tokenType    = null;
            refreshToken = null;
            expiresIn    = 0;
        }

        public bool isLogined()
        {
            return accessToken != null ? true : false;
        }

        // async post request
        private async new Task<string> PostAsync(string RequestUrl, string Context)
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
                httpWebRequest.Headers[HttpRequestHeader.Authorization]  
                    = "Basic NGJjODk5ZjI2OTVmNDE3ZWI3MzNmM2QyMWE3YmUwOmQxNzlhMWUxMWYyMzQ3YTQ4YzdmOTI4ZDM1NDYyMQ==";
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
    }
}
