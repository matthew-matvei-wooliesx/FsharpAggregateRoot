namespace Application

open Domain

type ProductEventStore() =
    let mutable _inMemoryStore: Map<string, ProductEvent list> = Map.empty

    member this.Save product =
        let (p, events) = Product.commitUncommittedEvents product
        _inMemoryStore <- _inMemoryStore.Change(
            Product.getId product,
            (fun es -> match es with
                        | Some storedEvents -> Some(List.append storedEvents events)
                        | None -> Some(events)))
        p

    member this.Get id =
        _inMemoryStore.TryFind(id)
        |> Option.map Product.fromEvents
