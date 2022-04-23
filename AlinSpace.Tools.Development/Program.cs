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
                    var defaultConfiguration = Configuration.CreateTemplateConfiguration();

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

                                switch(args[2])
                                {
                                    case "build":
                                        new Commands.Project.Update.Build.Command().Execute(context, args.Skip(3).ToList());
                                        break;

                                    case "info":
                                        new Commands.Project.Update.Info.Command().Execute(context, args.Skip(3).ToList());
                                        break;

                                    default:
                                        Console.WriteLine($"Unknown command: {args[2]}");
                                        break;
                                }

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