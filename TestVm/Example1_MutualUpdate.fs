namespace Example1_MutualUpdate

open impF.VmBase


module MutualUpdate =
    type Msg = 
        | Number of decimal
        | MultipliedNumber of decimal
        
    type Model = 
        { Number : decimal
        ; MultipliedNumber : decimal
        } 

    let init = 
        { Number = 0m
        ; MultipliedNumber = 0m 
        }
 
    let update msg _ = 
        match msg with 
        | Number n -> 
            { Number = n
            ; MultipliedNumber = n * 2m 
            }
        | MultipliedNumber n -> 
            { Number = n / 2m
            ; MultipliedNumber = n 
            }


type MutualUpdateVm(p) =
    let sm = 
        stateManager MutualUpdate.update p

    member val Number = 
        sm.Field MutualUpdate.Msg.Number (fun m -> m.Number)
    member val MultipliedNumber = 
        sm.Field MutualUpdate.Msg.MultipliedNumber (fun m -> m.MultipliedNumber)

module VmFactory =
    let newVm () = 
        createRootVm MutualUpdateVm MutualUpdate.init