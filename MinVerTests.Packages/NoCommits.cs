using System.IO;
using System.Threading.Tasks;
using MinVerTests.Packages.Infra;
using Xunit;
using static MinVerTests.Infra.Git;

namespace MinVerTests.Packages
{
    public static class NoCommits
    {
        [Fact]
        public static async Task NoCommitsHasDefaultVersion()
        {
            // arrange
            var output = Path.Combine(Tests.TestPackageBaseOutput, "no-commits");
            var project = await Project.Create("no-commits");
            Init(project);

            // act
            await Project.CleanAndPack(project, 123, output, "diagnostic");

            // assert
            AssertEx.Version(new Version(0, 0, 0, new[] { "alpha", "0" }), output);

            // cli
            Assert.Equal($"0.0.0-alpha.0+build.{123}", await Cli.RunAsync(project, "trace", 123));
        }
    }
}
