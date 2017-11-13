#r "Microsoft.Azure.Devices.Client"

using System.IO;
using Microsoft.Azure.Devices.Client;

public static async Task Run(Message messageReceived, IAsyncCollector<Message> output, TraceWriter log)
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