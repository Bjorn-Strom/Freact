namespace Freact

[<RequireQualifiedAccess>]
module HookInternals =
    let mutable reRender = fun () -> ()
    let mutable hooks: ResizeArray<obj> = ResizeArray([])
    let mutable currentHook = 0

[<RequireQualifiedAccess>]
module Hooks =
    type Hooks =
        static member useState<'t> (initialValue: 't) =
            if HookInternals.hooks.[HookInternals.currentHook] = null then
                HookInternals.hooks.[HookInternals.currentHook] <- initialValue :> obj

            let setStateHookIndex = HookInternals.currentHook

            let setState (newState: 't) =
                // her må vi sjekke om det er en funksjon
                HookInternals.hooks.[setStateHookIndex] <- newState :> obj
                HookInternals.reRender()

            let currentState = (box HookInternals.hooks.[setStateHookIndex]) :?> 't

            HookInternals.currentHook <- HookInternals.currentHook + 1
            currentState, setState
