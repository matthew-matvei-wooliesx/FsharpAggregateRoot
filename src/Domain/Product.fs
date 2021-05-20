namespace Domain

open System

type Product = private { id: string; name: string; price: decimal }

type ProductError =
    | CreationError of string list
    | SetNameError of string
    | SetPriceError of string

module Product =
    let private validateCreate id name price =
        let mutable errors = []

        if String.IsNullOrWhiteSpace(id) then
            errors <- List.append errors ["id must be defined"]

        if String.IsNullOrWhiteSpace(name) then
            errors <- List.append errors ["name must be defined"]

        if price < 0m then
            errors <- List.append errors ["price must not be negative"]

        errors

    let create id name price =
        let validationErrors = validateCreate id name price
        if List.isEmpty validationErrors then
            Ok { id = id; name = name; price = price }
        else
            Error (CreationError validationErrors)

    let getName (product: Product) =
        product.name

    let setName name product =
        if String.IsNullOrWhiteSpace(name) then
            Error (SetNameError "name must be defined")
        else
            Ok { product with name = name }

    let setPrice price product =
        if price < 0m then
            Error (SetPriceError "price must not be negative")
        else
            Ok { product with price = price }
