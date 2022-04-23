using AlinSpace.ProjectManipulator;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AlinSpace.Tools.Development.Commands.Project.Update.Info
{
    public class Command : ICommand
    {
        public void Execute(Context context, IEnumerable<string> args)
        {
            var solution = Solution.Read(context.Configuration.PathToSolutionFile);

            foreach (var project in solution.Projects)
            {
                var projectConfiguration = context
                    .Configuration
                    .Projects
                    .FirstOrDefault(x => x.Name == project.Name);

                if (projectConfiguration == null)
                    continue;

                UpdateProjectInfo(context, project, projectConfiguration);
            }
        }

        void UpdateProjectInfo(Context context, IProjectLink projectLink, ProjectConfiguration projectConfiguration)
        {
            var project = ProjectManipulator.Project.Open(projectLink.PathToProjectFile);

            Console.WriteLine($"Updating project information for {project.Name} ...");

            #region Authors

            var authors = context.Configuration.Authors;

            if (string.IsNullOrWhiteSpace(authors))
            {
                authors = projectConfiguration.Authors;
            }

            if (!string.IsNullOrWhiteSpace(authors))
            {
                project.Authors = authors;
            }

            #endregion

            #region Copyright

            var copyright = context.Configuration.Copyright;

            if (string.IsNullOrWhiteSpace(copyright))
            {
                copyright = projectConfiguration.Authors;
            }

            if (!string.IsNullOrWhiteSpace(copyright))
            {
                project.Copyright = copyright;
            }

            #endregion

            #region PackageTags

            var tags = $"{context.Configuration.Tags?.Trim(' ', ',') ?? ""}, {projectConfiguration.Tags?.Trim(' ', ',') ?? ""}".Trim(' ', ',');

            if (!string.IsNullOrWhiteSpace(tags))
            {
                project.PackageTags = tags;
            }

            #endregion

            #region PackageProjectUrl

            var packageProjectUrl = context.Configuration.PackageProjectUrl;

            if (string.IsNullOrWhiteSpace(packageProjectUrl))
            {
                authors = projectConfiguration.PackageProjectUrl;
            }

            if (!string.IsNullOrWhiteSpace(packageProjectUrl))
            {
                project.PackageProjectUrl = new Uri(packageProjectUrl);
            }

            #endregion

            #region RepositoryUrl

            var repositoryUrl = context.Configuration.RepositoryUrl;

            if (string.IsNullOrWhiteSpace(repositoryUrl))
            {
                authors = projectConfiguration.RepositoryUrl;
            }

            if (!string.IsNullOrWhiteSpace(repositoryUrl))
            {
                project.RepositoryUrl = new Uri(repositoryUrl);
            }

            #endregion

            project.Save();
        }
    }
}
