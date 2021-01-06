import { autoinject, bindable, View } from 'aurelia-framework';
import { AccountService, Roles } from '../services/account-service';
import { AppInfoService } from '../services/app-info-service';

@autoinject
export class NavTop {
    @bindable navigation: any;
    @bindable currenttitle: string;
    showAllPages: boolean = false;

    constructor(private accountService: AccountService,
        private appInfoService: AppInfoService) {
    }

    public created(owningView: View, myView: View) {
        this.accountService.updateLoggedInStatusFromServer();
        this.showAllPages = this.accountService.isUserInRole(Roles.GlobalAdministrator);
    }

    public async loginClick() {
        await this.accountService.updateLoggedInStatusFromServer();

        if (!this.accountService.userIsLoggedIn) {
            window.location.href = '/account/login';
        }
    }
}
