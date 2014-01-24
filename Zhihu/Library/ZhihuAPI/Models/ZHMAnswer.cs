using Newtonsoft.Json.Linq;

namespace Zhihu.Library.ZhihuAPI.Models
{
    public class ZHMAnswer
    {
        public ZHMAuthor author     = new ZHMAuthor();
        public ZHMQuestion question = new ZHMQuestion();
        public string url;
        public string excerpt;
        public string type;
        public long updatedTime;
        public int commentCount;
        public int id;
        public int voteupCount;

        public void LoadFromJson(JObject answer)
        {
            if (answer["author"] != null)
                author.LoadFromJson(answer["author"] as JObject);

            if (answer["question"] != null)
                question.LoadFromJson(answer["question"] as JObject);

            if (answer["url"] != null)
                this.url = (string)answer["url"];

            if (answer["excerpt"] != null)
                this.excerpt = (string)answer["excerpt"];

            if (answer["type"] != null)
                this.type = (string)answer["type"];

            if (answer["updated_time"] != null)
                this.updatedTime = (long)answer["updated_time"];

            if (answer["comment_count"] != null)
                this.commentCount = (int)answer["comment_count"];

            if (answer["id"] != null)
                this.id = (int)answer["id"];

            if (answer["voteup_count"] != null)
                this.voteupCount = (int)answer["voteup_count"];
        }
    }
}
