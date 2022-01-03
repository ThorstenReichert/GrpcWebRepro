using Grpc.Core;
using System;
using System.Threading.Tasks;
using Test;

namespace StupidTestServer
{
    public class TestServiceImplementation : TestService.TestServiceBase
    {
        public override async Task TestStream(Empty request, IServerStreamWriter<Empty> responseStream, ServerCallContext context)
        {
            while (!context.CancellationToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                await responseStream.WriteAsync(new Empty());
            }
        }
    }
}
