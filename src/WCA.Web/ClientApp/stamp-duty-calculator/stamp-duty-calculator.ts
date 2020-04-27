import { CssAnimator } from 'aurelia-animator-css';
import { autoinject } from 'aurelia-framework';
import { RouteConfig } from 'aurelia-router';
import { Tools } from '../tools/tools';
import { DataService } from '../services/data-service';
import { ErrorViewModel } from '../services/wca-api-types';

import {
    FinancialResults,
    IntendedPropertyUse,
    IPropertySaleInformationViewModel,
    PropertyBuyerViewModel,
    PropertyType,
    State,
} from '../services/wca-api-types';

/* Note! If you change enums, please check the view as it may have hard-coded numeric values!
   This is because TypeScript cannot be used in view expressions. */
interface IState {
    id: number;
    abbreviation: string;
    name: string;
}

enum YesNo {
    Yes,
    No
}

interface IRadioOption {
    id: IntendedPropertyUse | PropertyType | YesNo;
    name: string;
    value: any;
}

@autoinject
export class StampDutyCalculator {
    public results: FinancialResults;
    public saleInformation: IPropertySaleInformationViewModel;
    public resultsMessage: string;
    public resultsMessageIsError: boolean;

    // Radio values
    public intendedUseOptions: IRadioOption[] = [
        { id: IntendedPropertyUse.PrimaryResidence, name: 'Primary Residence', value: null },
        { id: IntendedPropertyUse.Investment, name: 'Investment', value: null }
    ];

    public propertyTypeOptions: IRadioOption[] = [
        { id: PropertyType.EstablishedHome, name: 'Established Home', value: null },
        { id: PropertyType.NewHome, name: 'New Home', value: null },
        { id: PropertyType.VacantLand, name: 'Vacant Land', value: null }
    ];

    public yesNoOptions: IRadioOption[] = [
        { id: YesNo.Yes, name: 'Yes', value: true },
        { id: YesNo.No, name: 'No', value: false }
    ];

    // Drop down values
    public states: IState[] = [
        // { id: 1, abbreviation: "ACT", name: "Australian Capitol Territory" },
        // { id: 2, abbreviation: "NSW", name: "New South Wales" },
        // { id: 3, abbreviation: "NT", name: "Northern Territory" },
        { id: 4, abbreviation: 'QLD', name: 'Queensland' }
        // { id: 5, abbreviation: "SA", name: "South Australia" },
        // { id: 6, abbreviation: "TAS", name: "Tasmania" },
        // { id: 7, abbreviation: "VIC", name: "Victoria" },
        // { id: 8, abbreviation: "WA", name: "Western Australia" }
    ];

    constructor(private dataService: DataService,
        private animator: CssAnimator,
        private element: Element) {
        this.resetCalculator();
    }

    public attached() {
        //$(this.element).tooltip({
        //    selector: '[data-toggle=tooltip]',
        //    container: 'body'
        //});
    }

    public detached() {
        //$(this.element).tooltip('destroy');
    }

    public activate(params: any, routeConfig: RouteConfig) {
        this.populateForm(params);
    }

    public populateForm(params: any) {
        // List of supported parameters
        // Note: * denotes default value. Defaults can be omitted
        //       parameter names are case sensitive, values are NOT case sensitive
        // - buyer1IsFirstHomeBuyer: true/false*
        // - buyer1IsForeignBuyer: true/false*
        // - buyer1IntendedUse: investment/primaryresidence*
        // - buyer1Fraction: [whole number fraction or number 1], e.g: 1/2, 1/4, 1
        // - propertyType: newhome/vacantland/establishedhome*
        // - purchasePrice: [number/float] (note, cannot have any special characters such as commas or currency symbols)
        // - state: act, nsw, nt, qld*, sa, tas, vic, wa
        //
        // Example: ?buyer1IsFirstHomeBuyer=true&purchasePrice=350000&propertyType=vacantland

        const paramsWithKeysInLowerCase = [params.length];
        for (const param in params) {
            if (params.hasOwnProperty(param)) {
                paramsWithKeysInLowerCase[param.toLowerCase()] = params[param];
            }
        }

        for (let i = 0; i < 6; i++) {
            const buyerNumber = i + 1;

            const isFirstHomeBuyerValue = Tools.ParseBooleanPropertyValue(
                paramsWithKeysInLowerCase,
                `buyer${buyerNumber}IsFirstHomeBuyer`.toLowerCase());

            const isForeignBuyerValue = Tools.ParseBooleanPropertyValue(
                paramsWithKeysInLowerCase,
                `buyer${buyerNumber}IsForeignBuyer`.toLowerCase());

            const purchaseFractionValue = Tools.ParseStringPropertyValue(
                paramsWithKeysInLowerCase,
                `buyer${buyerNumber}Fraction`.toLowerCase());

            const intendedUseValue = Tools.ParseStringPropertyValue(
                paramsWithKeysInLowerCase,
                `buyer${buyerNumber}IntendedUse`.toLowerCase());

            if (isFirstHomeBuyerValue === undefined && isForeignBuyerValue === undefined &&
                purchaseFractionValue === undefined && intendedUseValue === undefined) {
                continue;
            }

            const intendedUseToLower = intendedUseValue === undefined ? '' : intendedUseValue.toLowerCase();

            this.saleInformation.buyers[i] = new PropertyBuyerViewModel({
                buyerNumber,
                firstHomeBuyer: isFirstHomeBuyerValue === undefined ? false : isFirstHomeBuyerValue,
                intendedUse: (intendedUseToLower === 'investment'
                    ? IntendedPropertyUse.Investment
                    : IntendedPropertyUse.PrimaryResidence),
                isForeignBuyer: isForeignBuyerValue === undefined ? false : isForeignBuyerValue,
                purchaseFraction: purchaseFractionValue === undefined ? '1' : purchaseFractionValue
            });
        }

        const propertyTypeValue = Tools.ParseStringPropertyValue(paramsWithKeysInLowerCase, 'propertytype');
        const propertyTypeValueToLower = propertyTypeValue === undefined ? '' : propertyTypeValue.toLowerCase();
        if (propertyTypeValueToLower === 'newhome') {
            this.saleInformation.propertyType = PropertyType.NewHome;
        } else if (propertyTypeValueToLower === 'vacantland') {
            this.saleInformation.propertyType = PropertyType.VacantLand;
        } else {
            this.saleInformation.propertyType = PropertyType.EstablishedHome;
        }

        const purchasePriceValue = Tools.ParseNumberPropertyValue(paramsWithKeysInLowerCase, 'purchaseprice');
        this.saleInformation.purchasePrice = purchasePriceValue === undefined ? 0 : purchasePriceValue;

        const stateValue = Tools.ParseStringPropertyValue(paramsWithKeysInLowerCase, 'state');
        this.saleInformation.state = State.QLD;

        if (this.saleInformation.purchasePrice > 0) {
            this.calculate();
        }
    }

    public updateBuyerNumbers() {
        for (let i = 0; i < this.saleInformation.buyers.length; i++) {
            this.saleInformation.buyers[i].buyerNumber = i + 1;
        }
    }

    public removeBuyer(buyerIndex: number) {
        this.saleInformation.buyers.splice(buyerIndex, 1);
        this.updateBuyerNumbers();
    }

    public addBuyer() {
        const newBuyerNumber = this.saleInformation.buyers.length + 1;
        this.saleInformation.buyers.push(
            new PropertyBuyerViewModel({
                buyerNumber: newBuyerNumber,
                firstHomeBuyer: false,
                intendedUse: IntendedPropertyUse.PrimaryResidence,
                isForeignBuyer: false,
                purchaseFraction: `1/${newBuyerNumber}`
            })
        );
        this.updateBuyerNumbers();
    }

    public calculate() {
        const result = this.dataService.simplePost('/StampDuty', this.saleInformation);
        result
            .then((response: Response) => response.json())
            .then((stampDutyResults: FinancialResults) => {
                this.updateResults(stampDutyResults);
                this.showMessage('Results updated!');
            })
            .catch((reason: Response) => {
                reason.json()
                    .then((reasonData: ErrorViewModel) => {
                        this.showMessage(reasonData.message, true);
                    });
            });
    }

    public updateResults(stampDutyResults: FinancialResults) {
        this.results = stampDutyResults;
    }

    public resetMessage() {
        this.resultsMessage = null;
        this.resultsMessageIsError = false;
    }

    public showMessage(message: string, isError: boolean = false) {
        this.resultsMessage = message;
        this.resultsMessageIsError = isError;
        const list = this.element.querySelector('#changes-made') as HTMLElement;
        this.animator.addClass(list, 'fade-in');
    }

    public resetCalculator() {
        this.results = new FinancialResults({
            categories: []
        });

        this.saleInformation = {
            buyers: [
                new PropertyBuyerViewModel({
                    buyerNumber: 1,
                    firstHomeBuyer: false,
                    intendedUse: IntendedPropertyUse.PrimaryResidence,
                    isForeignBuyer: false,
                    purchaseFraction: '1',
                })
            ],
            propertyType: PropertyType.EstablishedHome,
            purchasePrice: 0,
            state: State.QLD
        };
    }
}
