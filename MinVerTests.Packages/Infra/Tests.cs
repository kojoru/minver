using System.IO;
using System.Reflection;

namespace MinVerTests.Packages.Infra
{
    public static class Tests
    {
        public static string TestPackageBaseOutput { get; } = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }
}
