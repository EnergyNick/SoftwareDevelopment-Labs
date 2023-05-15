using System.Collections.Concurrent;

namespace GrpcChat.Services;

public class ChatInvokeService
{
    private readonly ConcurrentQueue<MessageRequest> _queue = new();
    public bool Closed { get; set; }

    public void AddRequest(MessageRequest request) => _queue.Enqueue(request);

    public bool TryGetRequest(out MessageRequest? request) => _queue.TryDequeue(out request);
}