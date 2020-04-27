import { autoinject, View } from 'aurelia-framework';
import { AccountService } from './../services/account-service';

@autoinject
export class MyProfile {
    public firstName: string;
    public lastName: string;
    public email: string;

    constructor(private accountService: AccountService) {
    }

    public async created(owningView: View, myView: View) {
        // TODO
    }
}
