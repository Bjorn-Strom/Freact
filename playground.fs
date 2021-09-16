namespace Freact

open Freact
open Fable.Core.JsInterop

module playground =
    open Fss

    open Lib
    open Reconciler
    open Hooks

    let spinimation =
        let spin =
            keyframes
                [ frame 0 [ Transforms [ Transform.rotate <| deg 0. ] ]
                  frame 100 [ Transforms [ Transform.rotate <| deg 360. ] ]
                ]

        fss [ Position.relative
              Left' <| pct 50
              Width' <| px 100
              Height' <| px 100
              BackgroundColor.blue
              AnimationName' spin
              AnimationDuration' <| ms 5000.
              AnimationIterationCount.infinite
              AnimationTimingFunction.linear
            ]

    let container =
        fss [ Display.flex
              FlexDirection.column
              Width' <| px 200 ]

    let loginForm () =
         let (username, setUsername) = useState ""
         let (password, setPassword) = useState ""

         div
            |> className container
            <| [ p <| [ str "A paragraph of text? Naah" ]
                 input
                    |> type'.text
                    |> onChange (fun e -> setUsername $"{username}{e.target?value}")
                 input
                    |> type'.password
                    |> onChange (fun e -> setPassword $"{password}{e.target?value}")
                 p <| [ str $"Your username is: {username}" ]
                 p <| [ str $"Your password is: {password}" ]
                 button
                    |> onClick (fun _ -> printfn "Button!")
                    <| [ str "Click me!" ]
               ]

    let colorCounter start =
        div <|
           [ str "A color counter"
             let (counter, setCounter) = NewHooks.useState<int>(start)
             let style =
                 fss [
                     if counter >= 5 then
                         Color.green
                     if counter <= -5 then
                         Color.red
                 ]
             div
                <| [ button |> onClick (fun _ ->
                        setCounter (counter + 1))
                        <| [ str "+" ]

                     button |> onClick (fun _ ->
                        setCounter (counter - 1))
                        <| [ str "-" ]

                     p |> className style <| [ str <| string counter ]
                   ]
            ]

    let appearCounter start =
        div <|
           [ str "A appear counter"
             let (counter, setCounter) = NewHooks.useState<int>(start)
             div
                <| [ button |> onClick (fun _ ->
                        setCounter (counter + 1))
                        <| [ str "+" ]

                     button |> onClick (fun _ ->
                        setCounter (counter - 1))
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