using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using twe_todo_app.Data;

namespace twe_todo_app.Models.TodoModels {
    public class TweetStoreManager {
        private readonly ApplicationDbContext _db;

        //コンストラクタ
        public TweetStoreManager(ApplicationDbContext context) {
            _db = context;
        }

        //タスクの初回登録
        public bool CreateTask(TweetResult tr) {
            var tresult = new TweetResult {
                userId = tr.userId,
                tasks = tr.tasks,
                tweetId = tr.tweetId
            };
            try {
                _db.TweetResults.Add(tr);
                _db.SaveChanges();

                return true;   //正常終了値のつもり、あとでちゃんと考えようね
            } catch (Exception) {
                throw;
            }
        }

        //タスク読み込み
        public TweetResult ReadTask(string id) {
            try {
                //データ消さない方針ならendflag別にいらなくね？
                TweetResult tr = _db.TweetResults.Where(x => x.userId == id && x.endFlag == 0)
                                                .Include(x => x.tasks)
                                                .SingleOrDefault();
                return tr;  //最後に見つかったレコードは必ずendFlagが0?

            } catch (Exception) {
                throw;
            }
        }

        //タスクのステータス更新とツイートID（リプライ先）の更新
        public bool UpdateTask(TweetResult _tweetresult) {
            try {
                var tresult = _db.TweetResults.Where(x => x.id == _tweetresult.id && x.endFlag == 0)
                                         .Include(x => x.tasks)
                                         .SingleOrDefault();

                //task idが新規採番された状態となるため新規タスクとして登録されてしまう。
                tresult.tasks = _tweetresult.tasks;
                tresult.tweetId = _tweetresult.tweetId;
                _db.SaveChanges();
                return true;   //正常終了値のつもり、あとでちゃんと考えようね

            } catch (Exception) {
                throw;
            }
        }


        //タスクの削除（と言うかエンドフラグを立てて表示させないようにする。）
        public bool DeleteTask(string id) {
            try {
                TweetResult tr = _db.TweetResults.Where(x => x.userId == id && x.endFlag == 0)
                                                .Include(x => x.tasks)
                                                .SingleOrDefault();
                if (tr != null) {   //タスクが存在しなければ実行しない
                    tr.endFlag = 1;
                    _db.SaveChanges();
                }
                return true;
            } catch (Exception) {
                throw;
            }
        }

        //退会時のデータ全削除処理
        public void DeleteAllTask(string id) {
            try {
                var result = _db.TweetResults.Where(x => x.userId == id)
                                             .Include(x => x.tasks);
                _db.TweetResults.RemoveRange(result);

                _db.SaveChanges();
            } catch (Exception) {
                throw;
            }
        }
    }
}
