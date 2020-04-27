import { autoinject } from 'aurelia-framework';
import { RouteConfig, Router } from 'aurelia-router';
import { stripe } from 'stripe';
import { DataService } from '../services/data-service';
import { ReportSyncSignupCommand } from '../services/wca-api-types';
import { AppInfoService } from '../services/app-info-service';

interface IStripeWindow extends Window {
  Stripe: any;
}

declare var window: IStripeWindow;

@autoinject
export class Signup {
  // Form data
  public serviceContactFirstname: string;
  public serviceContactLastname: string;
  public serviceContactEmail: string;
  public serviceContactPhone: string;
  public billingContactFirstname: string;
  public billingContactLastname: string;
  public billingContactEmail: string;
  public billingContactPhone: string;
  public cardholderName: string;
  public billingFrequency: string;
  public company: string;
  public abn: string;
  public addressLine1: string;
  public addressLine2: string;
  public addressCity: string;
  public addressState: string;
  public addressPostcode: string;
  public acceptedTermsAndConditions: boolean = false;
  public acceptedFees: boolean = false;

  // Stripe
  public cardElement: stripe.elements.Element;
  public stripe: stripe.StripeStatic;

  // UI
  public errors: string[];
  public showCreditCardFields: boolean = false;
  public loading: boolean = false;

  constructor(private dataService: DataService,
    private router: Router,
    private appInfoService: AppInfoService) {
  }

  public async setOutcome(result: stripe.TokenResponse, attemptSubmit?: boolean) {
    this.loading = true;

    const customerData: ReportSyncSignupCommand = new ReportSyncSignupCommand({
      serviceContactFirstname: this.serviceContactFirstname,
      serviceContactLastname: this.serviceContactLastname,
      serviceContactEmail: this.serviceContactEmail,
      serviceContactPhone: this.serviceContactPhone,
      billingContactFirstname: this.billingContactFirstname,
      billingContactLastname: this.billingContactLastname,
      billingContactEmail: this.billingContactEmail,
      billingContactPhone: this.billingContactPhone,
      company: this.company,
      abn: this.abn,
      billingFrequency: this.billingFrequency,
      address1: this.addressLine1,
      address2: this.addressLine2,
      city: this.addressCity,
      state: this.addressState,
      postcode: this.addressPostcode,
      paymentGatewayToken: (result && result.token) ? result.token.id : null,
      acceptedTermsAndConditions: this.acceptedTermsAndConditions,
      acknowledgedFeesAndCharges: this.acceptedFees
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

    let createCustomerResponse;
    try {
      const createCustomerResponse = await this.dataService.simplePost('reportingsignup', customerData);
      if (createCustomerResponse.ok) {
        this.router.navigate('reporting-sync/signup-success');
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

  public async attached() {
    let key: string;
    try {
      const response = await this.dataService.getData('stripe/publickey', null);
      key = response.publicKey;
      this.showCreditCardFields = true;
      this.initializeStripe(key);
    } catch (err) {
      this.showCreditCardFields = false;
    }
  }

  public detached() {
    // $(this.element).tooltip('destroy');
  }

  public activate(params: any, routeConfig: RouteConfig) {
    // TODO
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

    if (!this.acceptedFees) {
      this.addError('You must acknowledge the fees and charges.');
    }

    if (this.errors.length > 0) {
      this.loading = false;
      return;
    }

    let response;

    if (this.showCreditCardFields) {
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
