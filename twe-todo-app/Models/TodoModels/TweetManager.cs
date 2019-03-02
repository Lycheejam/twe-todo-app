using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CoreTweet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using twe_todo_app.Data;
using twe_todo_app.Models.Keys;

namespace twe_todo_app.Models.TodoModels {
    public class TweetManager {
        private readonly IHttpContextAccessor _httpcontextaccessor;

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

        //token作成
        public async Task<Tokens> CreateTokens() {
            //init user access tokens
            var access = new AccessTokens();
            // get user access tokens
            var claims = _httpcontextaccessor.HttpContext.User.Claims;
            //claims null check
            //ありえないからチェック外しても良い？
            if (claims.FirstOrDefault(x => x.Type == "AccessToken") != null) { // TokenとTokenSecretはペアで登録されるのでnullチェックは片方のみ行う
                access.Token = claims.FirstOrDefault(x => x.Type == "AccessToken").Value;
                access.TokenSecret = claims.FirstOrDefault(x => x.Type == "AccessTokenSecret").Value;
            }

            //ツイート用トークン生成
            var tokens = Tokens.Create("ConsumerSecret"
                                     , "ConsumerSecret"
                                     , access.Token    //テーブルから参照
                                     , access.TokenSecret);    //テーブルから参照
            return tokens;
        }
    }
}