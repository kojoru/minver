using MinVer.Lib;
using MinVerTests.Lib.Infra;
using Xbehave;
using Xunit;
using static MinVerTests.Infra.FileSystem;
using static MinVerTests.Infra.Git;
using Version = MinVer.Lib.Version;

namespace MinVerTests.Lib
{
    public static class DefaultPreReleasePhases
    {
        [Scenario]
        [Example(default, "0.0.0-alpha.0")]
        [Example("", "0.0.0-alpha.0")]
        [Example("preview", "0.0.0-preview.0")]
        public static void DefaultPreReleasePhase(string phase, string expectedVersion, string path, Version actualVersion)
        {
            $"Given a git repository with a commit in '{path = GetScenarioDirectory($"default-pre-release-phase-{phase}")}'"
                .x(() => EnsureEmptyRepositoryAndCommit(path));

            $"When the version is determined using the default pre-release phase '{phase}'"
                .x(() => actualVersion = Versioner.GetVersion(path, default, default, default, default, phase, new TestLogger()));

            $"Then the version is '{expectedVersion}'"
                .x(() => Assert.Equal(expectedVersion, actualVersion.ToString()));
        }
    }
}
