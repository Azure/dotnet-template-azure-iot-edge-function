# dotnet-template-azure-iot-edge-function

[![Nuget](https://img.shields.io/nuget/v/Microsoft.Azure.IoT.Edge.Function.svg)](https://www.nuget.org/packages/Microsoft.Azure.IoT.Edge.Function/)

> dotnet template to do scaffolding for Azure IoT Edge Function development.

This README will introduce how to install the dotnet template and then create Edge Function with the template step by step.
The template will set up all necessary files for you to focus on functionality programming.

## Get Started

Make sure you have [.Net Core 3.1 SDK](https://www.microsoft.com/net/download/core) installed.

Run `dotnet` command to install the template:

```
dotnet new -i Microsoft.Azure.IoT.Edge.Function
```
You could find our template with short name *aziotedgefunction* in the output:

```
Templates                                         Short Name              Language          Tags
---------------------------------------------------------------------------------------------------------------
Console Application                               console                 [C#], F#, VB      Common/Console
Class library                                     classlib                [C#], F#, VB      Common/Library
Azure IoT Edge Function                           aziotedgefunction       [C#]              Console
Contoso Sample 06                                 sample06                [C#], F#          Console
Unit Test Project                                 mstest                  [C#], F#, VB      Test/MSTest
xUnit Test Project                                xunit                   [C#], F#, VB      Test/xUnit
ASP.NET Core Empty                                web                     [C#]              Web/Empty
ASP.NET Core Web App (Model-View-Controller)      mvc                     [C#], F#          Web/MVC
ASP.NET Core Web App (Razor Pages)                razor                   [C#]              Web/MVC/Razor Pages
ASP.NET Core with Angular                         angular                 [C#]              Web/MVC/SPA
ASP.NET Core with React.js                        react                   [C#]              Web/MVC/SPA
ASP.NET Core with React.js and Redux              reactredux              [C#]              Web/MVC/SPA
ASP.NET Core Web API                              webapi                  [C#]              Web/WebAPI
Nuget Config                                      nugetconfig                               Config
Web Config                                        webconfig                                 Config
Solution File                                     sln                                       Solution
Razor Page                                        page                                      Web/ASP.NET
MVC ViewImports                                   viewimports                               Web/ASP.NET
MVC ViewStart                                     viewstart                                 Web/ASP.NET
```

Check out details about the new dotnet template:

```
PS C:\> dotnet new aziotedgefunction --help
Usage: new [options]

Options:
  -h, --help          Displays help for this command.
  -l, --list          Lists templates containing the specified name. If no name is specified, lists all templates.
  -n, --name          The name for the output being created. If no name is specified, the name of the current directory is used.
  -o, --output        Location to place the generated output.
  -i, --install       Installs a source or a template pack.
  -u, --uninstall     Uninstalls a source or a template pack.
  --type              Filters templates based on available types. Predefined values are "project", "item" or "other".
  --force             Forces content to be generated even if it would change existing files.
  -lang, --language   Specifies the language of the template to create.


Azure IoT Edge Function (C#)
Author: Summer Sun
Options:
  -r|--repository
                      string - Optional
                      Default: <registry>/<repo-name> 
```
Parameter `-r` means the Docker repository to host your Azure IoT Edge module.

Now create the Azure IoT Edge Function by the template with name:

```
dotnet new aziotedgefunction -n <your_function_name>
```