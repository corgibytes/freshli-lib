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

    public YarnLockfileElement LastCreatedObject { get; set; }

    public string FormatVersion { get; private set; }

    public Stack<string> Stack { get; }

    public YarnLockfileElement CurrentElement {
      get {
        var objects = new List<YarnLockfileElement>();
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
        YarnLockfileTokenType.Identifier,
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

      var element = new YarnLockfilePropertyElement(document.CurrentElement, names);
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

  public class YarnLockfileStringValue : YarnLockfileElement {
    private string _innerValue;

    public YarnLockfileStringValue(YarnLockfileElement parent, string value): base(parent) {
      _innerValue = value;
    }

    public override string GetString() {
      return _innerValue;
    }
  }

  public class YarnLockfilePropertyElement : YarnLockfileElement {
    public YarnLockfilePropertyElement(
      YarnLockfileElement parent,
      string name,
      string value
    ) : base(parent) {
      Names.Add(name);
      Value = new YarnLockfileStringValue(this, value);
    }

    public YarnLockfilePropertyElement(
      YarnLockfileElement parent,
      List<string> names
    ): base(parent) {
      Names = names;
      Value = this;
    }

    public YarnLockfileElement Value { get; }

    public string PackageName {
      get {
        if (!Name.Contains("@")) {
          return Name;
        }

        var splits = Name.Split("@");
        return splits[0];
      }
    }
  }

  public class YarnLockfileElement {
    public YarnLockfileDocument Document { get; private set; }

    public YarnLockfileElement(YarnLockfileDocument document) {
      Parent = null;
      Document = document;
      Elements = new List<YarnLockfileElement>();
      Names = new List<string>();
    }

    public YarnLockfileElement(
      YarnLockfileElement parent
    ) : this(parent.Document) {
      Parent = parent;
    }

    public YarnLockfileElement(
      YarnLockfileElement parent,
      List<string> names
    ): this(parent) {
      Names = names;
    }

    public YarnLockfileElement Parent { get; }

    public IList<YarnLockfileElement> Elements { get; }
    public IList<string> Names { get; protected set; }
    public string Name => Names.FirstOrDefault();

    public ObjectEnumerator EnumerateObject() {
      return new ObjectEnumerator(this);
    }

    public YarnLockfilePropertyElement GetProperty(string name) {
      return (YarnLockfilePropertyElement) Elements.FirstOrDefault(
        (item) => item.Names.Contains(name)
      );
    }

    public string GetPropertyName() {
      return null;
    }

    public virtual string GetString() {
      return null;
    }

    public YarnLockfileCommentIndexer Comments =>
      new YarnLockfileCommentIndexer(this);
  }

  public class YarnLockfileCommentIndexer {
    private YarnLockfileElement _element;
    public YarnLockfileCommentIndexer(YarnLockfileElement element) {
      _element = element;
    }

    public string this[int index] {
      get {
        var commentElements = _element.Elements.Where(
          (value) => value is YarnLockfileCommentElement
        ).ToList();
        var commentElement = commentElements[index] as YarnLockfileCommentElement;
        return commentElement?.Value;
      }
    }
  }

  public class YarnLockfileCommentElement: YarnLockfileElement {
    public YarnLockfileCommentElement(YarnLockfileElement parent, string value)
      : base(parent) {
      Value = value;
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

  public struct ObjectEnumerator : IEnumerable<YarnLockfilePropertyElement>,
    IEnumerator<YarnLockfilePropertyElement> {
    private int index;
    private YarnLockfileElement _target;

    public ObjectEnumerator(YarnLockfileElement target) {
      _target = target;
      index = -1;
    }

    public IEnumerator<YarnLockfilePropertyElement> GetEnumerator() {
      return this;
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return GetEnumerator();
    }

    public bool MoveNext() {
      index++;
      while (index < _target.Elements.Count &&
        !(_target.Elements[index] is YarnLockfilePropertyElement)) {
        index++;
      }

      return index < _target.Elements.Count &&
        _target.Elements[index] is YarnLockfilePropertyElement;
    }

    public void Reset() {
      index = -1;
    }

    public YarnLockfilePropertyElement Current {
      get {
        if (index < 0) {
          return null!;
        }

        return (YarnLockfilePropertyElement) _target.Elements[index];
      }
    }

    object? IEnumerator.Current => Current;

    public void Dispose() {
    }
  }
}
