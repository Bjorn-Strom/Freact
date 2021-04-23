namespace Freact

module Freact =
    open Browser.Dom
    open Microsoft.FSharp.Reflection

    let inline duToString (x:'a): string=
        match FSharpValue.GetUnionFields(x, typeof<'a>) with
        | case, _ -> case.Name

    // https://developer.mozilla.org/en-US/docs/Web/HTML/Element/input
    type Type =
        | Password
        | Text

    type Element =
        | Div
        | Input
        | Button
        | Str of string

    type HtmlAttributes =
        { ClassName: string option
          OnClick: (unit -> unit) option
          Type: Type option
          Children: Html list
        }

    and Html = Element * HtmlAttributes

    let createNode element children =
        element,
        { ClassName = None
          OnClick = None
          Type = None
          Children = children
        }

    type DomNode =
        | TextNode of Browser.Types.Text
        | ElementNode of Browser.Types.HTMLElement

    let div children = createNode Div children
    let input = createNode Input []
    let button children = createNode Button children
    let str str = createNode (Str str) []

    let className className f children =
        let (element: Element, attributes) = f children
        element, { attributes with ClassName = Some className }

    let onClick onClick f children =
         let (element: Element, attributes) = f children
         element, { attributes with OnClick = Some onClick }

    type type' =
        static member password f =
            let (element, attributes) = f
            element, { attributes with Type = Some Password }
        static member text f =
            let (element, attributes) = f
            element, { attributes with Type = Some Text }

    let useState s =
        let mutable state = s
        (s, fun s' -> state <- s')

    let myView =
        let (n, setN) = useState 0
        div
            |> className "lol"
            |> onClick (fun () -> setN (n + 1))
            <| [ div
                    |> className "Duck"
                    <| [ str "Carts" ]

                 div
                    |> onClick (fun () -> printf "you")
                    <| [ str "Sticks"]

                 div
                    <| [ str (sprintf "%A" n)]

                 input
                    |> type'.password

                 input
                    |> type'.text

                 button
                    |> onClick (fun _ -> printfn "Click a button")
                    <| [ str "A button!" ]
               ]

    let rootContainer = document.getElementById "app"

    let rec render element (container: Browser.Types.HTMLElement) =
        let (element, attributes: HtmlAttributes) = element
        let newDomElement =
            match element with
            | Str s ->
                document.createTextNode s |> TextNode
            | _ ->
                document.createElement (duToString element)  |> DomNode.ElementNode

        match newDomElement with
        | TextNode t ->
            container.appendChild t |> ignore
        | ElementNode e ->
            if attributes.OnClick.IsSome then
                e.addEventListener("click", fun _ ->
                                                attributes.OnClick.Value ()
                                                // Hack hack hack
                                                rootContainer.innerHTML <- ""
                                                render myView rootContainer)
            List.iter (fun x -> render x e) attributes.Children
            container.appendChild e |> ignore

    render myView rootContainer