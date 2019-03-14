using System.ComponentModel.DataAnnotations;

namespace twe_todo_app.Models.TodoModels {
    /// <summary>
    /// Item.
    /// 親（Todo class）に紐づくTodo Item List
    /// </summary>
    public class Item {
        /// <summary>
        /// Gets or sets the identifier.
        /// itemのindex
        /// </summary>
        /// <value>The item index.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// Todoの各itemの内容
        /// </summary>
        /// <value>The content.</value>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// Todoの各itemの進捗状況（顔文字）
        /// </summary>
        /// <value>The state.</value>
        [MaxLength(1)]
        public string State { get; set; }
    }

}