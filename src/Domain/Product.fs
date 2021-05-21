namespace Domain

open System

type ProductEvent =
    | ProductCreated of id: string * name : string * price : decimal
    | NameSet of name : string
    | PriceSet of price : decimal

type Product = private {
    id: string;
    name: string;
    price: decimal;
    uncommittedEvents: ProductEvent list
}

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
            Ok { id = id; name = name; price = price; uncommittedEvents = [ProductCreated(id, name, price)] }
        else
            Error (CreationError validationErrors)

    let fromEvents productEvents =
        let initialProduct: Product = { id = ""; name = ""; price = 0m; uncommittedEvents = [] }
        let applyEvent product event =
            match event with
            | ProductCreated (id, name, price) -> { product with id = id; name = name; price = price }
            | NameSet name -> { product with name = name }
            | PriceSet price -> { product with price = price }

        List.fold applyEvent initialProduct productEvents

    let getId (product: Product) =
        product.id

    let getName (product: Product) =
        product.name

    let setName name product =
        if String.IsNullOrWhiteSpace(name) then
            Error (SetNameError "name must be defined")
        else
            Ok { product with name = name; uncommittedEvents = List.append product.uncommittedEvents [NameSet(name)] }

    let setPrice price product =
        if price < 0m then
            Error (SetPriceError "price must not be negative")
        else
            Ok { product with price = price; uncommittedEvents = List.append product.uncommittedEvents [PriceSet(price)] }

    let commitUncommittedEvents (product: Product) =
        let events = product.uncommittedEvents
        ({ product with uncommittedEvents = [] }, events)
