import * as React from 'react';
import { DefaultButton, PrimaryButton, IconButton, IButtonProps } from 'office-ui-fabric-react/lib/Button';
import { Modal, IDragOptions } from 'office-ui-fabric-react/lib/Modal';
import { initializeIcons } from '@uifabric/icons';
import * as toastr from 'toastr';

import { connect } from 'react-redux';

import SettlementInfo from './settlement-info';
import WaterUsageSection from './water-usage'
import Modals from './modals';

import * as CONSTANTS from './redux/constants'
import { generatePDF, changeState, clearSettlementState, saveSettlementMatter, getSettlementMatter, savePDF, deleteSettlementMatter } from './redux/actions';

import { ContextualMenu } from 'office-ui-fabric-react/lib/ContextualMenu';
import { SettlementMatterViewModel, SettlementInfo as SettlementInfoViewModel, ActionstepMatter, MatterDetails, ErrorViewModel, ActionstepMatterInfo, ActionstepDocument } from '../services/wca-api-types';
import { Checkbox } from 'office-ui-fabric-react/lib/Checkbox';

import SettlementInvalidMatter from './settlement-invalid-matter'
import ConnectToActionstep from '../components/connect-to-actionstep';
import { AppInfoService } from '../services/app-info-service';

interface IMapStateToProps {
    state: string;
    success: boolean;
    gotResponse: boolean;
    settlementMatter: SettlementMatterViewModel;
    actionstepPDF: ActionstepDocument;
    isOrgConnected: boolean;
    error: ErrorViewModel;
    requestType: string;
}
interface IMapDispatchToProps {
    generatePDF: (params) => void;
    savePDF: (params) => void;
    getSettlementMatter: (params) => void;
    saveSettlementMatter: (params) => void;
    deleteSettlementMatter: (params) => void;
    changeState: (params) => void;
    clearSettlementState: () => void;
}
interface IAppProps {
    appInfoService: AppInfoService;
}

type AppProps = IMapStateToProps & IMapDispatchToProps & IAppProps;

type AppStates = {
    matterDetails: MatterDetails,
    matterInfo: ActionstepMatterInfo | null;
    showModal: string,      //This is the title of the modal when the modal is visible. If the modal is invisible this is null.
    adjustments: { [key: string]: any; }[],
    fees: { [key: string]: any; }[],
    additionalRequirements: { [key: string]: any; }[],
    payees: { [key: string]: any; }[],
    ourRequirements: { [key: string]: any; }[],
    showAdditionalRequirements: boolean,
    showWaterUsage: boolean,
    refresh: boolean,
    selectedIndex: number,
    lc: string,
    settlementData: SettlementInfoViewModel,
    actionstepData: ActionstepMatter,
    isOrgConnected: boolean,
    includeAdditionalCostsInTotal: boolean,
    isWaiting: boolean,
    needConfirmation: boolean,
    isInvalidMatter: boolean,
    savePDFSuccess: boolean,
    hasError: boolean,
    dataInputError: string,
    actionstepPDF: ActionstepDocument
}

const modalIDs = {
    matterDetails: 'Matter Details',
    addAdjustment: 'Add Adjustment',

    contractPrice: 'Contract Price',
    releaseFee: 'Release Fee',
    waterUsage: 'Water Usage',
    councilRates: 'Council Rates',
    waterAccessFee: 'Water Access Fee',
    sewerageAccessFee: 'Sewerage Access Fee',
    administrationFund: 'Administration Fund',
    sinkingFund: 'Sinking Fund',
    insurance: 'Insurance',
    penaltyInterest: 'Penalty Interest',
    otherAdjustment: 'Other Adjustment',
    otherAdjustmentDate: 'Other Adjustment Date',
    strataLevies: 'Strata Levies',
    waterDrainageFee: 'Water Drainage Fee',
    parksCharge: 'Parks Charge',
    waterServiceCharge: 'Water Service Charge',
    sewerageUsage: 'Sewerage Usage',
    maintenanceFund: 'Maintenance Fund',
    landTax: 'Land Tax',
    rent: 'Rent',
    ownersCorporationFees: 'Owners Corporation Fees',
    councilRatesChargesLevies: 'Council Rates, Charges, Levies',
    waterRatesChargesLevies: 'Water Rates, Charges & Levies',
    sewerageServiceCharge: 'Sewerage Service Charge',
    ownersAdministrationFundFee: 'Owners Corporation - Administration Fund Fee',
    ownersMaintenanceFundFee: 'Owners Corporation - Maintenance Fund Fee',
    ownersSinkingFundFee: 'Owners Corporation - Sinking Fund Fee',
    ownersInsurance: 'Owners Corporation - Insurance',
    waterSewerageRates: 'Water/Sewerage Rates',
    emergencyServicesLevy: 'Emergency Services Levy',
    waterAndSewerageRates: 'Water and Sewerage Rates',

    fee: 'Fee',
    additionalRequirements: 'Additional Requirements',
    payeeDetails: 'Cheque Payee',
    ourRequirements: 'Our Requirements'
}

const errorMessages = {
    dateInputError: "Please input all the dates..."
}

export class SettlementCalculator extends React.Component<AppProps, AppStates> {
    public constructor(props: any) {
        super(props);

        initializeIcons();

        this.state = {
            matterDetails: new MatterDetails(),
            matterInfo: null,
            actionstepData: new ActionstepMatter(),
            showModal: null,
            adjustments: [],
            fees: [],
            additionalRequirements: [],
            payees: [],
            ourRequirements: [],
            showAdditionalRequirements: true,
            showWaterUsage: false,
            refresh: false,
            selectedIndex: -1,
            lc: '',
            settlementData: new SettlementInfoViewModel(),
            isOrgConnected: null,
            includeAdditionalCostsInTotal: false,
            isWaiting: false,
            hasError: null,
            dataInputError: "",
            needConfirmation: false,
            isInvalidMatter: false,
            savePDFSuccess: false,
            actionstepPDF: new ActionstepDocument()
        }
    }

    updatedState: {};

    _dragOptions: IDragOptions = {
        moveMenuItemText: "Move",
        closeMenuItemText: "Close",
        dragHandleSelector: '.modal-header',
        menu: ContextualMenu
    }

    public componentDidMount(): void {

        var adjustments = [
            {
                type: 'Contract Price',
                value: {
                    price: 0,
                    deposit: 0
                }
            },
        ];

        this.setState({
            adjustments: adjustments
        })

        const queryString = require('query-string');

        const urlParams = queryString.parse(window.location.search);

        const lowerCaseParams = {};
        for (const param in urlParams) {
            lowerCaseParams[param.toLowerCase()] = urlParams[param];
        }

        if (lowerCaseParams['actionsteporg'] == undefined || lowerCaseParams['matterid'] == undefined) {
            this.setState({
                isInvalidMatter: true
            });

            return;
        }

        const params = {
            matterId: lowerCaseParams['matterid'] ? lowerCaseParams['matterid'] : "*",
            actionstepOrg: lowerCaseParams['actionsteporg'] ? lowerCaseParams['actionsteporg'] : "*",
        };

        const matterInfo = new ActionstepMatterInfo({
            "matterId": parseInt(lowerCaseParams['matterid']),
            "orgKey": lowerCaseParams['actionsteporg'],
            "orgName": undefined,
            "matterName": undefined
        })

        this.setState({ matterInfo });

        this.props.getSettlementMatter(params);
    }

    public async loadData(useNewValue = false): Promise<void> {
        var { settlementData, actionstepData } = this.state;
        var matterDetails: MatterDetails = useNewValue
            ? this.state.matterDetails
            : settlementData.matterDetails;

        matterDetails.adjustmentDate = new Date(matterDetails.adjustmentDate);
        matterDetails.settlementDate = new Date(matterDetails.settlementDate);

        await this.setState({
            matterDetails: matterDetails,
            adjustments: settlementData.adjustments.map(adjustment => {
                var newValue = {};

                if (adjustment.type == 'Contract Price') {
                    if (useNewValue) {
                        newValue = {
                            'price': actionstepData.price['value'],
                            'deposit': actionstepData.deposit['value']
                        };
                    } else {
                        newValue = adjustment.value;
                    }
                } else {

                    Object.keys(adjustment.value).forEach(key => {
                        if (adjustment.value[key] == null)
                            adjustment.value[key] = 0;

                        if (key == 'from' || key == 'to' || key.includes("Date")) {
                            newValue[key] = this.convertStringToDate(adjustment.value[key]);
                        } else {
                            newValue[key] = adjustment.value[key];
                        }
                    })
                }

                return {
                    ...adjustment,
                    value: newValue
                };
            }),
            additionalRequirements: settlementData.additionalRequirements == null ? [] : settlementData.additionalRequirements,
            payees: settlementData.payees == null ? [] : settlementData.payees,
            ourRequirements: settlementData.ourRequirements == null ? [] : settlementData.ourRequirements,
            fees: settlementData.fees == null ? [] : settlementData.fees.map(fee => {
                var newValue = {};
                Object.keys(fee.value).forEach(key => {
                    if (key == 'from' || key == 'to') {
                        newValue[key] = this.convertStringToDate(fee.value[key]);
                    } else {
                        newValue[key] = fee.value[key];
                    }
                })

                return {
                    ...fee,
                    value: newValue
                };
            }),
            needConfirmation: false
        });

        this.props.clearSettlementState();
        this.props.changeState(matterDetails.state);

        this.updateAdjustmentDates();
    }

    protected convertStringToDate(dateString: string): Date {
        let date = new Date(dateString);
        const offset = date.getTimezoneOffset();

        date = new Date(date.getTime() - offset * 60000);

        date.setUTCHours(0);
        date.setUTCMinutes(0);
        date.setUTCSeconds(0);
        date.setUTCMilliseconds(0);

        return date;
    }

    public async componentWillReceiveProps(nextProps: AppProps): Promise<void> {
        if (nextProps.gotResponse == true) {
            switch (nextProps.requestType) {
                case CONSTANTS.GENERATE_PDF_REQUESTED:
                    this.setState({
                        isWaiting: false
                    })

                    if (nextProps.gotResponse == true && nextProps.success == false) {
                        toastr.error(nextProps.error.message);
                        this.setState({
                            hasError: true
                        })
                    } else {
                        //Generate PDF Succeed
                    }

                    this.props.clearSettlementState();
                    break;

                case CONSTANTS.SAVE_PDF_REQUESTED:
                    this.setState({
                        isWaiting: false
                    });

                    if (nextProps.gotResponse == true && nextProps.success == false) {
                        toastr.error(nextProps.error.message);
                        this.setState({
                            hasError: true
                        })
                    } else {
                        this.setState({
                            actionstepPDF: nextProps.actionstepPDF,
                            savePDFSuccess: true
                        })
                    }

                    this.props.clearSettlementState();
                    break;

                case CONSTANTS.GET_SETTLEMENT_MATTER_REQUESTED:
                    if (nextProps.success == true) {
                        if (nextProps.settlementMatter != null) {

                            var settlementMatter: SettlementMatterViewModel = nextProps.settlementMatter;

                            var actionstepData: ActionstepMatter = settlementMatter.actionstepData;
                            var settlementData: SettlementInfoViewModel = settlementMatter.settlementData;
                            var matterDetails: MatterDetails = settlementData.matterDetails;

                            var newActionstepData = new ActionstepMatter();
                            var newMatterDetails: MatterDetails = new MatterDetails();
                            var hasChangedData: boolean = false;
                            var isNew: boolean = settlementMatter.version == 0;

                            var labelsPerKeys = {
                                matterRef: "Matter ID",
                                matter: "Actionstep Matter Name",
                                price: "Contract Price",
                                deposit: "Deposit",
                                property: "Property",
                                adjustmentDate: "Adjustment Date",
                                settlementDate: "Settlement Date",
                                settlementPlace: "Settlement Place",
                                settlementTime: "Settlement Time",
                                state: "State",
                                conveyType: "Conveyancing Type"
                            }

                            this.setState({
                                isOrgConnected: true
                            });

                            Object.keys(actionstepData).forEach(key => {
                                if (actionstepData[key] != undefined) {
                                    var value = actionstepData[key],
                                        changed = false,
                                        oldValue = null,
                                        displayValue = "";

                                    if (key == "adjustmentDate" || key == "settlementDate") {
                                        value = this.convertStringToDate(value);
                                    }

                                    if (matterDetails[key]) {
                                        if (!isNew) {
                                            if (key == "adjustmentDate" || key == "settlementDate") {
                                                if (matterDetails[key] == null) {
                                                    matterDetails[key] = new Date();
                                                }
                                                var date = new Date(matterDetails[key]);

                                                if (date.getFullYear() != value.getFullYear() || date.getMonth() != value.getMonth() || date.getDate() != value.getDate())
                                                    changed = true;

                                                displayValue = value.toDateString();
                                                oldValue = date.toDateString();
                                            } else if (matterDetails[key] != value) {
                                                changed = true;

                                                displayValue = value;
                                                oldValue = matterDetails[key];
                                            }
                                        }
                                    } else {
                                        if (!isNew) {
                                            var adjustment = settlementData.adjustments[0];
                                            if (adjustment.value[key] == null) {
                                                adjustment.value[key] = 0;
                                            }

                                            if (adjustment.value[key] != value) {
                                                changed = true;
                                            }

                                            displayValue = "$" + value.toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 });
                                            oldValue = "$" + adjustment.value[key].toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 });
                                        }
                                    }

                                    hasChangedData = hasChangedData || changed;

                                    if (key != 'price' && key != 'deposit') {
                                        newMatterDetails[key] = value;
                                    }

                                    newActionstepData[key] = {
                                        value: value,
                                        oldValue: oldValue,
                                        displayValue: displayValue,
                                        label: labelsPerKeys[key],
                                        update: false,
                                        changed: changed
                                    }
                                }
                            })

                            await this.setState({
                                matterDetails: newMatterDetails,
                                actionstepData: newActionstepData,
                                settlementData: settlementData
                            });

                            if (isNew) {

                                settlementData.adjustments.push(
                                    {
                                        type: 'Contract Price',
                                        value: {
                                            price: actionstepData.price,
                                            deposit: actionstepData.deposit
                                        }
                                    }
                                );

                                await this.setState({
                                    settlementData,
                                    hasError: false
                                });

                                this.loadData(true);
                            } else if (hasChangedData) {
                                this.setState({
                                    needConfirmation: true,
                                    hasError: false
                                })
                            } else {
                                this.loadData();
                            }
                        }

                    } else {
                        if (nextProps.isOrgConnected == false) {
                            this.setState({
                                isOrgConnected: false,
                                hasError: false
                            })
                        } else {
                            toastr.error(nextProps.error.message);
                            this.setState({
                                hasError: true
                            })
                        }
                    }
                    break;

                case CONSTANTS.SAVE_SETTLEMENT_MATTER_REQUESTED:
                    this.setState({
                        isWaiting: false
                    })

                    if (nextProps.gotResponse == true && nextProps.success == false) {
                        toastr.error(nextProps.error.message);
                        this.setState({
                            hasError: true
                        })
                    } else {
                    }

                    this.props.clearSettlementState();

                    break;

                case CONSTANTS.DELETE_SETTLEMENT_MATTER_REQUESTED:
                    this.setState({
                        isWaiting: false
                    })

                    if (nextProps.gotResponse == true && nextProps.success == false) {
                        toastr.error(nextProps.error.message);
                        this.setState({
                            hasError: true
                        })
                    } else {
                        this.setState({
                            adjustments: [this.state.adjustments[0]],
                            fees: [],
                            additionalRequirements: [],
                            payees: [],
                            ourRequirements: [],
                            showAdditionalRequirements: true,
                            showWaterUsage: false,
                            settlementData: new SettlementInfoViewModel()
                        });
                    }

                    this.props.clearSettlementState();

                    break;

                default:
                    break;
            }
        }
    }

    private calculate(matter: any): number {

        switch (matter.type) {
            case 'Contract Price':
                return matter.value['price'] - matter.value['deposit'];

            case 'Release Fee':
                return matter.value['mortgages'] * matter.value['each'];

            case 'Water Usage':
                var startDate = matter.value['paidDate'];
                var endDate = matter.value['searchDate'];

                var numberOfDays = Math.round((endDate - startDate) / (1000 * 60 * 60 * 24));
                numberOfDays = numberOfDays == 0 ? 1 : numberOfDays;
                var diffAmountReading = matter.value['searchReadingAmount'] - matter.value['paidReadingAmount'];
                var dailyUsage = numberOfDays ? diffAmountReading / numberOfDays : 0;

                var startJune = new Date(startDate.getTime());
                startJune.setMonth(5);
                startJune.setDate(30);

                var numberOfDaysToJune = Math.round((startJune.valueOf() - startDate) / (1000 * 60 * 60 * 24));
                var numberOfDaysFromJune = Math.round((this.state.matterDetails.adjustmentDate.valueOf() - startJune.valueOf()) / (1000 * 60 * 60 * 24));

                if (matter.value['method'] == 'daily-average') {
                    dailyUsage = matter.value['averageKlCount'];
                }

                var partDays = Math.round((this.state.matterDetails.settlementDate.valueOf() - startDate) / (1000 * 60 * 60 * 24));
                partDays = partDays == 0 ? 1 : partDays;
                var dailyAndDays = dailyUsage * partDays;

                var tier1Count = matter.value['tier1KlCount'];
                var tier1Charge = matter.value['tier1Charge'];
                var tier2Count = matter.value['tier2KlCount'];
                var tier2Charge = matter.value['tier2Charge'];

                var balanceCalc = dailyAndDays - tier1Count - tier2Count;
                balanceCalc = balanceCalc < 0 ? 0 : balanceCalc;

                var tier1Result = tier1Count * tier1Charge;
                var tier2Result = tier2Count * tier2Charge;

                var balanceResult = balanceCalc * matter.value['balanceCharge'];
                var bulkResult = dailyAndDays * matter.value['bulkCharge'];
                var waterUsageCalcTotal = 0;

                var tier1CalcResult = 0, tier2CalcResult = 0, balanceCalcResult = 0, bulkCalcResult = 0;

                if (matter.value['tier1FeeIncrease'] > 0 || matter.value['tier2FeeIncrease'] > 0 || matter.value['balanceFeeIncrease'] > 0 || matter.value['bulkFeeIncrease'] > 0) {
                    tier1CalcResult = tier1Count * numberOfDaysToJune / partDays * tier1Charge;
                    tier2CalcResult = tier2Count * numberOfDaysToJune / partDays * tier2Charge

                    balanceCalcResult = balanceCalc * numberOfDaysToJune / partDays * matter.value['balanceCharge'];
                    bulkCalcResult = dailyAndDays * numberOfDaysToJune / partDays * matter.value['bulkCharge'];

                    tier1Result = tier1Count * matter.value['tier1FeeIncrease'] * numberOfDaysFromJune / partDays;
                    tier2Result = tier2Count * matter.value['tier2FeeIncrease'] * numberOfDaysFromJune / partDays;

                    balanceResult = balanceCalc * matter.value['balanceFeeIncrease'] * numberOfDaysFromJune / partDays;
                    bulkResult = dailyAndDays * matter.value['bulkFeeIncrease'] * numberOfDaysFromJune / partDays;

                    waterUsageCalcTotal = tier1Result + tier2Result + balanceResult + bulkResult;

                    waterUsageCalcTotal = waterUsageCalcTotal + tier1CalcResult + tier2CalcResult + balanceCalcResult + bulkCalcResult;
                } else {
                    waterUsageCalcTotal = tier1Result + tier2Result + balanceResult + bulkResult;
                }

                var finalWaterUsageResult = 0;
                if (matter.value['ctsOption'] == 'shared-percentage') {
                    var percent = matter.value['percentage'] / 100;
                    finalWaterUsageResult = waterUsageCalcTotal * percent;
                } else if (matter.value['ctsOption'] == 'do-not-apportion') {
                    finalWaterUsageResult = waterUsageCalcTotal;
                } else if (matter.value['ctsOption'] == 'Entitlement') {
                    var entitlement = matter.value['entitlementValue'].split('/');

                    var entitlement_final = parseInt(entitlement[1]) ? parseInt(entitlement[0]) / parseInt(entitlement[1]) : 0;
                    finalWaterUsageResult = waterUsageCalcTotal * entitlement_final;
                }

                return {
                    ...matter.value,
                    numberOfDays,
                    partDays,
                    dailyAndDays,
                    numberOfDaysToJune,
                    numberOfDaysFromJune,
                    tier1Result,
                    tier2Result,
                    balanceResult,
                    bulkResult,
                    balanceCalc,
                    tier1CalcResult,
                    tier2CalcResult,
                    balanceCalcResult,
                    bulkCalcResult,
                    finalWaterUsageResult,
                    dailyUsage,
                    diffAmountReading
                }

            case 'Penalty Interest':
                var contractPrice = this.state.adjustments[0].value.price - this.state.adjustments[0].value.deposit;
                return contractPrice * matter.value['rate'] / 100 * matter.value['days'] / 365;

            case 'Other Adjustment':
                return matter.value['amount'];

            case 'Fee':
                return matter.value['amount'];

            default:
                if (matter.value['days'] == 0)
                    return 0;
                var result = matter.value['amount'] * matter.value['adjustDays'] / matter.value['days'];
                return result;

        }

        return 0;
    }

    public toggleIncludeAdditionalRequirementsInTotal(checked): void {
        this.setState({
            includeAdditionalCostsInTotal: checked
        })
    }

    public render(): JSX.Element {
        const { matterDetails, adjustments, fees, additionalRequirements, payees, ourRequirements, additionalInfo, waterUsage } = this.generateUIData();
        const { showAdditionalRequirements, actionstepData, isOrgConnected, hasError, dataInputError, isInvalidMatter, actionstepPDF, showModal, savePDFSuccess, needConfirmation, isWaiting } = this.state;

        const { title, contractBalance, contractDebit, contractCredit, feeDebit, feeCredit, payeeDebit, payeeCredit, unallocated, additionalDebit, additionalCredit, hasWaterUsage } = additionalInfo;
        const isVendor = matterDetails.conveyType == 'vendor';

        const State = this.props.state;
        const domainUrl = this.props.appInfoService.domainUrl;
        const appLoadingImage = this.props.appInfoService.appLoadingImage;

        return isInvalidMatter
            ? <SettlementInvalidMatter appInfoService={this.props.appInfoService} />
            : isOrgConnected == true
                ? (
                    <div className="wrapper wrapper-content animated fadeIn vertical-container">

                        <div className="ibox float-e-margins">
                            <div className="ibox-title">
                                <h2>
                                    {title}
                                </h2>
                            </div>

                            <div className="ibox-content">

                                <div className="section">
                                    <div className="section-body">
                                        <div className="ms-Grid-row state-row">
                                            <div className="ms-Grid-col ms-sm2">
                                                <IconButton
                                                    className="button blue-icon-button"
                                                    data-automation-id="save_button"
                                                    data-cy="matter_detail_info"
                                                    onClick={() => this.showModal(modalIDs.matterDetails, { index: -1 })}
                                                    allowDisabledFocus={true}
                                                    iconProps={{ iconName: 'ChevronRightSmall' }}
                                                />
                                            </div>
                                            <div className="ms-Grid-col ms-sm10">
                                                <div className="ms-Grid" dir="ltr">

                                                    <div className="ms-Grid-row detail-row">

                                                        <div className="ms-Grid-col ms-sm2">
                                                            <b>Matter</b>
                                                        </div>
                                                        <div className="ms-Grid-col ms-sm10">
                                                            {matterDetails.matter ? matterDetails.matter + ' (' + matterDetails.matterRef + ')' : ""}
                                                        </div>
                                                    </div>

                                                    <div className="ms-Grid-row detail-row">
                                                        <div className="ms-Grid-col ms-sm2">
                                                            <b>Property</b>
                                                        </div>
                                                        <div className="ms-Grid-col ms-sm10">
                                                            {matterDetails.property}
                                                        </div>
                                                    </div>

                                                    <div className="ms-Grid-row detail-row">
                                                        <div className="ms-Grid-col ms-sm2">
                                                            <b>Adjustment Date</b>
                                                        </div>
                                                        <div className="ms-Grid-col ms-sm10">
                                                            {matterDetails.adjustmentDate == null ? "" : matterDetails.adjustmentDate.toDateString()}
                                                        </div>
                                                    </div>

                                                    <div className="ms-Grid-row detail-row">
                                                        <div className="ms-Grid-col ms-sm2">
                                                            <b>Settlement Date</b>
                                                        </div>
                                                        <div className="ms-Grid-col ms-sm10">
                                                            {matterDetails.settlementDate == null ? "" : matterDetails.settlementDate.toDateString()}
                                                        </div>
                                                    </div>

                                                    <div className="ms-Grid-row detail-row">
                                                        <div className="ms-Grid-col ms-sm2">
                                                            <b>Settlement Place</b>
                                                        </div>
                                                        <div className="ms-Grid-col ms-sm10">
                                                            {matterDetails.settlementPlace}
                                                        </div>
                                                    </div>

                                                    <div className="detail-row ms-Grid-row">
                                                        <div className="ms-Grid-col ms-sm2">
                                                            <b>Settlement Time</b>
                                                        </div>
                                                        <div className="ms-Grid-col ms-sm10">
                                                            {matterDetails.settlementTime}
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div className="section">
                                    <div className="ms-Grid-row">
                                        <div className="ms-Grid-col ms-sm10 ms-smPush2">
                                            <div className="ms-Grid-row">
                                                <div className="ms-Grid-col ms-sm2 ms-smPush8 right-align">
                                                    <b>$ DEBIT</b>
                                                </div>
                                                <div className="ms-Grid-col ms-sm2 ms-smPush8 right-align">
                                                    <b>$ CREDIT</b>
                                                </div>
                                            </div>
                                        </div>

                                    </div>

                                    <div className="section-body">

                                        {
                                            adjustments.map((adjustment, index) => {
                                                return (
                                                    <SettlementInfo
                                                        isVendor={isVendor}
                                                        toggleWaterUsage={() => this.toggleWaterUsage()}
                                                        key={'adjustment_' + index}
                                                        info={adjustment}
                                                        modalIDs={modalIDs}
                                                        index={index}
                                                        adjustmentDate={matterDetails.adjustmentDate}
                                                        showModal={(modalID, additionalInfo) => this.showModal(modalID, additionalInfo)}
                                                    />
                                                )
                                            })
                                        }
                                        {State == 'SA' && fees.map((fee, index) => {
                                            if (fee.value['showOnAdjustment']) {
                                                return (
                                                    <SettlementInfo
                                                        isVendor={isVendor}
                                                        key={'fee_' + index}
                                                        info={fee}
                                                        modalIDs={modalIDs}
                                                        index={index}
                                                        showModal={(modalID, additionalInfo) => this.showModal(modalID, additionalInfo)}
                                                    />
                                                )
                                            }

                                            return null;
                                        })}

                                        <div className="ms-Grid-row state-row">
                                            <div className="ms-Grid-col ms-sm2">
                                                <IconButton
                                                    className="button green-icon-button"
                                                    data-automation-id="save_button"
                                                    data-cy='add_adjustment_button'
                                                    onClick={() => this.showModal(modalIDs.addAdjustment, { index: -1 })}
                                                    allowDisabledFocus={true}
                                                    iconProps={{
                                                        iconName: 'Add',
                                                        style: {
                                                            fontSize: 15,
                                                            fontWeight: 900
                                                        }
                                                    }}
                                                />
                                            </div>
                                            <div className="ms-Grid-col ms-sm10">
                                                <div className="ms-Grid" dir="ltr">

                                                    <div className="ms-Grid-row detail-row">
                                                        <div className="ms-Grid-col ms-sm5 add-label">
                                                            Click + to Add an Adjustment
                                                </div>
                                                        <div className="ms-Grid-col ms-sm3">
                                                            <b>CONTRACT BALANCE</b>
                                                        </div>
                                                        <div className="ms-Grid-col ms-sm2 price-info">
                                                            {isVendor
                                                                ? <b data-cy="contract_credit_value">{contractCredit.toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</b>
                                                                : <b data-cy="contract_debit_value">{contractDebit.toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</b>}
                                                        </div>
                                                        <div className="ms-Grid-col ms-sm2 price-info">
                                                            {isVendor
                                                                ? <b data-cy="contract_debit_value">{contractDebit.toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</b>
                                                                : <b data-cy="contract_credit_value">{contractCredit.toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</b>}
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                {State == 'SA' &&
                                    <div className="section">
                                        <div className="left-align-section-header">
                                            <b>FEES</b>
                                        </div>

                                        <div className="section-body">

                                            {
                                                fees.map((fee, index) => {
                                                    if (fee.value['showOnAdjustment'] == false) {
                                                        return (
                                                            <SettlementInfo
                                                                isVendor={isVendor}
                                                                key={'fee_' + index}
                                                                info={fee}
                                                                modalIDs={modalIDs}
                                                                index={index}
                                                                showModal={(modalID, additionalInfo) => this.showModal(modalID, additionalInfo)}
                                                            />
                                                        )
                                                    } else {
                                                        return null;
                                                    }
                                                })
                                            }

                                            <div className="ms-Grid-col ms-sm12 separator">
                                                --------------
                                    </div>

                                            <div className="ms-Grid-row state-row">
                                                <div className="ms-Grid-col ms-sm2">
                                                    <IconButton
                                                        data-cy="add_fee"
                                                        className="button green-icon-button"
                                                        data-automation-id="save_button"
                                                        onClick={() => this.showModal(modalIDs.fee, { index: -1 })}
                                                        allowDisabledFocus={true}
                                                        iconProps={{
                                                            iconName: 'Add',
                                                            style: {
                                                                fontSize: 15,
                                                                fontWeight: 900
                                                            }
                                                        }}
                                                    />
                                                </div>
                                                <div className="ms-Grid-col ms-sm10">
                                                    <div className="ms-Grid" dir="ltr">

                                                        <div className="ms-Grid-row detail-row">
                                                            <div className="ms-Grid-col ms-sm5 add-label">
                                                                Click + to Add a Fee
                                                    </div>
                                                            <div className="ms-Grid-col ms-sm3">
                                                                <b>TOTAL</b>
                                                            </div>
                                                            <div className="ms-Grid-col ms-sm2 price-info">
                                                                {isVendor
                                                                    ? <b data-cy="fee_credit_value">${feeCredit.toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</b>
                                                                    : <b data-cy="fee_debit_value">${feeDebit.toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</b>}
                                                            </div>
                                                            <div className="ms-Grid-col ms-sm2 price-info">
                                                                {isVendor
                                                                    ? <b data-cy="fee_debit_value">${feeDebit.toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</b>
                                                                    : <b data-cy="fee_credit_value">${feeCredit.toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</b>}
                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                }

                                {State != "VIC" &&
                                    <div className="section">

                                        <div className="right-align-section-header">
                                            Show/Hide Additional Requirements

                                        <IconButton
                                                className="button show-hide-button"
                                                data-automation-id="save_button"
                                                onClick={() => this.toggleOurRequirements()}
                                                allowDisabledFocus={true}
                                                iconProps={{
                                                    iconName: (showAdditionalRequirements ? 'CaretUpSolid8' : 'CaretDownSolid8'),
                                                    style: { fontSize: 10 }
                                                }}
                                            />
                                        </div>
                                        {showAdditionalRequirements &&
                                            <div className="section-body animated fadeIn">

                                                <div className="ms-Grid-row additional-header">
                                                    <div className="ms-Grid-col ms-sm8 ms-smPush2">
                                                        <b>ADDITIONAL REQUIREMENTS AT SETTLEMENT</b>
                                                    </div>
                                                </div>

                                                <div className="ms-Grid-row additional-header">
                                                    <div className="ms-Grid-col ms-sm10 ms-smPush2">
                                                        <div className="ms-Grid-row">
                                                            <div className="ms-Grid-col ms-sm5">
                                                                <b>Balance at Settlement</b>
                                                            </div>

                                                            <div className="ms-Grid-col ms-sm2">
                                                                <b>${contractBalance.toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</b>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                {additionalRequirements.map((additionalRequirement, index) => {
                                                    return (
                                                        <SettlementInfo
                                                            isVendor={isVendor}
                                                            key={'additional_requirement_' + index}
                                                            info={additionalRequirement}
                                                            modalIDs={modalIDs}
                                                            index={index}
                                                            showModal={(modalID, additionalInfo) => this.showModal(modalID, additionalInfo)}
                                                        />
                                                    )
                                                })}

                                                <div className="ms-Grid-col ms-sm12 separator">
                                                    --------------
                                            </div>

                                                <div className="ms-Grid-row state-row">
                                                    <div className="ms-Grid-col ms-sm2">
                                                        <IconButton
                                                            data-cy="add_additional_requirement"
                                                            className="button green-icon-button"
                                                            data-automation-id="save_button"
                                                            onClick={() => this.showModal(modalIDs.additionalRequirements, { index: -1 })}
                                                            allowDisabledFocus={true}
                                                            iconProps={{
                                                                iconName: 'Add',
                                                                style: {
                                                                    fontSize: 15,
                                                                    fontWeight: 900
                                                                }
                                                            }}
                                                        />
                                                    </div>
                                                    <div className="ms-Grid-col ms-sm10">
                                                        <div className="ms-Grid-row detail-row">
                                                            <div className="ms-Grid-col ms-sm5 add-label">
                                                                Click + to Add Additional Costs
                                                    </div>
                                                            <div className="ms-Grid-col ms-sm3">
                                                                <b>TOTAL</b>
                                                            </div>
                                                            <div className="ms-Grid-col ms-sm2 price-info">
                                                                {isVendor
                                                                    ? <b data-cy="additional_credit_value">${additionalCredit.toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</b>
                                                                    : <b data-cy="additional_debit_value">${additionalDebit.toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</b>}
                                                            </div>
                                                            <div className="ms-Grid-col ms-sm2 price-info">
                                                                {isVendor
                                                                    ? <b data-cy="additional_debit_value">${additionalDebit.toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</b>
                                                                    : <b data-cy="additional_credit_value">${additionalCredit.toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</b>}
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        }
                                    </div>
                                }

                                <div className="section">
                                    <div className="left-align-section-header">
                                        <b>PAYEE</b>
                                    </div>

                                    <div className="section-body">

                                        {State != "VIC" &&
                                            <div className="ms-Grid-col ms-sm6 ms-smPush6 right-align-section-header">
                                                <Checkbox
                                                    label="Include additional costs in total"
                                                    onChange={(ev, checked) => this.toggleIncludeAdditionalRequirementsInTotal(checked)}
                                                />
                                            </div>
                                        }

                                        {payees.map((payee, index) => {
                                            return (
                                                <SettlementInfo
                                                    isVendor={isVendor}
                                                    key={'payee_' + index}
                                                    info={payee}
                                                    modalIDs={modalIDs}
                                                    index={index}
                                                    showModal={(modalID, additionalInfo) => this.showModal(modalID, additionalInfo)}
                                                />
                                            )
                                        })}

                                        <div className="ms-Grid-col ms-sm12 separator">
                                            --------------
                                        </div>

                                        <div className="ms-Grid-row state-row">
                                            <div className="ms-Grid-col ms-sm2">
                                                <IconButton
                                                    data-cy="add_payee"
                                                    className="button green-icon-button"
                                                    data-automation-id="save_button"
                                                    onClick={() => this.showModal(modalIDs.payeeDetails, { index: -1 })}
                                                    allowDisabledFocus={true}
                                                    iconProps={{
                                                        iconName: 'Add',
                                                        style: {
                                                            fontSize: 15,
                                                            fontWeight: 900
                                                        }
                                                    }}
                                                />
                                            </div>
                                            <div className="ms-Grid-col ms-sm10">
                                                <div className="ms-Grid" dir="ltr">

                                                    <div className="ms-Grid-row detail-row">
                                                        <div className="ms-Grid-col ms-sm5 add-label">
                                                            Click + to Add Cheque Payee's
                                                        </div>
                                                        <div className="ms-Grid-col ms-sm3">
                                                            <b>TOTAL</b>
                                                        </div>
                                                        <div className="ms-Grid-col ms-sm2 price-info">
                                                            {isVendor
                                                                ? <b data-cy="allocated_credit_value">${payeeCredit.toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</b>
                                                                : <b data-cy="allocated_debit_value">${payeeDebit.toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</b>}
                                                        </div>
                                                        <div className="ms-Grid-col ms-sm2 price-info">
                                                            {isVendor
                                                                ? <b data-cy="allocated_debit_value">${payeeDebit.toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</b>
                                                                : <b data-cy="allocated_credit_value">${payeeCredit.toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</b>}
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div className="ms-Grid-row">
                                        <div
                                            className={unallocated > 0 ? "right-align-section-header red" : "right-align-section-header green"}
                                            data-cy="unallocated_value">
                                            (unallocated: $ {unallocated.toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 })})
                                        </div>
                                    </div>
                                </div>

                                <div className="section">
                                    <div className="section-body">

                                        <div className="ms-Grid-row additional-header">
                                            <div className="ms-Grid-col ms-sm8 ms-smPush2">
                                                <b>OUR REQUIREMENTS AT SETTLEMENT</b>
                                            </div>
                                        </div>

                                        {ourRequirements.map((ourRequirement, index) => {
                                            return (
                                                <SettlementInfo
                                                    isVendor={isVendor}
                                                    key={'requirement_' + index}
                                                    info={ourRequirement}
                                                    modalIDs={modalIDs}
                                                    index={index}
                                                    showModal={(modalID, additionalInfo) => this.showModal(modalID, additionalInfo)}
                                                />
                                            )
                                        })}

                                        <div className="ms-Grid-row state-row">
                                            <div className="ms-Grid-col ms-sm2">
                                                <IconButton
                                                    data-cy="add_our_requirement"
                                                    className="button green-icon-button"
                                                    data-automation-id="save_button"
                                                    onClick={() => this.showModal(modalIDs.ourRequirements, { index: -1 })}
                                                    allowDisabledFocus={true}
                                                    iconProps={{
                                                        iconName: 'Add',
                                                        style: {
                                                            fontSize: 15,
                                                            fontWeight: 900
                                                        }
                                                    }}
                                                />
                                            </div>
                                            <div className="ms-Grid-col ms-sm10">
                                                <div className="ms-Grid" dir="ltr">

                                                    <div className="ms-Grid-row detail-row">
                                                        <div className="ms-Grid-col ms-sm12 add-label">
                                                            Click + to Add additional requirements
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                {this.state.showWaterUsage && hasWaterUsage == true &&
                                    <WaterUsageSection waterUsage={waterUsage} />
                                }
                            </div>

                            <div className="ibox-footer">
                                <PrimaryButton
                                    className="button"
                                    data-automation-id="save_button"
                                    data-cy="save_button"
                                    text="Save"
                                    onClick={() => this.save()}
                                    allowDisabledFocus={true}
                                />

                                {(State == 'SA' && isVendor) ?
                                    <div>
                                        <PrimaryButton
                                            className="button"
                                            data-automation-id="generate_button"
                                            data-cy="generate_button"
                                            text="Generate Vendor PDF"
                                            onClick={() => this.generatePDF(false)}
                                            allowDisabledFocus={true}
                                        />

                                        <PrimaryButton
                                            className="button"
                                            data-automation-id="generate_button"
                                            data-cy="generate_button"
                                            text="Generate Adjustment PDF"
                                            onClick={() => this.generatePDF(true)}
                                            allowDisabledFocus={true}
                                        />
                                    </div>
                                    :
                                    <PrimaryButton
                                        className="button"
                                        data-automation-id="generate_button"
                                        data-cy="generate_button"
                                        text="Generate a PDF"
                                        onClick={() => this.generatePDF()}
                                        allowDisabledFocus={true}
                                    />
                                }

                                <PrimaryButton
                                    className="button"
                                    data-automation-id="save_pdf_to_actionstep"
                                    data-cy="save_pdf_to_actionstep"
                                    text="Save PDF to Actionstep"
                                    onClick={() => this.generatePDF(false, true)}
                                    allowDisabledFocus={true}
                                />

                                <DefaultButton
                                    primary
                                    className="button"
                                    data-automation-id="test"
                                    allowDisabledFocus={true}
                                    persistMenu={true}
                                    text="Email Options"
                                    // tslint:disable-next-line:jsx-no-lambda
                                    onMenuClick={ev => {
                                    }}
                                    menuProps={{
                                        items: [
                                            {
                                                key: 'emailMessage',
                                                text: 'Email Cheques to Bank (Outlook Only)',
                                                iconProps: { iconName: 'Mail' },
                                                onClick: () => this.sendEmail(1)
                                            },
                                            {
                                                key: 'calendarEvent',
                                                text: 'Email Cheques to OS',
                                                iconProps: { iconName: 'System' },
                                                onClick: () => this.sendEmail(2)
                                            }
                                        ],
                                        directionalHintFixed: true
                                    }}
                                />

                                <DefaultButton
                                    className="button red-button"
                                    data-automation-id="delete_button"
                                    data-cy="delete_button"
                                    text="Delete"
                                    onClick={() => this.delete()}
                                    allowDisabledFocus={true}
                                />
                            </div>
                        </div>

                        <div data-cy="modal-header" />
                        <Modal
                            isOpen={needConfirmation}
                            isBlocking={false}
                            onDismiss={() => this.loadData(false)}
                            className={needConfirmation ? "animated fadeIn modal settlement-calculator" : "animated fadeOut modal settlement-calculator"}
                            dragOptions={this._dragOptions}
                        >
                            <div className="modal-header" data-cy="modal-header">
                                <span className="modal-title">Confirm</span>

                                <IconButton
                                    className="modal-close-button"
                                    onClick={() => this.loadData(false)}
                                    iconProps={{
                                        iconName: 'ChromeClose',
                                        style: {
                                            fontSize: 12,
                                            fontWeight: 500
                                        }
                                    }}
                                />
                            </div>

                            <div className="modal-body">
                                These fields have been changed since the last edit.
                                Would you like to use the new data?

                                <br /><br />

                                <div className="ms-Grid-row">
                                    <div className="ms-Grid-col ms-sm4">
                                        <b>Field</b>
                                    </div>
                                    <div className="ms-Grid-col ms-sm4">
                                        <b>Old Value</b>
                                    </div>
                                    <div className="ms-Grid-col ms-sm4">
                                        <b>New Value</b>
                                    </div>
                                </div>
                                {Object.keys(actionstepData).map(key => {
                                    if (actionstepData[key].changed) {
                                        return (
                                            <div className="ms-Grid-row" key={key}>
                                                <div className="ms-Grid-col ms-sm4">
                                                    {actionstepData[key].label}
                                                </div>
                                                <div className="ms-Grid-col ms-sm4">
                                                    {actionstepData[key].oldValue}
                                                </div>
                                                <div className="ms-Grid-col ms-sm4">
                                                    {actionstepData[key].displayValue}
                                                </div>
                                            </div>
                                        );
                                    }
                                })}
                            </div>

                            <div className="modal-footer">
                                <DefaultButton
                                    className="button gray-button"
                                    data-automation-id="use_old_button"
                                    data-cy="use_old_button"
                                    text="No"
                                    onClick={() => this.loadData(false)}
                                    allowDisabledFocus={true}
                                />

                                <PrimaryButton
                                    className="button"
                                    data-automation-id="use_new_button"
                                    data-cy="use_new_button"
                                    text="Yes"
                                    onClick={() => this.loadData(true)}
                                    allowDisabledFocus={true}
                                />
                            </div>
                        </Modal>

                        <Modal isOpen={isWaiting}
                            isBlocking={true}
                            className={isWaiting != null ? "animated fadeIn modal settlement-calculator" : "animated fadeOut modal settlement-calculator"}
                        >
                            <div className="modal-body">

                                <div className="ms-Grid-row loading-modal">
                                    <img src={appLoadingImage} className="col-lg-3"
                                        alt="Logo" />
                                    <h2>Please wait...</h2>
                                </div>

                            </div>
                        </Modal>

                        <Modal isOpen={savePDFSuccess}
                            isBlocking={false}
                            onDismiss={() => this.closeModal()}
                            className={savePDFSuccess ? "animated fadeIn modal settlement-calculator" : "animated fadeOut modal settlement-calculator"}
                        >
                            <div className="modal-header">
                                <span className="modal-title">PDF saved to Actionstep successfully!</span>
                            </div>
                            <div className="modal-body">
                                Your PDF has been saved to Actionstep at the following location: <a rel="noopener noreferrer" href={actionstepPDF.url} target="_blank" download >{actionstepPDF.fileName}</a> <br />
                                or <a rel="noopener noreferrer" href={actionstepPDF.documentUrl} target="_blank"><b>view in Actionstep</b></a> (This link requires you to be logged in to the related Actionstep org)
                            </div>

                            <div className="modal-footer">

                                <DefaultButton
                                    className="button gray-button"
                                    data-automation-id="close_modal"
                                    data-cy="close_modal"
                                    text="OK"
                                    onClick={() => this.closeModal()}
                                    allowDisabledFocus={true}
                                />
                            </div>
                        </Modal>

                        <Modal
                            isOpen={showModal != null}
                            isBlocking={false}
                            onDismiss={() => this.closeModal()}
                            className={showModal != null ? "animated fadeIn modal settlement-calculator" : "animated fadeOut modal settlement-calculator"}
                            dragOptions={this._dragOptions}
                        >

                            {showModal != null &&
                                <Modals
                                    modalID={showModal}
                                    modalIDs={modalIDs}
                                    closeModal={() => this.closeModal()}
                                    saveModal={() => this.saveModal()}
                                    deleteData={(index, modalID) => this.deleteData(index, modalID)}
                                    updateValue={(newValue, whichValue, needRefresh = false) => this.updateValue(newValue, whichValue, needRefresh)}
                                    matterDetails={this.state.matterDetails}
                                    updatedState={this.updatedState}
                                    index={this.state.selectedIndex}
                                    balanceFunds={payeeDebit - payeeCredit}
                                    actionstepData={actionstepData}
                                    dataInputError={dataInputError}
                                />
                            }

                        </Modal>

                    </div>
                ) : hasError == true ? (
                    <div className="wrapper wrapper-content animated fadeInRight vertical-container">
                        <div className="row">
                            <div className="col-lg-12">
                                <div className="panel panel-danger">
                                    <div className="panel-heading">
                                        <i className="fa fa-exclamation"></i> <span className="m-l-xs">Sorry, something went wrong</span>
                                    </div>
                                    <div className="panel-body p-v-xs">
                                        <p className="text-danger">
                                            Please contact <a href={"mailto:support@" + domainUrl + "?subject=Legacy%20Settlement%20Calculator"}> support@{domainUrl}</a>.
                                        </p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                ) : hasError == false
                        ?
                        <div className="wrapper wrapper-content animated fadeInRight vertical-container">
                            <ConnectToActionstep />
                        </div>
                        : <div />;
    }

    private generateUIData(isAdjustment = false) {
        var contractDebit = 0, contractCredit = 0;
        var waterUsage = {}, hasWaterUsage = false;
        var { includeAdditionalCostsInTotal } = this.state;

        var adjustments = this.state.adjustments.map((adjustment, index) => {
            var result = this.calculate(adjustment);

            if (adjustment.type == 'Water Usage') {
                waterUsage = result;
                result = result['finalWaterUsageResult'];

                hasWaterUsage = true;
            }

            if (adjustment.type == 'Contract Price') {
                contractDebit += adjustment.value['price'];
                contractCredit += adjustment.value['deposit'];
            } else {
                if (adjustment.value['status'] == 'unpaid' || adjustment.value['status'] == 'less')
                    contractCredit += result;
                else
                    contractDebit += result;
            }

            return {
                ...adjustment,
                title: adjustment.type,
                result: result,
                total: contractDebit - contractCredit,
                debit: contractDebit,
                credit: contractCredit
            };
        });

        var feeDebit = contractDebit, feeCredit = contractCredit, fees = [];
        if (this.state.matterDetails.state == 'SA') {
            fees = this.state.fees.map((fee, index) => {
                var result = this.calculate(fee);

                if (fee.value['showOnAdjustment'] == true) {
                    if (fee.value['status'] == 'less') {
                        contractCredit += result;
                        feeCredit += result;
                    } else {
                        contractDebit += result;
                        feeDebit += result;
                    }

                    return {
                        ...fee,
                        title: fee.type,
                        result: result,
                        total: contractDebit - contractCredit,
                        debit: contractDebit,
                        credit: contractCredit
                    };
                } else {
                    if (fee.value['status'] == 'less')
                        feeCredit += result;
                    else
                        feeDebit += result;

                    return {
                        ...fee,
                        title: fee.type,
                        result: result,
                        total: feeDebit - feeCredit,
                        debit: feeDebit,
                        credit: feeCredit
                    };
                }
            });
        }

        var additionalDebit = feeDebit, additionalCredit = feeCredit;

        var additionalRequirements = this.state.additionalRequirements.map((additionalRequirement, index) => {
            var result = additionalRequirement.value['amount'];
            if (additionalRequirement.value['status'] == 'less') {
                additionalCredit += result;
            } else {
                additionalDebit += result;
            }

            return {
                ...additionalRequirement,
                result: result,
                total: additionalDebit - additionalCredit,
                debit: additionalDebit,
                credit: additionalCredit
            };
        })

        var payeeDebit = includeAdditionalCostsInTotal ? additionalDebit : feeDebit,
            payeeCredit = includeAdditionalCostsInTotal ? additionalCredit : feeCredit,
            payeeTotal = 0;

        var payees = this.state.payees.map((payee, index) => {
            var result = payee.value['amount'];
            payeeCredit += result;
            payeeTotal += result;
            result = Math.abs(result);

            return {
                ...payee,
                result: result,
                total: payeeCredit,
                debit: 0,
                credit: payeeCredit,
            };
        })

        var ourRequirements = this.state.ourRequirements.map((ourRequirement, index) => {
            return {
                ...ourRequirement,
                result: 0,
                total: 0
            };
        })

        var State = this.state.matterDetails.state;
        var isVendor = this.state.matterDetails.conveyType && this.state.matterDetails.conveyType.toLowerCase() == 'vendor';;

        return {
            matterDetails: this.state.matterDetails,
            adjustments,
            fees,
            additionalRequirements,
            payees,
            ourRequirements,
            waterUsage,
            additionalInfo: {
                title: isAdjustment ? "ADJUSTMENT STATEMENT" : State == "VIC" ? (isVendor ? "Vendor's " : "Purchaser's ") + "Statement of Adjustments" :
                    (isVendor ? "VENDOR'S " : "PURCHASER'S ") + (State == "NSW" ? "SETTLEMENT ADJUSTMENT SHEET" : "SETTLEMENT STATEMENT"),
                contractBalance: feeDebit - feeCredit,
                contractDebit,
                contractCredit,
                feeDebit,
                feeCredit,
                totalPayee: payeeCredit,
                payeeCredit: payeeCredit,
                payeeDebit: payeeDebit,
                payeeTotal: payeeTotal,
                totalAdditionalRequirements: additionalDebit - additionalCredit,
                totalWithAdditionalRequirements: contractDebit - contractCredit,
                additionalDebit: additionalDebit,
                additionalCredit: additionalCredit,
                unallocated: payeeDebit - payeeCredit,
                showAdditionalRequirements: this.state.showAdditionalRequirements,
                hasWaterUsage: hasWaterUsage
            }
        }
    }

    private updateValue(newValue, whichValue, needRefresh = false): void {
        if (typeof (this.updatedState[whichValue]) == 'number') {
            newValue = parseFloat(newValue);
        }

        this.updatedState[whichValue] = newValue;

        if (needRefresh) {
            this.setState({
                refresh: !this.state.refresh
            })
        }
    }

    private toggleWaterUsage() {
        this.setState({
            showWaterUsage: !this.state.showWaterUsage
        })
    }

    private deleteData(index, modalID): void {
        switch (modalID) {
            case modalIDs.additionalRequirements:
                var additionalRequirements = [...this.state.additionalRequirements];
                additionalRequirements.splice(index, 1);
                this.setState({
                    additionalRequirements: additionalRequirements
                });

                break;

            case modalIDs.payeeDetails:
                var payees = [...this.state.payees];
                payees.splice(index, 1);
                this.setState({
                    payees: payees
                });

                break;

            case modalIDs.ourRequirements:
                var ourRequirements = [...this.state.ourRequirements];
                ourRequirements.splice(index, 1);
                this.setState({
                    ourRequirements: ourRequirements
                });

                break;

            case modalIDs.fee:
                var fees = [...this.state.fees];
                fees.splice(index, 1);
                this.setState({
                    fees: fees
                });

                break;

            default:
                var adjustments = [...this.state.adjustments];
                adjustments.splice(index, 1);
                this.setState({
                    adjustments: adjustments
                });

                break;
        }

        this.closeModal();
    }

    private showModal(modalID, additionalInfo = null): void {
        var updatedState = {};

        switch (modalID) {
            case modalIDs.matterDetails:
                updatedState = Object.assign({}, this.state.matterDetails);
                break;

            case modalIDs.addAdjustment:
                updatedState = {
                    itemType: 'Water Usage'
                };
                break;

            case modalIDs.contractPrice:
                updatedState = this.state.adjustments[additionalInfo['index']].value;
                break;

            case modalIDs.releaseFee:

                if (additionalInfo['index'] < 0) {
                    updatedState = {
                        mortgages: 0,
                        each: 0,
                        status: 'less'
                    };
                } else {
                    updatedState = this.state.adjustments[additionalInfo['index']].value;
                }
                break;

            case modalIDs.waterUsage:

                if (additionalInfo['index'] < 0) {
                    updatedState = {
                        method: 'search-reading',
                        paidDate: null,
                        paidReadingAmount: 0,
                        searchDate: null,
                        searchReadingAmount: 0,
                        tier1Charge: 0,
                        tier1KlCount: 0,
                        tier1FeeIncrease: 0,
                        tier2Charge: 0,
                        tier2KlCount: 0,
                        tier2FeeIncrease: 0,
                        balanceCharge: 0,
                        balanceFeeIncrease: 0,
                        bulkCharge: 0,
                        bulkFeeIncrease: 0,
                        ctsOption: 'do-not-apportion',
                        percentage: 0,
                        entitlementValue: 0,
                        averageKlCount: 0,
                        status: 'less'
                    };
                } else {
                    updatedState = this.state.adjustments[additionalInfo['index']].value;
                }
                break;

            case modalIDs.otherAdjustment:
                if (additionalInfo['index'] < 0) {
                    updatedState = {
                        description: '',
                        amount: 0,
                        status: 'plus'
                    };
                } else {
                    updatedState = this.state.adjustments[additionalInfo['index']].value;
                }
                break;

            case modalIDs.penaltyInterest:
                if (additionalInfo['index'] < 0) {
                    updatedState = {
                        rate: 0,
                        from: null,
                        to: null,
                        status: 'paid'
                    };
                } else {
                    updatedState = this.state.adjustments[additionalInfo['index']].value;
                }
                break;

            case modalIDs.otherAdjustmentDate:
                if (additionalInfo['index'] < 0) {
                    updatedState = {
                        description: '',
                        amount: 0,
                        from: null,
                        to: null,
                        status: 'paid'
                    };
                } else {
                    updatedState = this.state.adjustments[additionalInfo['index']].value;
                }
                break;

            case modalIDs.fee:
                if (additionalInfo['index'] < 0) {
                    updatedState = {
                        description: '',
                        amount: 0,
                        status: 'plus',
                        showOnAdjustment: false
                    };
                } else {
                    updatedState = this.state.fees[additionalInfo['index']].value;
                }
                break;

            case modalIDs.additionalRequirements:
                if (additionalInfo['index'] < 0) {
                    updatedState = {
                        description: '',
                        amount: 0,
                        status: 'plus'
                    };
                } else {
                    updatedState = this.state.additionalRequirements[additionalInfo['index']].value;
                }
                break;

            case modalIDs.payeeDetails:
                if (additionalInfo['index'] < 0) {
                    updatedState = {
                        description: '',
                        amount: 0
                    };
                } else {
                    updatedState = this.state.payees[additionalInfo['index']].value;
                }
                break;

            case modalIDs.ourRequirements:
                if (additionalInfo['index'] < 0) {
                    updatedState = {
                        detail: ''
                    };
                } else {
                    updatedState = this.state.ourRequirements[additionalInfo['index']].value;
                }
                break;
            default:

                if (additionalInfo['index'] < 0) {
                    updatedState = {
                        amount: 0,
                        from: null,
                        to: null,
                        status: 'paid'
                    };
                } else {
                    updatedState = this.state.adjustments[additionalInfo['index']].value;
                }
                break;
        }
        this.updatedState = updatedState;

        this.setState({
            showModal: modalID,
            selectedIndex: additionalInfo['index']
        })
    }

    private updateAdjustmentDates(): void {
        var { matterDetails, adjustments } = this.state;

        var newAdjustments = adjustments.map(adjustment => {
            if (adjustment.value['adjustDays']) {
                var adjustDays = adjustment.value['status'] == 'unpaid' ? Math.floor((matterDetails["adjustmentDate"].valueOf() - adjustment.value['from']) / (1000 * 60 * 60 * 24)) + 1
                    : Math.floor((adjustment.value['to'] - matterDetails["adjustmentDate"].valueOf()) / (1000 * 60 * 60 * 24));

                return {
                    ...adjustment,
                    value: {
                        ...adjustment.value,
                        adjustDays
                    }
                }
            }

            return adjustment;
        });

        this.setState({
            adjustments: newAdjustments
        });
    }

    private async saveModal(): Promise<any> {
        var modalID = this.state.showModal;

        switch (modalID) {
            case modalIDs.matterDetails:
                if (this.updatedState['adjustmentDate'] == null || this.updatedState['settlementDate'] == null) {
                    this.setState({
                        dataInputError: errorMessages.dateInputError
                    })
                    return;
                }

                this.updatedState['adjustmentDate'] = this.convertStringToDate(this.updatedState['adjustmentDate'].toISOString());
                this.updatedState['settlementDate'] = this.convertStringToDate(this.updatedState['settlementDate'].toISOString());

                await this.setState({
                    matterDetails: MatterDetails.fromJS(this.updatedState)
                })

                this.updateAdjustmentDates();

                this.props.changeState(this.updatedState["state"]);

                break;

            case modalIDs.addAdjustment:
                var newModalID = this.updatedState['itemType'];

                this.showModal(newModalID, { index: -1 });
                return;

            case modalIDs.penaltyInterest:
                if (this.updatedState['from'] == null || this.updatedState['to'] == null) {
                    this.setState({
                        dataInputError: errorMessages.dateInputError
                    })
                    return;
                }

                this.updatedState['from'] = this.convertStringToDate(this.updatedState['from'].toISOString());
                this.updatedState['to'] = this.convertStringToDate(this.updatedState['to'].toISOString());

                var newDays = Math.floor((this.updatedState['to'] - this.updatedState['from']) / (1000 * 60 * 60 * 24)) + 1;
                this.updatedState['days'] = newDays > 0 ? newDays : 0;

                var adjustments = [...this.state.adjustments];

                if (this.state.selectedIndex < 0) {
                    var newAdjustment = {
                        value: Object.assign({}, this.updatedState),
                        type: modalID
                    };

                    adjustments.push(newAdjustment);

                } else {
                    adjustments[this.state.selectedIndex].value = this.updatedState;
                }

                this.setState({
                    adjustments: adjustments
                });

                break;

            case modalIDs.waterUsage:
                if (this.updatedState['paidDate'] == null || this.updatedState['searchDate'] == null) {
                    this.setState({
                        dataInputError: errorMessages.dateInputError
                    })
                    return;
                }

                this.updatedState['paidDate'] = this.convertStringToDate(this.updatedState['paidDate'].toISOString());
                this.updatedState['searchDate'] = this.convertStringToDate(this.updatedState['searchDate'].toISOString());

            case modalIDs.contractPrice:
            case modalIDs.releaseFee:
            case modalIDs.otherAdjustment:

                var adjustments = [...this.state.adjustments];

                if (this.state.selectedIndex < 0) {
                    var newAdjustment = {
                        value: Object.assign({}, this.updatedState),
                        type: modalID
                    };

                    adjustments.push(newAdjustment);

                } else {
                    adjustments[this.state.selectedIndex].value = this.updatedState;
                }

                this.setState({
                    adjustments: adjustments
                });

                break;

            case modalIDs.fee:

                var fees = [...this.state.fees];

                if (this.state.selectedIndex < 0) {
                    var newFee = {
                        value: Object.assign({}, this.updatedState),
                        type: modalID
                    };

                    fees.push(newFee);

                } else {
                    fees[this.state.selectedIndex].value = this.updatedState;
                }

                this.setState({
                    fees: fees
                });

                break;

            case modalIDs.additionalRequirements:
                var additionalRequirements = [...this.state.additionalRequirements];

                if (this.state.selectedIndex < 0) {
                    var additionalRequirement = {
                        value: Object.assign({}, this.updatedState),
                        type: modalID
                    };

                    additionalRequirements.push(additionalRequirement);

                } else {
                    additionalRequirements[this.state.selectedIndex].value = this.updatedState;
                }

                this.setState({
                    additionalRequirements: additionalRequirements
                });

                break;

            case modalIDs.payeeDetails:
                var payees = [...this.state.payees];

                if (this.state.selectedIndex < 0) {
                    var payee = {
                        value: Object.assign({}, this.updatedState),
                        type: modalID
                    };

                    payees.push(payee);

                } else {
                    payees[this.state.selectedIndex].value = this.updatedState;
                }

                this.setState({
                    payees: payees
                });

                break;

            case modalIDs.ourRequirements:
                var ourRequirements = [...this.state.ourRequirements];

                if (this.state.selectedIndex < 0) {
                    var ourRequirement = {
                        value: Object.assign({}, this.updatedState),
                        type: modalID
                    };

                    ourRequirements.push(ourRequirement);

                } else {
                    ourRequirements[this.state.selectedIndex].value = this.updatedState;
                }

                this.setState({
                    ourRequirements: ourRequirements
                });

                break;

            default:

                if (this.updatedState['from'] == null || this.updatedState['to'] == null) {
                    this.setState({
                        dataInputError: errorMessages.dateInputError
                    })
                    return;
                }

                this.updatedState['from'] = this.convertStringToDate(this.updatedState['from'].toISOString());
                this.updatedState['to'] = this.convertStringToDate(this.updatedState['to'].toISOString());

                var adjustDays = this.updatedState['status'] == 'unpaid' ? Math.floor((this.state.matterDetails.adjustmentDate.valueOf() - this.updatedState['from']) / (1000 * 60 * 60 * 24)) + 1
                    : Math.floor((this.updatedState['to'] - this.state.matterDetails.adjustmentDate.valueOf()) / (1000 * 60 * 60 * 24));

                var days = Math.floor((this.updatedState['to'] - this.updatedState['from']) / (1000 * 60 * 60 * 24)) + 1;

                this.updatedState['adjustDays'] = adjustDays;
                this.updatedState['days'] = days;

                var adjustments = [...this.state.adjustments];

                if (this.state.selectedIndex < 0) {
                    var newAdjustment = {
                        value: Object.assign({}, this.updatedState),
                        type: modalID
                    };

                    adjustments.push(newAdjustment);

                } else {
                    adjustments[this.state.selectedIndex].value = this.updatedState;
                }

                this.setState({
                    adjustments: adjustments
                });

                break;
        }

        this.closeModal();
    }

    private closeModal(): void {
        this.updatedState = {};

        this.setState({
            showModal: null,
            dataInputError: "",
            savePDFSuccess: false
        })
    }

    private toggleOurRequirements(): void {
        this.setState({
            showAdditionalRequirements: !this.state.showAdditionalRequirements
        });
    }

    private save(): void {
        var settlementData = this.generateUIData();

        var { matterInfo } = this.state;

        const params = {
            actionstepOrg: matterInfo.orgKey,
            matterId: matterInfo.matterId,
            version: 0,
            settlementData: settlementData,
            acionstepData: {}
        }

        this.props.saveSettlementMatter(params);

        this.setState({
            isWaiting: true
        })
    }

    private delete(): void {
        const { matterInfo } = this.state;

        const params = {
            actionstepOrg: matterInfo.orgKey,
            matterId: matterInfo.matterId,
        }

        this.props.deleteSettlementMatter(params);

        this.setState({
            isWaiting: true
        })
    }

    private generatePDF(isAdjustment = false, saveToActionstep = false): void {
        var { matterDetails, adjustments, fees, additionalRequirements, payees, ourRequirements, additionalInfo, waterUsage } = this.generateUIData(isAdjustment);

        adjustments = adjustments.map((adjustment, index) => {

            var newAdjustment = {
                ...adjustment,
                ...adjustment["value"]
            }

            delete newAdjustment.value;

            return newAdjustment;

        })

        adjustments = adjustments.map((item, index) => {
            item["status"] = item["status"] == 'unpaid' ? "Less" : "Plus";

            return item;
        })

        fees = fees.map((item, index) => {
            var newItem = {
                ...item,
                ...item.value
            }

            delete newItem.value;

            return newItem;
        });

        additionalRequirements = additionalRequirements.map((item, index) => {
            var newItem = {
                ...item,
                ...item["value"]
            }

            delete newItem.value;

            return newItem;
        });

        payees = payees.map((item, index) => {
            var newItem = {
                ...item,
                ...item["value"]
            }

            delete newItem.value;

            return newItem;
        });

        ourRequirements = ourRequirements.map((item, index) => {
            var newItem = {
                ...item,
                ...item["value"]
            }

            delete newItem.value;

            return newItem;
        });

        additionalInfo["isAdjustment"] = isAdjustment;
        additionalInfo["saveToActionstep"] = saveToActionstep;

        var settlementData: SettlementInfoViewModel = new SettlementInfoViewModel({
            matterDetails: new MatterDetails(matterDetails),
            adjustments,
            fees,
            additionalRequirements,
            payees,
            ourRequirements,
            waterUsage,
            additionalInfo
        });

        const { matterInfo } = this.state;

        const params: SettlementMatterViewModel = new SettlementMatterViewModel({
            actionstepOrg: matterInfo.orgKey,
            matterId: matterInfo.matterId,
            version: 0,
            settlementData: settlementData
        });

        if (saveToActionstep) {
            this.props.savePDF(params);
        } else {
            this.props.generatePDF(params);
        }

        this.setState({
            isWaiting: true
        })
    }

    private sendEmail(type): void {
        var uiData = this.generateUIData();

        var emailContent = this.state.matterDetails.matter + "%0AProperty: " + this.state.matterDetails.property + "%0A%0A";

        emailContent += `Dear Sir/Madam %0AWe confirm settlement of the above matter has been booked for ` + this.state.matterDetails.settlementTime +
            ` on ` + this.state.matterDetails.settlementDate.toDateString() + ' at the office of ' + this.state.matterDetails.settlementPlace + '.%0A';

        emailContent += `We hereby direct cheques to be made payable as follows:%0A%0A`;

        emailContent += 'The total of the cheque above is $' + uiData.additionalInfo.totalPayee + `.`;

        emailContent += `%0A%0ARegards.%0A`;

        var mailToLink = "mailto:" + this.state.lc + "?subject=Settlement Cheque Details - Matter Ref#" + this.state.matterDetails.matterRef + "&body=" + emailContent;

        window.location.href = mailToLink;
    }
}

const mapStateToProps = state => {
    return {
        state: state.settlementInfo.state,
        success: state.settlementInfo.success,
        gotResponse: state.settlementInfo.gotResponse,
        settlementMatter: state.settlementInfo.settlementMatter,
        actionstepPDF: state.settlementInfo.actionstepPDF,
        requestType: state.settlementInfo.requestType,
        isOrgConnected: state.settlementInfo.isOrgConnected,
        error: state.settlementInfo.error
    }
}

const mapDispatchToProps = dispatch => {
    return {
        generatePDF: params => dispatch(generatePDF(params)),
        savePDF: params => dispatch(savePDF(params)),
        getSettlementMatter: params => dispatch(getSettlementMatter(params)),
        saveSettlementMatter: params => dispatch(saveSettlementMatter(params)),
        deleteSettlementMatter: params => dispatch(deleteSettlementMatter(params)),
        changeState: params => dispatch(changeState(params)),
        clearSettlementState: () => dispatch(clearSettlementState())
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(SettlementCalculator);