namespace Example3_VmComposition

open impF.VmBase

module Person =
    type Msg = 
        | First of string
        | Last of string

     type Model = 
        { FirstName : string
        ; LastName  : string
        } 
        with
        member x.FullName = 
            sprintf "%s %s" x.FirstName x.LastName
        
    let init = 
        { FirstName = ""
        ; LastName = "" 
        }
 
    let update msg state = 
        match msg with 
        | First s -> 
            { state with FirstName = s }
        | Last s -> 
            { state with LastName = s }
      
module Order =
    type Msg =
        | Seller of Person.Model
        | Buyer of Person.Model
        | Swap 
        | SwapBack 


    type Model = 
        { Seller : Person.Model
        ; Buyer : Person.Model
        ; IsSwapped : bool 
        } 
        with
        member x.IsNotSwapped = 
            not x.IsSwapped

    let init = 
        { Seller = Person.init
        ; Buyer = Person.init
        ; IsSwapped = false
        }

    let update msg state = 
        let swap() = 
            { state 
                with 
                Seller = state.Buyer 
                ; Buyer = state.Seller
                ; IsSwapped = not state.IsSwapped
            }   

        match msg with 
        | Seller s -> 
            { state with Seller = s ; }
        | Buyer s ->  
            { state with Buyer = s ; } 
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

type OrderVm(p)  = 
    let vm = 
        stateManager Order.update p

    member val Buyer = 
        vm.VmField Order.Msg.Buyer (fun m -> m.Buyer) PersonVm
    member val Seller = 
        vm.VmField Order.Msg.Seller (fun m -> m.Seller) PersonVm 
 
    member val SwapCommand = 
        vm.Command Order.Msg.Swap (fun m -> m.IsNotSwapped) 
    member val SwapBackCommand = 
        vm.Command Order.Msg.SwapBack (fun m -> m.IsSwapped) 


module VmFactory =
    let newVm () = 
        createRootVm OrderVm Order.init