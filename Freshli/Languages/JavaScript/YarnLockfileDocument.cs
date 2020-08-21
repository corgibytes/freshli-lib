using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace Freshli.Languages.JavaScript {
  public class YarnLockfileDocument {
    public YarnLockfileReader Reader { get; }

    public YarnLockfileElement RootElement { get; }

    public string FormatVersion { get; private set; }

    public YarnLockfileDocument(string contents) {
      Reader = new YarnLockfileReader(contents);
      RootElement = new YarnLockfileElement(this);
    }
  }

  public struct YarnLockfileElement {
    public YarnLockfileDocument Document { get; private set; }

    public YarnLockfileElement(YarnLockfileDocument document) {
      Document = document;
    }

    public ObjectEnumerator EnumerateObject() {
      return new ObjectEnumerator(this);
    }

    public YarnLockfileProperty GetProperty(string name) {
      return default;
    }

    public string GetPropertyName() {
      return null;
    }

    public string GetString() {
      return null;
    }
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
