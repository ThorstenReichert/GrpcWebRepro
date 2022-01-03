using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Test;

namespace StupidTestClient
{
    class Program
    {
        static async Task Main()
        {
            var channel = GrpcChannel.ForAddress(
                new UriBuilder
                {
                    Port = 4444,
                    Host = "localhost",
                    Scheme = "http",
                }.Uri,
                new GrpcChannelOptions
                {
                    HttpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWebText, new HttpClientHandler())
                });

            var client = new TestService.TestServiceClient(channel);

            using var call = client.TestStream(new Empty());
            while (await call.ResponseStream.MoveNext(default))
            {
                Console.WriteLine("Response from server");
            }
        }
    }
}
