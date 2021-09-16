namespace Freact

module Hooks =
    let mutable reRender = fun () -> ()
    let mutable hooks: obj array = Array.zeroCreate 10
    let mutable currentHook = 0

    let useState<'t> (initialValue: 't) =
        if hooks.[currentHook] = null then
            hooks.[currentHook] <- initialValue :> obj

        let setStateHookIndex = currentHook

        let setState (newState: 't) =
            // her må vi sjekke om det er en funksjon
            hooks.[setStateHookIndex] <- newState :> obj
            reRender()

        let currentState = (box hooks.[setStateHookIndex]) :?> 't

        currentHook <- currentHook + 1
        currentState, setState





