using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Zhihu.Library.ZhihuAPI.Models
{
    public class ZHMFeed
    {
        public List<ZHMAuthor> actors = new List<ZHMAuthor>();
        public ZHMTarget target = new ZHMTarget();
        public string verb;
        public string type;
        public string id;
        public long updatedTime;
        public int count;

        public void LoadFromJson(JObject feed)
        {
            if (feed["count"] != null)
                this.count = (int)feed["count"];

            if (feed["target"] != null)
                this.target.LoadFromJson(feed["target"] as JObject);

            if (feed["updated_time"] != null)
                this.updatedTime = (long)feed["updated_time"];

            if (feed["verb"] != null)
                this.verb = (string)feed["verb"];

            if (feed["actors"] != null)
            {
                foreach (JObject item in (feed["actors"] as JArray))
                {
                    ZHMAuthor actor = new ZHMAuthor();
                    actor.LoadFromJson(item);
                    actors.Add(actor);
                }
            }

            if (feed["type"] != null)
                this.type = (string)feed["type"];

            if (feed["id"] != null)
                this.id = (string)feed["id"];
        }
    }
}
