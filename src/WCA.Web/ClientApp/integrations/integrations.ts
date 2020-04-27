import { DialogService } from 'aurelia-dialog';
import { autoinject, View } from 'aurelia-framework';
import { Ibox } from '../components/ibox';
import { Router } from 'aurelia-router';
import { AccountService } from '../services/account-service';
import { DataService } from '../services/data-service';
import {
    IConnectedActionstepOrgsResponse,
    IListInfoTrackCredentialResponse
} from '../services/wca-api-types';
import * as toastr from 'toastr';
import { AppInfoService } from '../services/app-info-service';
import { Tools } from '../tools/tools';

@autoinject
export class Integrations {

    public actionstepOrgs: IConnectedActionstepOrgsResponse[];
    public infoTrackConnections: IListInfoTrackCredentialResponse[];
    public message: any;
    public ibox1: Ibox;
    public actionsteplogo: string = 'actionstep';
    public infoTrackConnectionsLoaded: boolean = false;

    constructor(
        private accountService: AccountService,
        private dialogService: DialogService,
        private dataService: DataService,
        private router: Router,
        private element: Element,
        private appInfoService: AppInfoService) {
    }

    public async connectToActionstepOrg() {
        Tools.PopupConnectToActionstep(() => {
            toastr.success('Connected Successfully');
            this.getActionstepOrgs();
            this.getInfoTrackConnections();
        });
    }

    public async connectToInfoTrack() {
        this.router.navigate('integrations/infotrack-connection')
    }

    public async created(owningView: View, myView: View) {
        this.getActionstepOrgs();
        this.getInfoTrackConnections();
    }

    public async refreshOrg(actionstepCredential) {
        try {
            actionstepCredential.refreshInProgress = true;
            let response = await this.dataService.simplePost('integrations/actionstep/refresh-credentials',
                {
                    ActionstepCredentialIdToRefresh: actionstepCredential.actionstepCredentialId,
                    ForceRefreshIfNotExpired: true
                });

            const authenticateHeaderValue = response.headers.get("WWW-Authenticate");

            if (response.ok) {
                actionstepCredential.refreshInProgress = false;
                this.getActionstepOrgs();
                toastr.success(`Actionstep connection has been refreshed.`);
            } else if (authenticateHeaderValue && authenticateHeaderValue.startsWith("ActionstepConnection OrgKey=")) {
                window.location.href = '/integrations/actionstep/connect';
            } else {
                toastr.error("Oops! Sorry, we couldn't refresh this connection. Please click 'Connect' instead.");
            }
        } catch (error) {

            toastr.error("Oops! Sorry, we couldn't refresh this connection. Please click 'Connect' instead.");
        }
    }

    public async disconnectFromOrg(actionstepOrg) {
        try {
            actionstepOrg.disconnectInProgress = true;
            let response = await this.dataService.simplePost('integrations/actionstep/disconnect',
                { actionstepOrgKey: actionstepOrg.actionstepOrgKey });
            if (response.ok) {
                this.getActionstepOrgs();
                toastr.success(`Actionstep organisation ${actionstepOrg.actionstepOrgKey} has been disconnected.`);
            }
        } catch (error) {
            var responseBody = await error.json();
            toastr.error(responseBody.message);
        }
    }

    private async getActionstepOrgs() {
        this.actionstepOrgs = await this.dataService
            .getData('/integrations/actionsteporgs', null) as IConnectedActionstepOrgsResponse[];
    }

    private async getInfoTrackConnections() {
        this.infoTrackConnections = await this.dataService.getData('/integrations/infotrack', null) as IListInfoTrackCredentialResponse[];
        this.infoTrackConnectionsLoaded = true;
    }

}