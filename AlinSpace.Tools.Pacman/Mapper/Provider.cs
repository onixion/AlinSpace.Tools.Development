using System.IO;

namespace AlinSpace.Tools.Pacman.Mapper
{
    public static class Provider
    {
        public static AutoMapper.Mapper Mapper { get; } = new AutoMapper.Mapper(new AutoMapper.MapperConfiguration(configuration =>
        {
            configuration
                .CreateMap<Configuration.Configuration, Solution.Solution>()
                .ForMember(dest => dest.GlobalProject, opt => opt.MapFrom(src => src.GlobalPackage))
                .ForMember(dest => dest.Projects, opt => opt.MapFrom(src => src.Packages))
                .AfterMap((src, dest) =>
                {
                    foreach(var project in dest.Projects)
                    {
                        var pathToSolution = Path.GetDirectoryName(dest.PathToSolutionFile);
                        var pathToProject = Path.Combine(pathToSolution, project.ProjectFilePath);
                        var absolutePathToProject = Path.GetFullPath(pathToProject);

                        project.ProjectFilePath = absolutePathToProject;
                        project.Parent = dest.GlobalProject;
                    }
                });

            configuration
                .CreateMap<Configuration.Package, Solution.Project>();
        }));
    }
}
