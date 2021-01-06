import { autoinject } from 'aurelia-framework';
import { AppInfoService } from '../services/app-info-service';

@autoinject
export class NavTopRightMenu {
    constructor(private appInfoService: AppInfoService) {
    }

    public created() {
    }
}
