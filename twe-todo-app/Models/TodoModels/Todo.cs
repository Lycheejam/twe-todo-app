using System.Collections.Generic;

namespace twe_todo_app.Models.TodoModels {
    /// <summary>
    /// Todo.
    /// Item classの親となるTodo Class
    /// </summary>
    public class Todo {
        /// <summary>
        /// Gets or sets the identifier.
        /// TodoのIndex
        /// </summary>
        /// <value>The todo index.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// 登録時のTwitter Account Id
        /// </summary>
        /// <value>The twitter account id.</value>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the tweet identifier.
        /// Todo更新時の親Tweet特定用Tweet Id
        /// </summary>
        /// <value>The tweet id.</value>
        public long TweetId { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// Todoに紐づく各タスクアイテム
        /// </summary>
        /// <value>The items.</value>
        public virtual List<Item> Items { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:twe_todo_app.Models.TodoModels.Todo"/> is close.
        /// 現在進行中のTodoであるか否か
        /// True = クローズ済み
        /// False = 進行中
        /// </summary>
        /// <value><c>true</c> if is close; otherwise, <c>false</c>.</value>
        public bool IsClose { get; set; }
    }
}

