namespace Freact

[<RequireQualifiedAccess>]
module HookInternals =
    let mutable reRender = fun () -> ()
    let mutable hooks: ResizeArray<obj> = ResizeArray([])
    let mutable currentHook = 0

[<Sealed>]
type SetState(setStateHookIndex) =
    member this.setState (newState: 't) =
        HookInternals.hooks.[setStateHookIndex] <- newState :> obj
        HookInternals.reRender()

    member this.setState (newState: 't -> 't) =
        let currentState = (box HookInternals.hooks.[setStateHookIndex]) :?> 't
        HookInternals.hooks.[setStateHookIndex] <- newState(currentState) :> obj
        HookInternals.reRender()


[<RequireQualifiedAccess>]
module Hooks =
    let useState<'t> (initialValue: 't) =
        if HookInternals.hooks.[HookInternals.currentHook] = null then
            HookInternals.hooks.[HookInternals.currentHook] <- initialValue :> obj

        let setStateHookIndex = HookInternals.currentHook

        let setState (newState: 't) =
            HookInternals.hooks.[setStateHookIndex] <- newState :> obj
            HookInternals.reRender()

        let newSetState (newState: 't -> 't) =
            let currentState = (box HookInternals.hooks.[setStateHookIndex]) :?> 't
            HookInternals.hooks.[setStateHookIndex] <- newState(currentState) :> obj
            HookInternals.reRender()

        let currentState = (box HookInternals.hooks.[setStateHookIndex]) :?> 't

        HookInternals.currentHook <- HookInternals.currentHook + 1
        currentState, SetState(setStateHookIndex)
