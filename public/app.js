import { MatchFailureException, Record, Union } from "./.fable/fable-library.3.1.15/Types.js";
import { class_type, obj_type, record_type, list_type, lambda_type, unit_type, option_type, union_type, string_type } from "./.fable/fable-library.3.1.15/Reflection.js";
import { iterate, singleton, isEmpty, empty } from "./.fable/fable-library.3.1.15/List.js";
import { createAtom } from "./.fable/fable-library.3.1.15/Util.js";
import { empty as empty_1 } from "./.fable/fable-library.3.1.15/Map.js";
import { empty as empty_2, singleton as singleton_1, append, delay, toList } from "./.fable/fable-library.3.1.15/Seq.js";
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
    return record_type("Freact.Freact.HtmlAttributes", [], HtmlAttributes, () => [["ClassName", option_type(string_type)], ["OnClick", option_type(lambda_type(unit_type, unit_type))], ["Children", list_type(Html$reflection())]]);
}

export class Html extends Union {
    constructor(tag, ...fields) {
        super();
        this.tag = (tag | 0);
        this.fields = fields;
    }
    cases() {
        return ["RegularHtml", "Component"];
    }
}

export function Html$reflection() {
    return union_type("Freact.Freact.Html", [], Html, () => [[["Item1", HtmlTag$reflection()], ["Item2", HtmlAttributes$reflection()]], [["Item1", lambda_type(list_type(obj_type), Html$reflection())], ["Item2", lambda_type(unit_type, Html$reflection())]]]);
}

export function op_LessDivideGreater(comp, props) {
    return new Html(1, comp, () => comp(props));
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
    const tupledArg = [new HtmlTag(0), new HtmlAttributes(void 0, void 0, children)];
    return new Html(0, tupledArg[0], tupledArg[1]);
}

export function str(str_1) {
    const tupledArg = [new HtmlTag(1, str_1), new HtmlAttributes(void 0, void 0, empty())];
    return new Html(0, tupledArg[0], tupledArg[1]);
}

export function className(className_1, f, children) {
    const matchValue = f(children);
    if (matchValue.tag === 0) {
        const tag = matchValue.fields[0];
        const rec$0027 = matchValue.fields[1];
        const tupledArg = [tag, new HtmlAttributes(className_1, rec$0027.OnClick, rec$0027.Children)];
        return new Html(0, tupledArg[0], tupledArg[1]);
    }
    else {
        const a = matchValue;
        return a;
    }
}

export function onClick(onClick_1, f, children) {
    const matchValue = f(children);
    if (matchValue.tag === 0) {
        const tag = matchValue.fields[0];
        const rec$0027 = matchValue.fields[1];
        const tupledArg = [tag, new HtmlAttributes(rec$0027.ClassName, onClick_1, rec$0027.Children)];
        return new Html(0, tupledArg[0], tupledArg[1]);
    }
    else {
        const a = matchValue;
        return a;
    }
}

export let state = createAtom(empty_1());

export function useState(s) {
    let state_1 = s;
    return [s, (s$0027) => {
        state_1 = s$0027;
    }];
}

export function Kom(_arg1) {
    if (isEmpty(_arg1)) {
        return div(singleton(str("Chicks")));
    }
    else {
        throw (new Error("Match failure"));
    }
}

export function MyView(_arg1) {
    if (isEmpty(_arg1)) {
        const patternInput = useState(0);
        const setN = patternInput[1];
        const n = patternInput[0] | 0;
        return onClick(() => {
            setN(n + 1);
        }, (children_1) => className("lol", (children) => div(children), children_1), toList(delay(() => append(singleton_1(className("Duck", (children_3) => div(children_3), singleton(str("Carts")))), delay(() => append((n > 3) ? singleton_1(op_LessDivideGreater((arg00$0040) => Kom(arg00$0040), empty())) : empty_2(), delay(() => append(empty_2(), delay(() => append(singleton_1(onClick(() => {
            const value = toConsole(printf("you"));
        }, (children_5) => div(children_5), singleton(str("Sticks")))), delay(() => singleton_1(div(singleton(str(toText(printf("%A"))(n))))))))))))))));
    }
    else {
        throw (new Error("Match failure"));
    }
}

export const rootContainer = document.getElementById("app");

export function render(element_mut, container_mut) {
    render:
    while (true) {
        const element = element_mut, container = container_mut;
        if (element.tag === 1) {
            const html = element.fields[1];
            const f = element.fields[0];
            element_mut = html();
            container_mut = container;
            continue render;
        }
        else {
            const tag = element.fields[0];
            const attributes = element.fields[1];
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
                        render(op_LessDivideGreater((arg00$0040) => MyView(arg00$0040), empty()), rootContainer);
                    });
                }
                iterate((x) => {
                    render(x, e_1);
                }, attributes.Children);
                void container.appendChild(e_1);
            }
            else {
                throw (new MatchFailureException("/Users/bjornivarstrom/Documents/functional/Felm/app.fs", 109, 18));
            }
        }
        break;
    }
}

render(op_LessDivideGreater((arg00$0040) => MyView(arg00$0040), empty()), rootContainer);

