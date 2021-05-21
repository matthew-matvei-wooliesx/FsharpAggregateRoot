open System
open Application
open Domain

let createProduct storeProduct =
    Product.create "PD1" "Product Name" 99.95m
    |> Result.bind (Product.setName "Blah")
    |> Result.bind (Product.setPrice 49.95m)
    |> Result.map storeProduct

let printProductResult productResult =
    match productResult with
    | Ok product -> printfn "We have a product %s" (Product.getName product)
    | Error e -> match e with
                    | CreationError creationErrors -> List.iter (fun (error: string) -> Console.WriteLine(error)) creationErrors
                    | SetNameError error -> Console.WriteLine(error)
                    | SetPriceError error -> Console.WriteLine(error)

let printRehydratedProductOption productOption =
    match productOption with
    | Some product -> printfn "We successfully rehydrated product %s" (Product.getName product)
    | None -> printfn "We failed to rehydrate product"

[<EntryPoint>]
let main _ =

    let eventStore = ProductEventStore()

    let productResult = createProduct eventStore.Save

    printProductResult productResult

    match productResult with
    | Ok product -> eventStore.Get(Product.getId product)
    | Error _ -> None
    |> printRehydratedProductOption

    0
