namespace my_api

open System
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Serilog
open my_api.Helpers

type Startup(configuration: IConfiguration) =
    member _.Configuration = configuration

    // This method gets called by the runtime. Use this method to add services to the container.
    member _.ConfigureServices(services: IServiceCollection) =
        // Add framework services.
        services.AddControllers() |> ignore

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    member _.Configure(app: IApplicationBuilder, env: IWebHostEnvironment) =
        
        if (env.IsDevelopment()) then
            app.UseDeveloperExceptionPage() |> ignore
        //app.UseHttpsRedirection()
        app.UseSerilogRequestLogging(
                System.Action<AspNetCore.RequestLoggingOptions>(fun e ->
                    e.EnrichDiagnosticContext <-
                        System.Action<IDiagnosticContext, HttpContext>(LogHelper.EnrichFromRequest))
             )
            .UseRouting()
            //.UseAuthorization()
            .UseEndpoints(fun endpoints -> endpoints.MapControllers() |> ignore)
                |> ignore
