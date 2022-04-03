using System.Collections.Generic;

namespace AlinSpace.Tools.Pacman.Configuration
{
    public class Configuration
    {
        public string PathToSolutionFile { get; set; }

        public Package GlobalPackage { get; set; }

        public IList<Package> Packages { get; set; }
    }
}
