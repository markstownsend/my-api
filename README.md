# my-api
ASP.NET example of logging HttpRequest headers with Serilog 

```
dotnet add package serilog.sinks.file.header
```
In my Startup.fs Startup method I have this.
 
```
app.UseSerilogRequestLogging(
        System.Action<AspNetCore.RequestLoggingOptions>(fun e ->
            e.EnrichDiagnosticContext <-
                System.Action<IDiagnosticContext, HttpContext>(LogHelper.EnrichFromRequest))
     )
```
In my LogHelper.EnrichFromRequest function I have this.
 
```
diagnosticContext.Set("CustomerName", request.Headers.Item("customer-name"))
```
Then in my Program.fs in my outputTemplate I just add {CustomerName} and it picks it up automagically!
 
When I curl the endpoint I just add ``` -H “customer-name: some customer”.

