using System.IO;
using System.Threading.Tasks;
using MinVerTests.Packages.Infra;
using Xunit;

namespace MinVerTests.Packages
{
    public static class NoRepo
    {
        public static async Task VersioningNoRepo()
        {
            // arrange
            var output = Path.Combine(Tests.TestPackageBaseOutput, $"{buildNumber}-test-package-no-repo");

            // act
            await CleanAndPack(testProject, output, "diagnostic", packageTestsSdk);

            // assert
            AssertVersion(new Version(0, 0, 0, new[] { "alpha", "0" }), output);

            // cli
            Assert.Equal($"0.0.0-alpha.0+build.{buildNumber}", await Cli.RunAsync(testProject, "trace", buildNumber));
        }
    }
}
