namespace Freact

module Hooks =
    type PathElement =
        { Name: string
          Index: int
        }

    // Temporary hack
    let mutable reRender = fun () -> ()

    let mutable stateMap: Map<PathElement list, obj list> = Map.empty
    let mutable currentState: obj list = []
    let mutable currentIndex = 0
    let mutable currentPath: PathElement list = []

    let useState<'t> (initialState: 't) =
        if currentIndex >= currentState.Length then
            currentState <- currentState @ [ initialState ]

        let state: 't = (box currentState.[currentIndex]) :?> 't

        let i = currentIndex
        let s = currentState
        let p = currentPath
        currentIndex <- currentIndex + 1

        let setState (newState: 't) =
            let replace = newState :> obj
            stateMap <- stateMap.Add(p, List.mapi (fun index obj -> if i = index then replace else obj) s)
            reRender ()
        state, setState

module NewHooks =
    let mutable hooks: obj array = Array.zeroCreate 10
    let mutable currentHook = 0

    let useState<'t> (initialValue: 't) =
        if hooks.[currentHook] = null then
            printfn "Using initial value"
            hooks.[currentHook] <- initialValue :> obj

        let setStateHookIndex = currentHook

        printfn "Setting initial hook state %A" initialValue
        printfn "hooks is: %A" hooks

        let setState (newState: 't) =
            printfn "Set state: %A" newState
            printfn "Hooks is: %A" hooks
            printfn "Set state hook index : %A" currentHook
            // her må vi sjekke om det er en funksjon
            hooks.[setStateHookIndex] <- newState :> obj
            Hooks.reRender()

        let currentState = (box hooks.[setStateHookIndex]) :?> 't

        currentHook <- currentHook + 1
        printfn "Incrementing current hook. Is now: %A" currentHook
        currentState, setState





