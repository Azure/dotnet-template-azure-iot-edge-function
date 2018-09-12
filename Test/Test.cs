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

        public Test(DotNetFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void TestArchitecture()
        {
            var repository = "test.azurecr.io/test";
            var scaffoldName = Path.GetRandomFileName().Replace(".", "").ToString();
            var command = "new aziotedgefunction -n " + scaffoldName + " -r " + repository;
            Process.Start("dotnet", command).WaitForExit();

            var filesToCheck = new List<string>() {
                    ".gitignore",
                    "Dockerfile.amd64",
                    "Dockerfile.windows-amd64",
                    "Dockerfile.amd64.debug",
                    "Dockerfile.arm32v7",
                    "host.json",
                    "module.json",
                    "nuget.config",
                    $"{scaffoldName}.cs",
                    $"{scaffoldName}.csproj"
                };

            foreach (var file in filesToCheck)
            {
                Assert.True(File.Exists(Path.Combine(scaffoldName, file)));
            }
            Assert.Equal(filesToCheck.Count, Directory.GetFiles(scaffoldName, "*", SearchOption.AllDirectories).Length);

            string text = File.ReadAllText(Path.Combine(scaffoldName, "module.json"));
            Assert.Contains(repository, text);

            Directory.Delete(scaffoldName, true);
        }
    }
}
