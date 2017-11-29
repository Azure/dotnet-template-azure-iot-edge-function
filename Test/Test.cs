using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Xunit;

namespace Test
{
    public class DotNetFixture : IDisposable
    {
        private static string RelativeTemplatePath = @"../../../../content/dotnet-template-azure-iot-edge-function/CSharp/";
        public DotNetFixture()
        {
            Process.Start("dotnet", "new -i " + RelativeTemplatePath).WaitForExit();
        }

        public void Dispose()
        {
            Console.WriteLine("DotNetFixture: Disposing DotnetFixture");

            // uninstall does not work now according to dotnet issue
            // issue link: https://github.com/dotnet/templating/issues/1226
            Process.Start("dotnet", "new -u " + RelativeTemplatePath).WaitForExit();
        }
    }

    public class Test : IClassFixture<DotNetFixture>
    {
        private DotNetFixture fixture;
        private const string TargetAll = "all";
        private const string TargetDeploy = "deploy";
        private const string ArchLinux64 = "linux64";
        private const string ArchWindowsNano = "windowsNano";

        public Test(DotNetFixture fixture)
        {
            this.fixture = fixture;
        }

        public static Dictionary<string, List<string>> FlagFilesMapping = new Dictionary<string, List<string>>{
            {
                TargetAll, new List<string> {
                    "EdgeHubTrigger-Csharp/run.csx",
                    "EdgeHubTrigger-Csharp/function.json",
                    "host.json",
                    "bin/extensions.json"
                }
            },
            {
                TargetDeploy, new List<string> {
                    "deployment.json"
                }
            },
            {
                ArchLinux64, new List<string> {
                    "Docker/linux-x64/Dockerfile",
                    "Docker/linux-x64/Dockerfile.debug"
                }
            },
            {
                ArchWindowsNano, new List<string> {
                    "Docker/windows-nano/Dockerfile"
                }
            }
        };

        private static string BeforeEach(string target = TargetAll, bool linux64 = true, bool windowsNano = true)
        {
            var scaffoldName = Path.GetRandomFileName().Replace(".", "").ToString();
            var command = "new aziotedgefunction -n " + scaffoldName + " -t " + target + " -lx " + linux64 + " -wn " + windowsNano;
            Process.Start("dotnet", command).WaitForExit();
            return scaffoldName;
        }

        [Theory]
        [InlineData(TargetAll, true, true)]
        [InlineData(TargetAll, false, true)]
        [InlineData(TargetAll, true, false)]
        [InlineData(TargetAll, false, false)]
        [InlineData(TargetDeploy, true, true)]
        [InlineData(TargetDeploy, false, true)]
        [InlineData(TargetDeploy, true, false)]
        [InlineData(TargetDeploy, false, false)]
        public void TestArchitecture(string target, bool linux64, bool windowsNano)
        {
            var scaffoldName = BeforeEach(target, linux64, windowsNano);

            var filesToCheck = new List<string>();

            if (target == TargetDeploy)
            {
                filesToCheck = FlagFilesMapping[TargetDeploy];
            }
            else if (target == TargetAll)
            {
                filesToCheck = FlagFilesMapping[TargetAll].Union(FlagFilesMapping[TargetDeploy]).ToList();
                if (linux64)
                {
                    filesToCheck.AddRange(FlagFilesMapping[ArchLinux64]);

                }
                if (windowsNano)
                {
                    filesToCheck.AddRange(FlagFilesMapping[ArchWindowsNano]);
                }
            }

            foreach (var file in filesToCheck)
            {
                Assert.True(File.Exists(Path.Combine(scaffoldName, file)));
            }
            Assert.Equal(filesToCheck.Count, Directory.GetFiles(scaffoldName, "*", SearchOption.AllDirectories).Length);
            Directory.Delete(scaffoldName, true);
        }

        [Fact]
        public void TestDeployUnnecessaryFiles()
        {
            var scaffoldName = BeforeEach(TargetDeploy);
            var filesExistsToCheck = FlagFilesMapping[TargetDeploy];
            var filesNonExistsToCheck = FlagFilesMapping[ArchLinux64].Union(FlagFilesMapping[ArchWindowsNano]).Union(FlagFilesMapping[TargetAll]);

            foreach (var file in filesExistsToCheck)
            {
                Assert.True(File.Exists(Path.Combine(scaffoldName, file)));
            }
            foreach (var file in filesNonExistsToCheck)
            {
                Assert.True(!File.Exists(Path.Combine(scaffoldName, file)));
            }
            Directory.Delete(scaffoldName, true);
        }
    }
}
