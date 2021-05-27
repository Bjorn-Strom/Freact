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
            ()
        state, setState