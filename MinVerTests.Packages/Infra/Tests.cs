using System;
using System.IO;
using System.Reflection;

namespace MinVerTests.Packages.Infra
{
    public static class Tests
    {
        public static string TestPackageBaseOutput { get; } = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static string Sdk { get; } = Environment.GetEnvironmentVariable("MINVER_PACKAGES_TESTS_SDK");
    }
}
