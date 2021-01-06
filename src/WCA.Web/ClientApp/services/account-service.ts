import { DialogService } from 'aurelia-dialog';
import { autoinject } from 'aurelia-framework';
import { DataService } from './data-service';
import { AccountModel } from './wca-api-types';

@autoinject
export class AccountService {
    public userIsLoggedIn: boolean = false;
    public roles: string[] = [];
    public loggedInUserDisplayName: string;

    constructor(private dataService: DataService,
        private dialogService: DialogService) {
    }

    public async updateLoggedInStatusFromServer() {
        const that = this;

        const currentUserDetails = await this.getCurrentUser() as AccountModel;
        that.userIsLoggedIn = currentUserDetails.isLoggedIn;
        that.roles = currentUserDetails.roles;
        that.loggedInUserDisplayName = currentUserDetails.firstName + ' ' + currentUserDetails.lastName;
    }

    public isUserInRole(role: string) {
        return this.roles.includes(role);
    }

    private reset() {
        this.loggedInUserDisplayName = '';
        this.userIsLoggedIn = false;
    }

    public getCurrentUser() {
        return this.dataService.getData('/account/currentUser', null);
    }
}

export class Roles {
    static GlobalAdministrator = 'GlobalAdministrator';
    static AlphaTester = 'AlphaTester';
    static BetaTester = 'BetaTester';
    static AllowedToHavePassword = 'AllowedToHavePassword';
}