# GrpcWebRepro
## Solution
In appsettings.json, change
```json
{ "Kestrel": { "EndpointDefaults": { "Protocols": "Http2" }}}
```
to
```json
{ "Kestrel": { "EndpointDefaults": { "Protocols": "Http1AndHttp2" }}}
```

## Issue
Issue when calling a .NET 5.0 gRPC server from a .NET 4.8 or netcoreapp 3.1 gRPC.Web client. When using .NET 5.0 on the client, everything works as expected.

Error message (.NET 4.8 client)
```
Unhandled Exception: Grpc.Core.RpcException: Status(StatusCode="Internal", Detail="Error starting gRPC call. HttpRequestException: An error occurred while sending the request. WebException: The server committed a protocol violation. Section=ResponseStatusLine", DebugException="System.Net.Http.HttpRequestException: An error occurred while sending the request. ---> System.Net.WebException: The server committed a protocol violation. Section=ResponseStatusLine
   at System.Net.HttpWebRequest.EndGetRequestStream(IAsyncResult asyncResult, TransportContext& context)
   at System.Net.Http.HttpClientHandler.GetRequestStreamCallback(IAsyncResult ar)
   --- End of inner exception stack trace ---
   at System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   at Grpc.Net.Client.Web.GrpcWebHandler.<SendAsyncCore>d__18.MoveNext()
--- End of stack trace from previous location where exception was thrown ---
   at System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   at Grpc.Net.Client.Internal.GrpcCall`2.<RunCall>d__72.MoveNext()")
   at Grpc.Net.Client.Internal.HttpContentClientStreamReader`2.<MoveNextCore>d__18.MoveNext()
--- End of stack trace from previous location where exception was thrown ---
   at System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   at System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   at StupidTestClient.Program.<Main>d__0.MoveNext() in C:\Users\treiche\Source\Repos\Github.GrpcWebRepro\TestClient\Program.cs:line 29
--- End of stack trace from previous location where exception was thrown ---
   at System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   at System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   at StupidTestClient.Program.<Main>()
```
Error message (netcoreapp 3.1 client):
```
Unhandled exception. Grpc.Core.RpcException: Status(StatusCode="Unavailable", Detail="Error starting gRPC call. HttpRequestException: An error occurred while sending the request. IOException: The response ended prematurely.", DebugException="System.Net.Http.HttpRequestException: An error occurred while sending the request.
 ---> System.IO.IOException: The response ended prematurely.
   at System.Net.Http.HttpConnection.FillAsync()
   at System.Net.Http.HttpConnection.ReadNextResponseHeaderLineAsync(Boolean foldedHeadersAllowed)
   at System.Net.Http.HttpConnection.SendAsyncCore(HttpRequestMessage request, CancellationToken cancellationToken)
   --- End of inner exception stack trace ---
   at System.Net.Http.HttpConnection.SendAsyncCore(HttpRequestMessage request, CancellationToken cancellationToken)
   at System.Net.Http.HttpConnectionPool.SendWithNtConnectionAuthAsync(HttpConnection connection, HttpRequestMessage request, Boolean doRequestAuth, CancellationToken cancellationToken)
   at System.Net.Http.HttpConnectionPool.SendWithRetryAsync(HttpRequestMessage request, Boolean doRequestAuth, CancellationToken cancellationToken)
   at System.Net.Http.RedirectHandler.SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
   at Grpc.Net.Client.Web.GrpcWebHandler.SendAsyncCore(HttpRequestMessage request, CancellationToken cancellationToken)
   at Grpc.Net.Client.Internal.GrpcCall`2.RunCall(HttpRequestMessage request, Nullable`1 timeout)")
   at Grpc.Net.Client.Internal.HttpContentClientStreamReader`2.MoveNextCore(CancellationToken cancellationToken)
   at StupidTestClient.Program.Main() in C:\Users\treiche\Source\Repos\Github.GrpcWebRepro\TestClient\Program.cs:line 29
   at StupidTestClient.Program.<Main>()
```
Error message (server, both cases)
```
dbug: Microsoft.AspNetCore.Server.Kestrel[39]
      Connection id "0HMEESG5T174T" accepted.
dbug: Microsoft.AspNetCore.Server.Kestrel[1]
      Connection id "0HMEESG5T174T" started.
dbug: Microsoft.AspNetCore.Server.Kestrel[29]
      Connection id "0HMEESG5T174T": HTTP/2 connection error.
      Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http2.Http2ConnectionErrorException: HTTP/2 connection error (PROTOCOL_ERROR): Invalid HTTP/2 connection preface.
         at Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http2.Http2Connection.ParsePreface(ReadOnlySequence`1& buffer, SequencePosition& consumed, SequencePosition& examined)
         at Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http2.Http2Connection.TryReadPrefaceAsync()
         at Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http2.Http2Connection.ProcessRequestsAsync[TContext](IHttpApplication`1 application)
dbug: Microsoft.AspNetCore.Server.Kestrel[36]
      Connection id "0HMEESG5T174T" is closed. The last processed stream ID was 0.
dbug: Microsoft.AspNetCore.Server.Kestrel.Transport.Sockets[6]
      Connection id "0HMEESG5T174T" received FIN.
dbug: Microsoft.AspNetCore.Server.Kestrel[2]
      Connection id "0HMEESG5T174T" stopped.
dbug: Microsoft.AspNetCore.Server.Kestrel.Transport.Sockets[7]
      Connection id "0HMEESG5T174T" sending FIN because: "The Socket transport's send loop completed gracefully."
```
