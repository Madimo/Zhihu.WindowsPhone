using Newtonsoft.Json.Linq;

namespace Zhihu.Library.ZhihuAPI.Models
{
    public class ZHMQuestion
    {
        public string url;
        public string type;
        public string title;
        public int id;

        public void LoadFromJson(JObject question)
        {
            if (question["url"] != null)
                this.url = (string)question["url"];

            if (question["type"] != null)
            this.type = (string)question["type"];

            if (question["title"] != null)
            this.title = (string)question["title"];

            if (question["id"] != null)
                this.id = (int)question["id"];
        }
    }
}
