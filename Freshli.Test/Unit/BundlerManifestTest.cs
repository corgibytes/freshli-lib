using Freshli.Languages.Ruby;
using Xunit;

namespace Freshli.Test.Unit
{
  public class BundlerManifestTest
  {
    private static readonly string Contents = @"GEM
  remote: https://rubygems.org/
  specs:
    mini_portile2 (2.4.0)
    nokogiri (1.9.1)
      mini_portile2 (~> 2.4.0)

PLATFORMS
  ruby

DEPENDENCIES
  nokogiri (= 1.9.1)

BUNDLED WITH
   2.1.3
";

    // Copied from Feedbin project https://github.com/feedbin/feedbin
    private static readonly string FeedbinContents = @"GIT
  remote: git://github.com/benubois/html-pipeline.git
  revision: 652162b4c6c6a320cf8a45ee035704c10f802522
  ref: 652162b
  specs:
    html-pipeline (0.0.12)
      activesupport (>= 2)
      escape_utils (~> 0.3)
      gemoji (~> 1.0)
      github-markdown (~> 0.5)
      nokogiri (~> 1.4)
      rinku (~> 1.7)
      sanitize (~> 2.0)

GIT
  remote: git://github.com/feedbin/activerecord-import.git
  revision: b7851b1b0bb50412240d37afe2ceeb10af406b99
  ref: b7851b1
  specs:
    activerecord-import (0.4.0)
      activerecord (>= 3.0)

GIT
  remote: git://github.com/feedbin/feedzirra.git
  revision: c782e93eaf613aa29e75fcd64c077a281e7c261d
  ref: c782e93
  specs:
    feedzirra (0.2.0.rc2)
      curb (~> 0.8.1)
      loofah (~> 1.2.1)
      nokogiri (~> 1.5.3)
      sax-machine (~> 0.2.0.rc1)

GIT
  remote: git://github.com/feedbin/multi_fetch_fragments.git
  revision: e99b6f7d82d1cca35969d999c60985fcaa583b9b
  ref: e99b6f7
  specs:
    multi_fetch_fragments (0.0.16)

GIT
  remote: git://github.com/feedbin/opml_saw.git
  revision: 61d8c2dd3d46e9cae38178754469dd9da7c8bd6a
  ref: 61d8c2d
  specs:
    opml_saw (0.0.2)
      nokogiri

GIT
  remote: git://github.com/redis/redis-rb.git
  revision: 77c2a9965772c7a6786d90ac40344a632959f229
  ref: 77c2a99
  specs:
    redis (3.0.4)

GIT
  remote: git://github.com/sosedoff/capistrano-unicorn.git
  revision: 52376ad3d1fb20c21b7d79806bcf79fb91c78471
  ref: 52376ad
  specs:
    capistrano-unicorn (0.1.9)
      capistrano

GEM
  remote: https://rubygems.org/
  specs:
    actionmailer (4.0.0)
      actionpack (= 4.0.0)
      mail (~> 2.5.3)
    actionpack (4.0.0)
      activesupport (= 4.0.0)
      builder (~> 3.1.0)
      erubis (~> 2.7.0)
      rack (~> 1.5.2)
      rack-test (~> 0.6.2)
    activemodel (4.0.0)
      activesupport (= 4.0.0)
      builder (~> 3.1.0)
    activerecord (4.0.0)
      activemodel (= 4.0.0)
      activerecord-deprecated_finders (~> 1.0.2)
      activesupport (= 4.0.0)
      arel (~> 4.0.0)
    activerecord-deprecated_finders (1.0.3)
    activesupport (4.0.0)
      i18n (~> 0.6, >= 0.6.4)
      minitest (~> 4.2)
      multi_json (~> 1.3)
      thread_safe (~> 0.1)
      tzinfo (~> 0.3.37)
    addressable (2.3.5)
    aggregate (0.2.2)
    arel (4.0.0)
    atomic (1.1.13)
    autoprefixer-rails (0.7.20130824)
      execjs
    bcrypt-ruby (3.0.1)
    better_errors (0.9.0)
      coderay (>= 1.0.0)
      erubis (>= 2.6.6)
    builder (3.1.4)
    bust_rails_etags (0.0.5)
    capistrano (2.15.5)
      highline
      net-scp (>= 1.0.0)
      net-sftp (>= 2.0.0)
      net-ssh (>= 2.0.14)
      net-ssh-gateway (>= 1.1.0)
    carrierwave (0.9.0)
      activemodel (>= 3.2.0)
      activesupport (>= 3.2.0)
      json (>= 1.7)
    carrierwave_direct (0.0.13)
      carrierwave
      fog
      uuidtools
    celluloid (0.14.1)
      timers (>= 1.0.0)
    clockwork (0.6.0)
      activesupport
      tzinfo (~> 0.3.35)
    coderay (1.0.9)
    coffee-rails (4.0.0)
      coffee-script (>= 2.2.0)
      railties (>= 4.0.0.beta, < 5.0)
    coffee-script (2.2.0)
      coffee-script-source
      execjs
    coffee-script-source (1.6.3)
    connection_pool (1.1.0)
    curb (0.8.4)
    dalli (2.6.4)
    dotenv (0.8.0)
    erubis (2.7.0)
    escape_utils (0.3.2)
    excon (0.25.3)
    execjs (1.4.0)
      multi_json (~> 1.0)
    faraday (0.8.8)
      multipart-post (~> 1.2.0)
    faraday_middleware (0.9.0)
      faraday (>= 0.7.4, < 0.9)
    fog (1.15.0)
      builder
      excon (~> 0.25.0)
      formatador (~> 0.2.0)
      mime-types
      multi_json (~> 1.0)
      net-scp (~> 1.1)
      net-ssh (>= 2.1.3)
      nokogiri (~> 1.5)
      ruby-hmac
    foreman (0.63.0)
      dotenv (>= 0.7)
      thor (>= 0.13.6)
    formatador (0.2.4)
    gemoji (1.4.0)
    github-markdown (0.5.3)
    hashie (1.2.0)
    highline (1.6.19)
    hike (1.2.3)
    honeybadger (1.8.0)
      json
    i18n (0.6.5)
    jbuilder (1.5.0)
      activesupport (>= 3.0.0)
      multi_json (>= 1.2.0)
    jquery-rails (3.0.4)
      railties (>= 3.0, < 5.0)
      thor (>= 0.14, < 2.0)
    json (1.8.0)
    kgio (2.8.0)
    librato-metrics (1.1.0)
      aggregate (~> 0.2.2)
      faraday (~> 0.7)
      multi_json
    librato-rails (0.9.0)
      librato-metrics (~> 1.1.0)
      rails (>= 3.0)
    libv8 (3.11.8.17)
    lograge (0.2.0)
      actionpack
      activesupport
    longurl (0.1.6)
      json
    loofah (1.2.1)
      nokogiri (>= 1.4.4)
    mail (2.5.4)
      mime-types (~> 1.16)
      treetop (~> 1.4.8)
    mime-types (1.24)
    minitest (4.7.5)
    multi_json (1.7.9)
    multi_xml (0.5.5)
    multipart-post (1.2.0)
    net-scp (1.1.2)
      net-ssh (>= 2.6.5)
    net-sftp (2.1.2)
      net-ssh (>= 2.6.5)
    net-ssh (2.6.8)
    net-ssh-gateway (1.2.0)
      net-ssh (>= 2.6.5)
    nokogiri (1.5.10)
    pg (0.16.0)
    polyglot (0.3.3)
    postmark (1.0.1)
      json
      rake
    postmark-rails (0.5.1)
      actionmailer (>= 3.0.0)
      postmark (~> 1.0)
    quiet_assets (1.0.2)
      railties (>= 3.1, < 5.0)
    rack (1.5.2)
    rack-protection (1.5.0)
      rack
    rack-test (0.6.2)
      rack (>= 1.0)
    rails (4.0.0)
      actionmailer (= 4.0.0)
      actionpack (= 4.0.0)
      activerecord (= 4.0.0)
      activesupport (= 4.0.0)
      bundler (>= 1.3.0, < 2.0)
      railties (= 4.0.0)
      sprockets-rails (~> 2.0.0)
    railties (4.0.0)
      actionpack (= 4.0.0)
      activesupport (= 4.0.0)
      rake (>= 0.8.7)
      thor (>= 0.18.1, < 2.0)
    raindrops (0.11.0)
    rake (10.1.0)
    readability_parser (0.0.3)
      faraday (~> 0.8.4)
      faraday_middleware (~> 0.9.0)
      hashie (~> 1.2.0)
      multi_json (~> 1.7.2)
      multi_xml (~> 0.5.2)
    redis-namespace (1.3.1)
      redis (~> 3.0.0)
    ref (1.0.5)
    request_exception_handler (0.4)
      actionpack (>= 2.1)
    rest-client (1.6.7)
      mime-types (>= 1.16)
    rinku (1.7.3)
    ruby-hmac (0.4.0)
    sanitize (2.0.6)
      nokogiri (>= 1.4.4)
    sass (3.2.10)
    sass-rails (4.0.0)
      railties (>= 4.0.0.beta, < 5.0)
      sass (>= 3.1.10)
      sprockets-rails (~> 2.0.0)
    sax-machine (0.2.0.rc1)
      nokogiri (~> 1.5.2)
    sidekiq (2.13.1)
      celluloid (>= 0.14.1)
      connection_pool (>= 1.0.0)
      json
      redis (>= 3.0)
      redis-namespace
    sinatra (1.4.3)
      rack (~> 1.4)
      rack-protection (~> 1.4)
      tilt (~> 1.3, >= 1.3.4)
    slim (2.0.1)
      temple (~> 0.6.6)
      tilt (>= 1.3.3, < 2.1)
    sprockets (2.10.0)
      hike (~> 1.2)
      multi_json (~> 1.0)
      rack (~> 1.0)
      tilt (~> 1.1, != 1.3.0)
    sprockets-rails (2.0.0)
      actionpack (>= 3.0)
      activesupport (>= 3.0)
      sprockets (~> 2.8)
    stripe (1.8.5)
      multi_json (>= 1.0.4, < 2)
      rest-client (~> 1.4)
    stripe_event (0.6.0)
      rails (>= 3.1)
      stripe (~> 1.6)
    temple (0.6.6)
    therubyracer (0.11.4)
      libv8 (~> 3.11.8.12)
      ref
    thor (0.18.1)
    thread_safe (0.1.2)
      atomic
    tilt (1.4.1)
    timers (1.1.0)
    treetop (1.4.15)
      polyglot
      polyglot (>= 0.3.1)
    tzinfo (0.3.37)
    uglifier (2.1.2)
      execjs (>= 0.3.0)
      multi_json (~> 1.0, >= 1.0.2)
    unicorn (4.6.3)
      kgio (~> 2.6)
      rack
      raindrops (~> 0.7)
    uuidtools (2.1.4)
    will_paginate (3.0.4)
    yajl-ruby (1.1.0)

PLATFORMS
  ruby

DEPENDENCIES
  activerecord-import!
  addressable
  autoprefixer-rails (~> 0.7)
  bcrypt-ruby (~> 3.0.0)
  better_errors
  bust_rails_etags
  capistrano-unicorn!
  carrierwave
  carrierwave_direct
  clockwork
  coffee-rails (~> 4.0.0)
  dalli
  feedzirra!
  fog
  foreman
  honeybadger
  html-pipeline!
  jbuilder
  jquery-rails
  librato-rails
  lograge
  longurl
  multi_fetch_fragments!
  nokogiri (= 1.5.10)
  opml_saw!
  pg
  postmark-rails
  quiet_assets
  rails (~> 4.0.0)
  readability_parser
  redis!
  request_exception_handler
  sanitize
  sass-rails (~> 4.0.0)
  sidekiq
  sinatra
  slim
  stripe
  stripe_event
  therubyracer
  uglifier (>= 1.0.3)
  unicorn
  will_paginate
  yajl-ruby
";

    [Fact]
    public void Parse()
    {
      var manifest = new BundlerManifest();
      manifest.Parse(Contents);

      AssertManifestContents(manifest);
    }

    [Fact]
    public void DoubleParse()
    {
      var manifest = new BundlerManifest();
      manifest.Parse(Contents);
      manifest.Parse(Contents);

      AssertManifestContents(manifest);
    }

    private static void AssertManifestContents(BundlerManifest manifest)
    {
      Assert.Equal(2, manifest.Count);
      Assert.Equal("2.4.0", manifest["mini_portile2"].Version);
      Assert.Equal("1.9.1", manifest["nokogiri"].Version);
    }

    [Fact]
    public void ParseFeedbin()
    {
      var manifest = new BundlerManifest();
      manifest.Parse(FeedbinContents);

      Assert.Equal(108, manifest.Count);
      Assert.Equal("4.0.0", manifest["actionmailer"].Version);
      Assert.Equal("4.0.0", manifest["actionpack"].Version);
      Assert.Equal("4.0.0", manifest["activemodel"].Version);
      Assert.Equal("4.0.0", manifest["activerecord"].Version);
      Assert.Equal("1.0.3", manifest["activerecord-deprecated_finders"].Version);
      Assert.Equal("4.0.0", manifest["activesupport"].Version);
      Assert.Equal("2.3.5", manifest["addressable"].Version);
      Assert.Equal("0.2.2", manifest["aggregate"].Version);
      Assert.Equal("4.0.0", manifest["arel"].Version);
      Assert.Equal("1.1.13", manifest["atomic"].Version);
      Assert.Equal("0.7.20130824", manifest["autoprefixer-rails"].Version);
      Assert.Equal("3.0.1", manifest["bcrypt-ruby"].Version);
      Assert.Equal("0.9.0", manifest["better_errors"].Version);
      Assert.Equal("3.1.4", manifest["builder"].Version);
      Assert.Equal("0.0.5", manifest["bust_rails_etags"].Version);
      Assert.Equal("2.15.5", manifest["capistrano"].Version);
      Assert.Equal("0.9.0", manifest["carrierwave"].Version);
      Assert.Equal("0.0.13", manifest["carrierwave_direct"].Version);
      Assert.Equal("0.14.1", manifest["celluloid"].Version);
      Assert.Equal("0.6.0", manifest["clockwork"].Version);
      Assert.Equal("1.0.9", manifest["coderay"].Version);
      Assert.Equal("4.0.0", manifest["coffee-rails"].Version);
      Assert.Equal("2.2.0", manifest["coffee-script"].Version);
      Assert.Equal("1.6.3", manifest["coffee-script-source"].Version);
      Assert.Equal("1.1.0", manifest["connection_pool"].Version);
      Assert.Equal("0.8.4", manifest["curb"].Version);
      Assert.Equal("0.8.0", manifest["dotenv"].Version);
      Assert.Equal("2.7.0", manifest["erubis"].Version);
      Assert.Equal("0.3.2", manifest["escape_utils"].Version);
      Assert.Equal("0.25.3", manifest["excon"].Version);
      Assert.Equal("1.4.0", manifest["execjs"].Version);
      Assert.Equal("0.8.8", manifest["faraday"].Version);
      Assert.Equal("0.9.0", manifest["faraday_middleware"].Version);
      Assert.Equal("1.15.0", manifest["fog"].Version);
      Assert.Equal("0.63.0", manifest["foreman"].Version);
      Assert.Equal("0.2.4", manifest["formatador"].Version);
      Assert.Equal("1.4.0", manifest["gemoji"].Version);
      Assert.Equal("0.5.3", manifest["github-markdown"].Version);
      Assert.Equal("1.2.0", manifest["hashie"].Version);
      Assert.Equal("1.6.19", manifest["highline"].Version);
      Assert.Equal("1.2.3", manifest["hike"].Version);
      Assert.Equal("1.8.0", manifest["honeybadger"].Version);
      Assert.Equal("0.6.5", manifest["i18n"].Version);
      Assert.Equal("1.5.0", manifest["jbuilder"].Version);
      Assert.Equal("3.0.4", manifest["jquery-rails"].Version);
      Assert.Equal("1.8.0", manifest["json"].Version);
      Assert.Equal("2.8.0", manifest["kgio"].Version);
      Assert.Equal("1.1.0", manifest["librato-metrics"].Version);
      Assert.Equal("0.9.0", manifest["librato-rails"].Version);
      Assert.Equal("3.11.8.17", manifest["libv8"].Version);
      Assert.Equal("0.2.0", manifest["lograge"].Version);
      Assert.Equal("0.1.6", manifest["longurl"].Version);
      Assert.Equal("1.2.1", manifest["loofah"].Version);
      Assert.Equal("2.5.4", manifest["mail"].Version);
      Assert.Equal("1.24", manifest["mime-types"].Version);
      Assert.Equal("4.7.5", manifest["minitest"].Version);
      Assert.Equal("1.7.9", manifest["multi_json"].Version);
      Assert.Equal("0.5.5", manifest["multi_xml"].Version);
      Assert.Equal("1.2.0", manifest["multipart-post"].Version);
      Assert.Equal("1.1.2", manifest["net-scp"].Version);
      Assert.Equal("2.1.2", manifest["net-sftp"].Version);
      Assert.Equal("2.6.8", manifest["net-ssh"].Version);
      Assert.Equal("1.2.0", manifest["net-ssh-gateway"].Version);
      Assert.Equal("1.5.10", manifest["nokogiri"].Version);
      Assert.Equal("0.16.0", manifest["pg"].Version);
      Assert.Equal("0.3.3", manifest["polyglot"].Version);
      Assert.Equal("1.0.1", manifest["postmark"].Version);
      Assert.Equal("0.5.1", manifest["postmark-rails"].Version);
      Assert.Equal("1.0.2", manifest["quiet_assets"].Version);
      Assert.Equal("1.5.2", manifest["rack"].Version);
      Assert.Equal("1.5.0", manifest["rack-protection"].Version);
      Assert.Equal("0.6.2", manifest["rack-test"].Version);
      Assert.Equal("4.0.0", manifest["rails"].Version);
      Assert.Equal("4.0.0", manifest["railties"].Version);
      Assert.Equal("0.11.0", manifest["raindrops"].Version);
      Assert.Equal("10.1.0", manifest["rake"].Version);
      Assert.Equal("0.0.3", manifest["readability_parser"].Version);
      Assert.Equal("1.3.1", manifest["redis-namespace"].Version);
      Assert.Equal("1.0.5", manifest["ref"].Version);
      Assert.Equal("0.4", manifest["request_exception_handler"].Version);
      Assert.Equal("1.6.7", manifest["rest-client"].Version);
      Assert.Equal("1.7.3", manifest["rinku"].Version);
      Assert.Equal("0.4.0", manifest["ruby-hmac"].Version);
      Assert.Equal("2.0.6", manifest["sanitize"].Version);
      Assert.Equal("3.2.10", manifest["sass"].Version);
      Assert.Equal("4.0.0", manifest["sass-rails"].Version);
      Assert.Equal("0.2.0.rc1", manifest["sax-machine"].Version);
      Assert.Equal("2.13.1", manifest["sidekiq"].Version);
      Assert.Equal("1.4.3", manifest["sinatra"].Version);
      Assert.Equal("2.0.1", manifest["slim"].Version);
      Assert.Equal("2.10.0", manifest["sprockets"].Version);
      Assert.Equal("2.0.0", manifest["sprockets-rails"].Version);
      Assert.Equal("1.8.5", manifest["stripe"].Version);
      Assert.Equal("0.6.0", manifest["stripe_event"].Version);
      Assert.Equal("0.6.6", manifest["temple"].Version);
      Assert.Equal("0.11.4", manifest["therubyracer"].Version);
      Assert.Equal("0.18.1", manifest["thor"].Version);
      Assert.Equal("0.1.2", manifest["thread_safe"].Version);
      Assert.Equal("1.4.1", manifest["tilt"].Version);
      Assert.Equal("1.1.0", manifest["timers"].Version);
      Assert.Equal("1.4.15", manifest["treetop"].Version);
      Assert.Equal("0.3.37", manifest["tzinfo"].Version);
      Assert.Equal("2.1.2", manifest["uglifier"].Version);
      Assert.Equal("4.6.3", manifest["unicorn"].Version);
      Assert.Equal("2.1.4", manifest["uuidtools"].Version);
      Assert.Equal("3.0.4", manifest["will_paginate"].Version);
      Assert.Equal("1.1.0", manifest["yajl-ruby"].Version);
    }
  }
}
