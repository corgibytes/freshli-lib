using System;

namespace Freshli.Exceptions {
  public class DependencyNotFoundException : Exception {

    public DependencyNotFoundException(string dependency, Exception e)
      : base($"Unable to find dependency {dependency}.", e)
    {
    }

  }
}
