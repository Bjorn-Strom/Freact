namespace Freact

open Freact

module playground =
    open Fss

    open Lib
    open Reconciler

    let colorCounter start =
        div <|
           [ str "A color counter"
             let (counter, setCounter) = Hooks.useState<int>(start)
             let style =
                 fss [
                     if counter >= 5 then
                         Color.green
                     if counter <= -5 then
                         Color.red
                 ]
             let test =
                 fun old ->
                            printfn "This is a function"
                            old + 1
             div
                <| [ button |> onClick (fun _ ->
                        setCounter.setState (fun old ->
                            printfn "This is a function"
                            old + 1))
                        <| [ str "+" ]

                     button |> onClick (fun _ ->
                        setCounter.setState (counter - 1))
                        <| [ str "-" ]

                     p |> className style <| [ str <| string counter ]
                   ]
            ]

    let appearCounter start =
        div <|
           [ str "A appear counter"
             let (counter, setCounter) = Hooks.useState<int>(start)
             div
                <| [ button |> onClick (fun _ ->
                        setCounter.setState (counter + 1))
                        <| [ str "+" ]

                     button |> onClick (fun _ ->
                        setCounter.setState (counter - 1))
                        <| [ str "-" ]

                     p <| [ str <| string counter ]
                     if counter >= 5 then
                        p <| [ str "It is above 5" ]
                     else if counter <= -5 then
                        p <| [ str "It is below 5" ]
                   ]
            ]

    let app () =
        div
            <| [ h3 <| [ str "Set state testing" ]
                 colorCounter 0
                 colorCounter 5
                 appearCounter 0
                 appearCounter 5
                ]


    render app