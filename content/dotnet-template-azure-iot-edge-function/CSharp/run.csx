#r "System.Text.Encoding"
#r "System.IO"

using System.IO;

public static async Task Run(Message messageReceived, IAsyncCollector<Message> output, ILogger log)
{
    byte[] messageBytes = messageReceived.GetBytes();
    var messageString = System.Text.Encoding.UTF8.GetString(messageBytes);   
  
    log.Info($"Received message: {messageString}");

    await output.AddAsync(messageReceived);
}
