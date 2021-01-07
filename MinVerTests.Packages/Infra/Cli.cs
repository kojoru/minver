using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static SimpleExec.Command;

namespace MinVerTests.Packages.Infra
{
    public static class Cli
    {
        public static async Task<string> RunAsync(string repo, string verbosity, int buildNumber, Action<IDictionary<string, string>> configureEnvironment = null) =>
            (await ReadAsync(
                "dotnet",
                $"exec ./minver-cli/bin/Release/netcoreapp2.1/minver-cli.dll {repo}",
                configureEnvironment: env =>
                {
                    configureEnvironment?.Invoke(env);
                    env.Add("MinVerBuildMetadata".ToAltCase(), $"build.{buildNumber}");
                    env.Add("MinVerVerbosity".ToAltCase(), verbosity ?? "");
                })).Trim();
    }
}
