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
          Type: Type option
          Children: VirtualDom list
        }
    and VirtualDom = VirtualDomElement * VirtualDomAttributes

    let createVirtualDomAttributes (attributes: HtmlAttributes) children =
        { ClassName = attributes.ClassName
          OnClick = attributes.OnClick
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

    let rec private reconcile (currentDom: VirtualDom option) (newDom: VirtualDom): VirtualDom =
        printfn "CurrentDom: %A" currentDom
        printfn "NewDom: %A" newDom

        (*

if the old fiber and the new element have the same type,
we can keep the DOM node and just update it with the new props

if the type is different and there is a new element,
it means we need to create a new DOM node

and if the types are different and there is an old fiber,
we need to remove the old node

        *)

        match currentDom with
        | Some dom ->
            let (currentElement, currentAttributes) = dom
            let (newElement, newAttributes) = newDom

            printfn "Dom already exists, reconciling..."

            // Er det samme element? Reconcile barna
            if currentElement = newElement then
                printfn "THe elements are the same, we need to reconcile the children"
                let reconciledChildren = List.map2 (fun c n -> reconcile (Some c) n) currentAttributes.Children newAttributes.Children
                (newElement, { newAttributes with Children = reconciledChildren })
            // Dersom typen er forskjellig bruker vi den nye
            else
                printfn "The elements are not the same. Just use the new one"
                newDom
        // Hvis currentdom ikke finnes, bruk newDom
        | None ->
            printfn "No dom exists. Use newDom"
            printfn ""
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
        let reconciledVirtualDom = reconcile currentVirtualDom newVirtualDom

        currentVirtualDom <- Some <| reconciledVirtualDom

        printfn "%A" reconciledVirtualDom

        container.innerText <- ""

        //newVirtualDom
        reconciledVirtualDom
        |> createDomElement
        |> attach container




















    // Vi trenger 2 V-Doms
    // - Nåværende og den vi vil tegne
    // Så vil vi finne diffen mellom disse to




    (*
    let rec private createElement element =
        match element with
        | Str s ->
            document.createTextNode s |> TextNode
        | _ ->
            document.createElement (duToString element) |> DomNode.ElementNode

    let rec attachElement (container: Browser.Types.HTMLElement) (attributes: HtmlAttributes) domNode =
        match domNode with
        | TextNode t ->
            container.appendChild t |> ignore
        | ElementNode e ->
            if attributes.ClassName.IsSome then
                e.setAttribute("class", attributes.ClassName.Value)
            if attributes.Type.IsSome then
                e.setAttribute("type", duToString attributes.Type.Value)
            if attributes.OnClick.IsSome then
                e.addEventListener("click", fun _ -> attributes.OnClick.Value ())
                                                // Hack hack hack
                                                //rootContainer.innerHTML <- ""
                                                // render myView rootContainer)
            List.iter (fun (element, attributes) -> createElement element) |> attachElement e (attributes ()) attributes.Children
            container.appendChild e |> ignore

let rec private reconcile (currentDom: Html) (newDom: Html) =
        let (currentDomElement, currentDomAttributes) = currentDom
        let (newDomElement, newDomAttributes) = newDom

        if currentDomElement = newDomElement then
            // Keys her?
            let children =
                if List.length newDomAttributes.Children > List.length currentDomAttributes.Children then
                    newDomAttributes.Children
                else
                    List.map2 reconcile currentDomAttributes.Children newDomAttributes.Children
            (currentDomElement, {newDomAttributes with Children = children })
        else
            newDom
*)


    /////////////////////////


    (*
    type Fiber =
        { Type: Html
          Attributes: HtmlAttributes
          DomElement: DomNode
          Path: PathElement list
          Key: string
          Children: Fiber list
        }
        Type: Div/BUutton (min type)
        Attributes: HtmlAttributes
        Element: HTMLElement
        Path: string list
        Key: Unikt identifisere denne
        State: Har denne state?
        let mutable foobar: Fiber option = None
    *)

    //let mutable rootElement: Html option = None

        //let (element', attributes) = element
        //match rootElement with
        //| None ->
        //let domElement = createElement element' (attributes ())
        //attachElement container (attributes ()) domElement
        //rootElement <- Some element
        //| Some model ->
        //    let (newElement, newAttributes) = reconcile model element
        //    container.innerText <- ""
        //    let domElement = createElement newElement
         //   attachElement container (newAttributes ()) domElement
