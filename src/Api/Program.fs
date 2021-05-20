namespace Api

open System
open System.Collections.Generic
open System.IO
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging

open Domain

module Program =
    let exitCode = 0

    let CreateHostBuilder args =
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(fun webBuilder ->
                webBuilder.UseStartup<Startup>() |> ignore
            )

    [<EntryPoint>]
    let main args =
        CreateHostBuilder(args).Build().Run()

        let a = Product.create "PD1" "Product Name" 99.95m
        let b = Result.bind (Product.setName "Blah") a
        let c = Result.bind (Product.setPrice 49.95m) b

        match c with
        | Ok product -> printfn "We have a product %s" (Product.getName product)
        | Error e -> match e with
                        | CreationError creationErrors -> List.iter (fun (error: string) -> Console.WriteLine(error)) creationErrors
                        | SetNameError error -> Console.WriteLine(error)
                        | SetPriceError error -> Console.WriteLine(error)

        exitCode
