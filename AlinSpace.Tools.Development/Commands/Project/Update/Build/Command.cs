using AlinSpace.ProjectManipulator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace AlinSpace.Tools.Development.Commands.Project.Update.Build
{
    /// <summary>
    /// Represents the project update build command.
    /// </summary>
    /// <remarks>
    /// This command increments the build number of the project, builds it,
    /// copies the nupkg to the local nuget folder and continues recursively 
    /// for all dependent projects.
    /// </remarks>
    public class Command : ICommand
    {
        public void Execute(Context context, IEnumerable<string> args)
        {
            var projectName = args.FirstOrDefault();
            if (projectName == null)
            {
                Console.WriteLine("Please specific a project name.");
                return;
            }

            Console.WriteLine("WARNING: Check-in local changes before continuing with updating the project.");
            Console.WriteLine("WARNING: Press Y to continue ...");
            if (Console.ReadKey().Key != ConsoleKey.Y)
                return;

            var solution = Solution.Read(context.Configuration.PathToSolutionFile);
            
            var nodes = new List<DependencyNode>();

            foreach (var project in solution.Projects)
            {
                nodes.Add(new DependencyNode()
                {
                    Project = ProjectManipulator.Project.Open(project.PathToProjectFile),
                });
            }

            // Increment build number of the root node.
            var rootNode = nodes.FirstOrDefault(x => x.Project.Name == projectName);

            Recursive(context, nodes, rootNode, 0);
        }

        void Recursive(Context context, IEnumerable<DependencyNode> nodes, DependencyNode node, int depth)
        {
            // Update build number.
            var version = node.Project.Version;
            node.Project.VersionIncrementBuild();
            var newVersion = node.Project.Version;

            // Generate package on build is required for this to work.
            node.Project.GeneratePackageOnBuild = true;

            // Save project.
            node.Project.Save();

            Console.WriteLine($"Updated project {node.Project.Name}: {version} -> {newVersion}");

            var pathToNugetPackage = BuildProjectToNugetPackage(context, node);
            CopyNugetPackageToLocalNugetPath(context, node, pathToNugetPackage);

            if(context.Configuration.BuildInDebugConfiguration.GetValueOrDefault())
            {
                CopyDebugFileToLocalNugetPath(context, node, pathToNugetPackage);
            }

            node.Updated = true;

            // Get dependent nodes.
            var dependentNodes = GetDependentNodes(nodes, node);

            foreach (var dependentNode in dependentNodes)
            {
                if (dependentNode.Updated)
                    continue;

                // todo fix this

                if (dependentNode.Project.Name.EndsWith(".Tests"))
                    continue;

                if (dependentNode.Project.Name.EndsWith(".TestApp"))
                    continue;

                var dependency = dependentNode.Project
                    .GetDependencies()
                    .FirstOrDefault(x => x.Name == node.Project.Name);

                // Update dependency version.
                dependency.Version = node.Project.Version;
                dependentNode.Project.Save();

                // Trigger build

                Recursive(context, nodes, dependentNode, depth + 1);
            }
        }

        private void CopyNugetPackageToLocalNugetPath(Context context, DependencyNode node, string pathToNugetPackage)
        {
            var destinationPathToNupkg = Path.Combine(
                context.Configuration.PathToLocalNugetFolder,
                $"{node.Project.Name}.{node.Project.Version}.nupkg");

            if (!File.Exists(pathToNugetPackage))
                throw new Exception($"File could not be found: {pathToNugetPackage}");

            File.Copy(pathToNugetPackage, destinationPathToNupkg, true);
            Console.WriteLine($"Copied nuget package {Path.GetFileName(pathToNugetPackage)} to {destinationPathToNupkg}.");
        }

        private void CopyDebugFileToLocalNugetPath(Context context, DependencyNode node, string pathToNugetPackage)
        {
            var pathToDebugFile = Path.Combine(
                Path.GetDirectoryName(pathToNugetPackage),
                "net6.0",
                $"{node.Project.Name}.pdb");

            if (!File.Exists(pathToDebugFile))
            {
                Console.WriteLine($"Debug file could not be found: {pathToDebugFile}");
                return;
            }

            var pathToDebugFiles = AbsolutePath.Get(context.Configuration.PathToDebugFiles ?? context.Configuration.PathToLocalNugetFolder);

            var pathToDebugFileDestination = Path.Combine(
                pathToDebugFiles,
                $"{node.Project.Name}.pdb");

            if (!Directory.Exists(pathToDebugFiles))
                Directory.CreateDirectory(pathToDebugFiles);

            File.Copy(pathToDebugFile, pathToDebugFileDestination, true);
            Console.WriteLine($"Copied debug file {Path.GetFileName(pathToDebugFile)} to {pathToDebugFileDestination}.");
        }

        string BuildProjectToNugetPackage(Context context, DependencyNode node)
        {
            Console.WriteLine($"Building project {node.Project.Name} ...");

            var buildInDebug = context.Configuration.BuildInDebugConfiguration.GetValueOrDefault();

            var process = Process.Start("dotnet", $"build {node.Project.PathToProjectFile} -c {(buildInDebug ? "DEBUG" : "RELEASE")}");
            process.WaitForExit();

            var pathToNupkg = Path.Combine(
                Path.GetDirectoryName(node.Project.PathToProjectFile),
                "bin",
                buildInDebug ? "debug" : "release",
                $"{node.Project.Name}.{node.Project.Version}.nupkg");

            return pathToNupkg;

        }

        string Spaces(int depth)
        {
            var builder = new StringBuilder();

            foreach (var _ in Enumerable.Range(1, depth))
                builder.Append(" ");

            return builder.ToString();
        }

        IEnumerable<DependencyNode> GetDependentNodes(IEnumerable<DependencyNode> nodes, DependencyNode no)
        {
            foreach(var node in nodes)
            {
                foreach(var dependency in node.Project.GetDependencies())
                {
                    if (dependency.Name == no.Project.Name)
                        yield return node;
                }
            }
        }
    }
}
