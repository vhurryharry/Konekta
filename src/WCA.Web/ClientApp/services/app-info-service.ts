import { autoinject } from "aurelia-framework";
import { DataService } from "./data-service";
import { AppUrlSettings } from "./wca-api-types";

@autoinject
export class AppInfoService {
    public appLogo: string;
    public appName: string;
    public appShortName: string;
    public appLoadingImage: string;
    public isWCA: boolean;

    public supportUrl: string;
    public appUrl: string;
    public domainUrl: string;

    private appUrlSettings: AppUrlSettings;

    constructor(private dataService: DataService) {
        this.updateFromServer();

        this.isWCA = location.hostname.includes("workcloud") || location.hostname.includes("appwca-test");

        this.appLogo = this.isWCA ? "/images/workcloud-sidebyside.svg" : "/images/Konekta_standard_extralight.svg";
        this.appName = this.isWCA ? "WorkCloud" : "Konekta";
        this.appShortName = this.isWCA ? "WCA" : "Konekta";
        this.appLoadingImage = this.isWCA ? "/images/wca-spinner.svg" : "/images/Konekta_loading.svg";
    }

    public async updateFromServer() {
        this.appUrlSettings = (await this.dataService.getData('/appurls', null) as AppUrlSettings);

        this.supportUrl = this.isWCA ? this.appUrlSettings.workCloud.supportUrl : this.appUrlSettings.konekta.supportUrl;
        this.appUrl = this.isWCA ? this.appUrlSettings.workCloud.appUrl : this.appUrlSettings.konekta.appUrl;
        this.domainUrl = this.isWCA ? this.appUrlSettings.workCloud.domainUrl : this.appUrlSettings.konekta.domainUrl;
    }
}
