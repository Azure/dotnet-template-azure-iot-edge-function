#r "Microsoft.WindowsAzure.Storage"
#r "System.Text.Encoding"
#r "Microsoft.Azure.Devices.Client"
#r "System.IO"
#r "Newtonsoft.Json"
 
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Azure.Devices.Client;
using System.IO;
using Newtonsoft.Json;
 
public static async Task Run(Message messageReceived, IAsyncCollector<Message> output, ILogger log)
{
    byte[] messageBytes = messageReceived.GetBytes();
    var messageString = System.Text.Encoding.UTF8.GetString(messageBytes);   
  
    const int temperatureThreshold = 25;
    // Get message body
    var messageBody = JsonConvert.DeserializeObject<MessageBody>(messageString);
 
    if (messageBody != null && messageBody.machine.temperature > temperatureThreshold)
    {
        var filteredMessage = new Message(messageBytes);
        foreach (KeyValuePair<string, string> prop in messageReceived.Properties)
        {
            filteredMessage.Properties.Add(prop.Key, prop.Value);
        }
 
        filteredMessage.Properties.Add("MessageType", "Alert");
 
        await output.AddAsync(filteredMessage);
    }
}
 
class MessageBody
{
    public Machine machine {get;set;}
    public Ambient ambient {get; set;}
    public string timeCreated {get; set;}
}
class Machine
{
   public double temperature {get; set;}
   public double pressure {get; set;}         
}
class Ambient
{
   public double temperature {get; set;}
   public int humidity {get; set;}         
