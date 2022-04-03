using Newtonsoft.Json;
using System.IO;

namespace AlinSpace.Tools.Pacman.Configuration
{
    public static class Reader
    {
        public static bool TryReadFromJsonFile(string path, out Configuration configuration)
        {
            try
            {
                var data = File.ReadAllText(path);
                configuration = JsonConvert.DeserializeObject<Configuration>(data);
                return true;
            }
            catch
            {
                configuration = null;
                return false; 
            }
        }
    }
}
