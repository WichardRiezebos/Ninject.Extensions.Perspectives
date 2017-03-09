# Ninject.Extensions.Perspectives
[![Build status](https://ci.appveyor.com/api/projects/status/r5sje87dnrakpayw/branch/master?svg=true)](https://ci.appveyor.com/project/WichardRiezebos/ninject-extensions-perspectives/branch/master) [![NuGet](https://buildstats.info/nuget/Ninject.Extensions.Perspectives)](https://www.nuget.org/packages/Ninject.Extensions.Perspectives/)

Allows to create interfaces for non-mockable classes.

## Prerequisites

- .NET Framework 4.5

## Installation

Install the NuGet package using the command below:

```
Install-Package Ninject.Extensions.Perspectives
```

...or search for `Ninject.Extensions.Perspectives` in the NuGet index.

## How does it work?

If you want to test classes without interfaces. This package is the best you can get. 

For example take the `System.IO.Path` class:

1. Create a new interface called `IPath.cs` and added the (same) methods you need of the original class.
    ```
    interface IPath 
    {
        string GetRandomFileName();
    }
    ```

2. Add a perspective binding:
    ```
    using Ninject.Extensions.Perspectives;
    ...
    
    // Bindings
    kernel.Bind<IPath>().ToPerspective(typeof(Path));
    
    // Resolve
    var path = kernel.Get<IPath>();
    
    // Use
    var randomFileName = path.GetRandomFileName();
    ```
3. Have fun.

## What is supported?

- Statics (e.g. `System.IO.Path`)
- Instances (e.g. `System.Net.Sockets.Socket`)
- Generics (e.g. `System.Collections.Generic.List<>`)