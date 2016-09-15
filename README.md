# impF
Elm inspired imutable MVVM prototype library for Wpf

Example:

```fsharp
module Person = 
    type Model = 
        { FirstName : string
        ; LastName  : string
        ; IsSwapped : bool
        } 
        with
        member x.FullName = 
            sprintf "%s %s" x.FirstName x.LastName
        member x.IsNotSwapped = 
            not x.IsSwapped
            
    type Msg = 
        | First of string
        | Last of string
        | Swap
        | SwapBack
 
    let init =
        { FirstName = ""
        ; LastName = ""
        ; IsSwapped = false
        }

    let update msg state = 
        let swap () = 
            { IsSwapped = not state.IsSwapped
            ; FirstName = state.LastName
            ; LastName = state.FirstName 
            }

        match msg with 
        | First s -> 
            { state with FirstName = s }
        | Last s -> 
            { state with LastName = s }
        | Swap -> 
            swap()
        | SwapBack -> 
            swap()


 type PersonVm(p) =
    let sm = 
        stateManager Person.update p

    member val FirstName = 
        sm.Field Person.Msg.First (fun m -> m.FirstName) 
    member val LastName = 
        sm.Field Person.Msg.Last (fun m -> m.LastName)
    member val FullName = 
        sm.RoField (fun m -> m.FullName)
    member val SwapCommand = 
        sm.Command Person.Msg.Swap (fun m -> m.IsNotSwapped)
    member val SwapBackCommand = 
        sm.Command Person.Msg.SwapBack (fun m -> m.IsSwapped)


module VmFactory =
    let newVm () = 
        createRootVm PersonVm Person.init
```
