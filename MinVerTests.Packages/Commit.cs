using System.IO;
using System.Threading.Tasks;
using MinVerTests.Infra;
using MinVerTests.Packages.Infra;
using Xunit;

namespace MinVerTests.Packages
{
    public static class Commit
    {
        [Fact]
        public static async Task CommitHasDefaultVersion()
        {
            // arrange
            var output = Path.Combine(Tests.TestPackageBaseOutput, "commit");
            var project = await Project.Create("commit");
            await Git.Init(project);
            await Git.PrepareForCommits(project);
            await Git.Commit(project);

            // act
            await Project.CleanAndPack(project, 123, output, "diagnostic");

            // assert
            AssertEx.Version(new Version(0, 0, 0, new[] { "alpha", "0" }), output);

            // cli
            Assert.Equal($"0.0.0-alpha.0+build.{123}", await MinVerCli.RunAsync(project, "trace", 123));
        }
    }
}
