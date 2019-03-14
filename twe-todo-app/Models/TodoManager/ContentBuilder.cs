using System.Text;
using twe_todo_app.Models.TodoModels;

namespace twe_todo_app.Models.TodoManager {
    /// <summary>
    /// Todo content builder.
    /// ツイート内容を作成します。
    /// </summary>
    public class ContentBuilder {

        /// <summary>
        /// String Builder
        /// </summary>
        private readonly StringBuilder _stringbuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:twe_todo_app.Models.TodoModels.ContentBuilder"/> class.
        /// </summary>
        public ContentBuilder() {
            _stringbuilder = new StringBuilder();
        }

        /// <summary>
        /// Creates the todo string.
        /// 新規Todo用のツイート文字列を作成します。
        /// </summary>
        /// <returns>ツイート文字列</returns>
        /// <param name="todo">Todo class</param>
        public string Create(Todo todo) {
            if (todo.Items.Count == 1) {
                _stringbuilder.Append("─[ ] " + todo.Items[0].Content + "\r\n");
            } else if (todo.Items.Count == 2) {
                _stringbuilder.Append("┬[ ] " + todo.Items[0].Content + "\r\n");
                _stringbuilder.Append("└[ ] " + todo.Items[1].Content + "\r\n");
            } else if (todo.Items.Count == 3) {
                _stringbuilder.Append("┬[ ] " + todo.Items[0].Content + "\r\n");
                _stringbuilder.Append("├[ ] " + todo.Items[1].Content + "\r\n");
                _stringbuilder.Append("└[ ] " + todo.Items[2].Content + "\r\n");
            } else if (todo.Items.Count == 4) {
                _stringbuilder.Append("┬[ ] " + todo.Items[0].Content + "\r\n");
                _stringbuilder.Append("├[ ] " + todo.Items[1].Content + "\r\n");
                _stringbuilder.Append("├[ ] " + todo.Items[2].Content + "\r\n");
                _stringbuilder.Append("└[ ] " + todo.Items[3].Content + "\r\n");
            } else if (todo.Items.Count == 5) {
                _stringbuilder.Append("┬[ ] " + todo.Items[0].Content + "\r\n");
                _stringbuilder.Append("├[ ] " + todo.Items[1].Content + "\r\n");
                _stringbuilder.Append("├[ ] " + todo.Items[2].Content + "\r\n");
                _stringbuilder.Append("├[ ] " + todo.Items[3].Content + "\r\n");
                _stringbuilder.Append("└[ ] " + todo.Items[4].Content + "\r\n");
            }
            _stringbuilder.Append("#3分間本気出す");

            return _stringbuilder.ToString();
        }

        /// <summary>
        /// Updates the todo string.
        /// 既存Todo更新用のツイート文字列を作成します。
        /// </summary>
        /// <returns>ツイート文字列</returns>
        /// <param name="todo">Todo class</param>
        public string Update(Todo todo) {
            if (todo.Items.Count == 1) {
                _stringbuilder.Append("─[" + todo.Items[0].State + "] " + todo.Items[0].Content + "\r\n");
            } else if (todo.Items.Count == 2) {
                _stringbuilder.Append("┬[" + todo.Items[0].State + "] " + todo.Items[0].Content + "\r\n");
                _stringbuilder.Append("└[" + todo.Items[1].State + "] " + todo.Items[1].Content + "\r\n");
            } else if (todo.Items.Count == 3) {
                _stringbuilder.Append("┬[" + todo.Items[0].State + "] " + todo.Items[0].Content + "\r\n");
                _stringbuilder.Append("├[" + todo.Items[1].State + "] " + todo.Items[1].Content + "\r\n");
                _stringbuilder.Append("└[" + todo.Items[2].State + "] " + todo.Items[2].Content + "\r\n");
            } else if (todo.Items.Count == 4) {
                _stringbuilder.Append("┬[" + todo.Items[0].State + "] " + todo.Items[0].Content + "\r\n");
                _stringbuilder.Append("├[" + todo.Items[1].State + "] " + todo.Items[1].Content + "\r\n");
                _stringbuilder.Append("├[" + todo.Items[2].State + "] " + todo.Items[2].Content + "\r\n");
                _stringbuilder.Append("└[" + todo.Items[3].State + "] " + todo.Items[3].Content + "\r\n");
            } else if (todo.Items.Count == 5) {
                _stringbuilder.Append("┬[" + todo.Items[0].State + "] " + todo.Items[0].Content + "\r\n");
                _stringbuilder.Append("├[" + todo.Items[1].State + "] " + todo.Items[1].Content + "\r\n");
                _stringbuilder.Append("├[" + todo.Items[2].State + "] " + todo.Items[2].Content + "\r\n");
                _stringbuilder.Append("├[" + todo.Items[3].State + "] " + todo.Items[3].Content + "\r\n");
                _stringbuilder.Append("└[" + todo.Items[4].State + "] " + todo.Items[4].Content + "\r\n");
            }

            _stringbuilder.Append("#3分間本気出す");

            return _stringbuilder.ToString();
        }
    }
}
