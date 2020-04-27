import { inject } from 'aurelia-dependency-injection';
import { autoinject } from 'aurelia-framework';
import { NavigationMenu } from 'aurelia-navigation-menu';
import { PLATFORM } from 'aurelia-pal';
import { NavigationInstruction,
         Next,
         Router,
         RouterConfiguration } from 'aurelia-router';
import { AccountService } from './services/account-service';
import { AppInfoService } from './services/app-info-service';

@autoinject
export class App {
    public router: Router;
    public navigationInstruction: NavigationInstruction;
    public actionstepurl: string;
    public insideIframe: boolean;

    constructor(private navigationMenu: NavigationMenu,
        private appInfoService: AppInfoService) {
        try {
            if(window.self !== window.top)
                this.insideIframe = true;
            else 
                this.insideIframe = false;
        } catch (e) {
            this.insideIframe = true;
        }
    }

    public configureRouter(config: RouterConfiguration, router: Router) {
        config.addPipelineStep('postRender', PostRenderStep);
        config.addPipelineStep('authorize', AuthorizeStep);
        config.addPipelineStep('authorize', OneTimeNavInterceptStep);
        config.title = this.appInfoService.appShortName;
        config.options.pushState = true;
        config.options.root = '/wca/';
        config.options.eagerLoadAll = true;
        config.options.eagerIgnoreNav = true;
        // tslint:disable:max-line-length
        config.map([
            { route: '',                                                    name: 'home',                           moduleId: PLATFORM.moduleName('home/home'),                                     nav: true,  title: this.appInfoService.appName + ' Home',   settings: {allowAnonymous: false, standalonePage: false, hasBreadCrumb: true}},
            { route: '',                                                    name: 'home',                           moduleId: PLATFORM.moduleName('home/home'),                                     nav: true,  title: 'WorkCloud Home',                        settings: {allowAnonymous: false, standalonePage: false, hasBreadCrumb: true}},
            { route: 'account/add-credit-card',                             name: 'AccountAddCreditCard',           moduleId: PLATFORM.moduleName('account/add-credit-card'),                       nav: true,  title: 'Add Credit Card',                       settings: {allowAnonymous: false, standalonePage: false, hasBreadCrumb: false}},
            { route: 'stamp-duty-calculator',                               name: 'stampDutyCalculator',            moduleId: PLATFORM.moduleName('stamp-duty-calculator/stamp-duty-calculator'),   nav: true,  title: 'Stamp Duty Calculator',                 settings: {allowAnonymous: true,  standalonePage: false, hasBreadCrumb: true}},
            { route: 'calculators/settlement-invalid-link',                 name: 'settlementCalculatorInvalidLink',moduleId: PLATFORM.moduleName('calculators/settlement-invalid-link'),           nav: true,  title: 'Settlement Calculator - Invalid Link',  settings: {allowAnonymous: true,  standalonePage: false, hasBreadCrumb: true}},
            { route: 'calculators/settlement',                              name: 'settlementCalculator',           moduleId: PLATFORM.moduleName('calculators/settlement-page'),                   nav: true,  title: 'Settlement Calculator',                 settings: {allowAnonymous: false, standalonePage: false, hasBreadCrumb: true}},
            { route: 'myprofile',                                           name: 'myProfile',                      moduleId: PLATFORM.moduleName('myprofile/myprofile'),                           nav: false, title: 'My Profile',                            settings: {allowAnonymous: false, standalonePage: false, hasBreadCrumb: false, roles: ['any']}},
            { route: 'infotrack/redirect-with-matter-info',                 name: 'infoTrackRedirectWithMatterInfo',moduleId: PLATFORM.moduleName('infotrack/redirect-to-infotrack'),               nav: true,  title: 'Redirect to InfoTrack with Matter Info',settings: {allowAnonymous: false, standalonePage: false, customLoginView: 'LoginAndReconnectToActionstep', hasBreadCrumb: false}},
            { route: 'infotrack/orders',                                    name: 'infoTrackOrders',                moduleId: PLATFORM.moduleName('infotrack/orders'),                              nav: true,  title: 'InfoTrack Orders',                      settings: {allowAnonymous: false, standalonePage: false, hasBreadCrumb: true}},
            {
                route: ['pexa/create-workspace'],
                name: 'pexaCreateWorkspace',
                moduleId: PLATFORM.moduleName('pexa/create-workspace-page'),
                nav: true,
                title: 'Create New PEXA Workspace',
                settings: { allowAnonymous: false, standalonePage: false, hasBreadCrumb: true },
                href: "",
            },
            { route: 'infotrack/orders-preview',                            name: 'infoTrackOrdersPreview',         moduleId: PLATFORM.moduleName('infotrack/orders-preview'),                      nav: true,  title: 'InfoTrack Orders Preview',              settings: {allowAnonymous: false, standalonePage: false, hasBreadCrumb: true}},
            { route: 'integrations',                                        name: 'integrations',                   moduleId: PLATFORM.moduleName('integrations/integrations'),                     nav: false, title: 'Integrations',                          settings: {allowAnonymous: false, standalonePage: false, hasBreadCrumb: true}},
			{ route: 'integrations/health-check',                           name: 'integrationsHealthCheck',        moduleId: PLATFORM.moduleName('integrations/integrations-health-check'),        nav: true,  title: 'Integrations Health Check',             settings: {allowAnonymous: false, standalonePage: false, hasBreadCrumb: true}},
            { route: 'integrations/infotrack-connection/:orgkey?',          name: 'infoTrackConnection',            moduleId: PLATFORM.moduleName('integrations/infotrack-connection'),             nav: false, title: 'InfoTrack Integration',                 settings: {allowAnonymous: false, standalonePage: false, redirectOverride: '/integrations/actionstep/connect', returnUrlOverride: '/wca/integrations/infotrack-connection', hasBreadCrumb: false }},
            { route: 'integrations/infotrack-connection-signup/:orgkey?',   name: 'infoTrackConnection',            moduleId: PLATFORM.moduleName('integrations/infotrack-connection'),             nav: false, title: 'InfoTrack Integration',                 settings: {allowAnonymous: false, standalonePage: true, hasBreadCrumb: false}},
            {
                route: 'conveyancing/signup',
                name: 'conveyancingSignupsignup',
                moduleId: PLATFORM.moduleName('conveyancing/signup'),
                nav: true,
                title: 'Sign-up for Conveyancing',
                settings: {
                    allowAnonymous: false,
                    standalonePage: true,
                    redirectOverride: '/integrations/actionstep/connect',
                    returnUrlOverride: '/wca/conveyancing/signup',
                    hasBreadCrumb: false
                }
            },
            { route: 'reporting-sync/signup',                               name: 'reportSyncSignup',               moduleId: PLATFORM.moduleName('reporting-sync/signup'),                         nav: true, title: 'Sign-up for Reporting Synchronisation',  settings: {allowAnonymous: false, standalonePage: false, hasBreadCrumb: false } },
            { route: 'reporting-sync/signup-success',                       name: 'reportSyncSignupSuccess',        moduleId: PLATFORM.moduleName('reporting-sync/signup-success'),                 nav: false, title: 'Sign-up Success',                       settings: {allowAnonymous: false, standalonePage: false, hasBreadCrumb: false } }
        ]);
        // tslint:enable:max-line-length

        config.fallbackRoute('/');
        this.router = router;
    }
}

// tslint:disable:max-classes-per-file

@autoinject
class AuthorizeStep {
    constructor(private accountService: AccountService, private config: RouterConfiguration, private router: Router) { }

    public async run(instruction: NavigationInstruction, next: Next) {

        // Pass through if all instructions allow anonymous
        if (instruction.getAllInstructions().every((i) => i.config.settings.allowAnonymous === true)) {
            return next();
        }
        else {
            // Not all are anonymous, so make sure the user is logged in
            await this.accountService.updateLoggedInStatusFromServer();
            if (this.accountService.userIsLoggedIn) {
                return next();
            } else {
                // Required as Aurelia hijacks the href and fails because
                // there is no Aurelia route mapped to this absolute URL

                var redirectOverride = instruction.config.settings.redirectOverride;
                var returnUrlOverride = instruction.config.settings.returnUrlOverride;

                if (redirectOverride !== undefined) {

                    if (returnUrlOverride !== undefined) {
                        redirectOverride = redirectOverride + '?returnUrl=' + returnUrlOverride;
                    }

                    window.location.href = redirectOverride;

                } else {
                    var returnUrlParam = 'returnurl=' + encodeURIComponent(window.location.origin + "/wca" + instruction.fragment + (instruction.queryString != "" ? "?" + instruction.queryString : ""));
                    var loginUrl = `../Identity/Account/Login?${returnUrlParam}`;

                    if (instruction.config.settings.customLoginView !== undefined)
                        loginUrl = `../Identity/Account/${instruction.config.settings.customLoginView}?${returnUrlParam}`;

                    window.location.href = loginUrl;
                }

                return next.complete();
            }
        }
    }
}

class PostRenderStep {
    public run(instruction: NavigationInstruction, next: Next) {
        if (!instruction.config.settings.noScrollToTop) {
            window.scrollTo(0, 0);
        }

        return next();
    }
}

@autoinject
class OneTimeNavInterceptStep {
    constructor(private router: Router) {
    }

    public run(instruction: NavigationInstruction, next: Next) {
        let navPath = localStorage.getItem('one-time-nav-intercept');
        console.log('navPath', navPath);
        if (navPath && navPath.length > 0) {
            localStorage.removeItem('one-time-nav-intercept');
            this.router.navigate(navPath)
            return next.complete();
        }

        return next();
    }
}
// tslint:enable:max-classes-per-file
