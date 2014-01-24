using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zhihu.Library.ZhihuAPI.Models;

namespace Zhihu.Library.ZhihuAPI
{
    public class ZHTimeline : ZhihuAPI
    {
        private string nextUrl;
        private string previousUrl;

        public List<ZHMFeed> timeline = new List<ZHMFeed>();
        public int pageNumber;

        public async Task<int> GetFirstPage()
        {
            try
            {
                timeline.Clear();

                string result = await GetAsync("https://api.zhihu.com/feeds?hash=" + new Random().Next());
                JObject json  = JObject.Parse(result);

                nextUrl     = (string)json["paging"]["next"];
                previousUrl = (string)json["paging"]["previous"];
                pageNumber  = 1;

                foreach (JObject item in (json["data"] as JArray))
                {
                    ZHMFeed feed = new ZHMFeed();
                    feed.LoadFromJson(item);
                    timeline.Add(feed);
                }

                return timeline.Count;
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
                timeline.Clear();

                string result = await GetAsync(nextUrl + "&hash=" + new Random().Next());
                JObject json  = JObject.Parse(result);

                nextUrl     = (string)json["paging"]["next"];
                previousUrl = (string)json["paging"]["previous"];
                pageNumber++;

                foreach (JObject item in (json["data"] as JArray))
                {
                    ZHMFeed feed = new ZHMFeed();
                    feed.LoadFromJson(item);
                    timeline.Add(feed);
                }

                return timeline.Count;
            }
            catch
            {
                throw;
            }
        }
    }
}
