import { autoinject, bindable, bindingMode } from 'aurelia-framework';
import { DataService } from '../services/data-service';
import { Router } from 'aurelia-router';
import { InfoTrackCredentialsForOrgCommand } from './infotrack-credential-command';
import { Ibox } from '../components/ibox';
import * as toastr from 'toastr';
import { ValidationRules, ValidationControllerFactory, ValidationController, validateTrigger } from 'aurelia-validation';
import { IConnectedActionstepOrgsResponse } from '../services/wca-api-types';
import { AppInfoService } from '../services/app-info-service';

@autoinject
export class InfoTrackConnection {

    public ibox: Ibox;
    public redirectUrl: string;
    public fromRedirect: boolean = false;
    public userOrgs: IConnectedActionstepOrgsResponse[];

    private validationController: ValidationController = null;

    public infoTrackCredentialsForOrg: InfoTrackCredentialsForOrgCommand = new InfoTrackCredentialsForOrgCommand();

    constructor(
        private router: Router,
        private dataService: DataService,
        private validationControllerFactory: ValidationControllerFactory,
        private appInfoService: AppInfoService
    ) {
        this.validationController = validationControllerFactory.createForCurrentScope();
        this.validationController.validateTrigger = validateTrigger.changeOrBlur;

        ValidationRules
            .ensure((i: InfoTrackCredentialsForOrgCommand) => i.ActionstepOrgKey).required().withMessage('Organisation is required')
            .ensure('InfoTrackUsername').required().withMessage('Username is required')
            .ensure('InfoTrackPassword').required().withMessage('Password is required')
            .ensure((i: InfoTrackCredentialsForOrgCommand) => i.AcceptedTermsAndConditions)
            .required().withMessage('You need to accept terms and condition to proceed')
            .equals(true).withMessage('You need to accept terms and condition to proceed')
            .on(InfoTrackCredentialsForOrgCommand);

    }

    public async activate(params: any) {

        this.infoTrackCredentialsForOrg.ActionstepOrgKey = '';

        if (params.redirect) {
            this.redirectUrl = params.redirect;
            this.fromRedirect = true;

            if (params.orgkey) {
                this.infoTrackCredentialsForOrg.ActionstepOrgKey = params.orgkey;
                this.infoTrackCredentialsForOrg.IsSignUp = true;
            }
            else {
                toastr.error('No valid organisation key was found, please contact your administrator.')
            }

        } else {
            this.userOrgs = await this.dataService.getData('/integrations/actionsteporgs', null) as IConnectedActionstepOrgsResponse[];

            if (this.userOrgs.length === 0)
                toastr.error('No valid organisation found available for integration with InfoTrack')

        }
    }

    public async enable() {

        var result = await this.validationController.validate();
        if (!result.valid)
            return;

        this.ibox.toggleLoading(true);
        const response = await this.dataService.simplePost('integrations/infotrack/connect', this.infoTrackCredentialsForOrg);

        if (response.ok) {
            toastr.success('InfoTrack Integration Success!')

            if (this.redirectUrl) {
                this.router.navigate(this.redirectUrl);
            } else {
                this.router.navigate('integrations');
            }


        } else {
            response.json().then(function (message) {
                toastr.error(message);
            });

            this.ibox.toggleLoading(false);
        }

    }

    public async connectToActionstepOrg() {
        window.location.href = '/integrations/actionstep/connect?returnUrl=/wca/integrations/infotrack-connection';
    }
}