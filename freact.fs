namespace Freact

// Used to build an internal representation of the component tree
module Lib =
    open Microsoft.FSharp.Reflection

    let inline duToString (x:'a): string=
        match FSharpValue.GetUnionFields(x, typeof<'a>) with
        | case, _ -> case.Name

    // https://developer.mozilla.org/en-US/docs/Web/HTML/Element/input
    type Type =
        | Password
        | Text

    type Element =
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

    and HtmlAttributes =
        { ClassName: string option
          // Burde være Browser.Types.Mouseevent -> unit?
          OnClick: (unit -> unit) option
          OnChange: (Browser.Types.Event -> Unit) option
          Type: Type option
          Children: Html list
        }

    and Html = Element * (unit -> HtmlAttributes)

    let createNode element children: Html =
          element,
          fun () ->
            { ClassName = None
              OnClick = None
              OnChange = None
              Type = None
              Children = children
            }

    let h1 children = createNode H1 children
    let h2 children = createNode H2 children
    let h3 children = createNode H3 children
    let h4 children = createNode H4 children
    let h5 children = createNode H5 children
    let h6 children = createNode H6 children
    let p children = createNode P children
    let div children = createNode Div children
    let input = createNode Input []
    let button children = createNode Button children
    let str str = createNode (Str str) []

    let className className (f: Html list -> Html) (children: Html list): Html =
        let (element, attributes) = f children
        element, fun () -> { attributes () with ClassName = Some className }

    let onClick onClick (f: Html list -> Html) (children: Html list): Html =
        let (element, attributes) = f children
        element, fun () -> { attributes () with OnClick = Some onClick }

    let onChange (onChange: Browser.Types.Event -> Unit) (f: Element * (unit -> HtmlAttributes)): Html =
        let (element, attributes) = f
        element, fun () -> { attributes () with OnChange = Some onChange }

    type type' =
        static member password f =
            let (element, attributes) = f
            element, fun () -> { attributes () with Type = Some Password }
        static member text f =
            let (element, attributes) = f
            element, fun () -> { attributes () with Type = Some Text }