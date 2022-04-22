using System;
using System.IO;
using System.Linq;

namespace AlinSpace.Tools.Pacman
{
    /// <summary>
    /// 
    /// update AlinSpace.Zen
    /// </summary>
    public static class Program
    {
        public static void Main(string[] args)
        {
            var pathToConfiguration = args.FirstOrDefault() ?? "pacman.json";

            if (!Path.IsPathRooted(pathToConfiguration))
            {
                pathToConfiguration = Path.Combine(Environment.CurrentDirectory, pathToConfiguration);
            }

            if (!Configuration.Reader.TryReadFromJsonFile(pathToConfiguration, out var configuration))
            {
                Configuration.Writer.WriteToJsonFile(pathToConfiguration, new Configuration.Configuration()
                {
                    GlobalPackage = new Configuration.Package(),
                });

                Console.WriteLine($"Warning: No configuration file found.");
                Console.WriteLine($"Created new configuration file at: {pathToConfiguration}");
                return;
            }

            if (Path.IsPathRooted(pathToConfiguration))
            {
                configuration.PathToSolutionFile = Path.Combine(
                    Path.GetDirectoryName(pathToConfiguration),
                    configuration.PathToSolutionFile);
            }
            else
            {
                string exeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string exeWorkPath = Path.GetDirectoryName(exeFilePath);

                configuration.PathToSolutionFile = Path.Combine(
                    exeWorkPath,
                    Path.GetDirectoryName(pathToConfiguration),
                    configuration.PathToSolutionFile);
            }

            var solution = Mapper.Provider.Mapper.Map<Solution.Solution>(configuration);

            Console.WriteLine($"Found {solution.Projects.Count} projects(s) in configuration.");

            var pathToSolutionFile = configuration.PathToSolutionFile;
            var pathToSolution = Path.GetDirectoryName(pathToSolutionFile);

            Console.WriteLine();

            foreach (var project in solution.Projects)
            {
                var pathToProject = Path.Combine(pathToSolution, project.ProjectFilePath);

                if (!File.Exists(pathToProject))
                    throw new Exception($"Project file {pathToProject} not found.");

                Console.WriteLine($"---- {project.Name} ----");

                Project.Writer.WriteGeneratePackageOnBuild(project);
                Project.Writer.WriteInformationFromProject(project);

                if (project.ShouldAutoIncrementVersion())
                {
                    Project.Writer.AutoIncrementVersion(project);
                }

                // todo try to build
                //project.Build();

                // try to push to nuget package provider

                Console.WriteLine();
            }
        }
    }
}