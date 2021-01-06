import { DialogService } from 'aurelia-dialog';
import { DialogController } from 'aurelia-dialog';
import { autoinject, bindable, bindingMode, } from 'aurelia-framework';
import { DataService } from '../../services/data-service';
import { Router } from 'aurelia-router';
import { Tools } from '../../tools/tools';

@autoinject
export class ActionstepConnectionModal {

    private returnUrl: string;

    constructor(private controller: DialogController,
        public dialogService: DialogService,
        private router: Router) {
        this.controller.settings.centerHorizontalOnly = true;
        this.controller.settings.keyboard = true;
    }

    public async activate(data: any) {
        if (data)
            this.returnUrl = data.returnUrl;
    }

    public ConnectToActionstep() {
        localStorage.setItem('one-time-nav-intercept', this.router.currentInstruction.getWildCardName() + this.router.currentInstruction.getWildcardPath());

        Tools.PopupConnectToActionstep(() => {
            this.controller.ok();
        });
    }
}
