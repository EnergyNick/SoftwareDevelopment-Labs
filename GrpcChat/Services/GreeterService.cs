using Grpc.Core;

namespace GrpcChat.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly ChatInvokeService _service;

    public GreeterService(ChatInvokeService service)
    {
        _service = service;
    }

    public override async Task SubscribeMessagesBiDir(IAsyncStreamReader<MessageRequest> requestStream, IServerStreamWriter<MessageRequest> responseStream,
        ServerCallContext context)
    {
        var readTask = Task.Run(async () =>
        {
            await foreach (var response in requestStream.ReadAllAsync())
            {
                Console.WriteLine($"[{DateTime.Now.ToLongDateString()}] {response.User}: {response.Message}");
            }
        });

        while (!_service.Closed)
        {
            if (_service.TryGetRequest(out var toSend))
                await responseStream.WriteAsync(toSend);
            else
                await Task.Delay(500);
        }
    }
}