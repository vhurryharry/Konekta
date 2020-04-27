import { autoinject } from 'aurelia-framework'
import { AppInfoService } from '../services/app-info-service';

@autoinject
export class SettlementCalculator {
    public reactProps;

    constructor(private appInfoService: AppInfoService) {
        this.reactProps = {
            appInfoService
        };
    }
}