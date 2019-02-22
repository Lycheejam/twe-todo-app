using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using twe_todo_app.Data;
using twe_todo_app.Models.TodoModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace twe_todo_app.Controllers {
    public class TodoController : Controller {
        private readonly TweetStoreManager _tweetstoremanager;
        private readonly TweetCreater _tweetcreater;
        private readonly TweetManager _tweetmanager;
        private readonly Claim _claim;

        // コンストラクタ
        public TodoController(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context) {
            _claim = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            _tweetstoremanager = new TweetStoreManager(context);
            _tweetcreater = new TweetCreater();
            _tweetmanager = new TweetManager();
        }

        // GET: Tweet
        public async Task<IActionResult> Index() {
            var id = _claim.Value;
            if (null == id) {
                var res = _tweetstoremanager.ReadTask(id);
                if (null == res) {
                    //ログイン中 かつ タスクが存在しない場合
                    return View();
                }

                //var _tweetmanager = new TweetManager();
                var emb = await _tweetmanager.EmbedTweetGet(res.tweetId);
                //ログイン中 かつ タスクが存在した場合の表示
                return View(emb);
            }
            //ログインしていない場合の初期表示
            return View();
        }

        //タスク登録画面初期表示
        [HttpGet]
        public IActionResult Regist() {
            //最新タスクの削除処理
            var id = _claim.Value;
            if (_tweetstoremanager.DeleteTask(id)) {
                return View();  //削除正常終了
            }
            //なにかによって失敗した場合
            return View("Index");
        }

        //タスク登録画面からタスクをツイート&登録
        [HttpPost]
        public async Task<IActionResult> Regist(TweetResult _tweetresult) {
            //nullのタスクを削除
            _tweetresult.tasks.RemoveAll(x => x.task == null);
            //Tweet用文字列生成
            var tweet = _tweetcreater.CreateTask(_tweetresult);

            //var _tweetmanager = new TweetManager();
            var res = await _tweetmanager.PostTweet(tweet);
            _tweetresult.tweetId = res.Id;
            _tweetresult.userId = _claim.Value;

            if (_tweetstoremanager.CreateTask(_tweetresult)) {
                return View("Index", await _tweetmanager.EmbedTweetGet(_tweetresult.tweetId));    //DBへの登録が正常終了
            }
            //失敗の時
            return View("Index");
        }

        //タスク更新画面の初期表示
        [HttpGet]
        public ActionResult Update() {
            var id = _claim.Value;
            //現状表示？
            return View(_tweetstoremanager.ReadTask(id));
        }

        //タスク更新画面からタスクのステータスを更新
        [HttpPost]
        public async Task<ActionResult> Update(TweetResult _tweetresult) {
            //Tweet用文字列生成
            var tweet = _tweetcreater.UpdateTask(_tweetresult);

            var id = _claim.Value;
            var tr = _tweetstoremanager.ReadTask(id);

            //var _tweetmanager = new TweetManager();
            var res = await _tweetmanager.ReplyTweet(tweet, tr.tweetId);
            _tweetresult.userId = tr.userId;
            _tweetresult.id = tr.id;
            _tweetresult.tweetId = res.Id;

            if (_tweetstoremanager.UpdateTask(_tweetresult)) {
                return View("Index", await _tweetmanager.EmbedTweetGet(_tweetresult.tweetId));    //DBへの登録が正常終了
            }
            //失敗の時
            return View("Index");
        }

        //これ別にいらなくね？
        //タスクの削除
        public IActionResult DeleteMyTask() {
            return View("Index");
        }
    }
}
