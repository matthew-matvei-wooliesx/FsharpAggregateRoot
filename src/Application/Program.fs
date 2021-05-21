open System
open Domain

[<EntryPoint>]
let main _ =
    let p =
        Product.create "PD1" "Product Name" 99.95m
        |> Result.bind (Product.setName "Blah")
        |> Result.bind (Product.setPrice 49.95m)

    match p with
    | Ok product -> printfn "We have a product %s" (Product.getName product)
    | Error e -> match e with
                    | CreationError creationErrors -> List.iter (fun (error: string) -> Console.WriteLine(error)) creationErrors
                    | SetNameError error -> Console.WriteLine(error)
                    | SetPriceError error -> Console.WriteLine(error)

    0
