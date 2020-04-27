import { autoinject, bindable } from 'aurelia-framework';
import * as $ from 'jquery';
import { AppInfoService } from '../services/app-info-service';

@autoinject
export class Ibox {
    @bindable public closed: boolean = false;
    @bindable public collapsable: boolean = true;
    @bindable public imagesource: string;
    private collapsed: boolean = false;

    constructor(private element: Element,
        private appInfoService: AppInfoService) {
    }

    public attached() {
        if (this.closed) {
            this.toggleCollapse();
        }
    }

    public toggleCollapse(): void {
        if (this.collapsable) {
            this.collapsed = !this.collapsed;

            const ibox = $(this.element);
            const content = ibox.find('.ibox-content');
            content.slideToggle(200);
            ibox.toggleClass('').toggleClass('border-bottom');
            setTimeout(() => {
                ibox.resize();
                ibox.find('[id^=map-]').resize();
            }, 50);
        }
    }

    public toggleLoading(state: boolean): void {
        const ibox = $(this.element);
        if (state) {
            ibox.find('.ibox-content').first().removeClass('sk-loading').addClass('sk-loading');
        } else {
            ibox.find('.ibox-content').first().removeClass('sk-loading');
        }
    }
}
