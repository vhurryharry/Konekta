import { autoinject } from 'aurelia-framework';
import { CommonInfoService } from '../services/common-info-service';
import { EventAggregator } from 'aurelia-event-aggregator';
import { ActionstepMatterInfo } from '../services/wca-api-types';

@autoinject
export class NavTopMenu {
    public actionstepurl: string;
    matterInfo: ActionstepMatterInfo;

    constructor(
        private commonInfoService: CommonInfoService,
        private eventAggregator: EventAggregator) {
    }

    public created() {
        this.actionstepurl = (this.commonInfoService &&
            this.commonInfoService.uiDefinitions &&
            this.commonInfoService.uiDefinitions.backToActionstepURL)
            ? this.commonInfoService.uiDefinitions.backToActionstepURL
            : '';

        this.eventAggregator.subscribe('matterInfoAvaialble', (matterInfo) => {
            this.matterInfo = matterInfo;
        });
    }
}
