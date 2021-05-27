namespace Freact

open Freact

module playground =
    open Browser.Dom
    open Fss

    open Lib
    open Reconciler
    open Hooks

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

    let container =
        fss [ Display.flex
              FlexDirection.column
              Width' <| px 200 ]

    let propExample name =
        p <| [ str $"Hello {name}" ]

    let slow () =
        let hover = fss [
            BorderColor.black
            BorderStyle.solid
            BorderWidth.thin
            Width' <| px 20
            Height' <| px 20
            Hover [ BackgroundColor.blue ] ]
        div
            |> className (fss [Display.flex; FlexDirection.row; FlexWrap.wrap])
            <| ([0..100]
            |> List.map (fun _ ->
                div
                |> className hover
                <|  []))

    let myView =
        let (n, setN) = useState 0
        div
            <| [ div
                    |> className background
                    <| [ h1 <| [ str "Hello world" ]

                         div
                            |> className spinimation
                            <| [ ]

                         h2
                            |> className text
                            <| [ str "from Freact" ]]

                 div
                    |> className container
                    <| [ p <| [ str "A paragraph of text? Naah" ]
                         input
                            |> type'.text
                         input
                            |> type'.password
                         button
                            |> onClick (fun _ -> printfn "Button!")
                            <| [ str "Click me!" ]
                       ]
                 propExample "you!"

                 slow ()
                ]

    let rootContainer = document.getElementById "app"

    //render myView rootContainer

    let reconcileTest1 =
        div
         <| [ div <| [ p <| [ str "Hello there" ] ]
              div <| []
            ]

    let reconcileTest2 =
        div
         <| [ div <| [ button <| [ str "Hello son!" ] ]
              div <| []
            ]

    let komponent props =
        let (counter, setCounter) = useState<int>(props)
        div
            <| [ button |> onClick (fun _ ->
                    setCounter (counter + 1))
                    <| [ str "+" ]

                 button |> onClick (fun _ ->
                    setCounter (counter - 1))
                    <| [ str "-" ]

                 p <| [ str <| string counter ]

               ]

    let k3 props =
        div <| [ str (string props) ]

    let k2 props =
        k3 </> props

    let k1 props =
        k2 </> props

    let reconcileTest3 =
        div
         <| [ //div
               // <| [ p
              //          <| [ str "Hello there" ]
              //       button
              //          |> onClick (fun _ -> printfn "Hello son")
              //          <| [ str "Hello son!" ] ]
              //div <| [ str "Some text here" ]
              komponent </> 0
              //k1 </> 0
            ]

    //render reconcileTest1 rootContainer
    //render reconcileTest2 rootContainer
    reRender <- fun () ->
        rootContainer.innerHTML <- ""
        render reconcileTest3 rootContainer

    render reconcileTest3 rootContainer



