using Newtonsoft.Json.Linq;

namespace Zhihu.Library.ZhihuAPI.Models
{


    public class ZHMTarget
    {    
        public const string ZHMTARGET_TYPE_ANSWER_CREATE   = "ANSWER_CREATE";
        public const string ZHMTARGET_TYPE_ANSWER_VOTE_UP  = "ANSWER_VOTE_UP";
        public const string ZHMTARGET_TYPE_QUESTION_FOLLOW = "QUESTION_FOLLOW";
        public const string ZHMTARGET_TYPE_QUESTION_CREATE = "QUESTION_CREATE";

        public ZHMAuthor author = new ZHMAuthor();
        public ZHMQuestion question = new ZHMQuestion();
        public string url;
        public string excerpt;
        public string type;
        public string title;
        public long updatedTime;
        public int commentCount;
        public int id;
        public int voteupCount;

        public void LoadFromJson(JObject target)
        {
            if (target["author"] != null)
                this.author.LoadFromJson(target["author"] as JObject);

            if (target["url"] != null)
                this.url = (string)target["url"];

            if (target["question"] != null)
                this.question.LoadFromJson(target["question"] as JObject);

            if (target["excerpt"] != null)
                this.excerpt = (string)target["excerpt"];

            if (target["updated_time"] != null)
                this.updatedTime = (long)target["updated_time"];

            if (target["commentCount"] != null)
                this.commentCount = (int)target["comment_count"];

            if (target["type"] != null)
                this.type = (string)target["type"];

            if (target["id"] != null)
                this.id = (int)target["id"];

            if (target["title"] != null)
                this.title = (string)target["title"];

            if (target["voteup_count"] != null)
                this.voteupCount = (int)target["voteup_count"];
        }
    }
}
