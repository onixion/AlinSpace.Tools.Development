using System.Collections.Generic;

namespace AlinSpace.Tools.Pacman.Solution
{
    public class Solution
    {
        public string PathToSolutionFile { get; set; }

        public Project GlobalProject { get; set; }

        public IList<Project> Projects { get; set; }
    }
}
