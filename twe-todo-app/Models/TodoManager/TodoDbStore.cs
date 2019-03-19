using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using twe_todo_app.Data;
using twe_todo_app.Models.TodoModels;

namespace twe_todo_app.Models.TodoManager {
    /// <summary>
    /// Todo db store.
    /// </summary>
    public class TodoDbStore {
        /// <summary>
        /// DB Context
        /// </summary>
        private readonly ApplicationDbContext _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:twe_todo_app.Models.TodoManager.TodoDbStore"/> class.
        /// </summary>
        /// <param name="context">DB Context</param>
        public TodoDbStore(ApplicationDbContext context) {
            _db = context;
        }

        /// <summary>
        /// Regists the todo.
        /// Todo新規登録
        /// </summary>
        /// <returns><c>true</c>, if todo was registed, <c>false</c> otherwise.</returns>
        /// <param name="todo">todo class.</param>
        public bool Regist(Todo todo) {

            //不要？動作確認後削除
            //var tresult = new Todo {
            //    userId = todo.userId,
            //    items = todo.items,
            //    tweetId = todo.tweetId
            //};
            try {
                _db.Todos.Add(todo);
                _db.SaveChanges();

                return true;   //正常終了値のつもり、あとでちゃんと考えようね
            } catch (Exception e) {
                throw e;
            }
        }

        /// <summary>
        /// Gets the todo.
        /// 既存タスク取得
        /// </summary>
        /// <returns>The todo.</returns>
        /// <param name="userId">User Id.（Twitter Account Id）</param>
        public Todo Read(string userId) {
            try {
                var result = _db.Todos.Where(x => x.UserId == userId && x.IsClose == false)
                                                .Include(x => x.Items)
                                                .SingleOrDefault();
                //最後に見つかったレコードは必ずendFlagが0?
                return result;
            } catch (Exception e) {
                throw e;
            }
        }

        /// <summary>
        /// Updates the todo.
        /// Todo ItemのstateとツイートID（リプライ先）の更新
        /// </summary>
        /// <returns><c>true</c>, if todo was updated, <c>false</c> otherwise.</returns>
        /// <param name="todo">todo class.</param>
        public bool Update(Todo todo) {
            try {
                var result = _db.Todos.Where(x => x.Id == todo.Id && x.IsClose == false)
                                         .Include(x => x.Items)
                                         .SingleOrDefault();

                //task idが新規採番された状態となるため新規タスクとして登録されてしまう。
                result.Items = todo.Items;
                result.TweetId = todo.TweetId;
                _db.SaveChanges();
                return true;   //正常終了値のつもり、あとでちゃんと考えようね

            } catch (Exception e) {
                throw e;
            }
        }

        /// <summary>
        /// Closes the todo.
        /// 既存タスクのクローズ（新規Todo登録の前処理）
        /// </summary>
        /// <returns><c>true</c>, if todo was closed, <c>false</c> otherwise.</returns>
        /// <param name="userId">User Id.（Twitter Account Id）</param>
        public bool Close(string userId) {
            try {
                var result = _db.Todos.Where(x => x.UserId == userId && x.IsClose == false)
                                                .Include(x => x.Items)
                                                .SingleOrDefault();
                if (result != null) {   //タスクが存在しなければ実行しない
                    result.IsClose = true;
                    _db.SaveChanges();
                }
                return true;
            } catch (Exception e) {
                throw e;
            }
        }

        /// <summary>
        /// Deletes the todo.
        /// 退会時Todoデータ削除処理
        /// </summary>
        /// <param name="userId">User Id.（Twitter Account Id）</param>
        public void Delete(string userId) {
            try {
                var result = _db.Todos.Where(x => x.UserId == userId)
                                             .Include(x => x.Items);
                _db.Todos.RemoveRange(result);

                _db.SaveChanges();
            } catch (Exception e) {
                throw e;
            }
        }
    }
}
