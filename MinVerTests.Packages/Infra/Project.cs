using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CliWrap;
using CliWrap.Builders;
using static MinVerTests.Infra.FileSystem;

namespace MinVerTests.Packages.Infra
{
    public static class Project
    {
        public static async Task<string> Create(string scenarioName)
        {
            var path = GetScenarioDirectory(scenarioName);
            EnsureEmptyDirectory(path);

#if DEBUG
            var source = Path.GetFullPath($"../../../../MinVer/bin/Debug/");
#else
            var source = Path.GetFullPath($"../../../../MinVer/bin/Release/");
#endif

            var version = Path.GetFileNameWithoutExtension(Directory.EnumerateFiles(source, "*.nupkg").First()).Split("MinVer.", 2)[1];

            if (!string.IsNullOrWhiteSpace(Tests.Sdk))
            {
                File.WriteAllText(
                    Path.Combine(path, "global.json"),
$@"{{
{"  "}""sdk"": {{
{"    "}""version"": ""{Tests.Sdk.Trim()}"",
{"    "}""rollForward"": ""disable""
{"  "}}}
}}
");
            }

            await Cli.Wrap("dotnet").WithArguments("new classlib").WithWorkingDirectory(path).ExecuteAsync();
            await Cli.Wrap("dotnet").WithArguments($"add package MinVer --source {source} --version {version} --package-directory packages").WithWorkingDirectory(path).ExecuteAsync();
            await Cli.Wrap("dotnet").WithArguments($"restore --source {source} --packages packages").WithWorkingDirectory(path).ExecuteAsync();

            return path;
        }

        public static async Task CleanAndPack(string path, int buildNumber, string output, string verbosity, Action<EnvironmentVariablesBuilder> configure = null)
        {
            EnsureEmptyDirectory(output);

            var noLogo = (Tests.Sdk?.StartsWith("2.") ?? false) ? "" : " --nologo";

            _ = await Cli.Wrap("dotnet").WithArguments($"build --no-restore{noLogo}").WithWorkingDirectory(path).WithEnvironmentVariables(configure ?? (_ => { })).ExecuteAsync();
            _ = await Cli.Wrap("dotnet")
                .WithArguments($"pack --no-build --output {output}{noLogo}")
                .WithWorkingDirectory(path)
                .WithEnvironmentVariables(builder =>
                {
                    configure?.Invoke(builder);
                    _ = builder
                        .Set("MinVerBuildMetadata".ToAltCase(), $"build.{buildNumber}")
                        .Set("MinVerVerbosity".ToAltCase(), verbosity ?? "")
                        .Set("NoPackageAnalysis", "true")
                        .Build();
                })
                .ExecuteAsync();
        }
    }
}
