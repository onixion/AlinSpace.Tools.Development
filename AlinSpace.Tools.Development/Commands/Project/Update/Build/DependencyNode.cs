using AlinSpace.ProjectManipulator;
using System.Collections.Generic;

namespace AlinSpace.Tools.Development.Commands.Project.Update.Build
{
    /// <summary>
    /// Represents the dependency node.
    /// </summary>
    /// <remarks>
    /// Helps with tracking dependency nodes.
    /// </remarks>
    public class DependencyNode
    {
        public bool Updated { get; set; }

        public IProject Project { get; set; }

        public IList<DependencyNode> DependentNodes { get; set; } = new List<DependencyNode>();
    }
}
