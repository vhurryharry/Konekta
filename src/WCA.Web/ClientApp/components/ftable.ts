import { autoinject, bindable } from 'aurelia-framework';
import * as $ from 'jquery';
import 'footable';
@autoinject
export class FTable {
    FTableElement: any;
    constructor(private element: Element) {
    }

    public attached()
    {
        (<any>$(".footable")).footable();
    }
}
