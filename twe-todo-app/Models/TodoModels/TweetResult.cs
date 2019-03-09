using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace twe_todo_app.Models.TodoModels {
    public class TweetResult {
        public int id { get; set; }
        public string userId { get; set; }
        public long tweetId { get; set; }
        public virtual List<Task> tasks { get; set; }
        public int endFlag { get; set; }
    }
    public class Task {
        public int id { get; set; }
        public string task { get; set; }
        [MaxLength(1)]
        public string state { get; set; }
    }

}

