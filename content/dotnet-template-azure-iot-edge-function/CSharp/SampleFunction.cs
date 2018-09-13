using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EdgeHub;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace Functions.Samples
{
    public static class SampleFunction
    {
        [FunctionName("SampleFunction")]
        public static async Task FilterMessageAndSendMessage(
                    [EdgeHubTrigger("input1")] Message messageReceived,
                    [EdgeHub(OutputName = "output1")] IAsyncCollector<Message> output,
                    TraceWriter log)
        {
            byte[] messageBytes = messageReceived.GetBytes();
            var messageString = System.Text.Encoding.UTF8.GetString(messageBytes);

            if (!string.IsNullOrEmpty(messageString))
            {
                log.Info("Info: Received one non-empty message");
                var pipeMessage = new Message(messageBytes);
                foreach (KeyValuePair<string, string> prop in messageReceived.Properties)
                {
                    pipeMessage.Properties.Add(prop.Key, prop.Value);
                }
                await output.AddAsync(pipeMessage);
                log.Info("Info: Piped out the message");
            }
        }
    }
}