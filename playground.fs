namespace Freact

open Freact
open Fable.Core.JsInterop

module playground =
    open Browser.Dom
    open Fss

    open Lib
    open Reconciler
    open Hooks

    let background =
        fss [ BackgroundColor.salmon ]

    let text=
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

    let clicker () =
        let length = 100
        let (blocks, setBlocks) = useState<int array> (Array.create length 0)
        let hover n = fss [
            BorderColor.black
            BorderStyle.solid
            BorderWidth.thin
            Width' <| px 20
            Height' <| px 20
            if n = 1 then BackgroundColor.red
            Hover [ BackgroundColor.blue ] ]
        div
            |> className (fss [Display.flex; FlexDirection.row; FlexWrap.wrap])
            <| (blocks
            |> Array.mapi (fun index value ->
                div
                |> className (hover value)
                |> onClick (fun _ ->
                    blocks.[index] <- if blocks.[index] = 1 then 0 else 1
                    setBlocks blocks)
                <|  []) |> Array.toList)

    let counter props =
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

    let appearDissapear =
        let aComponent () =
            let (count, setCount) = useState 0
            div <| [
                button
                |> onClick (fun _ -> setCount (count + 1))
                <| [ str "+" ]
                button
                |> onClick (fun _ -> setCount (count - 1))
                <| [ str "-" ]
                str (string count)
                if count > 5 then
                    h2 [ str "Text is big now" ]
                else if count <  -5 then
                    h2 [ str "Text is small now" ]
            ]

        div
            <| [
                h2 <| [ str "Hello" ]
                aComponent </> ()

            ]

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

    let setStateTests =
        div <|
           [ h2 <| [ str "Set state testing" ]

             str "A counter"
             counter </> 0
             str "A clicker that changes color"
             clicker </> ()
             str "A clicker that renders text conditionally"
             appearDissapear
             str "Please log in"
             loginForm </> ()
            ]

    let app =
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

                 setStateTests
                ]


    let rootContainer = document.getElementById "app"



    render app rootContainer

    reRender <- fun () ->
        rootContainer.innerText <- ""
        render app rootContainer
