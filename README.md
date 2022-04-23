# AlinSpace.Tools.Development

## A .NET development tool.

This command-line tool should always be called from the solution directory (where the .sln file is located).

Note: This tool requires a local folder as a nuget feed for development purposes.

## Why?

Most solutions will contain **a couple of projects**. And those projects will **directly reference each other**. This works nicely.

But, when working on a solution with **more than 20 projects**, which are **indirectly referencing each other** (referencing each other through nuget packages), it gets **exponentially harder to maintain** the whole solution. For example, when incrementing version numbers, you have to manually go through each project in the correct order (depth-first search order or breadth-first search order). The more projects, the higher the chance of making a mistake.

This tool tries to automate this process.

# Getting started

Build the tool and make it accessible to the command line interface.

Go to the solution folder.
Then execute the tool:

```
AlinSpace.Tools.Development
```

The file **AlinSpace.Tools.Development.json** will be created.

Now configure it for your setup:

``` json
{
  "PathToSolutionFile":"MySolution.sln",
  "PathToLocalNugetFolder":"C:\\AlinSpace\\Nuget"
}
```

# Project command

The project command helps with managing a project.

## Update

### Info

Updates the project meta data. It reads the configuration and applies the changes to the project files.

```
AlinSpace.Tools.Development project update info
```

### Build

The update command will **increment the build number** of the given project of the solution, build it and copy the nupkg file to the local nuget folder.
Additionally, it will **recursively find all dependent projects**, **increase their build number**, build them and copy their nupgk files to the local nuget folder.

```
AlinSpace.Tools.Development project update build <PROJECT-NAME>
```
