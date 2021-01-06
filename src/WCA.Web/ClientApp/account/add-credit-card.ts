import { autoinject } from 'aurelia-framework';
import { stripe } from 'stripe';
import { DataService } from '../services/data-service';
import { AccountService } from '../services/account-service';
import {
    AccountModel,
    AddCreditCardCommand
} from '../services/wca-api-types';
import { AppInfoService } from '../services/app-info-service';

interface IStripeWindow extends Window {
    Stripe: any;
}

declare var window: IStripeWindow;

@autoinject
export class AddCreditCard {
    private currentUser: AccountModel;

    // Form data
    public cardholderName: string;
    public acceptedTermsAndConditions: boolean = false;

    // Stripe
    public cardElement: stripe.elements.Element;
    public stripe: stripe.StripeStatic;

    // UI
    public errors: string[];
    public loading: boolean = false;

    // Success
    public showSuccess: boolean = false;

    constructor(
        private dataService: DataService,
        private accountService: AccountService,
        private appInfoService: AppInfoService
    ) { }

    public async setOutcome(result: stripe.TokenResponse, attemptSubmit?: boolean) {
        this.loading = true;

        const customerData: AddCreditCardCommand = new AddCreditCardCommand({
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
            const addCreditCard = await this.dataService.simplePost('account/AddCreditCard', customerData);
            if (addCreditCard.ok) {
                this.showSuccess = true;
            } else {
                const responseData = await addCreditCard.json();
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
        this.currentUser = await this.accountService.getCurrentUser() as AccountModel;
        const stripePublicKeyResponse = await this.dataService.getData('stripe/publickey', null);
        this.initializeStripe(stripePublicKeyResponse.publicKey);
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

    public async addCreditCard() {
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

        const cardData = {
            name: this.cardholderName,
        };

        response = await this.stripe.createToken(this.cardElement, cardData);

        this.setOutcome(response, true);
    }
}
