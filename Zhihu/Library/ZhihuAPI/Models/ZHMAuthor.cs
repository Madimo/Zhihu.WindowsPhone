using Newtonsoft.Json.Linq;

namespace Zhihu.Library.ZhihuAPI.Models
{
    public class ZHMAuthor
    {
        public string headline;
        public string avatarUrl;
        public string name;
        public string url;
        public string type;
        public string id;
        public int gender;

        public void LoadFromJson(JObject author)
        {
            if (author["headline"] != null)
                this.headline = (string)author["headline"];

            if (author["avatar_url"] != null)
                this.avatarUrl = (string)author["avatar_url"];

            if (author["name"] != null)
                this.name = (string)author["name"];

            if (author["url"] != null)
                this.url = (string)author["url"];

            if (author["type"] != null)
                this.type = (string)author["type"];

            if (author["id"] != null)
                this.id = (string)author["id"];

            if (author["gender"] != null)
                this.gender = (int)author["gender"];
        }
    }
}
