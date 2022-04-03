using Newtonsoft.Json;
using System.IO;

namespace AlinSpace.Tools.Pacman.Configuration
{
    public static class Writer
    {
        public static void WriteToJsonFile(string path, Configuration configuration)
        {
            var data = JsonConvert.SerializeObject(configuration, Formatting.Indented);
            File.WriteAllText(path, data);
        }
    }
}
