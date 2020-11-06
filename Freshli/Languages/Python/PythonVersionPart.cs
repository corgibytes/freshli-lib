namespace Freshli.Languages.Python {
  public class PythonVersionPart {
    public string Label { get; set; }
    public long? Increment { get; set; }

    public PythonVersionPart Suffix { get; set; }
    public PythonVersionInfo.SuffixType Type { get; set; }

    public PythonVersionInfo.SuffixType SuffixType {
      get {
        if (Suffix != null) {
          return Suffix.Type;
        }
        return PythonVersionInfo.SuffixType.NoSuffix;
      }
    }
  }
}
