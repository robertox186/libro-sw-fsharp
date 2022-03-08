namespace test

open Xunit  
open FsUnit
open customer

open customer.Domain

module `` Convert customer to eligible``=
    let sourceCustomer= 
          {CustomerId="jhon"; IsRegistered=true;IsEligible=true}
    [<Fact>]      
    let ``should succeed if not currently eligible``()=
         let customer = {sourceCustomer with IsEligible = false}
         let upgraded = convertToEligible customer
         upgraded |> should equal sourceCustomer
    [<Fact>]
     let `` should return eligible customer unchanged``()=
          let upgraded= convertToEligible sourceCustome
           upgraded |> should equal sourceCustomer 