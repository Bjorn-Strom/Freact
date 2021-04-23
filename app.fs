namespace Freact
module Freact =
    open Browser.Dom

    // Todo: Get state working
    type HtmlTag =
        | Div
        | Str of string

    type HtmlAttributes = 
        { ClassName: string option
          OnClick: (unit -> unit) option
          Children: Html list
        }
    and Html =
        | RegularHtml of HtmlTag * HtmlAttributes
        | Component of (obj list -> Html) * (unit -> Html)

    let ( </> ) (comp: 'a -> Html) (props: 'a) =
        Component (comp, fun () -> comp props)

    type IDomNode = interface end
    type TextNode =
        | TextNode of Browser.Types.Text
        interface IDomNode
    let private unboxTextNode (TextNode t) = t
    type ElementNode = 
        | ElementNode of Browser.Types.HTMLElement
        interface IDomNode
    let private unboxElementNode (ElementNode e) = e

    let div children =
        (Div, { ClassName = None
                OnClick = None 
                Children = children
              })
        |> RegularHtml

    let str str =
        (Str str, { ClassName = None
                    OnClick = None 
                    Children = []
                  })
        |> RegularHtml

    let className className f children =
        match f children with
        | RegularHtml (tag, rec') -> 
            (tag, { rec' with 
                        ClassName = Some className 
                  })
            |> RegularHtml
        | a -> a

    let onClick onClick f children =
        match f children with
        | RegularHtml (tag, rec') ->
            (tag, { rec' with 
                        OnClick = Some onClick
                  })
            |> RegularHtml
        | a -> a

    let mutable state: Map<(obj list -> Html), string> = Map.empty

    let useState s =
        let mutable state = s
        (s, fun s' -> state <- s')

    let Kom [] =
        div <| [ str "Chicks" ]
            
    let MyView []: Html =
        let (n, setN) = useState 0
        div
        |> className "lol"
        |> onClick (fun () -> setN (n + 1))
        <| [
            div
            |> className "Duck"
            <| [ str "Carts" ]

            if n > 3 then
                Kom </> []

            if false then
                div []

            div
            |> onClick (fun () -> printf "you" |> ignore)
            <| [ str "Sticks"]
            div
            <| [ str (sprintf "%A" n) ]
        ]


    let rootContainer = document.getElementById "app"

    let rec render element (container: Browser.Types.HTMLElement) =
        match element with
        | RegularHtml (tag, attributes: HtmlAttributes) ->
            let newDomElement =
                match tag with
                | Str s ->
                    document.createTextNode s |> TextNode :> IDomNode
                | Div ->
                    document.createElement "div" |> ElementNode :> IDomNode

            match newDomElement with
            | :? TextNode as t ->
                container.appendChild <| unboxTextNode t |> ignore
            | :? ElementNode as e ->
                let e = unboxElementNode e
                if attributes.OnClick.IsSome then
                    e.addEventListener("click", fun _ -> 
                                                    attributes.OnClick.Value ()
                                                    // Hack hack hack
                                                    rootContainer.innerHTML <- ""
                                                    render (MyView </> []) rootContainer)
                List.iter (fun x -> render x e) attributes.Children
                container.appendChild e |> ignore
        | Component (f, html) ->
            render (html ()) container
        

    render (MyView </> []) rootContainer