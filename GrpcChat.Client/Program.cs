using Grpc.Core;
using Grpc.Net.Client;
using GrpcChat.Client;


Console.WriteLine("Enter http address and port:");
if (!Uri.TryCreate(Console.ReadLine(), UriKind.RelativeOrAbsolute, out var address))
{
    Console.Error.WriteLine($"Incorrect ip address or port {address}");
    return 1;
}

Console.WriteLine("Enter username:");
var username = Console.ReadLine();
if (string.IsNullOrWhiteSpace(username))
{
    Console.Error.WriteLine($"Username can't be empty");
    return 1;
}

using var channel = GrpcChannel.ForAddress(address);
var client = new Greeter.GreeterClient(channel);

var isRunning = true;
Console.CancelKeyPress += (_, _) => isRunning = false;

var messages = client.SubscribeMessagesBiDir();
var invoker = Task.Run(async () =>
{
    var stream = messages.ResponseStream;
    while (await stream.MoveNext())
        Console.WriteLine($"[{DateTime.Now.ToLongDateString()}] {stream.Current.User}: {stream.Current.Message}");
});

var stream = messages.RequestStream;
while (isRunning)
{
    Console.WriteLine("Enter message");
    var message = Console.ReadLine();
    if(!string.IsNullOrWhiteSpace(message))
        await stream.WriteAsync(new MessageRequest { Message = message, User = username });
}


return 0;