using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CliWrap;
using CliWrap.Buffered;
using static MinVerTests.Infra.FileSystem;

namespace MinVerTests.Infra
{
    public static class Git
    {
        public static async Task EnsureEmptyRepositoryAndCommit(string path)
        {
            await EnsureEmptyRepository(path);
            await Commit(path);
        }

        public static Task Commit(string path) =>
            Cli.Wrap("git").WithArguments("commit -m '.' --allow-empty").WithWorkingDirectory(path).ExecuteAsync();

        public static async Task EnsureEmptyRepository(string path)
        {
            EnsureEmptyDirectory(path);
            await Init(path);
            await PrepareForCommits(path);
        }

        public static Task Init(string path) =>
            Cli.Wrap("git").WithArguments("init").WithWorkingDirectory(path).ExecuteAsync();

        public static async Task PrepareForCommits(string path)
        {
            _ = await Cli.Wrap("git").WithArguments("config user.email johndoe@tempuri.org").WithWorkingDirectory(path).ExecuteAsync();
            _ = await Cli.Wrap("git").WithArguments("config user.name John Doe").WithWorkingDirectory(path).ExecuteAsync();
            _ = await Cli.Wrap("git").WithArguments("config commit.gpgsign false").WithWorkingDirectory(path).ExecuteAsync();
        }

        public static async Task<string> GetGraph(string path) =>
            (await Cli.Wrap("git").WithArguments("log --graph --pretty=format:'%d'").WithWorkingDirectory(path).ExecuteBufferedAsync())
                .StandardOutput;

        public static Task Tag(string path, string tag) =>
            Cli.Wrap("git").WithArguments($"tag {tag}").WithWorkingDirectory(path).ExecuteAsync();

        public static Task Tag(string path, string tagName, string sha) =>
            Cli.Wrap("git").WithArguments($"tag {tagName} {sha}").WithWorkingDirectory(path).ExecuteAsync();

        public static Task AnnotatedTag(string path, string tag, string message) =>
            Cli.Wrap("git").WithArguments($"tag {tag} -a -m '{message}'").WithWorkingDirectory(path).ExecuteAsync();

        public static async Task<IEnumerable<string>> GetCommitShas(string path) =>
            (await Cli.Wrap("git").WithArguments("log --pretty=format:\"%H\"").WithWorkingDirectory(path).ExecuteBufferedAsync())
                .StandardOutput
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        public static Task Checkout(string path, string sha) => Cli.Wrap("git").WithArguments($"checkout {sha}").WithWorkingDirectory(path).ExecuteAsync();
    }
}
