using System;

namespace AlinSpace.Tools.Pacman.Configuration
{
    public class Package
    {
        public string Name { get; set; }

        public string ProjectFilePath { get; set; }

        public Version Version { get; set; }

        public string Icon { get; set; }

        public string Author { get; set; }

        public string Copyright { get; set; }

        public string RepositoryUrl { get; set; }

        public string ProjectUrl { get; set; }

        public string Tags { get; set; }
    }
}
