[<AutoOpen>]
module VmFieldFactory

open impF.VmBase

let buildField  
    (vm : IVmStateManager<_,_>) 
    (fieldStateSelector : 'state -> 'fieldState) 
    (msgBuilder : 'fieldState -> 'msg) 
    (fieldFactory : 'state -> ('state -> 'fieldState) -> ('fieldState -> unit) -> 'field)
    (fieldStateUpdator : 'field -> ('state -> unit)) 
    : 'field 
    = 
    let currentStateModifier (newFieldState : 'fieldState) : unit = 
        let msg = 
            msgBuilder newFieldState
        vm.UpdateState msg
                    
    let field = 
        fieldFactory (vm.Get()) fieldStateSelector currentStateModifier
    vm.AddChildStateUpdator (fieldStateUpdator field)
    field

type IVmStateManager<'msg, 'state when 'state : equality> with
    member x.Command 
        (msg : 'msg)
        (canExecuteSelector : 'state -> bool) 
        = 
        let setFromParent = Event<bool>()
        let cmd = ImmutableCommand(canExecuteSelector <| x.Get(), (fun () ->  x.UpdateState msg), setFromParent.Publish)
        x.AddChildStateUpdator (canExecuteSelector >> setFromParent.Trigger)
        cmd

    member x.Field
        (msgBuilder : 'fieldState -> 'msg) 
        (fieldStateSelector : 'state -> 'fieldState) 
        = 
        buildField x fieldStateSelector msgBuilder 
            (fun state selector onChange -> createField(selector state, onChange) ) 
            (fun fld -> fieldStateSelector >> (fun x -> fld.V <- x))

    member x.RoField
        (fieldStateSelector : 'state -> 'fieldState) 
        = 
        let fld, setFromParent = createRoField(fieldStateSelector <| x.Get())
        x.AddChildStateUpdator (fieldStateSelector >> setFromParent.Trigger)
        fld

    member x.VmField<'msg,'state,'vmState, 'vm  when 'state : equality>
        (msgBuilder : 'vmState -> 'msg) 
        (fieldStateSelector : 'state -> 'vmState) 
        (vmFactory : VmStateManagerParams<'vmState> -> 'vm) 
        = 
        let setFromParent = Event<'vmState>()
        buildField x fieldStateSelector msgBuilder 
            (fun state selector onChange -> vmFactory({Init =  selector state; OnStateChanged = onChange; SetFromParent = setFromParent.Publish } ) )
            (fun _ -> fieldStateSelector >> setFromParent.Trigger)