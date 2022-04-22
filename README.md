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

The update command will **increment the build number** of the given project of the solution.
Additionally, it will **recursively find all dependent projects** and **increase their build number**.

```
AlinSpace.Tools.Development project update MyProject
```
