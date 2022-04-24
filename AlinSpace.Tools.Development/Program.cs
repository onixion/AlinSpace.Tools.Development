using AlinSpace.ProjectManipulator;
using System;
using System.IO;
using System.Linq;

namespace AlinSpace.Tools.Development
{
    /// <summary>
    /// Represents the program.
    /// </summary>
    public static class Program
    {
        public static void Main(string[] args)
        {
            #region Reading configuration file

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

            #endregion

            var context = new Context()
            {
                Configuration = configuration,
            };

            #region Execute command

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

                    case "help":

                        Console.WriteLine("Usage:");
                        Console.WriteLine("\t project update info                     Update project information.");
                        Console.WriteLine("\t project update build <project-name>     Build project and dependent projects.");
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

            #endregion
        }
    }
}