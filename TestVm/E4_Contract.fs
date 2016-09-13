namespace Example4

open impF.VmBase
open System


module Person =
    type Msg = 
        | First of string
        | Last of string
        | Reward of decimal

     type Model = 
        { FirstName : string
        ; LastName  : string
        ; Reward : decimal
        } 
        with
        member x.FullName = 
            sprintf "%s %s" x.FirstName x.LastName

    let init = 
        { FirstName = ""
        ; LastName = "" 
        ; Reward = 0m 
        }
 
    let update msg state = 
        match msg with 
        | First s -> 
            { state with FirstName = s }
        | Last s -> 
            { state with LastName = s }
        | Reward n -> 
            { state with Reward = n }
        

module Contract =
    type Msg =
        | Seller of Person.Model
        | Buyer of Person.Model
        | Condition of string
        | Swap 
        | SwapBack 
        | Sign

    type Model = 
        { Seller : Person.Model
        ; Buyer : Person.Model
        ; IsSwapped : bool 
        ; Condition : string
        ; IsSigned : bool
        } 
        with
        member x.IsNotSwapped = 
            not x.IsSwapped

        member x.CanSign = 
            if x.IsSigned 
            then
                false
            else
                let stringIsSet s = 
                    not <| String.IsNullOrWhiteSpace s

                let personIsSet (p : Person.Model) = 
                    stringIsSet p.FirstName 
                    && stringIsSet p.LastName

                personIsSet x.Buyer 
                && personIsSet x.Seller 
                && stringIsSet x.Condition

        member x.SignedContractText =
            if x.IsSigned 
            then
                sprintf "The Seller: %s, \nThe Buyer: %s \nContracting Parties  undertake to: \n%s" x.Seller.FullName x.Buyer.FullName x.Condition
            else
                ""

    let init = 
        { Seller = Person.init
        ; Buyer = Person.init
        ; Condition = ""
        ; IsSwapped = false
        ; IsSigned = false; 
        }

    let update msg state = 

        let swap() = 
            { state 
                with 
                Seller = { state.Buyer with Reward = state.Seller.Reward } 
                ; Buyer = { state.Seller with Reward = state.Buyer.Reward } 
                ; IsSwapped = not state.IsSwapped
            }   

        let newState =
            match msg with 
            | Seller s -> 
                { state with Seller = s ; Buyer = { state.Buyer with Reward = s.Reward * 2m }  }
            | Buyer s ->  
                { state with Buyer = s ; Seller = { state.Seller with Reward = s.Reward / 2m }  } 
            | Condition n -> 
                { state with Condition = n; } 
            | Swap -> 
                swap()
            | SwapBack -> 
                swap()
            | Sign -> 
                { state with IsSigned = true }
    
        if msg <> Sign 
            && newState.SignedContractText <> state.SignedContractText then
            { newState with IsSigned = false }
        else
            newState
    
             
 type PersonVm(p) =
    let vm = 
        stateManager Person.update p

    member val FirstName = 
        vm.Field Person.Msg.First (fun m -> m.FirstName) 
    member val LastName = 
        vm.Field Person.Msg.Last (fun m -> m.LastName)
    member val FullName = 
        vm.RoField (fun m -> m.FullName)
    member val Reward = 
        vm.Field Person.Msg.Reward (fun m -> m.Reward)


type ContractVm(p)  = 
    let vm = 
        stateManager Contract.update p

    member val Buyer = 
        vm.VmField Contract.Msg.Buyer (fun m -> m.Buyer) PersonVm
    member val Seller = 
        vm.VmField Contract.Msg.Seller (fun m -> m.Seller) PersonVm 
    member val Condition = 
        vm.Field Contract.Msg.Condition (fun m -> m.Condition)

    member val IsSigned = 
        vm.RoField (fun m -> m.IsSigned)
    member val SignedContractText = 
        vm.RoField (fun m -> m.SignedContractText)
    member val SignCommand = 
        vm.Command Contract.Msg.Sign (fun m -> m.CanSign) 

    member val SwapCommand = 
        vm.Command Contract.Msg.Swap (fun m -> m.IsNotSwapped) 
    member val SwapBackCommand = 
        vm.Command Contract.Msg.SwapBack (fun m -> m.IsSwapped) 


module VmFactory =
    let newVm () = 
        createRootVm ContractVm Contract.init

