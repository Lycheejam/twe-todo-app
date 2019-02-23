using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CoreTweet;
//using CoreTweet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using twe_todo_app.Data;

namespace twe_todo_app.Models.TodoModels {
    public class TweetManager {
        private readonly IHttpContextAccessor _httpcontextaccessor;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public TweetManager(IHttpContextAccessor httpcontextaccessor) {
            _httpcontextaccessor = httpcontextaccessor;
        }

        //ReplyTweetと同じ
        public async Task<StatusResponse> PostTweet(string tweetStr) {
            var tokens = await CreateTokens();
            var res = tokens.Statuses.Update(status => tweetStr);
            return res;
        }

        //UpdateTaskTweetを分離して引数をstringにする
        //単純にリプライだけのメソッドにしたい。
        public async Task<StatusResponse> ReplyTweet(string tweetStr, long tweetId) {
            var tokens = await CreateTokens();
            var res = tokens.Statuses.Update(status => tweetStr
                                            , in_reply_to_status_id => tweetId);
            return res;
        }
        //ツイート埋め込みウィジェット用のjsonデータを取得
        public async Task<Embed> EmbedTweetGet(long tweetId) {
            var tokens = await CreateTokens();
            var emb = tokens.Statuses.Oembed(tweetId);
            return emb;
        }
        //claimテーブルの参照はコピペ AccessToken&Secretをテーブルから参照する。
        //コピペ元 » ASP.NET Identity：Twitter認証時の情報でツイートする方法 - なか日記 
        // http://blog.nakajix.jp/entry/2014/09/12/074000
        public async Task<Tokens> CreateTokens() {
            //var claimkeys = new GetToken();

            //User.Identity.IsAuthentication;

            //var accessToken = _httpcontextaccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "AccessToken")?.Value;
            //var accessSecret = _httpcontextaccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "AccessTokenSecret")?.Value;


            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            var accessToken = info.Principal.Claims.FirstOrDefault(x => x.Type == "AccessToken")?.Value;
            var accessSecret = info.Principal.Claims.FirstOrDefault(x => x.Type == "AccessTokenSecret")?.Value;

            Console.WriteLine(accessToken);
            Console.WriteLine(accessSecret);

            //httpcontextクラス httpリクエストに反応していろいろしてくれるらしい
            //var usermgr = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            //var claims = await usermgr.GetClaimsAsync(HttpContext.Current.User.Identity.GetUserId());

            //var firstOrDefault = claims.FirstOrDefault(x => x.Type == "ExternalAccessToken");
            //if (firstOrDefault != null) // TokenとTokenSecretはペアで登録されるのでnullチェックは片方のみ行う
            //{
            //claimkeys.accessToken = firstOrDefault.Value;
            //claimkeys.accessTokenSecret = claims.FirstOrDefault(x => x.Type == "ExternalAccessTokenSecret").Value;
            //}

            //ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            //var accessToken = _claims.FirstOrDefault(x => x.Type == "AccessToken")?.Value;
            //var accessSecret = _claims.FirstOrDefault(x => x.Type == "AccessTokenSecret")?.Value;

            //var accessToken = HttpContext.User.FindFirst(x => x.Type == "AccessToken")?.Value;
            //var accessSecret = _claims.FirstOrDefault(x => x.Type == "AccessTokenSecret")?.Value;


            //Models.ReadToken APIキー取ってきてる。
            //var keys = MyTokens;

            //ツイート用トークン生成
            var tokens = Tokens.Create("jTVMXAbPHFOnyBaYwb3L2Tfx4"
                                     , "9GjqYtlmKQo3Uv0XLsyiWTuLoaJiL8ELRIa5vnI29byggnYrHf"
                                     , accessToken    //テーブルから参照
                                     , accessSecret);    //テーブルから参照
            return tokens;
        }
    }
}