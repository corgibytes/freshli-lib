using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Freshli.Languages.JavaScript {
  public class YarnLockfileReader {
    public YarnLockfileTokenType CurrentTokenType { get; private set; }
    public string CurrentTokenValue { get; private set; }

    private TextReader _reader;
    private String _currentLine;

    public YarnLockfileReader(string contents) :
      this(new StringReader(contents)) { }

    public YarnLockfileReader(TextReader reader) {
      _reader = reader;
    }

    public void Read() {
      var packageNameExpression = new Regex(@"^(\w(?:\w|\d|-|@|\^|\.)*)");
      var identifierExpression = new Regex(@"^(\w(?:\w|\d|_)*)");
      if (_currentLine == null) {
        _currentLine = _reader.ReadLine();
        if (_reader.Peek() != -1) {
          _currentLine += "\n";
        }
      }

      if (String.IsNullOrEmpty(_currentLine)) {
        CurrentTokenType = YarnLockfileTokenType.Eof;
        CurrentTokenValue = null;
      } else if (_currentLine == "\n") {
        CurrentTokenType = YarnLockfileTokenType.Newline;
        CurrentTokenValue = null;
        _currentLine = null;
      } else if (_currentLine.StartsWith("#")) {
        CurrentTokenType = YarnLockfileTokenType.Comment;
        var commentLength = _currentLine.Length - 1;
        if (_currentLine.EndsWith("\n")) {
          commentLength--;
        }

        CurrentTokenValue = _currentLine.Substring(
          startIndex: 1,
          commentLength
        );
        _currentLine = _currentLine.Remove(startIndex: 0, commentLength + 1);
      } else if (_currentLine.StartsWith("  ")) {
        CurrentTokenType = YarnLockfileTokenType.Indent;
        _currentLine = _currentLine.Remove(startIndex: 0, count: 2);
      } else if (_currentLine.StartsWith("sha512-")) {
        var terminatorStart = _currentLine.IndexOf("==");
        CurrentTokenType = YarnLockfileTokenType.Number;
        CurrentTokenValue = _currentLine.Substring(0, terminatorStart + 2);

        _currentLine = _currentLine.Remove(
          startIndex: 0,
          CurrentTokenValue.Length
        );
      } else if (packageNameExpression.IsMatch(_currentLine)) {
        CurrentTokenType = YarnLockfileTokenType.PackageName;
        CurrentTokenValue =
          packageNameExpression.Match(_currentLine).Groups[0].Value;
        _currentLine = _currentLine.Remove(0, CurrentTokenValue.Length);
      } else if (identifierExpression.IsMatch(_currentLine)) {
        CurrentTokenType = YarnLockfileTokenType.Identifier;
        CurrentTokenValue =
          identifierExpression.Match(_currentLine).Groups[0].Value;
        _currentLine = _currentLine.Remove(0, CurrentTokenValue.Length);
      } else if (_currentLine.StartsWith("\"")) {
        if (_currentLine.Length > 1) {
          var index = 1;
          while (_currentLine[index] != '\"') {
            index++;
            if (index >= _currentLine.Length) {
              // TODO: invalid - throw an exception
              // string end token expected but end of line found
            }
          }

          CurrentTokenType = YarnLockfileTokenType.String;
          CurrentTokenValue = _currentLine.Substring(
            startIndex: 1,
            length: index - 1
          );

          _currentLine = _currentLine.Remove(startIndex: 0, count: index + 1);
        } else {
          // TODO: invalid throw an exception
          // string end token expected but end of line found
        }
      } else if (_currentLine.StartsWith(":")) {
        CurrentTokenType = YarnLockfileTokenType.Colon;
        CurrentTokenValue = null;

        _currentLine = _currentLine.Remove(startIndex: 0, count: 1);
      } else if (_currentLine.StartsWith(",")) {
        CurrentTokenType = YarnLockfileTokenType.Comma;
        CurrentTokenValue = null;

        _currentLine = _currentLine.Remove(startIndex: 0, count: 1);
      } else if (_currentLine.StartsWith(" ")) {
        _currentLine = _currentLine.Remove(startIndex: 0, count: 1);
        Read();
      }
    }
  }
}
