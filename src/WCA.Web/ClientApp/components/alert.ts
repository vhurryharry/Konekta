import { DialogController } from 'aurelia-dialog';
import { autoinject } from 'aurelia-framework';
@autoinject
export class Alert {
    public message: ErrorMessage;

    constructor(private controller: DialogController) {
        this.controller = controller;
        controller.settings.centerHorizontalOnly = true;
        controller.settings.keyboard = true;
    }

    public activate(message) {
        this.message = message;
    }
}

// tslint:disable-next-line:max-classes-per-file
export class ErrorMessage {
    public title: string;
    public message: string;
    public errorList: string[];
}
