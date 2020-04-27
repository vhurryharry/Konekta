import { DialogController } from 'aurelia-dialog';
import { autoinject, bindable, bindingMode } from 'aurelia-framework';

@autoinject
export class TagCustomElement {
    @bindable({ defaultBindingMode: bindingMode.twoWay }) message;
}
