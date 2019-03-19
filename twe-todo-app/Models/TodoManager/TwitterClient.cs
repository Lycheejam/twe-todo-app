using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CoreTweet;
using Microsoft.AspNetCore.Http;
using twe_todo_app.Models.Keys;

namespace twe_todo_app.Models.TodoManager {
    /// <summary>
    /// Twitter API client.
    /// </summary>
    public class TwitterClient {

        /// <summary>
        /// The httpcontextaccessor.
        /// </summary>
        private readonly IHttpContextAccessor _httpcontextaccessor;

        /// <summary>
        /// Twitter API Consumer Keys
        /// </summary>
        private readonly ConsumerKeys _consumer;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:twe_todo_app.Models.TodoManager.TwitterClient"/> class.
        /// </summary>
        /// <param name="httpcontextaccessor">Httpcontextaccessor.</param>
        /// <param name="consumer">Twitter API Consumer Keys.</param>
        public TwitterClient(IHttpContextAccessor httpcontextaccessor, ConsumerKeys consumer) {
            _httpcontextaccessor = httpcontextaccessor;
            _consumer = consumer;
        }

        /// <summary>
        /// Posts the new tweet.
        /// Todo登録用Tweet新規投稿処理
        /// </summary>
        /// <returns>ツイート投稿レスポンス</returns>
        /// <param name="tweetStr">ツイート文字列</param>
        public async Task<StatusResponse> NewPost(string tweetStr) {
            try {
                var tokens = await CreateTokens();
                var res = tokens.Statuses.Update(status => tweetStr);

                return res;
            } catch (TwitterException twiex) {
                throw twiex;
            } catch (Exception e) {
                throw e;
            }
        }

        /// <summary>
        /// Posts the reply tweet.
        /// Todo更新用ツイート処理（リプライ形式）
        /// </summary>
        /// <returns>The reply tweet.</returns>
        /// <param name="tweetStr">ツイート文字列</param>
        /// <param name="tweetId">リプライ先ツイートID</param>
        public async Task<StatusResponse> ReplyPost(string tweetStr, long tweetId) {
            try {
                var tokens = await CreateTokens();
                var res = tokens.Statuses.Update(status => tweetStr
                                                , in_reply_to_status_id => tweetId);
                return res;
            } catch (TwitterException twiex) {
                throw twiex;
            } catch (Exception e) {
                throw e;
            }
        }

        /// <summary>
        /// Gets the tweet embed.
        /// MyPage表示用Tweet Embed取得処理
        /// </summary>
        /// <returns>取得対象のEmbed情報</returns>
        /// <param name="tweetId">Embed取得対象のツイートID</param>
        public async Task<Embed> GetEmbed(long tweetId) {
            try {
                var tokens = await CreateTokens();
                var emb = tokens.Statuses.Oembed(tweetId);
                return emb;
            } catch (TwitterException twiex) {
                //tweetが存在いしなかった場合
                if (twiex.Status == HttpStatusCode.NotFound) {
                    return null;
                }
                throw twiex;
            } catch (Exception e) {
                throw e;
            }
        }

        /// <summary>
        /// Creates the tokens.
        /// Twitter api認証用トークン作成処理
        /// </summary>
        /// <returns>The tokens.</returns>
        private async Task<Tokens> CreateTokens() {
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
            var tokens = Tokens.Create(_consumer.Key
                                     , _consumer.Secret
                                     , access.Token
                                     , access.TokenSecret);
            return tokens;
        }
    }
}