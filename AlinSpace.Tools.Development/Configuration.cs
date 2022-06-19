using AlinSpace.ProjectManipulator;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace AlinSpace.Tools.Development
{
    /// <summary>
    /// Represents the configuration.
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// Gets or sets the path to solution file.
        /// </summary>
        public string PathToSolutionFile { get; set; }

        /// <summary>
        /// Gets or sets the path to the local nuget folder.
        /// </summary>
        public string PathToLocalNugetFolder { get; set; }

        /// <summary>
        /// Gets or sets the flag indicating whether to build the nuget package in
        /// the DEBUG configuration.
        /// </summary>
        public bool? BuildInDebugConfiguration { get; set; }

        /// <summary>
        /// Gets or sets the path to store the debug files (if not set will use the PathToLocalNugetFolder).
        /// </summary>
        public string PathToDebugFiles { get; set; }

        /// <summary>
        /// Gets or sets the project configurations.
        /// </summary>
        public IList<ProjectConfiguration> Projects { get; set; } = new List<ProjectConfiguration>();

        /// <summary>
        /// Gets or sets the global tags (seperated with ;).
        /// </summary>
        public string Tags { get; set; } = "";

        /// <summary>
        /// Gets or sets the global authors.
        /// </summary>
        public string Authors { get; set; } = "";

        /// <summary>
        /// Gets or sets the global copyright.
        /// </summary>
        public string Copyright { get; set; } = "";

        /// <summary>
        /// Gets or sets the repository URL.
        /// </summary>
        public string RepositoryUrl { get; set; } = "";

        /// <summary>
        /// Gets or sets the package project URL.
        /// </summary>
        public string PackageProjectUrl { get; set; } = "";

        /// <summary>
        /// Try to read configuration from a JSON file.
        /// </summary>
        /// <param name="pathToConfigurationFile">Path to the configuration file.</param>
        /// <param name="configuration">Configuration.</param>
        /// <returns>True, file could be read; false otherwise.</returns>
        public static bool TryReadFromJsonFile(string pathToConfigurationFile, out Configuration configuration)
        {
            try
            {
                var jsonData = File.ReadAllText(pathToConfigurationFile);

                configuration = JsonConvert.DeserializeObject<Configuration>(jsonData);
                configuration.PathToSolutionFile = AbsolutePath.Get(configuration.PathToSolutionFile);
                configuration.PathToLocalNugetFolder = AbsolutePath.Get(configuration.PathToLocalNugetFolder);

                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);

                configuration = null;
                return false;
            }
        }

        /// <summary>
        /// Write configuration to JSON file.
        /// </summary>
        /// <param name="pathToConfigurationFile">Path to configuration file.</param>
        public void WriteToJsonFile(string pathToConfigurationFile)
        {
            var jsonData = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(pathToConfigurationFile, jsonData);
        }

        /// <summary>
        /// Creates template configuration.
        /// </summary>
        /// <returns>Configuration.</returns>
        public static Configuration CreateTemplateConfiguration()
        {
            var configuration = new Configuration();

            configuration.Projects.Add(new ProjectConfiguration()
            {
                Name = "MyProject",
            });

            return configuration;
        }
    }
}
