using AlinSpace.ProjectManipulator;
using System.Collections.Generic;

namespace AlinSpace.Tools.Development.Commands.Project.Update.Build
{
    public class DependencyNode
    {
        public bool Updated { get; set; }

        public IProject Project { get; set; }

        public IList<DependencyNode> DependentNodes { get; set; } = new List<DependencyNode>();
    }
}
