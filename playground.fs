namespace Freact

open Freact

module playground =
    open Fss
    open Lib
    open Browser.Dom

    let background =
        fss [ BackgroundColor.salmon ]

    let text =
        fss [ TextAlign.right ]

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


    let myView =
        let (n, setN) = useState 0
        div
            |> className background
            <| [ h1 <| [ str "Hello world" ]

                 div
                    |> className spinimation
                    <| [ ]

                 h2
                    |> className text
                    <| [ str "from Freact" ]]

    let rootContainer = document.getElementById "app"

    render myView rootContainer



