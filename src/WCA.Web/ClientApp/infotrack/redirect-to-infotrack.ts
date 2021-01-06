import { DialogService } from 'aurelia-dialog';
import { autoinject } from 'aurelia-framework';
import { RouteConfig } from 'aurelia-router';
import { Router } from 'aurelia-router';
import { DataService } from '../services/data-service';
import { Tools } from '../tools/tools';
import { ActionstepConnectionModal } from '../integrations/components/actionstep-connection-modal';
import { AppInfoService } from '../services/app-info-service';

@autoinject
export class RedirectToInfotrack {
    public errorState: boolean;
    public errorMessage: string;
    public matterInfo: any; // ActionstepMatterInfo;
    public paramsWithKeysInLowerCase: any;

    constructor(
        private dataService: DataService,
        private dialogService: DialogService,
        private router: Router,
        private appInfoService: AppInfoService
    ) { }

    public activate(params: any, routeConfig: RouteConfig) {
        this.GetQueryStrings(params);
    }

    public GetQueryStrings(params: any) {
        // List of supported parameters
        // Note: * denotes default value. Defaults can be omitted
        //       parameter names are case sensitive, values are NOT case sensitive
        // - matterid: number e.g: 1, 2,3
        // - actionsteporg: string, e.g: wcamaster, btrcloud
        //
        // Example: ?matterId=1&actionstepOrg=wcamaster

        this.paramsWithKeysInLowerCase = [params.length];
        for (const param in params) {
            if (params.hasOwnProperty(param)) {
                this.paramsWithKeysInLowerCase[param.toLowerCase()] = params[param];
            }
        }
    }

    public attached() {
        const matterId = Tools.ParseNumberPropertyValue(
            this.paramsWithKeysInLowerCase,
            `matterId`.toLowerCase())

        const actionstepOrg = Tools.ParseStringPropertyValue(
            this.paramsWithKeysInLowerCase,
            `actionstepOrg`.toLowerCase())

        const resolvableEntryPoint = Tools.ParseStringPropertyValue(
            this.paramsWithKeysInLowerCase,
            `resolvableEntryPoint`.toLowerCase())

        if (matterId && actionstepOrg) {
            this.matterInfo = {
                matterId: matterId,
                orgKey: actionstepOrg,
                resolvableEntryPoint: resolvableEntryPoint,
            };
            this.AddMatterToInfoTrack();
        } else {
            // TODO: because this is a location known to the SPA (Aurelie),
            // this should use the router instead of a location.href.
            // location.href should be reserved for URLs outside of the context
            // of the SPA (Aurelia app).
            location.href = '../wca';
        }
    }

    public async AddMatterToInfoTrack() {
        const response = await this.dataService
            .simplePost('/infotrack/services/getinfotrackurlwithmatterinfo', this.matterInfo);

        const responseData = await response.json();

        if (response.ok) {
            window.location.href = responseData.url;
        } else if (response.status === 401) {
            const authenticateHeaderValue = response.headers.get("WWW-Authenticate");
            if (authenticateHeaderValue && authenticateHeaderValue.startsWith("ActionstepConnection OrgKey=")) {
                this.dialogService.open(
                    {
                        viewModel: ActionstepConnectionModal
                    }
                );
            } else if (authenticateHeaderValue && authenticateHeaderValue.startsWith("InfoTrackConnection OrgKey=")) {
                this.router.navigate('integrations/infotrack-connection');
            } else {
                this.errorMessage = responseData.message + ' Please contact ' +
                    '<a href="mailto:support@' + this.appInfoService.domainUrl + '?subject=Set%20up%20InfoTrack">' +
                    'support@' + this.appInfoService.domainUrl + '</a> and we will set it up for you.';
                this.errorState = true;
            }
        } else {
            if (responseData.message.length > 0) {
                this.errorMessage = responseData.message;
            } else {
                this.errorMessage = 'Oops! We\'re sorry, there was a problem passing your property information to InfoTrack. Please try again.';
            }

            this.errorState = true;
        }
    }
}
