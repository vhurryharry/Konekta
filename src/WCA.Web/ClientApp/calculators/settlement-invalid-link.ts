import { autoinject } from 'aurelia-framework'
import { AppInfoService } from '../services/app-info-service';

@autoinject
export class SettlementCalculatorInvalidLink {
    constructor(private appInfoService: AppInfoService) { }
}