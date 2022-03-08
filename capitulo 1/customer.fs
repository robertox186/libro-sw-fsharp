
namespace customer

type Customer ={
    CustomerId:string
    IsRegistered:bool
    isElegible:bool
}
 module Db=
 let tryGetCustomer (customerId: string) =
   try
     [
     {CustomerId="jhon"; IsRegistered=true;isElegible=true}
     {CustomerId="mary"; IsRegistered=true;isElegible=true}
     {CustomerId="richard"; IsRegistered=true;isElegible=false}
     {CustomerId="luisa"; IsRegistered=false;isElegible=false}
     ]
     |> List.tryFind (fun c-> c.CustomerId=customerId)
     |> Ok
    with
      |ex-> Error ex
 let saveCustomer (customer:Customer)=
      try 
        Ok()
      with
       | ex -> Error ex      
module Domain=
 let trySaveCustomer customer =
     match customer with 
     |Some c-> c|> Db.saveCustomer
     |None -> Ok ()
 let createCustomer customerId=
     {CustomerId=customerId;IsRegistered=true;isElegible=false}
 let tryCreateCustomer customerId (customer:Customer option)=
      try
        match customer  with
        |Some _ -> raise (exn $"Customer '{customerId}' already exists")
        |None -> Ok (createCustomer customerId)   
        with
        |ex->Error ex 
 let convertToEligible  customer= 
   if not customer.isElegible then { customer with isElegible = true  }
   else customer

 let upgradeCustomer customerId=
     customerId
     |>Db.tryGetCustomer
     |>Result.map(Option.map convertToEligible)
     |>Result.bind  trySaveCustomer
 let registerCustomer customerId = 
      customerId
      |> Db.tryGetCustomer 
      |>  Result.bind (tryCreateCustomer customerId)
      |> Result.bind Db.saveCustomer  