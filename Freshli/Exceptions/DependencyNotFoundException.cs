using System;

namespace Freshli.Exceptions {
  public class DependencyNotFoundException : Exception {

    public DependencyNotFoundException(string dependency)
      : base($"Unable to find dependency {dependency}.")
    {
    }

  }
}
