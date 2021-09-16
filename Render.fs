namespace Freact

module Reconciler =
    open Browser.Dom
    open Fable.Import
    open Browser.Types

    open Freact
    open Lib

    // Used to build a virtual dom, AKA what do we want to draw
    type VirtualDomElement =
        | H1
        | H2
        | H3
        | H4
        | H5
        | H6
        | P
        | Div
        | Input
        | Button
        | Str of string
    and VirtualDomAttributes =
        { ClassName: string option
          OnClick: (unit -> unit) option
          OnChange: (Browser.Types.Event -> Unit) option
          Type: Type option
          Children: VirtualDom list
        }
    and VirtualDom = VirtualDomElement * VirtualDomAttributes

    let createVirtualDomAttributes (attributes: HtmlAttributes) children =
        { ClassName = attributes.ClassName
          OnClick = attributes.OnClick
          OnChange = attributes.OnChange
          Type = attributes.Type
          Children = children
        }

    // Used to create the actual dom to render
    type Dom =
        { DomNode: DomNode
          Children: Dom list
        }
    and DomNode =
        | TextNode of Browser.Types.Text
        | ElementNode of Browser.Types.HTMLElement

    let rec createDomElement virtualDom =
        let (element, attributes) = virtualDom
        match element with
        | Str s ->
            { DomNode = document.createTextNode s |> TextNode
              Children = []
            }
        | _ ->
            let domElement = document.createElement (duToString element)

            if attributes.ClassName.IsSome then
                domElement.setAttribute("class", attributes.ClassName.Value)
            if attributes.Type.IsSome then
                domElement.setAttribute("type", duToString attributes.Type.Value)
            if attributes.OnClick.IsSome then
                domElement.addEventListener("click", fun _ -> attributes.OnClick.Value ())
            // Dersom input eller textArea så må dette være input noe annet change
            if attributes.OnChange.IsSome then
                domElement.addEventListener("input", fun e -> attributes.OnChange.Value e)

            let children = List.map createDomElement attributes.Children

            { DomNode = domElement |> ElementNode
              Children = children
            }

    let rec attach (container: HTMLElement) (dom: Dom) =
        match dom.DomNode with
        | TextNode t ->
            container.appendChild t |> ignore
        | ElementNode e ->
            List.iter (fun x -> attach e x) dom.Children
            container.appendChild e |> ignore

    let rec render (element: unit -> Html) =
        Hooks.reRender <- fun () ->
            render element
        let element = element ()
        let dom = document.getElementById "app"
        dom.innerHTML <- ""
        Hooks.currentHook <- 0
        let rec createVirtualDom (element: Element * (unit -> HtmlAttributes)) =
            let (element, attributes: unit -> HtmlAttributes) = element
            let attributes = attributes ()
            let children: VirtualDom list =
                List.mapi (fun i x -> createVirtualDom x) attributes.Children
            match element with
            | Element.H1 ->
                H1, (createVirtualDomAttributes attributes children)
            | Element.H2 ->
                H2, (createVirtualDomAttributes attributes children)
            | Element.H3 ->
                H3, (createVirtualDomAttributes attributes children)
            | Element.H4 ->
                H4, (createVirtualDomAttributes attributes children)
            | Element.H5 ->
                H5, (createVirtualDomAttributes attributes children)
            | Element.H6 ->
                H6, (createVirtualDomAttributes attributes children)
            | Element.P ->
                P, (createVirtualDomAttributes attributes children)
            | Element.Div ->
                Div, (createVirtualDomAttributes attributes children)
            | Element.Input ->
                Input, (createVirtualDomAttributes attributes children)
            | Element.Button ->
                Button, (createVirtualDomAttributes attributes children)
            | Element.Str s ->
                Str s, (createVirtualDomAttributes attributes children)

        createVirtualDom element //[]
        |> createDomElement
        |> attach dom