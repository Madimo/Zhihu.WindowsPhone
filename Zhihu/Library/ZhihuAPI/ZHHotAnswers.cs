using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zhihu.Library.ZhihuAPI.Models;

namespace Zhihu.Library.ZhihuAPI
{
    public class ZHHotAnswers : ZhihuAPI
    {
        private string nextUrl;
        private string previousUrl;

        public List<ZHMAnswer> hotAnswers = new List<ZHMAnswer>();
        public int pageNumber;

        public async Task<int> GetFirstPage()
        {
            try
            {
                hotAnswers.Clear();

                string result = await GetAsync("https://api.zhihu.com/explore/answers?hash=" + new Random().Next());
                JObject json  = JObject.Parse(result);

                nextUrl     = (string)json["paging"]["next"];
                previousUrl = (string)json["paging"]["previous"];
                pageNumber  = 1;

                foreach (JObject item in (json["data"] as JArray))
                {
                    ZHMAnswer answer = new ZHMAnswer();
                    answer.LoadFromJson(item);
                    hotAnswers.Add(answer);
                }

                return hotAnswers.Count;
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> GetNextPage()
        {
            try
            {
                hotAnswers.Clear();

                string result = await GetAsync(nextUrl + "&hash=" + new Random().Next());
                JObject json  = JObject.Parse(result);

                nextUrl     = (string)json["paging"]["next"];
                previousUrl = (string)json["paging"]["previous"];
                pageNumber++;

                foreach (JObject item in (json["data"] as JArray))
                {
                    ZHMAnswer answer = new ZHMAnswer();
                    answer.LoadFromJson(item);
                    hotAnswers.Add(answer);
                }

                return hotAnswers.Count;
            }
            catch
            {
                throw;
            }
        }
    }
}
