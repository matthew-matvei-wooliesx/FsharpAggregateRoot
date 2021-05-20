namespace Domain

type Product = private { id: string; name: string; price: decimal }

module Product =
    let create id name price =
        { id = id; name = name; price = price }

    let setName name product =
        { product with name = name }

    let setPrice price product =
        { product with price = price }
