# What's this...

やらないと行けないけど中々やる気にならない身近なことを周りに宣言して実行することをコンセプトにしています。  
3分間でできそうなことだけどやる気を出さないとやらないことをタスクとしてつぶやいて結果もツイートします。  

Twitter主体の生活をしている私ならではのTwitterを主体としたTodoアプリです。

# What's use...

|||||
|-|-|-|-|
|<blockquote class="twitter-tweet" data-lang="ja"><p lang="ja" dir="ltr">┬[] 銀行へ行く<br>├[] 新幹線の切符買いに行く<br>├[] ブログ記事書く<br>└[] ブログ記事書き溜める<a href="https://twitter.com/hashtag/3%E5%88%86%E9%96%93%E6%9C%AC%E6%B0%97%E5%87%BA%E3%81%99?src=hash&amp;ref_src=twsrc%5Etfw">#3分間本気出す</a></p>&mdash; あとらす (@Lychee_jam) <a href="https://twitter.com/Lychee_jam/status/990091857798483968?ref_src=twsrc%5Etfw">2018年4月28日</a></blockquote>|<blockquote class="twitter-tweet" data-lang="ja"><p lang="ja" dir="ltr">┬[○] 銀行へ行く<br>├[○] 新幹線の切符買いに行く<br>├[] ブログ記事書く<br>└[] ブログ記事書き溜める<a href="https://twitter.com/hashtag/3%E5%88%86%E9%96%93%E6%9C%AC%E6%B0%97%E5%87%BA%E3%81%99?src=hash&amp;ref_src=twsrc%5Etfw">#3分間本気出す</a></p>&mdash; あとらす (@Lychee_jam) <a href="https://twitter.com/Lychee_jam/status/990116605307715584?ref_src=twsrc%5Etfw">2018年4月28日</a></blockquote>|<blockquote class="twitter-tweet" data-lang="ja"><p lang="ja" dir="ltr">┬[○] 銀行へ行く<br>├[○] 新幹線の切符買いに行く<br>├[○] ブログ記事書く<br>└[] ブログ記事書き溜める<a href="https://twitter.com/hashtag/3%E5%88%86%E9%96%93%E6%9C%AC%E6%B0%97%E5%87%BA%E3%81%99?src=hash&amp;ref_src=twsrc%5Etfw">#3分間本気出す</a></p>&mdash; あとらす (@Lychee_jam) <a href="https://twitter.com/Lychee_jam/status/990150908083253248?ref_src=twsrc%5Etfw">2018年4月28日</a></blockquote>|<blockquote class="twitter-tweet" data-lang="ja"><p lang="ja" dir="ltr">┬[○] 銀行へ行く<br>├[○] 新幹線の切符買いに行く<br>├[○] ブログ記事書く<br>└[×] ブログ記事書き溜める<a href="https://twitter.com/hashtag/3%E5%88%86%E9%96%93%E6%9C%AC%E6%B0%97%E5%87%BA%E3%81%99?src=hash&amp;ref_src=twsrc%5Etfw">#3分間本気出す</a></p>&mdash; あとらす (@Lychee_jam) <a href="https://twitter.com/Lychee_jam/status/990269326321074176?ref_src=twsrc%5Etfw">2018年4月28日</a></blockquote>|

上記のような形でTodoを管理することができます。

# What's build...
## On OS X
* Visual Studio for Mac or Visual Studio Code
* .NET Core 2.2 SDK
  * Template: ASP.NET Core MVC add Individual authentication
  ```sh
  $ dotnet new mvc -n hoge -au Individual
  ```
* Packages
  * CoreTweet Version:1.0.0.483
  * Pomelo.EntityFrameworkCore.MySql Version:2.2.0

## On Windows

# What's code...
ASP.NET MVCで作りかけだったプロジェクトからロジック部分のコードをほぼ移植しています。  
そのためASP.NET Coreのお作法に則った修正が必要です。

# Future subject
作っておいてあれですがTwitterを使うためにわざわざ別のWebページを開くのかと言う問題点があります（特にスマフォの場合）  
本プロジェクトをMVCアプリではなくWebAPIプロジェクトとして作り直しWebアプリ+スマフォアプリのマルチプラットフォーム的なことをしたいなとか考えていたりします。

# License
