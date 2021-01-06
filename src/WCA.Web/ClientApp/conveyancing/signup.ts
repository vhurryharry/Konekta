import { autoinject, computedFrom } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import { stripe } from 'stripe';
import { DataService } from '../services/data-service';
import { AccountService } from '../services/account-service';
import {
    AccountModel,
    NewCustomerViewModel,
    IConnectedActionstepOrgsResponse
} from '../services/wca-api-types';
import { AppInfoService } from '../services/app-info-service';

interface IStripeWindow extends Window {
    Stripe: any;
}

declare var window: IStripeWindow;

@autoinject
export class Signup {

    private currentUser: AccountModel;
    public userOrgs: IConnectedActionstepOrgsResponse[];

    // Form data
    public firstname: string;
    public lastname: string;
    public cardholderName: string;
    public phone: string;
    public email: string;
    public company: string;
    public conveyancingApp: string;
    public abn: string;
    public addressLine1: string;
    public addressLine2: string;
    public addressCity: string;
    public addressState: string;
    public addressPostcode: string;
    public promoCode: string;
    public orgKey: string;
    public acceptedTermsAndConditions: boolean = false;
    public infoTrackEnabled: boolean = null;
    public infoTrackCredReady: boolean;

    // Stripe
    public cardElement: stripe.elements.Element;
    public stripe: stripe.StripeStatic;

    // UI
    public errors: string[];
    public loading: boolean = false;

    // Success
    public redirectLink: string;

    constructor(
        private dataService: DataService,
        private accountService: AccountService,
        private router: Router,
        private appInfoService: AppInfoService
    ) { }

    public async setOutcome(result: stripe.TokenResponse, attemptSubmit?: boolean) {
        this.loading = true;

        const customerData: NewCustomerViewModel = new NewCustomerViewModel({
            firstname: this.firstname,
            lastname: this.lastname,
            phone: this.phone,
            email: this.email,
            conveyancingApp: this.conveyancingApp,
            company: this.company,
            abn: this.abn,
            address1: this.addressLine1,
            address2: this.addressLine2,
            city: this.addressCity,
            state: this.addressState,
            postcode: this.addressPostcode,
            promoCode: this.promoCode,
            orgKey: this.orgKey,
            paymentGatewayToken: (result && result.token) ? result.token.id : null,
            acceptedTermsAndConditions: this.acceptedTermsAndConditions,
        });

        if (result && result.error) {
            // If the stripe result already contains an error,
            // we won't try submitting to our service.
            this.addError(result.error.message);
            this.loading = false;
            return;
        }

        if (this.errors.length > 0 || attemptSubmit === false) {
            this.loading = false;
            return;
        }

        try {
            const createCustomerResponse = await this.dataService.simplePost('customer', customerData);
            if (createCustomerResponse.ok) {

                let createCustomerResponseData = await createCustomerResponse.json()

                if (this.isTrial || !this.infoTrackEnabled)
                    window.location.href = createCustomerResponseData.actionstepInstallLink;
                else
                    this.router.navigate('integrations/infotrack-connection-signup/' + this.orgKey
                        + '?redirect=' + encodeURIComponent(createCustomerResponseData.actionstepInstallLink));

            } else {
                const responseData = await createCustomerResponse.json();
                this.addError(responseData.message);

                for (const i in responseData.errorList) {
                    if (responseData.errorList.hasOwnProperty(i)) {
                        this.addError(responseData.errorList[i]);
                    }
                }
                this.loading = false;
            }
        } catch (err) {
            this.addError('An unknown error occurred. Please try again.');
            this.loading = false;
        }
    }

    public resetErrors() {
        this.errors = [];
    }

    public addError(errorMessage: string) {
        if (!Array.isArray(this.errors)) {
            this.errors = [];
        }

        let messageAlreadyPresent: boolean = false;
        for (const currentMessage of this.errors) {
            if (currentMessage === errorMessage) {
                messageAlreadyPresent = true;
            }
        }

        if (!messageAlreadyPresent) {
            this.errors.push(errorMessage);
        }
    }

    public activate(params: any) {
    }

    public async attached() {

        // pre-populate the form with user's Actionstep account
        this.currentUser = await this.accountService.getCurrentUser() as AccountModel;
        this.firstname = this.currentUser.firstName;
        this.lastname = this.currentUser.lastName;
        this.email = this.currentUser.email;
        this.userOrgs = await this.dataService.getData('/integrations/actionsteporgs', null) as IConnectedActionstepOrgsResponse[];

        if (this.userOrgs.length == 1) this.orgKey = this.userOrgs[0].actionstepOrgKey;

        // Always set up stripe because the user may toggle options that turn on credit card fields
        const stripePublicKeyResponse = await this.dataService.getData('stripe/publickey', null);
        this.initializeStripe(stripePublicKeyResponse.publicKey);
    }

    @computedFrom('orgKey')
    get isTrial(): boolean {
        return this.orgKey && this.orgKey.toLocaleLowerCase().startsWith('trial');
    }

    public async connectToActionstepOrg() {
        window.location.href = '/integrations/actionstep/connect?returnUrl=/wca/conveyancing/signup';
    }

    public initializeStripe(publicKey: string) {
        this.stripe = window.Stripe(publicKey);
        const elements = this.stripe.elements();

        this.cardElement = elements.create('card', {
            style: {
                base: {
                    'iconColor': '#666EE8',
                    'color': '#31325F',
                    'lineHeight': '40px',
                    'fontFamily': '"Helvetica Neue", Helvetica, sans-serif',
                    'fontSize': '15px',

                    '::placeholder': {
                        color: '#CFD7E0'
                    }
                }
            }
        });

        this.cardElement.mount('#card-element');
    }

    public async signup() {
        this.loading = true;
        this.resetErrors();

        if (!this.acceptedTermsAndConditions) {
            this.addError('You must accept the terms and conditions.');
        }

        if (this.errors.length > 0) {
            this.loading = false;
            return;
        }

        let response;

        if (!this.isTrial) {
            const cardData = {
                name: this.cardholderName,
                address_line1: this.addressLine1,
                address_line2: this.addressLine2,
                address_city: this.addressCity,
                address_state: this.addressState,
                address_zip: this.addressPostcode
            };

            response = await this.stripe.createToken(this.cardElement, cardData);
        }

        this.setOutcome(response, true);
    }
}
