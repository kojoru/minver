using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CliWrap;
using CliWrap.Buffered;

namespace MinVerTests.Packages.Infra
{
    public static class MinVerCli
    {
        public static async Task<string> RunAsync(string repo, string verbosity, int buildNumber, Action<IDictionary<string, string>> configureEnvironment = null) =>
            (await
                Cli.Wrap("dotnet")
#if DEBUG
                    .WithArguments($"exec ../../../../minver-cli/bin/Debug/netcoreapp2.1/minver-cli.dll {repo}")
#else
                    .WithArguments($"exec ../../../../minver-cli/bin/Release/netcoreapp2.1/minver-cli.dll {repo}")
#endif
                    .WithEnvironmentVariables(c => c
                        .Set("MinVerBuildMetadata".ToAltCase(), $"build.{buildNumber}")
                        .Set("MinVerVerbosity".ToAltCase(), verbosity ?? "")
                        .Build())
                        .ExecuteBufferedAsync())
                .StandardOutput
                .Trim();
    }
}
