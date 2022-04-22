using AlinSpace.ProjectManipulator;
using Newtonsoft.Json;
using System;
using System.IO;

namespace AlinSpace.Tools.Development
{
    public class Configuration
    {
        public string PathToSolutionFile { get; set; }

        public string PathToLocalNugetFolder { get; set; }

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

        public void WriteToJsonFile(string pathToConfigurationFile)
        {
            var jsonData = JsonConvert.SerializeObject(this);
            File.WriteAllText(pathToConfigurationFile, jsonData);
        }

        public static Configuration CreateDefault()
        {
            return new Configuration();
        }
    }
}
