namespace Freact

module Reconciler =
    open Browser.Dom
    open Fable.Import
    open Browser.Types

    open Freact
    open Lib
    open Hooks

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

    // Crappy reconciliation that doesnt work. FIX!
    let rec private reconcile (currentDom: VirtualDom option) (newDom: VirtualDom): VirtualDom =
        match currentDom with
        | Some dom ->
            let (currentElement, currentAttributes) = dom
            let (newElement, newAttributes) = newDom

            if currentElement = newElement then
                let reconciledChildren = List.map2 (fun c n -> reconcile (Some c) n) currentAttributes.Children newAttributes.Children
                (newElement, { newAttributes with Children = reconciledChildren })
            else
                newDom
        | None ->
            newDom

    let mutable currentVirtualDom: VirtualDom option = None

    let render (element: Html) (container: HTMLElement) =
        let rec createVirtualDom (element: Element * (unit -> HtmlAttributes)) (path: PathElement list) =
            let (element, attributes: unit -> HtmlAttributes) = element
            currentIndex <- 0
            currentPath <- path
            currentState <-
                if (stateMap.TryFind currentPath).IsSome then
                    stateMap.[currentPath]
                else
                    []
            let attributes = attributes ()
            stateMap <- stateMap.Add(currentPath, currentState)
            let children: VirtualDom list =
                List.mapi (fun i x -> createVirtualDom x
                                          (let element, _ = x
                                           let nameOfElement = duToString element
                                           currentPath @ [{ Name = nameOfElement; Index = i }])) attributes.Children
            let mutable path = "";
            match element with
            | Element.H1 ->
                path <- duToString element
                H1, (createVirtualDomAttributes attributes children)
            | Element.H2 ->
                path <- duToString element
                H2, (createVirtualDomAttributes attributes children)
            | Element.H3 ->
                path <- duToString element
                H3, (createVirtualDomAttributes attributes children)
            | Element.H4 ->
                path <- duToString element
                H4, (createVirtualDomAttributes attributes children)
            | Element.H5 ->
                path <- duToString element
                H5, (createVirtualDomAttributes attributes children)
            | Element.H6 ->
                path <- duToString element
                H6, (createVirtualDomAttributes attributes children)
            | Element.P ->
                path <- duToString element
                P, (createVirtualDomAttributes attributes children)
            | Element.Div ->
                path <- duToString element
                Div, (createVirtualDomAttributes attributes children)
            | Element.Input ->
                path <- duToString element
                Input, (createVirtualDomAttributes attributes children)
            | Element.Button ->
                path <- duToString element
                Button, (createVirtualDomAttributes attributes children)
            | Element.Str s ->
                path <- duToString element
                Str s, (createVirtualDomAttributes attributes children)
            | Element.Component c ->
                createVirtualDom (List.head attributes.Children) currentPath

        let newVirtualDom = createVirtualDom element []
        // TODO: again, the reconciliation  doesnt work. Skip it for now :D
        //let reconciledVirtualDom = reconcile currentVirtualDom newVirtualDom

        //currentVirtualDom <- Some <| reconciledVirtualDom

        newVirtualDom
        //reconciledVirtualDom
        |> createDomElement
        |> attach container