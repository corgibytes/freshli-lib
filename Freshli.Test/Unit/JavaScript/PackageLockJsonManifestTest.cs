using System.IO;
using Freshli.Languages.JavaScript;
using Xunit;

namespace Freshli.Test.Unit.JavaScript {
  public class PackageLockJsonManifestTest {
    [Fact]
    public void Parse() {
      var contents = File.ReadAllText(
        Fixtures.Path("javascript", "npm-with-lock", "package-lock.json")
      );

      var manifest = new PackageLockJsonManifest();
      manifest.Parse(contents);

      Assert.Equal(528, manifest.Count);

      Assert.Equal("7.5.5", manifest["@babel/code-frame"].Version);
      Assert.Equal("7.5.0", manifest["@babel/highlight"].Version);
      Assert.Equal("1.0.9", manifest["abbrev"].Version);
      Assert.Equal("7.0.0", manifest["acorn"].Version);
      Assert.Equal("5.0.1", manifest["acorn-jsx"].Version);
      Assert.Equal("6.10.2", manifest["ajv"].Version);
      Assert.Equal("0.1.4", manifest["align-text"].Version);
      Assert.Equal("1.0.1", manifest["amdefine"].Version);
      Assert.Equal("4.2.1", manifest["ansi-escapes"].Version);
      Assert.Equal("2.1.1", manifest["ansi-regex"].Version);
      Assert.Equal("2.2.1", manifest["ansi-styles"].Version);
      Assert.Equal("1.3.2", manifest["anymatch"].Version);
      Assert.Equal("1.0.10", manifest["argparse"].Version);
      Assert.Equal("2.0.0", manifest["arr-diff"].Version);
      Assert.Equal("1.1.0", manifest["arr-flatten"].Version);
      Assert.Equal("3.0.3", manifest["array-includes"].Version);
      Assert.Equal("0.2.1", manifest["array-unique"].Version);
      Assert.Equal("0.1.11", manifest["asn1"].Version);
      Assert.Equal("1.4.1", manifest["assert"].Version);
      Assert.Equal("0.1.5", manifest["assert-plus"].Version);
      Assert.Equal("1.0.0", manifest["astral-regex"].Version);
      Assert.Equal("2.6.0", manifest["async"].Version);
      Assert.Equal("1.0.1", manifest["async-each"].Version);
      Assert.Equal("0.4.0", manifest["asynckit"].Version);
      Assert.Equal("0.5.0", manifest["aws-sign2"].Version);
      Assert.Equal("1.7.0", manifest["aws4"].Version);
      Assert.Equal("6.26.0", manifest["babel-code-frame"].Version);
      Assert.Equal("6.26.3", manifest["babel-core"].Version);
      Assert.Equal("6.26.1", manifest["babel-generator"].Version);
      Assert.Equal(
        "6.24.1",
        manifest["babel-helper-bindify-decorators"].Version
      );
      Assert.Equal(
        "6.24.1",
        manifest["babel-helper-builder-binary-assignment-operator-visitor"].
          Version
      );
      Assert.Equal("6.24.1", manifest["babel-helper-call-delegate"].Version);
      Assert.Equal("6.26.0", manifest["babel-helper-define-map"].Version);
      Assert.Equal(
        "6.24.1",
        manifest["babel-helper-explode-assignable-expression"].Version
      );
      Assert.Equal("6.24.1", manifest["babel-helper-explode-class"].Version);
      Assert.Equal("6.24.1", manifest["babel-helper-function-name"].Version);
      Assert.Equal(
        "6.24.1",
        manifest["babel-helper-get-function-arity"].Version
      );
      Assert.Equal("6.24.1", manifest["babel-helper-hoist-variables"].Version);
      Assert.Equal(
        "6.24.1",
        manifest["babel-helper-optimise-call-expression"].Version
      );
      Assert.Equal("6.26.0", manifest["babel-helper-regex"].Version);
      Assert.Equal(
        "6.24.1",
        manifest["babel-helper-remap-async-to-generator"].Version
      );
      Assert.Equal("6.24.1", manifest["babel-helper-replace-supers"].Version);
      Assert.Equal("6.24.1", manifest["babel-helpers"].Version);
      Assert.Equal("6.23.0", manifest["babel-messages"].Version);
      Assert.Equal(
        "6.22.0",
        manifest["babel-plugin-check-es2015-constants"].Version
      );
      Assert.Equal(
        "6.13.0",
        manifest["babel-plugin-syntax-async-functions"].Version
      );
      Assert.Equal(
        "6.13.0",
        manifest["babel-plugin-syntax-async-generators"].Version
      );
      Assert.Equal(
        "6.18.0",
        manifest["babel-plugin-syntax-class-constructor-call"].Version
      );
      Assert.Equal(
        "6.13.0",
        manifest["babel-plugin-syntax-class-properties"].Version
      );
      Assert.Equal(
        "6.13.0",
        manifest["babel-plugin-syntax-decorators"].Version
      );
      Assert.Equal(
        "6.13.0",
        manifest["babel-plugin-syntax-do-expressions"].Version
      );
      Assert.Equal(
        "6.18.0",
        manifest["babel-plugin-syntax-dynamic-import"].Version
      );
      Assert.Equal(
        "6.13.0",
        manifest["babel-plugin-syntax-exponentiation-operator"].Version
      );
      Assert.Equal(
        "6.13.0",
        manifest["babel-plugin-syntax-export-extensions"].Version
      );
      Assert.Equal(
        "6.13.0",
        manifest["babel-plugin-syntax-function-bind"].Version
      );
      Assert.Equal(
        "6.13.0",
        manifest["babel-plugin-syntax-object-rest-spread"].Version
      );
      Assert.Equal(
        "6.22.0",
        manifest["babel-plugin-syntax-trailing-function-commas"].Version
      );
      Assert.Equal(
        "6.24.1",
        manifest["babel-plugin-transform-async-generator-functions"].Version
      );
      Assert.Equal(
        "6.24.1",
        manifest["babel-plugin-transform-async-to-generator"].Version
      );
      Assert.Equal(
        "6.24.1",
        manifest["babel-plugin-transform-class-constructor-call"].Version
      );
      Assert.Equal(
        "6.24.1",
        manifest["babel-plugin-transform-class-properties"].Version
      );
      Assert.Equal(
        "6.24.1",
        manifest["babel-plugin-transform-decorators"].Version
      );
      Assert.Equal(
        "6.22.0",
        manifest["babel-plugin-transform-do-expressions"].Version
      );
      Assert.Equal(
        "6.22.0",
        manifest["babel-plugin-transform-es2015-arrow-functions"].Version
      );
      Assert.Equal(
        "6.22.0",
        manifest["babel-plugin-transform-es2015-block-scoped-functions"].Version
      );
      Assert.Equal(
        "6.26.0",
        manifest["babel-plugin-transform-es2015-block-scoping"].Version
      );
      Assert.Equal(
        "6.24.1",
        manifest["babel-plugin-transform-es2015-classes"].Version
      );
      Assert.Equal(
        "6.24.1",
        manifest["babel-plugin-transform-es2015-computed-properties"].Version
      );
      Assert.Equal(
        "6.23.0",
        manifest["babel-plugin-transform-es2015-destructuring"].Version
      );
      Assert.Equal(
        "6.24.1",
        manifest["babel-plugin-transform-es2015-duplicate-keys"].Version
      );
      Assert.Equal(
        "6.23.0",
        manifest["babel-plugin-transform-es2015-for-of"].Version
      );
      Assert.Equal(
        "6.24.1",
        manifest["babel-plugin-transform-es2015-function-name"].Version
      );
      Assert.Equal(
        "6.22.0",
        manifest["babel-plugin-transform-es2015-literals"].Version
      );
      Assert.Equal(
        "6.24.1",
        manifest["babel-plugin-transform-es2015-modules-amd"].Version
      );
      Assert.Equal(
        "6.26.2",
        manifest["babel-plugin-transform-es2015-modules-commonjs"].Version
      );
      Assert.Equal(
        "6.24.1",
        manifest["babel-plugin-transform-es2015-modules-systemjs"].Version
      );
      Assert.Equal(
        "6.24.1",
        manifest["babel-plugin-transform-es2015-modules-umd"].Version
      );
      Assert.Equal(
        "6.24.1",
        manifest["babel-plugin-transform-es2015-object-super"].Version
      );
      Assert.Equal(
        "6.24.1",
        manifest["babel-plugin-transform-es2015-parameters"].Version
      );
      Assert.Equal(
        "6.24.1",
        manifest["babel-plugin-transform-es2015-shorthand-properties"].Version
      );
      Assert.Equal(
        "6.22.0",
        manifest["babel-plugin-transform-es2015-spread"].Version
      );
      Assert.Equal(
        "6.24.1",
        manifest["babel-plugin-transform-es2015-sticky-regex"].Version
      );
      Assert.Equal(
        "6.22.0",
        manifest["babel-plugin-transform-es2015-template-literals"].Version
      );
      Assert.Equal(
        "6.23.0",
        manifest["babel-plugin-transform-es2015-typeof-symbol"].Version
      );
      Assert.Equal(
        "6.24.1",
        manifest["babel-plugin-transform-es2015-unicode-regex"].Version
      );
      Assert.Equal(
        "6.24.1",
        manifest["babel-plugin-transform-exponentiation-operator"].Version
      );
      Assert.Equal(
        "6.22.0",
        manifest["babel-plugin-transform-export-extensions"].Version
      );
      Assert.Equal(
        "6.22.0",
        manifest["babel-plugin-transform-function-bind"].Version
      );
      Assert.Equal(
        "6.26.0",
        manifest["babel-plugin-transform-object-rest-spread"].Version
      );
      Assert.Equal(
        "6.26.0",
        manifest["babel-plugin-transform-regenerator"].Version
      );
      Assert.Equal(
        "6.24.1",
        manifest["babel-plugin-transform-strict-mode"].Version
      );
      Assert.Equal("6.24.1", manifest["babel-preset-es2015"].Version);
      Assert.Equal("6.24.1", manifest["babel-preset-stage-0"].Version);
      Assert.Equal("6.24.1", manifest["babel-preset-stage-1"].Version);
      Assert.Equal("6.24.1", manifest["babel-preset-stage-2"].Version);
      Assert.Equal("6.24.1", manifest["babel-preset-stage-3"].Version);
      Assert.Equal("6.26.0", manifest["babel-register"].Version);
      Assert.Equal("6.26.0", manifest["babel-runtime"].Version);
      Assert.Equal("6.26.0", manifest["babel-template"].Version);
      Assert.Equal("6.26.0", manifest["babel-traverse"].Version);
      Assert.Equal("6.26.0", manifest["babel-types"].Version);
      Assert.Equal("6.18.0", manifest["babylon"].Version);
      Assert.Equal("1.0.0", manifest["balanced-match"].Version);
      Assert.Equal("1.3.0", manifest["base64-js"].Version);
      Assert.Equal("1.0.1", manifest["bcrypt-pbkdf"].Version);
      Assert.Equal("2.1.4", manifest["benchmark"].Version);
      Assert.Equal("3.2.0", manifest["big.js"].Version);
      Assert.Equal("1.11.0", manifest["binary-extensions"].Version);
      Assert.Equal("0.9.5", manifest["bl"].Version);
      Assert.Equal("1.0.0", manifest["boolbase"].Version);
      Assert.Equal("0.4.2", manifest["boom"].Version);
      Assert.Equal("1.1.11", manifest["brace-expansion"].Version);
      Assert.Equal("1.8.5", manifest["braces"].Version);
      Assert.Equal("1.3.1", manifest["browser-stdout"].Version);
      Assert.Equal("0.4.0", manifest["browserify-aes"].Version);
      Assert.Equal("0.1.4", manifest["browserify-zlib"].Version);
      Assert.Equal("4.9.1", manifest["buffer"].Version);
      Assert.Equal("1.1.1", manifest["builtin-modules"].Version);
      Assert.Equal("3.0.0", manifest["builtin-status-codes"].Version);
      Assert.Equal("3.1.0", manifest["callsites"].Version);
      Assert.Equal("1.2.1", manifest["camelcase"].Version);
      Assert.Equal("0.6.0", manifest["caseless"].Version);
      Assert.Equal("0.1.3", manifest["center-align"].Version);
      Assert.Equal("1.1.3", manifest["chalk"].Version);
      Assert.Equal("0.7.0", manifest["chardet"].Version);
      Assert.Equal("0.22.0", manifest["cheerio"].Version);
      Assert.Equal("1.7.0", manifest["chokidar"].Version);
      Assert.Equal("3.1.0", manifest["cli-cursor"].Version);
      Assert.Equal("2.2.0", manifest["cli-width"].Version);
      Assert.Equal("2.1.0", manifest["cliui"].Version);
      Assert.Equal("1.0.4", manifest["clone"].Version);
      Assert.Equal("4.6.0", manifest["co"].Version);
      Assert.Equal("1.1.0", manifest["code-point-at"].Version);
      Assert.Equal("0.1.6", manifest["codecov.io"].Version);
      Assert.Equal("1.9.3", manifest["color-convert"].Version);
      Assert.Equal("1.1.3", manifest["color-name"].Version);
      Assert.Equal("0.0.7", manifest["combined-stream"].Version);
      Assert.Equal("2.15.1", manifest["commander"].Version);
      Assert.Equal("0.0.1", manifest["concat-map"].Version);
      Assert.Equal("1.1.0", manifest["console-browserify"].Version);
      Assert.Equal("1.0.0", manifest["constants-browserify"].Version);
      Assert.Equal("0.1.0", manifest["contains-path"].Version);
      Assert.Equal("1.5.1", manifest["convert-source-map"].Version);
      Assert.Equal("2.5.6", manifest["core-js"].Version);
      Assert.Equal("1.0.2", manifest["core-util-is"].Version);
      Assert.Equal("2.13.3", manifest["coveralls"].Version);
      Assert.Equal("5.1.0", manifest["cross-spawn"].Version);
      Assert.Equal("0.2.2", manifest["cryptiles"].Version);
      Assert.Equal("3.3.0", manifest["crypto-browserify"].Version);
      Assert.Equal("1.2.0", manifest["css-select"].Version);
      Assert.Equal("2.1.0", manifest["css-what"].Version);
      Assert.Equal("0.5.3", manifest["ctype"].Version);
      Assert.Equal("0.8.12", manifest["curl-amd"].Version);
      Assert.Equal("1.14.1", manifest["dashdash"].Version);
      Assert.Equal("0.1.4", manifest["date-now"].Version);
      Assert.Equal("2.6.9", manifest["debug"].Version);
      Assert.Equal("1.2.0", manifest["decamelize"].Version);
      Assert.Equal("0.1.2", manifest["deep-equal"].Version);
      Assert.Equal("0.1.3", manifest["deep-is"].Version);
      Assert.Equal("1.1.3", manifest["define-properties"].Version);
      Assert.Equal("0.0.0", manifest["defined"].Version);
      Assert.Equal("0.0.5", manifest["delayed-stream"].Version);
      Assert.Equal("0.1.0", manifest["detect-file"].Version);
      Assert.Equal("4.0.0", manifest["detect-indent"].Version);
      Assert.Equal("3.5.0", manifest["diff"].Version);
      Assert.Equal("0.7.3", manifest["docdown"].Version);
      Assert.Equal("2.0.0", manifest["doctrine"].Version);
      Assert.Equal("1.13.0", manifest["dojo"].Version);
      Assert.Equal("0.1.0", manifest["dom-serializer"].Version);
      Assert.Equal("1.2.0", manifest["domain-browser"].Version);
      Assert.Equal("1.3.0", manifest["domelementtype"].Version);
      Assert.Equal("2.4.2", manifest["domhandler"].Version);
      Assert.Equal("1.5.1", manifest["domutils"].Version);
      Assert.Equal("0.1.1", manifest["duplexer"].Version);
      Assert.Equal("0.1.1", manifest["ecc-jsbn"].Version);
      Assert.Equal("2.2.2", manifest["ecstatic"].Version);
      Assert.Equal("8.0.0", manifest["emoji-regex"].Version);
      Assert.Equal("2.1.0", manifest["emojis-list"].Version);
      Assert.Equal("0.9.1", manifest["enhanced-resolve"].Version);
      Assert.Equal("1.0.2", manifest["ensure-posix-path"].Version);
      Assert.Equal("1.1.1", manifest["entities"].Version);
      Assert.Equal("0.1.7", manifest["errno"].Version);
      Assert.Equal("1.3.1", manifest["error-ex"].Version);
      Assert.Equal("1.13.0", manifest["es-abstract"].Version);
      Assert.Equal("1.2.0", manifest["es-to-primitive"].Version);
      Assert.Equal("1.0.5", manifest["escape-string-regexp"].Version);
      Assert.Equal("1.8.1", manifest["escodegen"].Version);
      Assert.Equal("6.2.0", manifest["eslint"].Version);
      Assert.Equal("0.3.2", manifest["eslint-import-resolver-node"].Version);
      Assert.Equal("2.4.1", manifest["eslint-module-utils"].Version);
      Assert.Equal("2.18.2", manifest["eslint-plugin-import"].Version);
      Assert.Equal("5.0.0", manifest["eslint-scope"].Version);
      Assert.Equal("1.4.2", manifest["eslint-utils"].Version);
      Assert.Equal("1.1.0", manifest["eslint-visitor-keys"].Version);
      Assert.Equal("3.1.3", manifest["esm"].Version);
      Assert.Equal("6.1.0", manifest["espree"].Version);
      Assert.Equal("2.7.3", manifest["esprima"].Version);
      Assert.Equal("1.0.1", manifest["esquery"].Version);
      Assert.Equal("4.2.1", manifest["esrecurse"].Version);
      Assert.Equal("4.3.0", manifest["estraverse"].Version);
      Assert.Equal("2.0.2", manifest["esutils"].Version);
      Assert.Equal("1.1.1", manifest["events"].Version);
      Assert.Equal("1.0.0", manifest["exists-stat"].Version);
      Assert.Equal("0.1.5", manifest["expand-brackets"].Version);
      Assert.Equal("1.8.2", manifest["expand-range"].Version);
      Assert.Equal("1.2.2", manifest["expand-tilde"].Version);
      Assert.Equal("3.0.2", manifest["extend"].Version);
      Assert.Equal("3.1.0", manifest["external-editor"].Version);
      Assert.Equal("0.3.2", manifest["extglob"].Version);
      Assert.Equal("1.3.0", manifest["extsprintf"].Version);
      Assert.Equal("1.1.0", manifest["fast-deep-equal"].Version);
      Assert.Equal("2.0.0", manifest["fast-json-stable-stringify"].Version);
      Assert.Equal("2.0.6", manifest["fast-levenshtein"].Version);
      Assert.Equal("3.0.0", manifest["figures"].Version);
      Assert.Equal("5.0.1", manifest["file-entry-cache"].Version);
      Assert.Equal("2.0.1", manifest["filename-regex"].Version);
      Assert.Equal("2.2.4", manifest["fill-range"].Version);
      Assert.Equal("1.1.2", manifest["find-up"].Version);
      Assert.Equal("0.4.3", manifest["findup-sync"].Version);
      Assert.Equal("2.0.1", manifest["flat-cache"].Version);
      Assert.Equal("2.0.1", manifest["flatted"].Version);
      Assert.Equal("1.0.2", manifest["for-in"].Version);
      Assert.Equal("0.1.5", manifest["for-own"].Version);
      Assert.Equal("0.5.2", manifest["forever-agent"].Version);
      Assert.Equal("0.1.4", manifest["form-data"].Version);
      Assert.Equal("0.1.0", manifest["fs-exists-sync"].Version);
      Assert.Equal("1.0.0", manifest["fs-extra"].Version);
      Assert.Equal("1.0.0", manifest["fs.realpath"].Version);
      Assert.Equal("1.2.9", manifest["fsevents"].Version);
      Assert.Equal("1.1.1", manifest["function-bind"].Version);
      Assert.Equal("1.0.1", manifest["functional-red-black-tree"].Version);
      Assert.Equal("2.0.0", manifest["generate-function"].Version);
      Assert.Equal("1.2.0", manifest["generate-object-property"].Version);
      Assert.Equal("1.0.2", manifest["get-caller-file"].Version);
      Assert.Equal("0.1.7", manifest["getpass"].Version);
      Assert.Equal("7.1.2", manifest["glob"].Version);
      Assert.Equal("0.3.0", manifest["glob-base"].Version);
      Assert.Equal("2.0.0", manifest["glob-parent"].Version);
      Assert.Equal("0.2.3", manifest["global-modules"].Version);
      Assert.Equal("0.1.5", manifest["global-prefix"].Version);
      Assert.Equal("9.18.0", manifest["globals"].Version);
      Assert.Equal("4.1.11", manifest["graceful-fs"].Version);
      Assert.Equal("1.0.1", manifest["graceful-readlink"].Version);
      Assert.Equal("1.10.5", manifest["growl"].Version);
      Assert.Equal("4.0.11", manifest["handlebars"].Version);
      Assert.Equal("2.0.0", manifest["har-schema"].Version);
      Assert.Equal("2.0.6", manifest["har-validator"].Version);
      Assert.Equal("1.0.3", manifest["has"].Version);
      Assert.Equal("2.0.0", manifest["has-ansi"].Version);
      Assert.Equal("1.0.0", manifest["has-flag"].Version);
      Assert.Equal("1.0.0", manifest["has-symbols"].Version);
      Assert.Equal("1.1.1", manifest["hawk"].Version);
      Assert.Equal("1.1.1", manifest["he"].Version);
      Assert.Equal("0.9.1", manifest["hoek"].Version);
      Assert.Equal("2.0.0", manifest["home-or-tmp"].Version);
      Assert.Equal("1.0.1", manifest["homedir-polyfill"].Version);
      Assert.Equal("2.6.0", manifest["hosted-git-info"].Version);
      Assert.Equal("3.9.2", manifest["htmlparser2"].Version);
      Assert.Equal("0.10.1", manifest["http-signature"].Version);
      Assert.Equal("0.0.1", manifest["https-browserify"].Version);
      Assert.Equal("0.4.24", manifest["iconv-lite"].Version);
      Assert.Equal("1.1.11", manifest["ieee754"].Version);
      Assert.Equal("4.0.6", manifest["ignore"].Version);
      Assert.Equal("3.1.0", manifest["import-fresh"].Version);
      Assert.Equal("0.1.4", manifest["imurmurhash"].Version);
      Assert.Equal("0.0.1", manifest["indexof"].Version);
      Assert.Equal("1.0.6", manifest["inflight"].Version);
      Assert.Equal("2.0.3", manifest["inherits"].Version);
      Assert.Equal("1.3.5", manifest["ini"].Version);
      Assert.Equal("6.5.1", manifest["inquirer"].Version);
      Assert.Equal("2.2.4", manifest["invariant"].Version);
      Assert.Equal("1.0.0", manifest["invert-kv"].Version);
      Assert.Equal("0.2.1", manifest["is-arrayish"].Version);
      Assert.Equal("1.0.1", manifest["is-binary-path"].Version);
      Assert.Equal("1.1.6", manifest["is-buffer"].Version);
      Assert.Equal("1.0.0", manifest["is-builtin-module"].Version);
      Assert.Equal("1.1.4", manifest["is-callable"].Version);
      Assert.Equal("1.0.1", manifest["is-date-object"].Version);
      Assert.Equal("1.0.3", manifest["is-dotfile"].Version);
      Assert.Equal("0.1.3", manifest["is-equal-shallow"].Version);
      Assert.Equal("0.1.1", manifest["is-extendable"].Version);
      Assert.Equal("1.0.0", manifest["is-extglob"].Version);
      Assert.Equal("1.0.2", manifest["is-finite"].Version);
      Assert.Equal("1.0.0", manifest["is-fullwidth-code-point"].Version);
      Assert.Equal("2.0.1", manifest["is-glob"].Version);
      Assert.Equal("1.0.0", manifest["is-my-ip-valid"].Version);
      Assert.Equal("2.17.2", manifest["is-my-json-valid"].Version);
      Assert.Equal("2.1.0", manifest["is-number"].Version);
      Assert.Equal("0.1.1", manifest["is-posix-bracket"].Version);
      Assert.Equal("2.0.0", manifest["is-primitive"].Version);
      Assert.Equal("2.1.0", manifest["is-promise"].Version);
      Assert.Equal("1.0.2", manifest["is-property"].Version);
      Assert.Equal("1.0.4", manifest["is-regex"].Version);
      Assert.Equal("1.0.2", manifest["is-symbol"].Version);
      Assert.Equal("1.0.0", manifest["is-typedarray"].Version);
      Assert.Equal("0.2.1", manifest["is-utf8"].Version);
      Assert.Equal("0.2.0", manifest["is-windows"].Version);
      Assert.Equal("1.0.0", manifest["isarray"].Version);
      Assert.Equal("2.0.0", manifest["isexe"].Version);
      Assert.Equal("2.1.0", manifest["isobject"].Version);
      Assert.Equal("0.1.2", manifest["isstream"].Version);
      Assert.Equal("0.4.5", manifest["istanbul"].Version);
      Assert.Equal("3.4.1", manifest["jquery"].Version);
      Assert.Equal("1.2.0", manifest["js-reporters"].Version);
      Assert.Equal("3.0.2", manifest["js-tokens"].Version);
      Assert.Equal("3.6.1", manifest["js-yaml"].Version);
      Assert.Equal("0.1.1", manifest["jsbn"].Version);
      Assert.Equal("1.3.0", manifest["jsesc"].Version);
      Assert.Equal("0.2.3", manifest["json-schema"].Version);
      Assert.Equal("0.3.1", manifest["json-schema-traverse"].Version);
      Assert.Equal(
        "1.0.1",
        manifest["json-stable-stringify-without-jsonify"].Version
      );
      Assert.Equal("5.0.1", manifest["json-stringify-safe"].Version);
      Assert.Equal("0.5.1", manifest["json5"].Version);
      Assert.Equal("2.4.0", manifest["jsonfile"].Version);
      Assert.Equal("0.0.0", manifest["jsonify"].Version);
      Assert.Equal("4.0.1", manifest["jsonpointer"].Version);
      Assert.Equal("1.4.1", manifest["jsprim"].Version);
      Assert.Equal("3.2.2", manifest["kind-of"].Version);
      Assert.Equal("1.3.1", manifest["klaw"].Version);
      Assert.Equal("1.0.4", manifest["lazy-cache"].Version);
      Assert.Equal("1.0.0", manifest["lcid"].Version);
      Assert.Equal("0.0.10", manifest["lcov-parse"].Version);
      Assert.Equal("0.3.0", manifest["levn"].Version);
      Assert.Equal("2.0.0", manifest["load-json-file"].Version);
      Assert.Equal("0.2.17", manifest["loader-utils"].Version);
      Assert.Equal("2.0.0", manifest["locate-path"].Version);
      Assert.Equal("4.17.15", manifest["lodash"].Version);
      Assert.Equal("0.1.2", manifest["lodash-doc-globals"].Version);
      Assert.Equal("4.2.0", manifest["lodash.assign"].Version);
      Assert.Equal("4.2.0", manifest["lodash.assignin"].Version);
      Assert.Equal("4.2.1", manifest["lodash.bind"].Version);
      Assert.Equal("4.2.0", manifest["lodash.defaults"].Version);
      Assert.Equal("4.6.0", manifest["lodash.filter"].Version);
      Assert.Equal("4.4.0", manifest["lodash.flatten"].Version);
      Assert.Equal("4.5.0", manifest["lodash.foreach"].Version);
      Assert.Equal("4.6.0", manifest["lodash.map"].Version);
      Assert.Equal("4.6.2", manifest["lodash.merge"].Version);
      Assert.Equal("4.4.0", manifest["lodash.pick"].Version);
      Assert.Equal("4.6.0", manifest["lodash.reduce"].Version);
      Assert.Equal("4.6.0", manifest["lodash.reject"].Version);
      Assert.Equal("4.6.0", manifest["lodash.some"].Version);
      Assert.Equal("1.2.5", manifest["log-driver"].Version);
      Assert.Equal("1.0.1", manifest["longest"].Version);
      Assert.Equal("1.3.1", manifest["loose-envify"].Version);
      Assert.Equal("4.1.3", manifest["lru-cache"].Version);
      Assert.Equal("0.9.1", manifest["markdown-doctest"].Version);
      Assert.Equal("1.0.5", manifest["matcher-collection"].Version);
      Assert.Equal("1.0.1", manifest["math-random"].Version);
      Assert.Equal("0.3.0", manifest["memory-fs"].Version);
      Assert.Equal("2.3.11", manifest["micromatch"].Version);
      Assert.Equal("1.6.0", manifest["mime"].Version);
      Assert.Equal("1.33.0", manifest["mime-db"].Version);
      Assert.Equal("1.0.2", manifest["mime-types"].Version);
      Assert.Equal("2.1.0", manifest["mimic-fn"].Version);
      Assert.Equal("3.0.4", manifest["minimatch"].Version);
      Assert.Equal("1.2.0", manifest["minimist"].Version);
      Assert.Equal("0.5.1", manifest["mkdirp"].Version);
      Assert.Equal("5.2.0", manifest["mocha"].Version);
      Assert.Equal("2.0.0", manifest["ms"].Version);
      Assert.Equal("0.0.8", manifest["mute-stream"].Version);
      Assert.Equal("1.4.0", manifest["natural-compare"].Version);
      Assert.Equal("1.0.5", manifest["nice-try"].Version);
      Assert.Equal("0.7.0", manifest["node-libs-browser"].Version);
      Assert.Equal("1.4.8", manifest["node-uuid"].Version);
      Assert.Equal("3.0.6", manifest["nopt"].Version);
      Assert.Equal("2.4.0", manifest["normalize-package-data"].Version);
      Assert.Equal("2.1.1", manifest["normalize-path"].Version);
      Assert.Equal("1.0.1", manifest["nth-check"].Version);
      Assert.Equal("1.0.1", manifest["number-is-nan"].Version);
      Assert.Equal("0.4.0", manifest["oauth-sign"].Version);
      Assert.Equal("4.1.1", manifest["object-assign"].Version);
      Assert.Equal("1.1.1", manifest["object-keys"].Version);
      Assert.Equal("2.0.1", manifest["object.omit"].Version);
      Assert.Equal("1.1.0", manifest["object.values"].Version);
      Assert.Equal("1.4.0", manifest["once"].Version);
      Assert.Equal("5.1.0", manifest["onetime"].Version);
      Assert.Equal("0.6.1", manifest["optimist"].Version);
      Assert.Equal("2.0.1", manifest["optional-dev-dependency"].Version);
      Assert.Equal("0.8.2", manifest["optionator"].Version);
      Assert.Equal("0.2.1", manifest["os-browserify"].Version);
      Assert.Equal("1.0.2", manifest["os-homedir"].Version);
      Assert.Equal("1.4.0", manifest["os-locale"].Version);
      Assert.Equal("1.0.2", manifest["os-tmpdir"].Version);
      Assert.Equal("1.3.0", manifest["p-limit"].Version);
      Assert.Equal("2.0.0", manifest["p-locate"].Version);
      Assert.Equal("1.0.0", manifest["p-try"].Version);
      Assert.Equal("0.2.9", manifest["pako"].Version);
      Assert.Equal("1.0.1", manifest["parent-module"].Version);
      Assert.Equal("3.0.4", manifest["parse-glob"].Version);
      Assert.Equal("2.2.0", manifest["parse-json"].Version);
      Assert.Equal("1.0.0", manifest["parse-passwd"].Version);
      Assert.Equal("0.0.0", manifest["path-browserify"].Version);
      Assert.Equal("2.1.0", manifest["path-exists"].Version);
      Assert.Equal("1.0.1", manifest["path-is-absolute"].Version);
      Assert.Equal("2.0.1", manifest["path-key"].Version);
      Assert.Equal("1.0.5", manifest["path-parse"].Version);
      Assert.Equal("2.0.0", manifest["path-type"].Version);
      Assert.Equal("2.0.1", manifest["pbkdf2-compat"].Version);
      Assert.Equal("2.1.0", manifest["performance-now"].Version);
      Assert.Equal("2.3.0", manifest["pify"].Version);
      Assert.Equal("2.0.4", manifest["pinkie"].Version);
      Assert.Equal("2.0.1", manifest["pinkie-promise"].Version);
      Assert.Equal("2.0.0", manifest["pkg-dir"].Version);
      Assert.Equal("1.3.5", manifest["platform"].Version);
      Assert.Equal("1.1.2", manifest["prelude-ls"].Version);
      Assert.Equal("0.2.0", manifest["preserve"].Version);
      Assert.Equal("0.1.8", manifest["private"].Version);
      Assert.Equal("0.11.10", manifest["process"].Version);
      Assert.Equal("2.0.0", manifest["process-nextick-args"].Version);
      Assert.Equal("2.0.3", manifest["progress"].Version);
      Assert.Equal("1.0.1", manifest["prr"].Version);
      Assert.Equal("1.0.2", manifest["pseudomap"].Version);
      Assert.Equal("1.4.1", manifest["punycode"].Version);
      Assert.Equal("1.2.2", manifest["qs"].Version);
      Assert.Equal("0.2.0", manifest["querystring"].Version);
      Assert.Equal("0.2.1", manifest["querystring-es3"].Version);
      Assert.Equal("3.0.0", manifest["qunit-extras"].Version);
      Assert.Equal("2.4.1", manifest["qunitjs"].Version);
      Assert.Equal("3.0.0", manifest["randomatic"].Version);
      Assert.Equal("2.0.0", manifest["read-pkg"].Version);
      Assert.Equal("2.0.0", manifest["read-pkg-up"].Version);
      Assert.Equal("2.3.6", manifest["readable-stream"].Version);
      Assert.Equal("2.1.0", manifest["readdirp"].Version);
      Assert.Equal("1.4.0", manifest["regenerate"].Version);
      Assert.Equal("0.11.1", manifest["regenerator-runtime"].Version);
      Assert.Equal("0.10.1", manifest["regenerator-transform"].Version);
      Assert.Equal("0.4.4", manifest["regex-cache"].Version);
      Assert.Equal("2.0.1", manifest["regexpp"].Version);
      Assert.Equal("2.0.0", manifest["regexpu-core"].Version);
      Assert.Equal("0.2.0", manifest["regjsgen"].Version);
      Assert.Equal("0.1.5", manifest["regjsparser"].Version);
      Assert.Equal("1.1.0", manifest["remove-trailing-separator"].Version);
      Assert.Equal("1.1.2", manifest["repeat-element"].Version);
      Assert.Equal("1.6.1", manifest["repeat-string"].Version);
      Assert.Equal("2.0.1", manifest["repeating"].Version);
      Assert.Equal("2.85.0", manifest["request"].Version);
      Assert.Equal("2.1.1", manifest["require-directory"].Version);
      Assert.Equal("1.0.1", manifest["require-main-filename"].Version);
      Assert.Equal("2.3.5", manifest["requirejs"].Version);
      Assert.Equal("1.12.0", manifest["resolve"].Version);
      Assert.Equal("0.1.1", manifest["resolve-dir"].Version);
      Assert.Equal("4.0.0", manifest["resolve-from"].Version);
      Assert.Equal("3.1.0", manifest["restore-cursor"].Version);
      Assert.Equal("0.0.0", manifest["resumer"].Version);
      Assert.Equal("0.1.3", manifest["right-align"].Version);
      Assert.Equal("2.6.3", manifest["rimraf"].Version);
      Assert.Equal("0.2.0", manifest["ripemd160"].Version);
      Assert.Equal("2.3.0", manifest["run-async"].Version);
      Assert.Equal("6.5.2", manifest["rxjs"].Version);
      Assert.Equal("5.1.2", manifest["safe-buffer"].Version);
      Assert.Equal("2.1.2", manifest["safer-buffer"].Version);
      Assert.Equal("2.5.0", manifest["sauce-tunnel"].Version);
      Assert.Equal("5.5.0", manifest["semver"].Version);
      Assert.Equal("2.0.0", manifest["set-blocking"].Version);
      Assert.Equal("1.0.1", manifest["set-immediate-shim"].Version);
      Assert.Equal("1.0.5", manifest["setimmediate"].Version);
      Assert.Equal("2.2.6", manifest["sha.js"].Version);
      Assert.Equal("1.2.0", manifest["shebang-command"].Version);
      Assert.Equal("1.0.0", manifest["shebang-regex"].Version);
      Assert.Equal("3.0.2", manifest["signal-exit"].Version);
      Assert.Equal("1.0.0", manifest["slash"].Version);
      Assert.Equal("2.1.0", manifest["slice-ansi"].Version);
      Assert.Equal("0.2.4", manifest["sntp"].Version);
      Assert.Equal("0.1.8", manifest["source-list-map"].Version);
      Assert.Equal("0.2.0", manifest["source-map"].Version);
      Assert.Equal("0.4.18", manifest["source-map-support"].Version);
      Assert.Equal("3.0.0", manifest["spdx-correct"].Version);
      Assert.Equal("2.1.0", manifest["spdx-exceptions"].Version);
      Assert.Equal("3.0.0", manifest["spdx-expression-parse"].Version);
      Assert.Equal("3.0.0", manifest["spdx-license-ids"].Version);
      Assert.Equal("0.2.10", manifest["split"].Version);
      Assert.Equal("1.0.3", manifest["sprintf-js"].Version);
      Assert.Equal("1.14.1", manifest["sshpk"].Version);
      Assert.Equal("2.0.1", manifest["stream-browserify"].Version);
      Assert.Equal("0.0.4", manifest["stream-combiner"].Version);
      Assert.Equal("2.8.2", manifest["stream-http"].Version);
      Assert.Equal("1.0.2", manifest["string-width"].Version);
      Assert.Equal("1.1.1", manifest["string_decoder"].Version);
      Assert.Equal("0.0.6", manifest["stringstream"].Version);
      Assert.Equal("3.0.1", manifest["strip-ansi"].Version);
      Assert.Equal("3.0.0", manifest["strip-bom"].Version);
      Assert.Equal("3.0.1", manifest["strip-json-comments"].Version);
      Assert.Equal("2.0.0", manifest["supports-color"].Version);
      Assert.Equal("5.4.6", manifest["table"].Version);
      Assert.Equal("0.1.10", manifest["tapable"].Version);
      Assert.Equal("2.3.0", manifest["tape"].Version);
      Assert.Equal("0.2.0", manifest["text-table"].Version);
      Assert.Equal("2.3.8", manifest["through"].Version);
      Assert.Equal("2.0.10", manifest["timers-browserify"].Version);
      Assert.Equal("0.0.33", manifest["tmp"].Version);
      Assert.Equal("1.0.1", manifest["to-arraybuffer"].Version);
      Assert.Equal("1.0.3", manifest["to-fast-properties"].Version);
      Assert.Equal("2.3.4", manifest["tough-cookie"].Version);
      Assert.Equal("1.0.1", manifest["trim-right"].Version);
      Assert.Equal("1.10.0", manifest["tslib"].Version);
      Assert.Equal("0.0.0", manifest["tty-browserify"].Version);
      Assert.Equal("0.4.3", manifest["tunnel-agent"].Version);
      Assert.Equal("0.14.5", manifest["tweetnacl"].Version);
      Assert.Equal("0.3.2", manifest["type-check"].Version);
      Assert.Equal("0.5.2", manifest["type-fest"].Version);
      Assert.Equal("2.7.5", manifest["uglify-js"].Version);
      Assert.Equal("1.0.2", manifest["uglify-to-browserify"].Version);
      Assert.Equal("4.2.2", manifest["uri-js"].Version);
      Assert.Equal("0.11.0", manifest["url"].Version);
      Assert.Equal("2.0.5", manifest["url-join"].Version);
      Assert.Equal("0.4.0", manifest["urlgrey"].Version);
      Assert.Equal("0.10.3", manifest["util"].Version);
      Assert.Equal("1.0.2", manifest["util-deprecate"].Version);
      Assert.Equal("2.1.0", manifest["v8-compile-cache"].Version);
      Assert.Equal("3.0.3", manifest["validate-npm-package-license"].Version);
      Assert.Equal("1.10.0", manifest["verror"].Version);
      Assert.Equal("0.0.4", manifest["vm-browserify"].Version);
      Assert.Equal("0.3.1", manifest["walk-sync"].Version);
      Assert.Equal("0.2.9", manifest["watchpack"].Version);
      Assert.Equal("1.15.0", manifest["webpack"].Version);
      Assert.Equal("0.6.9", manifest["webpack-core"].Version);
      Assert.Equal("1.3.0", manifest["which"].Version);
      Assert.Equal("1.0.0", manifest["which-module"].Version);
      Assert.Equal("0.1.0", manifest["window-size"].Version);
      Assert.Equal("1.0.0", manifest["wordwrap"].Version);
      Assert.Equal("2.1.0", manifest["wrap-ansi"].Version);
      Assert.Equal("1.0.2", manifest["wrappy"].Version);
      Assert.Equal("1.0.3", manifest["write"].Version);
      Assert.Equal("4.0.1", manifest["xtend"].Version);
      Assert.Equal("3.2.1", manifest["y18n"].Version);
      Assert.Equal("2.1.2", manifest["yallist"].Version);
      Assert.Equal("3.10.0", manifest["yargs"].Version);
      Assert.Equal("5.0.0", manifest["yargs-parser"].Version);
    }
  }
}
