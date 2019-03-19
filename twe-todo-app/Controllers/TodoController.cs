using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using twe_todo_app.Data;
using twe_todo_app.Models;
using twe_todo_app.Models.Keys;
using twe_todo_app.Models.TodoManager;
using twe_todo_app.Models.TodoModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace twe_todo_app.Controllers {
    /// <summary>
    /// Todo controller.
    /// </summary>
    public class TodoController : Controller {
        /// <summary>
        /// Todo Class DB Store
        /// </summary>
        private readonly TodoDbStore _tododbstore;

        /// <summary>
        /// The contentbuilder.
        /// Todo items contents to tweet string
        /// </summary>
        private readonly ContentBuilder _contentbuilder;

        /// <summary>
        /// The twitter client.
        /// ツイート投稿用クライアント
        /// </summary>
        private readonly TwitterClient _twitterclient;

        /// <summary>
        /// User ID（Twitter Account ID）
        /// </summary>
        private readonly Claim _userid;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:twe_todo_app.Controllers.TodoController"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">Http context accessor.</param>
        /// <param name="context">DB Context.</param>
        /// <param name="consumeraccesor">Twitter API Consumer key accesor.</param>
        public TodoController(IHttpContextAccessor httpContextAccessor
                            , ApplicationDbContext context
                            , IOptions<ConsumerKeys> consumeraccesor) {
            _userid = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            _tododbstore = new TodoDbStore(context);
            _contentbuilder = new ContentBuilder();
            _twitterclient = new TwitterClient(httpContextAccessor, consumeraccesor.Value);
        }

        /// <summary>
        /// Index this instance.
        /// GET : TOPページ表示
        /// </summary>
        /// <returns>The index.</returns>
        [Route("")]
        public IActionResult Index() {
            return View();
        }

        /// <summary>
        /// Mies the page.
        /// GET : マイページ初期表示
        /// </summary>
        /// <returns>The MyPage.</returns>
        [Authorize]
        [Route("MyPage")]
        public async Task<IActionResult> MyPage() {
            //MyPage表示用のツイート情報を取得する。
            var resTodo = _tododbstore.Read(_userid.Value);
            if (null == resTodo) {
                //ログイン中 かつ タスクが存在しない場合
                return View();
            }
            //取得成否の判定が必要 ここのエラー処理どうしよう？
            //ツイート取得
            var emb = await _twitterclient.GetEmbed(resTodo.TweetId);
            if (emb == null) {
                return View();
            }
            //ログイン中 かつ タスクが存在した場合の表示
            return View(emb);
        }

        /// <summary>
        /// Regist this instance.
        /// GET : タスク登録画面初期表示
        /// </summary>
        /// <returns>The regist page.</returns>
        [HttpGet]
        [Authorize]
        [Route("Regist")]
        public IActionResult Regist() {
            //最新Todoのクローズ処理
            if (_tododbstore.Close(_userid.Value)) {
                //クローズ正常終了
                return View();
            }
            //なにかによって失敗した場合
            return View("MyPage");
        }

        /// <summary>
        /// Regist the specified _tweetresult.
        /// POST : タスク登録画面からタスクをツイート&登録
        /// </summary>
        /// <returns>The regist page.</returns>
        /// <param name="todo">Todo Class from regist page form</param>
        [HttpPost]
        [Authorize]
        [Route("Regist")]
        public async Task<IActionResult> Regist(Todo todo) {
            //formより取得したnullのタスクを削除
            todo.Items.RemoveAll(x => x.Content == null);
            //Tweet用文字列生成
            var tweetString = _contentbuilder.Create(todo);

            //タスクをツイート＆ツイート結果を取得
            var resTodo = await _twitterclient.NewPost(tweetString);

            //リプライ用にツイートIDを格納
            todo.TweetId = resTodo.Id;
            todo.UserId = _userid.Value;

            //DBへの登録が正常終了
            if (_tododbstore.Regist(todo)) {
                return View("MyPage", await _twitterclient.GetEmbed(todo.TweetId));
            }

            //失敗の時
            return View("MyPage");
        }

        /// <summary>
        /// Update this instance.
        /// GET : タスク更新画面の初期表示
        /// </summary>
        /// <returns>The update page.</returns>
        [HttpGet]
        [Authorize]
        [Route("Update")]
        public ActionResult Update() {
            var result = _tododbstore.Read(_userid.Value);
            if (null == result) {
                // 過去TODOの読み込み成功
                return View(result);
            }

            // 過去TODOの読み込み失敗の時 エラーページ表示にしたい
            return View("MyPage");
        }

        /// <summary>
        /// Update the specified _tweetresult.
        /// POST : タスク更新画面からタスクのステータスを更新
        /// </summary>
        /// <returns>The update.</returns>
        /// <param name="todo">Todo Class from upfate page form.</param>
        [HttpPost]
        [Authorize]
        [Route("Update")]
        public async Task<ActionResult> Update(Todo todo) {
            //Tweet用文字列生成
            var tweetString = _contentbuilder.Update(todo);

            //直前 かつ 完了 ではないタスクの取得
            var resultTodo = _tododbstore.Read(_userid.Value);
            if (null == resultTodo) {
                //直前 かつ 完了ではないタスクに対してリプライ形式でツイートする。
                var resTweet = await _twitterclient.ReplyPost(tweetString, resultTodo.TweetId);
                todo.UserId = resultTodo.UserId;
                todo.Id = resultTodo.Id;
                todo.TweetId = resTweet.Id;

                if (_tododbstore.Update(todo)) {
                    //DBへの登録が正常終了
                    return View("MyPage", await _twitterclient.GetEmbed(todo.TweetId));
                }
            }
            //失敗の時
            return View("MyPage");
        }

        [Route("/Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
