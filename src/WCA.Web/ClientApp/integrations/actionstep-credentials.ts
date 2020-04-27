import { autoinject, bindable, bindingMode } from 'aurelia-framework';
import { RefreshActionstepTokenResponseViewModel } from '../services/wca-api-types';
import { DataService } from '../services/data-service';
import * as toastr from 'toastr';

@autoinject
export class ActionstepCredentials {

    @bindable({ defaultBindingMode: bindingMode.oneWay }) returnUrl;
    public credentials: ActionstepCredentialsResponse[];
    public expiredAccessExist: boolean = false;


    constructor(private dataService: DataService) {}

    public async attached(params: any) {
        this.credentials = await this.dataService.getData('/integrations/actionstep/credentials', null) as ActionstepCredentialsResponse[];
        if (this.credentials.find(c => c.status == 'Expired'))
            this.expiredAccessExist = true;
    }

    public async connect() {
        window.location.href = `/integrations/actionstep/connect?returnUrl=${this.returnUrl}`;
    }

    public async refresh(credential: ActionstepCredentialsResponse) {
        if (credential.status == 'Active') {
            var refreshActionstepCredential = new RefreshActionstepCredentials(credential.id, true);
            var response: RefreshActionstepTokenResponseViewModel = await this.dataService.postData('/integrations/actionstep/refresh-credentials', refreshActionstepCredential, true, null);
            credential.expiration = response.refreshTokenExpiry;
            toastr.success('Connection successfully refreshed')
        }
        else {
            window.location.href = `/integrations/actionstep/connect?returnUrl=${this.returnUrl}`;        
        }
        
    }

}

export class ActionstepCredentialsResponse {
    id: number;
    key: string;   
    status: string;
    expiration: Date;
}

export class RefreshActionstepCredentials {
    ActionstepCredentialIdToRefresh: number;
    ForceRefreshIfNotExpired: boolean;

    constructor(id, forceRefresh) {
        this.ActionstepCredentialIdToRefresh = id;
        this.ForceRefreshIfNotExpired = forceRefresh;
    }
}