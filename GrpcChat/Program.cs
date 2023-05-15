using GrpcChat;
using GrpcChat.Services;


Console.WriteLine("Enter username:");
var username = Console.ReadLine();
if (string.IsNullOrWhiteSpace(username))
{
    Console.Error.WriteLine("Username can't be empty");
    return 1;
}

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddSingleton<ChatInvokeService>();

var app = builder.Build();

app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "");

app.RunAsync();

var invoker = app.Services.GetService<ChatInvokeService>()!;
Console.CancelKeyPress += (_, _) => invoker.Closed = true;

while (!invoker.Closed)
{
    Console.WriteLine("Enter message");
    var message = Console.ReadLine();
    invoker.AddRequest(new MessageRequest {Message = message, User = username});
}

return 0;