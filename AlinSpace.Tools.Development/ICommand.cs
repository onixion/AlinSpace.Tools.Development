using System.Collections.Generic;

namespace AlinSpace.Tools.Development
{
    /// <summary>
    /// Represents the command interface.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Execute the command.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="args">Arguments.</param>
        void Execute(Context context, IEnumerable<string> args);
    }
}
