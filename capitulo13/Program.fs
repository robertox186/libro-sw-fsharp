
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Giraffe
open Giraffe.ViewEngine
open FSharp.Control.Tasks

module Handlers =
 let indexView = 
   html [] [
           head [] [
                title [] [ str "Giraffe Example" ]
              ]
           body [] [
     
                 h1 [] [ str "I am Roberto Acosta" ]
                 p [ _class "some-css-class"; _id "someId" ] [
                   str "esta es mi primera pagina con f#"
                  ]
               ]  
     ] 
 let sayHelloNameHandler (name:string) : HttpHandler = fun (next:HttpFunc) (ctx:HttpContext) ->
      task {
        let msg = $"Hello {name}, how are you?"
        return! json {| Response = msg |} next ctx
       }
 let apiRoutes = 
     choose [
        route "" >=> json {| Response = "Hello world!!" |}
        routef "/%s" sayHelloNameHandler
       ]

 let webApp = 
    choose [
        GET >=> choose [
          route "/" >=> htmlView indexView
          subRoute "/api" apiRoutes
            ]
        RequestErrors.NOT_FOUND "Not Found"
     ]



type Startup() =
        member _.ConfigureServices(services: IServiceCollection) =
           services.AddGiraffe() |> ignore
        member _.Configure(app: IApplicationBuilder, env: IWebHostEnvironment) =
          if env.IsDevelopment() 
            then app.UseDeveloperExceptionPage() |> ignore
          app.UseGiraffe Handlers.webApp  


[<EntryPoint>]
    let main args =  
      let builder = WebApplication.CreateBuilder(args)
      let app = builder.Build()

      app.UseGiraffe (route "/" >=> Handlers.webApp)

      app.Run()

      0  // Exit code

