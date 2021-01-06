import { DialogController } from 'aurelia-dialog';
import { autoinject, bindable, bindingMode } from 'aurelia-framework';

import * as $ from 'jquery';
import 'select2';

@autoinject
export class MultiselectCustomElement {
    @bindable({ defaultBindingMode: bindingMode.twoWay, changeHandler: 'selectedIsChanged',  }) selected: any = null; 
    @bindable({ defaultBindingMode: bindingMode.oneWay }) options: MultiSelectOption[] = null;
    @bindable({ defaultBindingMode: bindingMode.twoWay}) customHandler;

    public changedValue: any;
    public select2 = null;

    constructor(private element: Element) {}

    public async attached(params: any) {
        this.select2 = (<any>$(this.element).find('select')).select2();
        this.select2.on('change', evt => {
            this.selected = $(this.select2).val();
        });
    }

    public selectedIsChanged($event) {
        if (this.customHandler) this.customHandler($event);
    }

}

export interface MultiSelectOption {
    value: string;
    label: string;
}
