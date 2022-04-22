using AlinSpace.ProjectManipulator;
using System;
using System.IO;
using System.Linq;

namespace AlinSpace.Tools.Development
{
    /// <summary>
    /// 
    /// update AlinSpace.Zen
    /// </summary>
    public static class Program
    {
        public static void Main(string[] args)
        {
            var pathToConfiguration = AbsolutePath.Get("AlinSpace.Tools.Development.json");

            if (!Configuration.TryReadFromJsonFile(pathToConfiguration, out var configuration))
            {
                if (!File.Exists(pathToConfiguration))
                {
                    var defaultConfiguration = Configuration.CreateDefault();

                    var files = Directory.GetFiles(".", "*.sln");

                    if (files.Take(1).Any())
                    {
                        defaultConfiguration.PathToSolutionFile = Path.GetFileName(files.First());
                    }

                    defaultConfiguration.WriteToJsonFile(pathToConfiguration);

                    Console.WriteLine($"No configuration file found.");
                    Console.WriteLine($"Created new configuration file at: {pathToConfiguration}");
                }
                else
                {
                    Console.WriteLine($"Error: Unable to load configuration.");
                }

                return;
            }

            //configuration.PathToSolutionFile = @"C:\Code\Zen\AlinSpace.Zen\AlinSpace.Zen.sln";
            //configuration.PathToLocalNugetFolder = @"C:\AlinSpace\Nuget";

            var context = new Context()
            {
                Configuration = configuration,
            };

            try
            {
                switch (args[0])
                {
                    case "project":

                        switch(args[1])
                        {
                            case "update":
                                new Commands.UpdateProject.UpdateProject().Execute(context, args.Skip(2).ToList());
                                break;

                            default:
                                Console.WriteLine($"Unknown command: {args[1]}");
                                break;
                        }

                        break;

                    default:
                        Console.WriteLine($"Unknown command: {args[0]}");
                        break;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}