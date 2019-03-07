using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using twe_todo_app.Data;
using twe_todo_app.Models.Keys;
using twe_todo_app.Models.TodoModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace twe_todo_app.Controllers {
    public class TodoController : Controller {
        private readonly TweetStoreManager _tweetstoremanager;
        private readonly TweetCreater _tweetcreater;
        private readonly TweetManager _tweetmanager;
        private readonly Claim _userid;
        private readonly IHttpContextAccessor _httpcontextaccessor;
        private readonly ConsumerKeys _consumer;


        // コンストラクタ
        public TodoController(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context, IOptions<ConsumerKeys> consumeraccesor) {
            _userid = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            _httpcontextaccessor = httpContextAccessor;
            _tweetstoremanager = new TweetStoreManager(context);
            _tweetcreater = new TweetCreater();
            _consumer = consumeraccesor.Value;
            _tweetmanager = new TweetManager(_httpcontextaccessor, _consumer);
        }

        // GET: TOP page
        [Route("")]
        public IActionResult Index() {
            return View();
        }

        // GET: Tweet
        [Route("MyPage")]
        public async Task<IActionResult> MyPage() {
            if (_httpcontextaccessor.HttpContext.User.Identity.IsAuthenticated) {
                var tweet_task = _tweetstoremanager.ReadTask(_userid.Value);
                if (null == tweet_task) {
                    //ログイン中 かつ タスクが存在しない場合
                    return View();
                }

                //ツイート取得
                var emb = await _tweetmanager.EmbedTweetGet(tweet_task.tweetId);
                //ログイン中 かつ タスクが存在した場合の表示
                return View(emb);
            }

            //ログインしていない場合の初期表示
            //20190304 これおかしくない？ログインした状態でTodo indexに遷移したらここにいっちゃうべ？
            return View();
        }

        //タスク登録画面初期表示
        [HttpGet]
        [Route("Regist")]
        public IActionResult Regist() {
            if (_httpcontextaccessor.HttpContext.User.Identity.IsAuthenticated) {
                //最新タスクの削除処理
                if (_tweetstoremanager.DeleteTask(_userid.Value)) {
                    return View();  //削除正常終了
                }
            }
            //なにかによって失敗した場合
            return View("MyPage");
        }

        //タスク登録画面からタスクをツイート&登録
        [HttpPost]
        [Route("Regist")]
        public async Task<IActionResult> Regist(TweetResult _tweetresult) {
            if (_httpcontextaccessor.HttpContext.User.Identity.IsAuthenticated) {
                //formより取得したnullのタスクを削除
                _tweetresult.tasks.RemoveAll(x => x.task == null);
                //Tweet用文字列生成
                var tweet = _tweetcreater.CreateTask(_tweetresult);

                //タスクをツイート＆ツイート結果を取得
                var tweet_response = await _tweetmanager.PostTweet(tweet);
                //リプライ用にツイートIDを格納
                _tweetresult.tweetId = tweet_response.Id;
                _tweetresult.userId = _userid.Value;

                if (_tweetstoremanager.CreateTask(_tweetresult)) {
                    return View("MyPage", await _tweetmanager.EmbedTweetGet(_tweetresult.tweetId));    //DBへの登録が正常終了
                }
            }
            //失敗の時
            return View("Index");
        }

        //タスク更新画面の初期表示
        [HttpGet]
        [Route("Update")]
        public ActionResult Update() {
            if (_httpcontextaccessor.HttpContext.User.Identity.IsAuthenticated) {
                //現状表示？
                return View(_tweetstoremanager.ReadTask(_userid.Value));
            }
            return View("MyPage");
        }

        //タスク更新画面からタスクのステータスを更新
        [HttpPost]
        [Route("Update")]
        public async Task<ActionResult> Update(TweetResult _tweetresult) {
            if (_httpcontextaccessor.HttpContext.User.Identity.IsAuthenticated) {
                //Tweet用文字列生成
                var tweet = _tweetcreater.UpdateTask(_tweetresult);

                //直前 かつ 完了 ではないタスクの取得
                var tr = _tweetstoremanager.ReadTask(_userid.Value);

                //直前 かつ 完了ではないタスクに対してリプライ形式でツイートする。
                var tweet_response = await _tweetmanager.ReplyTweet(tweet, tr.tweetId);
                _tweetresult.userId = tr.userId;
                _tweetresult.id = tr.id;
                _tweetresult.tweetId = tweet_response.Id;

                if (_tweetstoremanager.UpdateTask(_tweetresult)) {
                    return View("MyPage", await _tweetmanager.EmbedTweetGet(_tweetresult.tweetId));    //DBへの登録が正常終了
                }
            }
            //失敗の時
            return View("MyPage");
        }

        //これ別にいらなくね？
        //タスクの削除
        public IActionResult DeleteMyTask() {
            return View("MyPage");
        }
    }
}
