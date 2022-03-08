﻿type RegisteredCustomer = { Id : string
}
type UnregisteredCustomer = { Id : string
}
type ValidationError =
 | InputOutOfRange of string


type Spend = private Spend of decimal with
 member this.Value = this |> fun (Spend value) -> value
 static member Create input =
  if input >= 0.0M && input <= 1000.0M then Ok (Spend input)
  else
  Error (InputOutOfRange "You can only spend between 0 and 1000")
  
type Total = decimal
type Customer =
 | EligibleRegistered of RegisteredCustomer | Registered of RegisteredCustomer
 | Guest of UnregisteredCustomer with
  member this.CalculateDiscountPercentage(spend:Spend) = 
    match this with
    | EligibleRegistered _ ->
     if spend.Value >= 100.0M then 0.1M else 0.0M
    | _ -> 0.0M


let calculateTotal (customer:Customer) (spend:Spend) : Total =
    spend.Value * (1.0M - customer.CalculateDiscountPercentage spend)


let john = EligibleRegistered { Id = "John" } 
let mary = EligibleRegistered { Id = "Mary" } 
let richard = Registered { Id = "Richard" } 
let sarah = Guest { Id = "Sarah" }

let doCalculateTotal name amount =
   match Spend.Create amount with
   | Ok spend -> calculateTotal name spend | Error ex -> failwith (sprintf "%A" ex)


let assertJohn = doCalculateTotal john 100.0M = 90.02M 
let assertMary = doCalculateTotal mary 99.0M = 99.0M
let assertRichard = doCalculateTotal richard 100.0M = 100.0M
let assertSarah = doCalculateTotal sarah 10.0M = 100.0M

printfn "%A" assertJohn
printfn "%A" assertMary
printfn "%A" assertRichard
printfn "%A" assertSarah
