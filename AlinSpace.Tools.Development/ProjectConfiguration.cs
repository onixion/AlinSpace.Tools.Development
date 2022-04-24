namespace AlinSpace.Tools.Development
{
    /// <summary>
    /// Represents the project configuration.
    /// </summary>
    public class ProjectConfiguration
    {
        /// <summary>
        /// Gets or sets the project name.
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Gets or sets the project tags (seperated with ;).
        /// </summary>
        public string Tags { get; set; } = "";

        /// <summary>
        /// Gets or sets the authors.
        /// </summary>
        public string Authors { get; set; } = "";

        /// <summary>
        /// Gets or sets the copyright.
        /// </summary>
        public string Copyright { get; set; } = "";

        /// <summary>
        /// Gets or sets the package project URL.
        /// </summary>
        public string PackageProjectUrl { get; set; } = "";

        /// <summary>
        /// Gets or sets the repository URL.
        /// </summary>
        public string RepositoryUrl { get; set; } = "";
    }
}