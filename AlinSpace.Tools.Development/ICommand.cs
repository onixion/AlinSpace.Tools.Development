using System.Collections.Generic;

namespace AlinSpace.Tools.Development
{
    public interface ICommand
    {
        void Execute(Context context, IEnumerable<string> args);
    }
}
