# impF
[Elm] (http://elm-lang.org/) inspired imutable MVVM prototype library for Wpf

Example:

```fsharp
module Person = 
```

Define your model as a simple record

```fsharp
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
```

Then a set of messages that can update the model as a Discriminated Union

```fsharp
    type Msg = 
        | First of string
        | Last of string
        | Swap
        | SwapBack
```
 
Define the initial state of the model

```fsharp
    let init =
        { FirstName = ""
        ; LastName = ""
        ; IsSwapped = false
        }
```

Now put the model and the messages together in the update function 

*Please note that it's a pure function with this signature:*

**msg -> model -> model**

```fsharp
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
```            


Now you need to define the ViewModel

```fsharp
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
```


You can create a factory that allows you to create an instance of the ViewModel

```fsharp
module VmFactory =
    let newVm () = 
        createRootVm PersonVm Person.init
```


The you'll use your ViewModel in XAML as usuall.

Please note that the value of the **Field** and **RoField** is in **.V** property

```xaml
 <TextBox Text="{Binding FirstName.V }"/>
 <Label Content="{Binding FullName.V}"/>
 <Button Content="Swap" Command="{Binding SwapCommand}"/>
```
