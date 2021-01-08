using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MinVerTests.Packages.Infra;
using static MinVerTests.Infra.FileSystem;

namespace MinVerTests.Packages
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

            await RunAsync("dotnet", "new classlib", path);
            await RunAsync("dotnet", $"add package MinVer --source {source} --version {version} --package-directory packages", path);
            await RunAsync("dotnet", $"restore --source {source} --packages packages", path);

            return path;
        }

        public static async Task CleanAndPack(string path, int buildNumber, string output, string verbosity, Action<IDictionary<string, string>> configureEnvironment = null)
        {
            EnsureEmptyDirectory(output);

            var noLogo = (Tests.Sdk?.StartsWith("2.") ?? false) ? "" : " --nologo";

            await RunAsync("dotnet", $"build --no-restore{noLogo}", path, configureEnvironment: configureEnvironment);
            await RunAsync(
                "dotnet",
                $"pack --no-build --output {output}{noLogo}",
                path,
                configureEnvironment: env =>
                {
                    configureEnvironment?.Invoke(env);
                    env.Add("MinVerBuildMetadata".ToAltCase(), $"build.{buildNumber}");
                    env.Add("MinVerVerbosity".ToAltCase(), verbosity ?? "");
                    env.Add("NoPackageAnalysis", "true");
                });
        }
    }
}
