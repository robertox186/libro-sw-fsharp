let articules=[]

let items = [1..5]



let readItems item = 

  match item with 
  | [] -> "is Empty"
  | [head] -> $"head: {head}" 
  | head::tail->  sprintf "head %A and tail %A" head tail

  

let resp=readItems ["d"]

printfn "%A" resp