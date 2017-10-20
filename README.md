# dotnet-template-azure-iot-edge-function
> dotnet template to do scaffolding tool for azure iot edge function development.

This ReadMe consists of two parts:
- Get Started to introduce how to install the dotnet template nuget package step by step
- Containerize the function

  The dotnet template sets up all necessary files for you to focus on functionality programming.

  After the coding part completed, following the steps in this part to leverage docker to containerize your azure function so that they can be deployed and monitored by the new features of Azure IoT Edge more straight forward.

## Get Started

Make sure you have [Nuget](https://www.nuget.org/) installed.

Run following command to add the template nuget source:

- For NuGet V3
```
nuget sources add -name AzureIoTEdgeFunction -source https://www.myget.org/F/dotnet-template-azure-iot-edge-function/api/v3/index.json
```

- For NuGet V2

```
nuget sources add -name AzureIoTEdgeFunction -source https://www.myget.org/F/dotnet-template-azure-iot-edge-function/api/v2
```

Check the nuget source is added successfully and enabled by executing command **nuget sources**, check the output:

```
Registered Sources:

  1.  nuget.org [Enabled]
      https://api.nuget.org/v3/index.json
  2.  https://www.nuget.org/api/v2/ [Disabled]
      https://www.nuget.org/api/v2/
  3.  AzureIoTEdgeFunction [Enabled]
      https://www.myget.org/F/dotnet-template-azure-iot-edge-function/api/v2
```

Install the nuget package:
```
nuget install Azure.IoT.Edge.Function
```
You will get a new folder with name *Azure.IoT.Edge.Function.0.0.1*, check out the content in this folder, run dotnet command to install the template with correct path:

```
dotnet new -i <.\Azure.IoT.Edge.Function.0.0.1\dotnet-template-azure-iot-edge-function\CSharp\>
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
PS D:\dotnet-template-azure-iot-edge-function> dotnet new aziotedgefunction --help
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
  -lx|--linux-x64
                   bool - Optional
                   Default: true

```

Now create the azure iot edge function by the template with name:

```
dotnet new aziotedgefunction -n <azure_function_name>
```

We will support multiple architectures, but now only linux-x64 is ready. So it is set the default.

## Build docker image of iot edge function

The scaffolding tool sets up the azure iot edge function development environment, generating all necessary files for you.

To run it in docker container, there are several steps to do.

### Install docker
Ubuntu

https://docs.docker.com/engine/installation/linux/docker-ce/ubuntu/

Windows 10

https://download.docker.com/win/stable/InstallDocker.msi

MAC

https://store.docker.com/editions/community/docker-ce-desktop-mac

Now navigate to the generated directory path in the first place.

### Setup azure resources

If you have develop experience with Azure, you could skip this part and go ahead to next one.

1. Create an active Azure account

(If you don't have an account, you can create one [free account](http://azure.microsoft.com/pricing/free-trial/) in minutes.)

2. Create an Azure IoT Hub

Reference [How to create an azure iot hub] (https://docs.microsoft.com/en-us/azure/iot-hub/iot-hub-create-through-portal) for step by step guidance.

3. Create a device in azure iot hub

Navigate to your iot hub in azure portal, find the **Device Explorer** to **Add** a device in the portal.
Mark up the device connection string after creating completed.

### Install the edge cli

```
npm install -g edge-explorer@latest --registry http://edgenpm.southcentralus.cloudapp.azure.com/
```
### Install tool to launch Azure IoT Edge

> On Windows, If you have issues on the command line with the --registry command, try to use a PowerShell session

```
npm install -g launch-edge-runtime@latest --registry http://edgenpm.southcentralus.cloudapp.azure.com/
```

### Launch edge runtime and login edge-explorer

Make sure you’re using a device connection string and not IoT Hub connection string if you get the error, “Connection string does not have a DeviceId element. Please supply a *device* connection string and not an Azure IoT Hub connection string.”

```
launch-edge-runtime -c "<IoT Hub device connection string>"
```

Use the edge cli to log into the IoT hub to which your edge device is registered. Note that you need the IoT hub’s owner connection string. You can find this in the Azure Portal by going to your IoT hub -> Shared Access Policies -> iothubowner

```
edge-explorer login "<IoT Hub connection string for iothubowner policy*>"
```

### Create and run local docker registry

```
docker run -d -p 5000:5000 --name registry registry:2
```

### Build your docker image

Navigate to the directory we just created, let's take linux-x64 for example:

```
docker build -f Docker\linux-x64\Dockerfile --build-arg EXE_DIR=. -t localhost:5000/<lower_case_image_name>:latest .
```

### Push the image to local registry

```
docker push localhost:5000/<lower_case_image_name>
```

### Deploy

Deployment is accomplished using the edge-explorer command line.

Update the deployment.json with lower case of your module name localhost:5000/<lower_case_module_name> 
before you run the following command:

```
edge-explorer edge deployment create -m <path to deployment file> -d <edge device ID>
```

Now we have the sample module deployed and running, you could monitor it with command 

```
edge-explorer monitor events <deviceID> --login <iothub connection string not device connection string>
```

There will be regular and continuing temperature message show in the console. If not, go back check if each step accomplished correctly.