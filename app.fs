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
        HtmlTag * HtmlAttributes

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

    let str str =
        (Str str, { ClassName = None
                    OnClick = None 
                    Children = []
                  })

    let className className f children =
        let (tag, rec') = f children
        (tag, { rec' with 
                    ClassName = Some className 
                    })

    let onClick onClick f children =
        let (tag, rec') = f children
        (tag, { rec' with 
                    OnClick = Some onClick
                        })

    let useState s =
        let mutable state = s
        (s, fun s' -> state <- s')
            
    let myView: Html =
        let (n, setN) = useState 0
        div
        |> className "lol"
        |> onClick (fun () -> setN (n + 1))
        <| [
            div
            |> className "Duck"
            <| [ str "Carts" ]
            div
            |> onClick (fun () -> printf "you" |> ignore)
            <| [ str "Sticks"]
            div
            <| [ str (sprintf "%A" n)]
        ]


    let rootContainer = document.getElementById "app"

    let rec render element (container: Browser.Types.HTMLElement) =
        let (tag, attributes: HtmlAttributes) = element
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
                                                render myView rootContainer)
            List.iter (fun x -> render x e) attributes.Children
            container.appendChild e |> ignore
        


    render myView rootContainer