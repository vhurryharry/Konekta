import { autoinject, View } from 'aurelia-framework';
import { DataService } from '../services/data-service';
import * as toastr from 'toastr';
import { AppInfoService } from '../services/app-info-service';

@autoinject
export class IntegrationsHealthCheck {

    public pageLoaded: boolean = false;

    constructor(private dataService: DataService,
        private appInfoService: AppInfoService) { }

    public async created() {
        this.getActionTypeObjects();
    }

    public async updateActionType() {
    }

    private async getActionTypeObjects() {
        var response = await this.dataService.getData('/integrations/actionstep/integrations-tab/list-status', null);
        this.pageLoaded = true;
    }
}