import { autoinject } from 'aurelia-framework';
import { DataService } from './data-service';
import { UISettings } from '../services/wca-api-types';

@autoinject
export class CommonInfoService {
    public uiDefinitions: UISettings;

    constructor(private dataService: DataService) {
        this.updateFromServer();
    }

    public async updateFromServer() {
        this.uiDefinitions = (await this.dataService.getData('/uidefinitions', null) as UISettings);
    }
}
