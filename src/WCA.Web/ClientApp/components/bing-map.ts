/// <reference path="types/MicrosoftMaps/Microsoft.Maps.All.d.ts" />
import { bindable, bindingMode, inlineView } from 'aurelia-framework';
const controlUrl = '//www.bing.com/api/maps/mapcontrol?callback=bingMapsLoaded';
const ready = new Promise(resolve => window['bingMapsLoaded'] = resolve);

let scriptTag: HTMLScriptElement = document.createElement('script');

scriptTag.async = true;
scriptTag.defer = true;
scriptTag.src = controlUrl;

document.head.appendChild(scriptTag);

@inlineView('<template><div ref="container" css="width: ${width}; height: ${height};"></div></template>')
export class BingMapCustomElement {
    private container: HTMLElement;
    private map: Microsoft.Maps.Map;
    private viewChangeHandler: Microsoft.Maps.IHandlerId;
    @bindable height = '600px';
    @bindable width = '400px';

    @bindable({ defaultBindingMode: bindingMode.twoWay }) public mapLocation: Microsoft.Maps.Location | string;
    @bindable({ defaultBindingMode: bindingMode.twoWay }) public apiKey: string;
    @bindable({ defaultBindingMode: bindingMode.twoWay }) public latitude: number;
    @bindable({ defaultBindingMode: bindingMode.twoWay }) public longitude: number;

    attached() {
        this.loadMap();
    }

    loadMap() {
        if ((Microsoft == null) || (Microsoft.Maps == null)) {
            // not yet available, keep trying (dirty checking)
            setTimeout(this.loadMap, 1000);
        } else {
            return ready.then(() => {
                this.map = new Microsoft.Maps.Map(this.container as HTMLElement, {
                    credentials: this.apiKey
                });
                this.map.setView({ center: new Microsoft.Maps.Location(this.latitude, this.longitude), zoom: 15 });
                this.mapLocation = this.map.getCenter();

                this.viewChangeHandler = Microsoft.Maps.Events.addHandler(this.map, 'viewchange', e => {
                    this.mapLocation = this.map.getCenter();
                });
            });
        }
    }

    detached() {
        if (this.viewChangeHandler) {
            Microsoft.Maps.Events.removeHandler(this.viewChangeHandler);
        }

        if (this.map) {
            this.map.dispose();
            this.map = null;
        }
    }
}