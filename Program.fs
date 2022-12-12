namespace my_api

open System
open System.Diagnostics
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Serilog
open Serilog.Events
open Serilog.Formatting.Compact
open Serilog.Formatting.Json
open Serilog.Sinks.File
open Serilog.Sinks.File.Header

module Program =
    let exitCode = 0

    let logFileLocation =
        @"./my-api.txt"

    let logTemplate =
        [ "{Timestamp:yyyy-MM-dd HH:mm:ss.fff}"
          "[{Level:u3}]"
          "{Protocol}"
          "{Host}"
          "{Scheme}"
          "{RequestMethod}"
          "{RequestPath}"
          "{QueryString}"
          "{StatusCode}"
          "{Elapsed:0.0000}"
          "{CustomerName}"
          "\n"
        ] |> String.concat ","
        //"{Timestamp:yyyy-MM-dd HH:mm:ss.fff},[{Level:u3}],{Protocol},{Host},{Scheme},{RequestMethod},{RequestPath},{QueryString},{StatusCode},{Elapsed:0.0000}\n"

    let ConfigureLogging () =
        Serilog.Debugging.SelfLog.Enable(Console.Error)

        LoggerConfiguration()
            .Destructure.FSharpTypes()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            // Filter out ASP.NET Core infrastructure logs that are Information and below
            .MinimumLevel
            .Override(
                "Microsoft.AspNetCore",
                LogEventLevel.Warning
            )
            .Enrich.FromLogContext()
            .WriteTo.Console(new JsonFormatter())
            .WriteTo
                .File(
                    path = logFileLocation,
                    outputTemplate = logTemplate,
                    fileSizeLimitBytes = 1000000L,
                    rollOnFileSizeLimit = true,
                    shared = false,
                    flushToDiskInterval = TimeSpan.FromSeconds(1.0),
                    hooks = new HeaderWriter("Timestamp,Level,Protocol,Host,Scheme,RequestMethod,RequestPath,QueryString,StatusCode,ElapsedTime,CustomerName", true)
                )
            .CreateLogger()

    let CreateHostBuilder args =
        Host
            .CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureWebHostDefaults(fun webBuilder -> webBuilder.UseStartup<Startup>() |> ignore)

    [<EntryPoint>]
    let main args =
        Log.Logger <- ConfigureLogging()
        CreateHostBuilder(args).Build().Run()

        exitCode
