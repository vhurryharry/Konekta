/// <reference types="aurelia-loader-webpack/src/webpack-hot-interface"/>
import { Aurelia } from 'aurelia-framework';
import { PLATFORM } from 'aurelia-pal';
import 'eonasdan-bootstrap-datetimepicker/build/css/bootstrap-datetimepicker.min.css';
import 'isomorphic-fetch';
import * as toastr from 'toastr';

// Import react components
import SettlementCalculatorApp from './calculators/settlement';

declare const IS_DEV_BUILD: boolean; // The value is supplied by Webpack during the build

export async function configure(aurelia: Aurelia) {
    aurelia.use
        .standardConfiguration()
        .globalResources(PLATFORM.moduleName("components/react-element"));

    // Register react components
    aurelia.container.registerSingleton("calculators/settlement", () => SettlementCalculatorApp);

    if (IS_DEV_BUILD) {
        aurelia.use.developmentLogging();
    } else {
        aurelia.use.plugin(PLATFORM.moduleName('aurelia-google-analytics'), (config) => {
            config.init('UA-91940818-2');
            config.attach({
                clickTracking: {
                    filter: (element) => {
                        return element instanceof HTMLElement &&
                            (element.nodeName.toLowerCase() === 'a' ||
                                element.nodeName.toLowerCase() === 'button' ||
                                element.nodeName.toLowerCase() === 'span');
                    }
                }
            });
        });
    }

    aurelia.use.plugin(PLATFORM.moduleName('aurelia-animator-css'));
    aurelia.use.plugin(PLATFORM.moduleName('aurelia-dialog'));
    aurelia.use.plugin(PLATFORM.moduleName('aurelia-plugins-switch'));
    aurelia.use.plugin(PLATFORM.moduleName('aurelia-validation'));

    aurelia.use.plugin(PLATFORM.moduleName('aurelia-bootstrap-datetimepicker'), config => {
        // extra attributes, with config.extra
        config.extra.iconBase = 'font-awesome';
        config.extra.withDateIcon = true;

        // or even any picker options, with config.options
        config.options.format = 'DD-MM-YYYY';
        config.options.showTodayButton = true;
    });
    // Anyone wanting to use HTMLImports to load views, will need to install the following plugin.
    // aurelia.use.plugin(PLATFORM.moduleName('aurelia-html-import-template-loader'));

    // Set toastr notification defaults
    toastr.options.progressBar = true;

    await aurelia.start();
    await aurelia.setRoot(PLATFORM.moduleName('app'));
}
