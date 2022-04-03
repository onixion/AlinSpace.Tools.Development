using System;
using System.IO;

namespace AlinSpace.Tools.Pacman.Solution
{
    public class Project
    {
        public Project Parent { get; set; }

        public string Name { get; set; }

        public string ProjectFilePath { get; set; }

        public string Version { get; set; }

        public bool? AutoIncrementVersion { get; set; }

        public bool ShouldAutoIncrementVersion()
        {
            if (AutoIncrementVersion.HasValue)
            {
                return AutoIncrementVersion.Value;
            }
            else if (Parent.AutoIncrementVersion.HasValue)
            {
                return Parent.AutoIncrementVersion.Value;
            }

            return false;
        }

        public string Icon { get; set; }

        public string Author { get; set; }

        public string Copyright { get; set; }

        public string RepositoryUrl { get; set; }

        public string ProjectUrl { get; set; }

        public string Tags { get; set; }

        public void Build()
        {
            Console.WriteLine("  Building project and package ...");

            Environment.SetEnvironmentVariable("MSBuildSDKsPath", "C:\\Program Files\\dotnet\\sdk\\6.0.200\\Sdks");
            Environment.SetEnvironmentVariable("MSBUILD_EXE_PATH ", "C:\\Program Files\\dotnet\\sdk\\6.0.200\\Sdks");

            var project = new Microsoft.Build.Evaluation.Project(ProjectFilePath);
            //project.SetGlobalProperty("Configuration", "Release");
            project.Build();
        }

        public string GetNugetFilePath()
        {
            return Path.Combine(ProjectFilePath, "bin", "release", $"{Name}.{Version}.nupkg");
        }
    }
}