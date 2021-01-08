using System;

namespace MinVerTests.Packages.Infra
{
    public class CommandException : Exception
    {
        public CommandException(string @out, string @error, int exitCode)
            : base($"Exit code code {exitCode}.") =>
            (this.Out, this.Error, this.ExitCode) = (@out, error, exitCode);

        public string Out { get; init; }

        public string Error { get; init; }

        public int ExitCode { get; init; }
    }
}
