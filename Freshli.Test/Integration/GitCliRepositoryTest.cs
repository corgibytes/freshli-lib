using System.IO;
using System.Linq;
using ApprovalTests;
using ApprovalTests.Reporters;
using ApprovalTests.Reporters.TestFrameworks;
using Xunit;

namespace Freshli.Test.Integration {
  [UseReporter(typeof(XUnit2Reporter))]
  public class GitCliRepositoryTest {
    [Fact]
    public void IsValidReturnsTrue() {
      var fixturePath = Fixtures.Path("ruby", "nokotest");
      Assert.True(GitCliRepository.IsValid(fixturePath));
    }

    [Fact]
    public void IsValidReturnsFalse() {
      var fixturePath = Fixtures.Path("empty");
      Assert.False(GitCliRepository.IsValid(fixturePath));
    }

    [Fact]
    public void IsCloneableReturnsTrue() {
      var path = "https://github.com/corgibytes/freshli-fixture-ruby-nokotest";
      Assert.True(GitCliRepository.IsCloneable(path));
    }

    [Fact]
    public void IsCloneableReturnsFalse() {
      var path = "invalid";
      Assert.False(GitCliRepository.IsCloneable(path));
    }

    private void ForceDelete(string path) {
      var directory = new DirectoryInfo(path);
      directory.DeleteReadOnly();
    }

    [Fact]
    public void CloneValidRepo() {
      var repoUrl =
        "https://github.com/corgibytes/freshli-fixture-ruby-nokotest";
      var targetDir = Path.Combine(Path.GetTempPath(), "clone-valid-repo");

      if (Directory.Exists(targetDir)) {
        ForceDelete(targetDir);
      }

      GitCliRepository.Clone(repoUrl, targetDir);
      var files = Directory.GetFiles(
        targetDir,
        "*",
        new EnumerationOptions() {
          AttributesToSkip = 0,
          RecurseSubdirectories = true
        }
      );
      Approvals.VerifyAll(
        files.Select(f =>
          f.Replace(targetDir, "").Replace("\\", "/")),
        "files"
      );
      ForceDelete(targetDir);
    }

    [Fact]
    public void LogEntriesForRubyGemfile() {
      var fixturePath = Fixtures.Path("ruby", "nokotest");
      var repository = new GitCliRepository(fixturePath);

      Approvals.VerifyAll(
        repository.LogEntriesFor("Gemfile"),
        (date, sha) => $"{date.ToString("u")} => {sha}"
      );
    }

    [Fact]
    public void LogEntriesForRubyGemfileDotLock() {
      var fixturePath = Fixtures.Path("ruby", "nokotest");
      var repository = new GitCliRepository(fixturePath);

      Approvals.VerifyAll(
        repository.LogEntriesFor("Gemfile.lock"),
        (date, sha) => $"{date.ToString("u")} => {sha}"
      );
    }

    [Fact]
    public void FileContentsForShaForRubyGemfile() {
      var fixturePath = Fixtures.Path("ruby", "nokotest");
      var repository = new GitCliRepository(fixturePath);

      Approvals.Verify(
        repository.FileContentsFromSha(
          "Gemfile",
          "902a3082740f83776eec419c59a56e54424fdec5"
        )
      );
    }

    [Fact]
    public void FileContentsForShaForRubyGemfileDotLock() {
      var fixturePath = Fixtures.Path("ruby", "nokotest");
      var repository = new GitCliRepository(fixturePath);

      Approvals.Verify(
        repository.FileContentsFromSha(
          "Gemfile.lock",
          "13963f09081c175c66d20f7dd15d23fedc789ce4"
        )
      );
    }
  }
}
