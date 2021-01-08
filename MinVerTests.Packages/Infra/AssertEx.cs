using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Loader;
using Xunit;

namespace MinVerTests.Packages.Infra
{
    public static class AssertEx
    {
        public static void Version(Version expected, string path)
        {
            var packagePath = Directory.EnumerateFiles(path, "*.nupkg", new EnumerationOptions { RecurseSubdirectories = true })
                .First();

            Assert.EndsWith(expected.ToString().Split('+')[0], Path.GetFileNameWithoutExtension(packagePath));

            ZipFile.ExtractToDirectory(
                packagePath,
                Path.Combine(Path.GetDirectoryName(packagePath), Path.GetFileNameWithoutExtension(packagePath)));

            var assemblyPath = Directory.EnumerateFiles(path, "*.dll", new EnumerationOptions { RecurseSubdirectories = true })
                .First();

            var context = new AssemblyLoadContext(default, true);
            var assembly = context.LoadFromAssemblyPath(assemblyPath);
            var assemblyVersion = assembly.GetName().Version;
            context.Unload();

            var fileVersion = FileVersionInfo.GetVersionInfo(assemblyPath);

            Assert.Equal(expected.Major, assemblyVersion.Major);
            Assert.Equal(0, assemblyVersion.Minor);
            Assert.Equal(0, assemblyVersion.Build);

            Assert.Equal(expected.Major, fileVersion.FileMajorPart);
            Assert.Equal(expected.Minor, fileVersion.FileMinorPart);
            Assert.Equal(expected.Patch, fileVersion.FileBuildPart);

            Assert.Equal(expected.ToString(), fileVersion.ProductVersion);
        }
    }
}
