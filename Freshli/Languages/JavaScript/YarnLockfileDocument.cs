using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Freshli.Languages.JavaScript {
  public class YarnLockfileStateMachine {
    private Dictionary<string, Dictionary<YarnLockfileTokenType, (string,
      Action<YarnLockfileDocument, string>)>> _states;

    public YarnLockfileStateMachine(YarnLockfileDocument document) {
      Document = document;
      _states = new Dictionary<string, Dictionary<YarnLockfileTokenType, (string,
        Action<YarnLockfileDocument, string>)>>();
    }

    public void Add(
      string stateName,
      YarnLockfileTokenType token,
      string nextStateName,
      Action<YarnLockfileDocument, string> operation
    ) {
      if (!_states.ContainsKey(stateName)) {
        _states[stateName] =
          new Dictionary<YarnLockfileTokenType, (string,
            Action<YarnLockfileDocument, string>)>();
      }

      _states[stateName][token] = (nextStateName, operation);
    }

    public void Process(YarnLockfileTokenType tokenType, string tokenValue) {
      CurrentStateName ??= StartStateName;

      if (!_states.ContainsKey(CurrentStateName)) {
        throw new ArgumentException(
          $"Undefined state transition {CurrentStateName} - {tokenType}"
        );
      }
      if (!_states[CurrentStateName].ContainsKey(tokenType)) {
        throw new ArgumentException(
          $"Undefined state transition {CurrentStateName} - {tokenType}"
        );
      }
      var nextState = _states[CurrentStateName][tokenType].Item1;
      var operation = _states[CurrentStateName][tokenType].Item2;

      operation(Document, tokenValue);

      CurrentStateName = nextState ??
        throw new ArgumentException(message: "Undefined next state");
    }

    public string StartStateName { get; set; }
    public string CurrentStateName { get; private set; }

    public YarnLockfileDocument Document { get; }
  }

  public class YarnLockfileDocument {
    public YarnLockfileReader Reader { get; }

    public YarnLockfileElement RootElement { get; }

    public IYarnLockfileElement LastCreatedObject { get; set; }

    public string FormatVersion { get; private set; }

    public Stack<string> Stack { get; }

    public IYarnLockfileElement CurrentElement {
      get {
        var objects = new List<IYarnLockfileElement>();
        var current = LastCreatedObject;
        while (current != null) {
          objects.Insert(0, current);
          current = current.Parent;
        }
        return objects[CurrentIndent];
      }
    }

    public int CurrentIndent { get; set; }

    public YarnLockfileDocument(string contents) {
      Reader = new YarnLockfileReader(contents);
      RootElement = new YarnLockfileElement(this);
      Stack = new Stack<string>();
      CurrentIndent = 0;
      LastCreatedObject = RootElement;

      var states = new YarnLockfileStateMachine(this);
      states.Add(
        stateName: "Ready",
        YarnLockfileTokenType.Comment,
        nextStateName: "Comment",
        ParseCommentAndPush
      );
      states.Add(
        stateName: "Comment",
        YarnLockfileTokenType.Newline,
        "Ready",
        AppendComment
      );
      states.Add(
        stateName: "Ready",
        YarnLockfileTokenType.Newline,
        "Ready",
        ResetIndent
      );
      states.Add(
        stateName: "Ready",
        YarnLockfileTokenType.PackageName,
        "Name",
        Push
      );
      states.Add(
        stateName: "Name",
        YarnLockfileTokenType.Colon,
        "Ready",
        StartObject
      );
      states.Add(
        stateName: "Ready",
        YarnLockfileTokenType.Indent,
        "Ready",
        IncreaseIndent
      );
      states.Add(
        stateName: "Name",
        YarnLockfileTokenType.String,
        "Value",
        Push
      );
      states.Add(
        stateName: "Value",
        YarnLockfileTokenType.Newline,
        "Ready",
        CreateProperty
      );
      states.Add(
        stateName: "Name",
        YarnLockfileTokenType.Number,
        "Value",
        Push
      );
      states.Add(
        stateName: "Value",
        YarnLockfileTokenType.Newline,
        "Ready",
        CreateProperty
      );
      states.Add(
        stateName: "Value",
        YarnLockfileTokenType.Eof,
        "End",
        CreateProperty
      );
      states.StartStateName = "Ready";

      while (states.CurrentStateName != "End") {
        Reader.Read();
        states.Process(Reader.CurrentTokenType, Reader.CurrentTokenValue);
      }
    }

    private void CreateProperty(
      YarnLockfileDocument document,
      string token
    ) {
      var value = document.Stack.Pop();
      var name = document.Stack.Pop();
      document.CurrentElement.Elements.Add(
        new YarnLockfilePropertyElement(document.CurrentElement, name, value)
      );
      document.CurrentIndent = 0;
    }

    private static void ParseCommentAndPush(
      YarnLockfileDocument document,
      string token
    ) {
      var versionMarker = "yarn lockfile v";
      if (token.Contains(versionMarker)) {
        var markerStart = token.IndexOf(versionMarker);
        var versionString = token.Remove(
          markerStart,
          versionMarker.Length
        );
        document.FormatVersion = versionString.Trim();
      }

      document.Stack.Push(token);
    }

    private static void AppendComment(
      YarnLockfileDocument document,
      string token
    ) {
      var commentValue = document.Stack.Pop();
      document.CurrentIndent = 0;
      document.CurrentElement.Elements.Add(
        new YarnLockfileCommentElement(document.CurrentElement, commentValue)
      );
    }

    private static void Push(
      YarnLockfileDocument document,
      string token
    ) {
      document.Stack.Push(token);
    }

    private static void StartObject(
      YarnLockfileDocument document,
      string token
    ) {
      var names = new List<string>();
      while (document.Stack.Count > 0) {
        names.Insert(index: 0, document.Stack.Pop());
      }

      var element = new YarnLockfileObjectElement(document.CurrentElement, names);
      document.CurrentElement.Elements.Add(element);
      document.LastCreatedObject = element;
    }

    private static void ResetIndent(
      YarnLockfileDocument document,
      string token
    ) {
      document.CurrentIndent = 0;
    }

    private static void IncreaseIndent(
      YarnLockfileDocument document,
      string token
    ) {
      document.CurrentIndent++;
    }
  }

  public interface IYarnLockfileElement {
    IYarnLockfileElement Parent { get; }
    IList<IYarnLockfileElement> Elements { get; }
    IList<string> Names { get; }
    string Name { get; }
    IYarnLockfileElement GetProperty(string name);
    string Value { get; }

  }

  public class YarnLockfilePropertyElement : IYarnLockfileElement {
    public YarnLockfilePropertyElement(
      IYarnLockfileElement parent,
      string name,
      string value
    ) {
      Parent = parent;
      Name = name;
      Value = value;
      Elements = new List<IYarnLockfileElement>();
    }

    public IYarnLockfileElement Parent { get; }
    public IList<IYarnLockfileElement> Elements { get; }
    public IList<string> Names => new List<string>() {Name};
    public string Name { get; }
    public IYarnLockfileElement GetProperty(string name) {
      return null;
    }

    public string Value { get; }
  }

  public class YarnLockfileObjectElement : IYarnLockfileElement {
    public YarnLockfileObjectElement(
      IYarnLockfileElement parent,
      List<string> names
    ) {
      Parent = parent;
      Names = names;
      Elements = new List<IYarnLockfileElement>();
    }

    public IYarnLockfileElement Parent { get; }
    public IList<string> Names { get; }
    public string Name => Names.FirstOrDefault();
    public IYarnLockfileElement GetProperty(string name) {
      return Elements.FirstOrDefault((item) => item.Names.Contains(name));
    }

    public string Value => null;

    public IList<IYarnLockfileElement> Elements { get; }
  }

  public class YarnLockfileElement : IYarnLockfileElement {
    public YarnLockfileDocument Document { get; private set; }

    public YarnLockfileElement(YarnLockfileDocument document) {
      Parent = null;
      Document = document;
      Elements = new List<IYarnLockfileElement>();
    }

    public IYarnLockfileElement Parent { get; }

    public IList<IYarnLockfileElement> Elements { get; }
    public IList<string> Names => new List<string>();
    public string Name => Names.FirstOrDefault();

    public ObjectEnumerator EnumerateObject() {
      return new ObjectEnumerator(this);
    }

    public IYarnLockfileElement GetProperty(string name) {
      return Elements.FirstOrDefault((item) => item.Names.Contains(name));
    }

    public string Value => null;

    public string GetPropertyName() {
      return null;
    }

    public string GetString() {
      return null;
    }
  }

  public class YarnLockfileCommentElement: IYarnLockfileElement {
    public YarnLockfileCommentElement(IYarnLockfileElement parent, string value) {
      Parent = parent;
      Elements = new List<IYarnLockfileElement>();
      Value = value;
    }

    public IYarnLockfileElement Parent { get; }

    public IList<IYarnLockfileElement> Elements { get; }
    public IList<string> Names => new List<string>();
    public string Name => Names.FirstOrDefault();
    public IYarnLockfileElement GetProperty(string name) {
      return null;
    }

    public string Value { get; }
  }

  public struct YarnLockfileProperty {
    public YarnLockfileElement Value { get; }

    public string Name => Value.GetPropertyName();

    public YarnLockfileProperty(YarnLockfileElement value) {
      Value = value;
    }
  }

  public struct ObjectEnumerator : IEnumerable<YarnLockfileProperty>,
    IEnumerator<YarnLockfileProperty> {
    public ObjectEnumerator(YarnLockfileElement target) {
      Current = default;
    }

    public IEnumerator<YarnLockfileProperty> GetEnumerator() {
      throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return GetEnumerator();
    }

    public bool MoveNext() {
      throw new NotImplementedException();
    }

    public void Reset() {
      throw new NotImplementedException();
    }

    public YarnLockfileProperty Current { get; }

    object? IEnumerator.Current => Current;

    public void Dispose() {
      throw new NotImplementedException();
    }
  }
}
