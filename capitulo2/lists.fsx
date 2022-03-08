let articules=[]

let items = [1..5]



let readItems item = 

  match item with 
  | [] -> printfn "is Empty"
  | [head] -> printfn "%A" head
  | [head::tail]-> printfn "head %A and tail %A" head tail

  