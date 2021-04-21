import { MatchFailureException, Record, Union } from "./.fable/fable-library.3.1.15/Types.js";
import { class_type, record_type, list_type, tuple_type, lambda_type, unit_type, option_type, union_type, string_type } from "./.fable/fable-library.3.1.15/Reflection.js";
import { iterate, ofArray, singleton, empty } from "./.fable/fable-library.3.1.15/List.js";
import { toText, printf, toConsole } from "./.fable/fable-library.3.1.15/String.js";
import { value as value_2 } from "./.fable/fable-library.3.1.15/Option.js";

export class HtmlTag extends Union {
    constructor(tag, ...fields) {
        super();
        this.tag = (tag | 0);
        this.fields = fields;
    }
    cases() {
        return ["Div", "Str"];
    }
}

export function HtmlTag$reflection() {
    return union_type("Freact.Freact.HtmlTag", [], HtmlTag, () => [[], [["Item", string_type]]]);
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
    return record_type("Freact.Freact.HtmlAttributes", [], HtmlAttributes, () => [["ClassName", option_type(string_type)], ["OnClick", option_type(lambda_type(unit_type, unit_type))], ["Children", list_type(tuple_type(HtmlTag$reflection(), HtmlAttributes$reflection()))]]);
}

export class TextNode extends Union {
    constructor(tag, ...fields) {
        super();
        this.tag = (tag | 0);
        this.fields = fields;
    }
    cases() {
        return ["TextNode"];
    }
}

export function TextNode$reflection() {
    return union_type("Freact.Freact.TextNode", [], TextNode, () => [[["Item", class_type("Browser.Types.Text")]]]);
}

function unboxTextNode(_arg1) {
    const t = _arg1.fields[0];
    return t;
}

export class ElementNode extends Union {
    constructor(tag, ...fields) {
        super();
        this.tag = (tag | 0);
        this.fields = fields;
    }
    cases() {
        return ["ElementNode"];
    }
}

export function ElementNode$reflection() {
    return union_type("Freact.Freact.ElementNode", [], ElementNode, () => [[["Item", class_type("Browser.Types.HTMLElement")]]]);
}

function unboxElementNode(_arg1) {
    const e = _arg1.fields[0];
    return e;
}

export function div(children) {
    return [new HtmlTag(0), new HtmlAttributes(void 0, void 0, children)];
}

export function str(str_1) {
    return [new HtmlTag(1, str_1), new HtmlAttributes(void 0, void 0, empty())];
}

export function className(className_1, f, children) {
    const patternInput = f(children);
    const tag = patternInput[0];
    const rec$0027 = patternInput[1];
    return [tag, new HtmlAttributes(className_1, rec$0027.OnClick, rec$0027.Children)];
}

export function onClick(onClick_1, f, children) {
    const patternInput = f(children);
    const tag = patternInput[0];
    const rec$0027 = patternInput[1];
    return [tag, new HtmlAttributes(rec$0027.ClassName, onClick_1, rec$0027.Children)];
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
    const element = [element_0, element_1];
    const tag = element[0];
    const attributes = element[1];
    let newDomElement;
    if (tag.tag === 0) {
        newDomElement = (new ElementNode(0, document.createElement("div")));
    }
    else {
        const s = tag.fields[0];
        newDomElement = (new TextNode(0, document.createTextNode(s)));
    }
    if (newDomElement instanceof TextNode) {
        const t = newDomElement;
        void container.appendChild(unboxTextNode(t));
    }
    else if (newDomElement instanceof ElementNode) {
        const e = newDomElement;
        const e_1 = unboxElementNode(e);
        if (attributes.OnClick != null) {
            e_1.addEventListener("click", (_arg1) => {
                value_2(attributes.OnClick)();
                rootContainer.innerHTML = "";
                render(myView[0], myView[1], rootContainer);
            });
        }
        iterate((x) => {
            render(x[0], x[1], e_1);
        }, attributes.Children);
        void container.appendChild(e_1);
    }
    else {
        throw (new MatchFailureException("/Users/bjornivarstrom/Documents/functional/Felm/app.fs", 85, 14));
    }
}

render(myView[0], myView[1], rootContainer);

