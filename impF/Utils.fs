module Utils

let inline chain f g x = 
    f(x)
    g(x)

let apply  parameter (items : (_ -> _) seq) = 
    items |> Seq.map (fun f -> f parameter)

let applyIter (items : (_ -> _) seq) parameter = 
    items |> apply parameter |> Seq.iter ignore

let ignoreParam f = fun _ -> f()