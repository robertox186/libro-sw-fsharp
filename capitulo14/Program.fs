
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Giraffe
open Giraffe.ViewEngine
open FSharp.Control.Tasks
open System.IO
 open System
type objeto = {

    Id:int
    Description:string
    Created:DateTime
    IsCompleted:bool

}
module Handlers =

 
 let todoList = [
    {
            Id=1;
            Description="manual de usuario";
            Created=DateTime.UtcNow;
            IsCompleted=false
    }
    {
            Id=2;
            Description="Halo Infinite";
            Created=DateTime.UtcNow;
            IsCompleted=false
    }
    {
            Id=3;
            Description="Unity Coin";
            Created=DateTime.UtcNow;
            IsCompleted=false
    }
   ]
 let showListItem (todo:objeto) =
       let style =  
         if todo.IsCompleted then [ _class "checked" ] else [] 
       li style [ str todo.Description ]
 let createMasterPage msg content = html [] [
        head [] [
            title [] [ str msg ]
            link [ _rel "stylesheet"; _href "main.css" ]
           ] 
        body [] content  
    ]

 let todoView  items= 
   [
        div [ _id "myDIV"; _class "header" ] [
            h2 [] [ str "Mi lista funcional" ]
            input [ _type "text"; _id "myInput"; _placeholder "placeholder del input" ]
            span [ _class "addBtn"; _onclick "newElement()" ] [ str "texto boton" ]
         ]
        ul [ _id "myUL" ] [
            for todo in items do 
              showListItem todo

         ]
        script [ _src "main.js"; _type "text/javascript" ] []

    ]
    
 let indexView =
    [
        h1 [] [ str "I |> F#" ]
        p [ _class "some-css-class"; _id "someId" ] [
            str "Hello World from the Giraffe View Engine"
        ]
    ]
    |> createMasterPage "Giraffe Example"
   
  


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
          route "/" >=> htmlView  (createMasterPage "hola" (todoView todoList))
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
          app.UseStaticFiles() |> ignore  
          app.UseGiraffe Handlers.webApp  
          
let createHostBuilder args =
 let contentRoot = Directory.GetCurrentDirectory() 
 let webRoot = Path.Combine(contentRoot, "WebRoot") 
 Host.CreateDefaultBuilder(args)
   .ConfigureWebHostDefaults(fun webBuilder -> 
      webBuilder
        .UseStartup<Startup>()
        .UseWebRoot(webRoot) |> ignore
     )




[<EntryPoint>]
    let main args =  
      let builder =createHostBuilder args
      let app = builder.Build()

    

      app.Run()

      0  // Exit code

