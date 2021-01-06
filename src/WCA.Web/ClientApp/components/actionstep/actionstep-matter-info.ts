import { autoinject } from 'aurelia-framework';
import { ActionstepMatterInfo as ActionstepMatterInfoFromServer } from '../../services/wca-api-types';
import { EventAggregator } from 'aurelia-event-aggregator';

@autoinject
export class ActionstepMatterInfo {

    matterInfo: ActionstepMatterInfoFromServer;

    constructor(
        private eventAggregator: EventAggregator) { }

    public async attached() {
        this.eventAggregator.subscribe('matterInfoAvailable', (matterInfo) => {
            this.matterInfo = matterInfo;
        });
    }
}
