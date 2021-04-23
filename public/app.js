import { Record, Union } from "./.fable/fable-library.3.1.15/Types.js";
import { name, getUnionFields, class_type, record_type, list_type, tuple_type, lambda_type, unit_type, option_type, union_type, string_type } from "./.fable/fable-library.3.1.15/Reflection.js";
import { iterate, ofArray, singleton, empty } from "./.fable/fable-library.3.1.15/List.js";
import { toText, printf, toConsole } from "./.fable/fable-library.3.1.15/String.js";
import { value as value_2 } from "./.fable/fable-library.3.1.15/Option.js";

export class Element$ extends Union {
    constructor(tag, ...fields) {
        super();
        this.tag = (tag | 0);
        this.fields = fields;
    }
    cases() {
        return ["Div", "Str"];
    }
}

export function Element$$reflection() {
    return union_type("Freact.Freact.Element", [], Element$, () => [[], [["Item", string_type]]]);
}

export class HtmlAttributes extends Record {
    constructor(ClassName, OnClick, Children) {
        super();
        this.ClassName = ClassName;
        this.OnClick = OnClick;
        this.Children = Children;
    }
}

export function HtmlAttributes$reflection() {
    return record_type("Freact.Freact.HtmlAttributes", [], HtmlAttributes, () => [["ClassName", option_type(string_type)], ["OnClick", option_type(lambda_type(unit_type, unit_type))], ["Children", list_type(tuple_type(Element$$reflection(), HtmlAttributes$reflection()))]]);
}

export function createNode(element, children) {
    return [element, new HtmlAttributes(void 0, void 0, children)];
}

export class DomNode extends Union {
    constructor(tag, ...fields) {
        super();
        this.tag = (tag | 0);
        this.fields = fields;
    }
    cases() {
        return ["TextNode", "ElementNode"];
    }
}

export function DomNode$reflection() {
    return union_type("Freact.Freact.DomNode", [], DomNode, () => [[["Item", class_type("Browser.Types.Text")]], [["Item", class_type("Browser.Types.HTMLElement")]]]);
}

export function div(children) {
    return createNode(new Element$(0), children);
}

export function str(str_1) {
    return createNode(new Element$(1, str_1), empty());
}

export function className(className_1, f, children) {
    const patternInput = f(children);
    const element = patternInput[0];
    const attributes = patternInput[1];
    return [element, new HtmlAttributes(className_1, attributes.OnClick, attributes.Children)];
}

export function onClick(onClick_1, f, children) {
    const patternInput = f(children);
    const element = patternInput[0];
    const attributes = patternInput[1];
    return [element, new HtmlAttributes(attributes.ClassName, onClick_1, attributes.Children)];
}

export function useState(s) {
    let state = s;
    return [s, (s$0027) => {
        state = s$0027;
    }];
}

export const myView = (() => {
    const patternInput = useState(0);
    const setN = patternInput[1];
    const n = patternInput[0] | 0;
    return onClick(() => {
        setN(n + 1);
    }, (children_1) => className("lol", (children) => div(children), children_1), ofArray([className("Duck", (children_3) => div(children_3), singleton(str("Carts"))), onClick(() => {
        const value = toConsole(printf("you"));
    }, (children_5) => div(children_5), singleton(str("Sticks"))), div(singleton(str(toText(printf("%A"))(n))))]));
})();

export const rootContainer = document.getElementById("app");

export function render(element_0, element_1, container) {
    let case$;
    const element = [element_0, element_1];
    const element_2 = element[0];
    const attributes = element[1];
    let newDomElement;
    if (element_2.tag === 1) {
        const s = element_2.fields[0];
        newDomElement = (new DomNode(0, document.createTextNode(s)));
    }
    else {
        newDomElement = (new DomNode(1, document.createElement((case$ = getUnionFields(element_2, Element$$reflection())[0], name(case$)))));
    }
    if (newDomElement.tag === 1) {
        const e = newDomElement.fields[0];
        if (attributes.OnClick != null) {
            e.addEventListener("click", (_arg1) => {
                value_2(attributes.OnClick)();
                rootContainer.innerHTML = "";
                render(myView[0], myView[1], rootContainer);
            });
        }
        iterate((x_1) => {
            render(x_1[0], x_1[1], e);
        }, attributes.Children);
        void container.appendChild(e);
    }
    else {
        const t = newDomElement.fields[0];
        void container.appendChild(t);
    }
}

render(myView[0], myView[1], rootContainer);

