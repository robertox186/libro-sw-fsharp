namespace MyProject.Orders

type Item = { ProductId : int
    Quantity : int
}
type Order = { Id : int
Items : Item list 
}
module Domain =

  let addItem item order =
  let items = item::order.Items { order with Items = items }
  let order = { Id = 1; Items = [ { ProductId = 1; Quantity = 1 } ] } let newItemExistingProduct = { ProductId = 1; Quantity = 1 }
  let newItemNewProduct = { ProductId = 2; Quantity = 2 }
    addItem newItemNewProduct order =
    { Id = 1; Items = [ { ProductId = 2; Quantity = 2 };{ ProductId = 1; Quantity = \1}]}
  addItem newItemExistingProduct order =
    { Id = 1; Items = [ { ProductId = 1; Quantity = 2 } ] }
