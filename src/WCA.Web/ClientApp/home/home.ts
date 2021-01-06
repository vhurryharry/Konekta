import { autoinject, computedFrom, View } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import { DialogService } from 'aurelia-dialog';
import { AccountService, Roles } from '../services/account-service';
import { Tools } from '../tools/tools';
import { CommonInfoService } from '../services/common-info-service';
import { AppInfoService } from '../services/app-info-service';
import { DataService } from '../services/data-service';
import { StampDutyCalculatorInfo } from '../services/wca-api-types';
import { ActionstepConnectionModal } from '../integrations/components/actionstep-connection-modal';
import * as toastr from 'toastr';
import { ActionstepMatterInfo as ActionstepMatterInfoFromServer } from '../services/wca-api-types';
import { EventAggregator } from 'aurelia-event-aggregator';
import { ActionstepCredentialsResponse } from '../integrations/actionstep-credentials';

@autoinject
export class Dashboard {

    private oldSettlmentCalculatorButtonDefaultText = 'Settlement Calculator';
    private stampDutyButtonDefaultText = 'Stamp Duty Calculator';
    private loadingText = 'Loading...';

    public loggedIn: boolean;
    public showPexa: boolean = false;
    public actionstepOrg: string;
    public navigateToOrgKey: string;
    public matterId: number;
    public navigateToMatterId: number;
    public matterName: string;
    public actionstepUrl: string;
    public oldSettlmentCalculatorButtonText: string = this.oldSettlmentCalculatorButtonDefaultText;
    public stampDutyButtonText: string = this.stampDutyButtonDefaultText;
    public matterInfoLoaded: boolean = false;
    public credentials: ActionstepCredentialsResponse[];

    constructor(
        private accountService: AccountService,
        private router: Router,
        private commonInfoService: CommonInfoService,
        private appInfoService: AppInfoService,
        private dataService: DataService,
        private dialogService: DialogService,
        private eventAggregator: EventAggregator
    ) { }

    @computedFrom('actionstepOrg', 'matterId')
    get matterInfoAvailable(): boolean {
        return this.actionstepOrg
            && this.actionstepOrg.length > 0
            && this.matterId > 0;
    }

    @computedFrom('oldSettlmentCalculatorButtonText')
    get oldSettlmentCalculatorButtonLoading(): boolean {
        return this.oldSettlmentCalculatorButtonText === this.loadingText;
    }

    @computedFrom('stampDutyButtonText')
    get stampDutyButtonLoading(): boolean {
        return this.stampDutyButtonText === this.loadingText;
    }

    public navigateRoute(route: string) {
        this.router.navigate(route);
    }

    public navigateUrl(url: string) {
        window.location.href = url;
    }

    public async selectMatter() {
        this.actionstepOrg = this.navigateToOrgKey;
        this.matterId = this.navigateToMatterId;
        if (this.matterInfoAvailable) {
            this.updateMatterInfo();
        }
    }

    public async navigateStampDutyCalculator() {
        this.stampDutyButtonText = this.loadingText;

        if (this.matterInfoAvailable) {
            const requestUrl = `/conveyancing/stamp-duty-calculator-data/${this.actionstepOrg}/${this.matterId}`;
            const response = await this.dataService.simpleGet(requestUrl);
            const responseData: StampDutyCalculatorInfo = await response.json();
            let showError = true;

            if (response.ok) {
                this.router.navigateToRoute('stampDutyCalculator', responseData);
                showError = false;
            } else if (response.status === 401) {
                const authenticateHeaderValue = response.headers.get("WWW-Authenticate");
                if (authenticateHeaderValue && authenticateHeaderValue.startsWith("ActionstepConnection OrgKey=")) {
                    showError = false;

                    await this.dialogService.open({ viewModel: ActionstepConnectionModal })
                        .whenClosed(response => {
                            if (!response.wasCancelled) {
                                toastr.success('Connected Successfully');
                                this.navigateStampDutyCalculator();
                            }
                        });
                }
            }

            if (showError) {
                toastr.error('Sorry, something went wrong. Please contact ' +
                    '<a href="mailto:support@' + this.appInfoService.domainUrl + '?subject=Legacy%20Settlement%20Calculator">' +
                    'support@' + this.appInfoService.domainUrl + '</a>.');
            }
        } else {
            this.router.navigateToRoute('stampDutyCalculator');
        }

        this.stampDutyButtonText = this.stampDutyButtonDefaultText;
    }

    private async navigateSettlementCalculator() {
        window.location.href = `/wca/calculators/settlement?actionstepOrg=${this.actionstepOrg}&matterId=${this.matterId}`;
    }

    public async created(owningView: View, myView: View) {
        this.showPexa = this.accountService.isUserInRole(Roles.BetaTester);
        this.actionstepUrl = this.commonInfoService.uiDefinitions.backToActionstepURL;
    }

    public async activate(params: any) {
        // First check URL
        const paramsWithKeysInLowerCase = Tools.LowerCaseParams(params);
        this.actionstepOrg = Tools.ParseStringPropertyValue(paramsWithKeysInLowerCase, 'actionsteporg');
        this.matterId = Tools.ParseNumberPropertyValue(paramsWithKeysInLowerCase, 'matterid');

        if (this.matterInfoAvailable) {
            // If we managed to get an orgkey/matter, get its details
            this.updateMatterInfo();
        } else {
            // No orgkey/matter, so get AS credentials to provide a list of orgs for the user
            this.credentials = await this.dataService.getData('/integrations/actionstep/credentials', null) as ActionstepCredentialsResponse[];
        }
    }

    private async updateMatterInfo() {
        var response = await this.dataService.simpleGet(`/actionstep/matter/${this.actionstepOrg}/${this.matterId}`)
        if (response.status == 200) {
            var matterInfo = await response.json()

            matterInfo = matterInfo as ActionstepMatterInfoFromServer;
            this.eventAggregator.publish('matterInfoAvailable', matterInfo);
            this.matterName = matterInfo.matterName;

            this.matterInfoLoaded = true;
        } else if (response.status == 401) {
            this.matterName = "Matter ID";
            this.matterInfoLoaded = true;

            const authenticateHeaderValue = response.headers.get("WWW-Authenticate");
            if (authenticateHeaderValue && authenticateHeaderValue.startsWith("ActionstepConnection OrgKey=")) {
                await this.dialogService.open({ viewModel: ActionstepConnectionModal })
                    .whenClosed(response => {
                        if (!response.wasCancelled) {
                            toastr.success('Connected Successfully');
                            this.updateMatterInfo();
                        }
                    });
            }
        } else {
            toastr.error('An unexpected server problem was encountered, please try again later or log a ticket '
                + '<a href="' + this.appInfoService.supportUrl + '/support/tickets/new" target="_blank"><u>here</u></a>');
        }
    }
}
