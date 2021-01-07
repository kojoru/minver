using System.Collections.Generic;
using System.Linq;

namespace MinVerTests.Packages.Infra
{
    public record Version
    {
        public Version(int major, int minor, int patch, IEnumerable<string> preReleaseIdentifiers = null, int height = 0, string buildMetadata = null) =>
            (this.Major, this.Minor, this.Patch, this.PreReleaseIdentifiers, this.Height, this.BuildMetadata) =
                (major, minor, patch, preReleaseIdentifiers?.ToList() ?? new List<string>(), height, buildMetadata);

        public int Major { get; }
        public int Minor { get; }
        public int Patch { get; }
        private List<string> PreReleaseIdentifiers { get; }
        private int Height { get; }
        private string BuildMetadata { get; }

        public override string ToString() =>
            $"{this.Major}.{this.Minor}.{this.Patch}{(this.PreReleaseIdentifiers.Count == 0 ? "" : $"-{string.Join(".", this.PreReleaseIdentifiers)}")}{(this.Height == 0 ? "" : $".{this.Height}")}{(string.IsNullOrEmpty(this.BuildMetadata) ? "" : $"+{this.BuildMetadata}")}";
    }
}
