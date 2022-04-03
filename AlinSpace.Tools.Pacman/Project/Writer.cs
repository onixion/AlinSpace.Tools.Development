using System;
using System.Xml;

namespace AlinSpace.Tools.Pacman.Project
{
    public static class Writer
    {
        public static void WriteGeneratePackageOnBuild(Solution.Project project)
        {
            var document = new XmlDocument();
            document.Load(project.ProjectFilePath);

            var propertyGroupNode = document.SelectSingleNode("/Project/PropertyGroup");

            var generatePackageOnBuildNode = propertyGroupNode.SelectSingleNode("GeneratePackageOnBuild");

            if (generatePackageOnBuildNode == null)
            {
                generatePackageOnBuildNode = document.CreateElement("GeneratePackageOnBuild");
                propertyGroupNode.AppendChild(generatePackageOnBuildNode);
            }

            generatePackageOnBuildNode.InnerText = "True";

            Console.WriteLine($"  GeneratePackageOnBuild = {generatePackageOnBuildNode.InnerText}");

            document.Save(project.ProjectFilePath);
        }

        public static void AutoIncrementVersion(Solution.Project project)
        {
            var document = new XmlDocument();
            document.Load(project.ProjectFilePath);

            var propertyGroupNode = document.SelectSingleNode("/Project/PropertyGroup");

            var versionNode = propertyGroupNode.SelectSingleNode("Version");

            if (versionNode == null)
            {
                versionNode = document.CreateElement("Version");
                propertyGroupNode.AppendChild(versionNode);

                versionNode.InnerText = "1.0.0";
            }
            else
            {
                var version = new Version(versionNode.InnerText);

                var newVersion = new Version(version.Major, version.Minor, version.Build + 1);
                versionNode.InnerText = newVersion.ToString();
            }

            Console.WriteLine($"  Version = {versionNode.InnerText}");

            document.Save(project.ProjectFilePath);
        }

        public static void WriteInformationFromProject(Solution.Project project)
        {
            var document = new XmlDocument();
            document.Load(project.ProjectFilePath);

            var propertyGroupNode = document.SelectSingleNode("/Project/PropertyGroup");

            #region PackageTags

            var packageTagsNode = propertyGroupNode.SelectSingleNode("PackageTags");

            if (packageTagsNode == null)
            {
                packageTagsNode = document.CreateElement("PackageTags");
                propertyGroupNode.AppendChild(packageTagsNode);
            }

            var tags = (project.Parent.Tags ?? "") + project.Tags ?? "";

            packageTagsNode.InnerText = tags.Trim().Trim(',').Trim();

            Console.WriteLine($"  PackageTags = {packageTagsNode.InnerText}");

            #endregion

            #region Copyright

            var copyrightNode = propertyGroupNode.SelectSingleNode("Copyright");

            if (copyrightNode == null)
            {
                copyrightNode = document.CreateElement("Copyright");
                propertyGroupNode.AppendChild(copyrightNode);
            }

            copyrightNode.InnerText = project.Copyright != null ? project.Copyright?.Trim() : project.Parent.Copyright?.Trim();

            Console.WriteLine($"  Copyright = {copyrightNode.InnerText}");

            #endregion

            #region Authors

            var authorsNode = propertyGroupNode.SelectSingleNode("Authors");

            if (authorsNode == null)
            {
                authorsNode = document.CreateElement("Authors");
                propertyGroupNode.AppendChild(authorsNode);
            }

            authorsNode.InnerText = project.Author != null ? project.Author?.Trim() : project.Parent.Author?.Trim();

            Console.WriteLine($"  Authors = {authorsNode.InnerText}");

            #endregion

            #region PackageProjectUrl

            var packageProjectUrlNode = propertyGroupNode.SelectSingleNode("PackageProjectUrl");

            if (packageProjectUrlNode == null)
            {
                packageProjectUrlNode = document.CreateElement("PackageProjectUrl");
                propertyGroupNode.AppendChild(packageProjectUrlNode);
            }

            packageProjectUrlNode.InnerText = project.ProjectUrl != null ? project.ProjectUrl?.Trim() : project.Parent.ProjectUrl?.Trim();

            Console.WriteLine($"  PackageProjectUrl = {packageProjectUrlNode.InnerText}");

            #endregion

            #region RepositoryUrl

            var repositoryUrlNode = propertyGroupNode.SelectSingleNode("RepositoryUrl");

            if (repositoryUrlNode == null)
            {
                repositoryUrlNode = document.CreateElement("RepositoryUrl");
                propertyGroupNode.AppendChild(repositoryUrlNode);
            }

            repositoryUrlNode.InnerText = project.RepositoryUrl != null ? project.RepositoryUrl?.Trim() : project.Parent.RepositoryUrl?.Trim();

            Console.WriteLine($"  RepositoryUrl = {repositoryUrlNode.InnerText}");

            #endregion

            document.Save(project.ProjectFilePath);
        }
    }
}
