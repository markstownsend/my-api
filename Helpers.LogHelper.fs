namespace my_api.Helpers

open Microsoft.AspNetCore.Http
open Serilog

[<AbstractClass; Sealed>]
type LogHelper() =
    static member EnrichFromRequest (diagnosticContext : IDiagnosticContext) (httpContext : HttpContext) = 
        let request = httpContext.Request;

        // Set all the common properties available for every request
        diagnosticContext.Set("Host", request.Host);
        diagnosticContext.Set("Protocol", request.Protocol);
        diagnosticContext.Set("Scheme", request.Scheme);
        diagnosticContext.Set("CustomerName", request.Headers.Item("customer-name"))
        // Only set it if available. You're not sending sensitive data in a querystring right?!
        match request.QueryString.HasValue with
        | true -> diagnosticContext.Set("QueryString", request.QueryString.Value)
        | _ -> diagnosticContext.Set("QueryString","\0")
