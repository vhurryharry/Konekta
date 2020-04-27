import * as React from 'react';
import { connect } from 'react-redux';

import { DefaultButton, PrimaryButton, IconButton } from 'office-ui-fabric-react/lib/Button';
import { Modal, IDragOptions } from 'office-ui-fabric-react/lib/Modal';
import { MessageBar, MessageBarType } from 'office-ui-fabric-react/lib/MessageBar'
import { ContextualMenu } from 'office-ui-fabric-react/lib/ContextualMenu';
import { Checkbox } from 'office-ui-fabric-react/lib/Checkbox';
import { Icon } from 'office-ui-fabric-react/lib/Icon';
import { Link } from 'office-ui-fabric-react/lib/Link'
import * as toastr from 'toastr';

import SettlementInfo from './settlementInfo';
import WaterUsageSection from './waterUsage'
import Modals from './modals';
import SettlementInvalidMatter from './settlementInvalidMatter'

import { ModalIDs } from './common'

import * as CONSTANTS from 'containers/calculators/settlement/redux/constants'
import {
    generatePDF,
    changeState,
    clearSettlementState,
    saveSettlementMatter,
    getSettlementMatter,
    savePDF,
    deleteSettlementMatter
} from 'containers/calculators/settlement/redux/actions';

import {
    SettlementMatterViewModel,
    SettlementInfo as SettlementInfoViewModel,
    ActionstepMatter,
    MatterDetails,
    ErrorViewModel,
    ActionstepMatterInfo,
    ActionstepDocument
} from 'utils/wcaApiTypes';

import { AppState, JwtMatterInfo } from 'app.types';

import Tools from 'utils/tools'

import "./settlementCalculator.css"

interface IMapStateToProps {
    state: string;
    jwtMatterInfo: JwtMatterInfo | undefined;
    success: boolean;
    gotResponse: boolean;
    settlementMatter: SettlementMatterViewModel | undefined;
    actionstepPDF: ActionstepDocument | undefined;
    orgConnected: boolean | null;
    error: ErrorViewModel | undefined;
    requestType: string;
}

interface IMapDispatchToProps {
    generatePDF: (params: SettlementMatterViewModel) => void;
    savePDF: (params: SettlementMatterViewModel) => void;
    getSettlementMatter: (params: ActionstepMatterInfo) => void;
    saveSettlementMatter: (params: SettlementMatterViewModel) => void;
    deleteSettlementMatter: (params: ActionstepMatterInfo) => void;
    changeState: (params: string) => void;
    clearSettlementState: () => void;
}

interface IAppProps {
}

type AppProps = IMapStateToProps & IMapDispatchToProps & IAppProps;

type AppStates = {
    matterDetails: MatterDetails,
    matterInfo: ActionstepMatterInfo | null;
    showModal: ModalIDs | null,      // This is the title of the modal when the modal is visible. If the modal is invisible this is null.
    adjustments: { [key: string]: any; }[] | null,
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
    modifiedActionstepData: ModifiedActionstepData | null,
    orgConnected: boolean | null,
    includeAdditionalCostsInTotal: boolean,
    isWaiting: boolean,
    needConfirmation: boolean,
    isInvalidMatter: boolean,
    savePDFSuccess: boolean,
    hasError: boolean | null,
    dataInputError: string,
    actionstepPDF: ActionstepDocument | undefined,
    loadData: boolean | null
}

type ModifiedActionstepValue = {
    value: any,
    oldValue: any,
    displayValue: string,
    label: string,
    update: boolean,
    changed: boolean
}

type ModifiedActionstepData = {
    matterRef: ModifiedActionstepValue,
    matter: ModifiedActionstepValue,
    property: ModifiedActionstepValue,
    adjustmentDate: ModifiedActionstepValue,
    settlementDate: ModifiedActionstepValue,
    settlementPlace: ModifiedActionstepValue,
    settlementTime: ModifiedActionstepValue,
    state: ModifiedActionstepValue,
    conveyType: ModifiedActionstepValue,
    price: ModifiedActionstepValue,
    deposit: ModifiedActionstepValue,
    [key: string]: ModifiedActionstepValue
}

const errorMessages = {
    dateInputError: "Please input all the dates..."
}

export class SettlementCalculator extends React.Component<AppProps, AppStates> {
    constructor(props: any) {
        super(props);

        this.state = {
            matterDetails: new MatterDetails(),
            matterInfo: null,
            actionstepData: new ActionstepMatter(),
            modifiedActionstepData: null,
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
            orgConnected: null,
            includeAdditionalCostsInTotal: false,
            isWaiting: false,
            hasError: null,
            dataInputError: "",
            needConfirmation: false,
            isInvalidMatter: false,
            savePDFSuccess: false,
            actionstepPDF: new ActionstepDocument(),
            loadData: null
        }
    }

    updatedState: any = {};

    _dragOptions: IDragOptions = {
        moveMenuItemText: "Move",
        closeMenuItemText: "Close",
        dragHandleSelector: '.modal-header',
        menu: ContextualMenu
    }

    public componentDidMount(): void {
        this.loadActionstepMatter();
    }

    public shouldComponentUpdate(nextProps: AppProps, nextState: AppStates): boolean {
        const { loadData } = this.state;

        if (loadData !== null) {
            this.loadData(loadData);
        }

        return true;
    }

    private loadActionstepMatter(): void {

        let adjustments = [
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

        const { jwtMatterInfo } = this.props;
        let matterInfo: ActionstepMatterInfo | null = null;
        if (jwtMatterInfo === undefined) {

            const queryString = require('query-string');

            const urlParams = queryString.parse(window.location.search);

            matterInfo = Tools.ParseActionstepMatterInfo(urlParams);
        }
        else {
            matterInfo = new ActionstepMatterInfo({
                orgKey: jwtMatterInfo.orgKey,
                matterId: jwtMatterInfo.matterId
            });
        }

        if (matterInfo == null) {
            this.setState({
                isInvalidMatter: true
            });

            return;
        }

        this.setState({
            matterInfo
        });

        this.props.getSettlementMatter(matterInfo);
    }

    public async loadData(useNewValue = false): Promise<void> {
        const { settlementData, modifiedActionstepData } = this.state;

        const matterDetails: MatterDetails = (!useNewValue && settlementData.matterDetails)
            ? settlementData.matterDetails
            : this.state.matterDetails;

        matterDetails.adjustmentDate = new Date(matterDetails.adjustmentDate);
        matterDetails.settlementDate = new Date(matterDetails.settlementDate);

        await this.setState({
            matterDetails: matterDetails,
            adjustments: settlementData.adjustments === undefined ? [] : settlementData.adjustments.map(adjustment => {
                let newValue: any = {};

                if (adjustment.type === 'Contract Price') {
                    if (useNewValue) {
                        newValue = {
                            'price': modifiedActionstepData!.price.value,
                            'deposit': modifiedActionstepData!.deposit.value
                        };
                    } else {
                        newValue = adjustment.value;
                    }
                } else {

                    Object.keys(adjustment.value).forEach(key => {
                        if (adjustment.value[key] === null)
                            adjustment.value[key] = 0;

                        if (key === 'from' || key === 'to' || key.includes("Date")) {
                            newValue[key] = SettlementCalculator.convertStringToDate(adjustment.value[key]);
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
            additionalRequirements: settlementData.additionalRequirements === undefined ? [] : settlementData.additionalRequirements,
            payees: settlementData.payees === undefined ? [] : settlementData.payees,
            ourRequirements: settlementData.ourRequirements === undefined ? [] : settlementData.ourRequirements,
            fees: settlementData.fees === undefined ? [] : settlementData.fees.map(fee => {
                let newValue: any = {};
                Object.keys(fee.value).forEach(key => {
                    if (key === 'from' || key === 'to') {
                        newValue[key] = SettlementCalculator.convertStringToDate(fee.value[key]);
                    } else {
                        newValue[key] = fee.value[key];
                    }
                })

                return {
                    ...fee,
                    value: newValue
                };
            }),
            needConfirmation: false,
            loadData: null
        });

        this.props.clearSettlementState();
        this.props.changeState(matterDetails.state || "VIC");

        this.updateAdjustmentDates();
    }

    static convertStringToDate(dateString: string): Date {
        let date = new Date(dateString);
        const offset = date.getTimezoneOffset();

        date = new Date(date.getTime() - offset * 60000);

        date.setUTCHours(0);
        date.setUTCMinutes(0);
        date.setUTCSeconds(0);
        date.setUTCMilliseconds(0);

        return date;
    }

    static getDerivedStateFromProps(nextProps: AppProps, prevState: AppStates): AppStates {
        let nextState = {} as AppStates;

        if (nextProps.gotResponse === true) {
            switch (nextProps.requestType) {
                case CONSTANTS.GENERATE_PDF_REQUESTED:
                    nextState = {
                        ...nextState,
                        isWaiting: false
                    };

                    if (nextProps.gotResponse === true && nextProps.success === false) {
                        toastr.error(nextProps.error!.message || "Unexpected Error occured...");
                        nextState = {
                            ...nextState,
                            hasError: true
                        };
                    } else {
                        //Generate PDF Succeed
                    }
                    break;

                case CONSTANTS.SAVE_PDF_REQUESTED:
                    nextState = {
                        ...nextState,
                        isWaiting: false
                    };

                    if (nextProps.gotResponse === true && nextProps.success === false) {
                        toastr.error(nextProps.error!.message || "Unexpected Error occured...");
                        nextState = {
                            ...nextState,
                            hasError: true
                        };
                    } else {
                        nextState = {
                            ...nextState,
                            actionstepPDF: nextProps.actionstepPDF,
                            savePDFSuccess: true
                        };
                    }
                    break;

                case CONSTANTS.GET_SETTLEMENT_MATTER_REQUESTED:
                    if (nextProps.success === true) {
                        if (nextProps.settlementMatter !== undefined) {
                            let settlementMatter: SettlementMatterViewModel = nextProps.settlementMatter;

                            let actionstepData: ActionstepMatter | any = settlementMatter.actionstepData || new ActionstepMatter();
                            let settlementData: SettlementInfoViewModel = settlementMatter.settlementData || new SettlementInfoViewModel();
                            let matterDetails: MatterDetails | any = settlementData.matterDetails || new MatterDetails();

                            let modifiedActionstepData: ModifiedActionstepData = {} as ModifiedActionstepData;
                            let newMatterDetails: MatterDetails | any = new MatterDetails();
                            let hasChangedData: boolean = false;
                            let isNew: boolean = settlementMatter.version === 0;

                            let labelsPerKeys: { [index: string]: string } = {
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

                            nextState = {
                                ...nextState,
                                orgConnected: true
                            };

                            Object.keys(actionstepData).forEach(key => {
                                if (actionstepData[key] !== undefined) {
                                    let value = actionstepData[key],
                                        changed = false,
                                        oldValue = null,
                                        displayValue = "";

                                    if (key === "adjustmentDate" || key === "settlementDate") {
                                        value = SettlementCalculator.convertStringToDate(value);
                                    }

                                    if (matterDetails[key] !== undefined) {
                                        if (!isNew) {
                                            if (key === "adjustmentDate" || key === "settlementDate") {
                                                if (matterDetails[key] === null) {
                                                    matterDetails[key] = new Date();
                                                }
                                                let date = new Date(matterDetails[key]);

                                                if (date.getFullYear() !== value.getFullYear() || date.getMonth() !== value.getMonth() || date.getDate() !== value.getDate())
                                                    changed = true;

                                                displayValue = value.toDateString();
                                                oldValue = date.toDateString();
                                            } else if (matterDetails[key] !== value) {
                                                changed = true;

                                                displayValue = value;
                                                oldValue = matterDetails[key];
                                            }
                                        }
                                    } else {
                                        if (!isNew) {
                                            let adjustment = settlementData.adjustments![0];
                                            if (adjustment.value[key] === null) {
                                                adjustment.value[key] = 0;
                                            }

                                            if (adjustment.value[key] !== value) {
                                                changed = true;
                                            }

                                            displayValue = "$" + value.toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 });
                                            oldValue = "$" + adjustment.value[key].toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 });
                                        }
                                    }

                                    hasChangedData = hasChangedData || changed;

                                    if (key !== 'price' && key !== 'deposit') {
                                        newMatterDetails[key] = value;
                                    }

                                    modifiedActionstepData[key] = {
                                        value: value,
                                        oldValue: oldValue,
                                        displayValue: displayValue,
                                        label: labelsPerKeys[key],
                                        update: false,
                                        changed: changed
                                    }
                                }
                            })

                            nextState = {
                                ...nextState,
                                matterDetails: newMatterDetails,
                                modifiedActionstepData: modifiedActionstepData,
                                settlementData: settlementData
                            };

                            if (isNew) {

                                settlementData.adjustments!.push(
                                    {
                                        type: 'Contract Price',
                                        value: {
                                            price: actionstepData.price,
                                            deposit: actionstepData.deposit
                                        }
                                    }
                                );
                                settlementData.matterDetails = undefined;

                                nextState = {
                                    ...nextState,
                                    settlementData,
                                    hasError: false,
                                    loadData: false
                                };

                            } else if (hasChangedData) {

                                nextState = {
                                    ...nextState,
                                    needConfirmation: true,
                                    hasError: false
                                };

                            } else {
                                nextState = {
                                    ...nextState,
                                    loadData: true
                                }
                            }
                        }

                    } else {
                        if (nextProps.orgConnected === false) {

                            nextState = {
                                ...nextState,
                                orgConnected: false,
                                hasError: false
                            };

                        } else {

                            toastr.error(nextProps.error!.message || "Unexpected Error occured...");
                            nextState = {
                                ...nextState,
                                hasError: true
                            };

                        }
                    }
                    break;

                case CONSTANTS.SAVE_SETTLEMENT_MATTER_REQUESTED:
                    nextState = {
                        ...nextState,
                        isWaiting: false
                    };

                    if (nextProps.gotResponse === true && nextProps.success === false) {
                        toastr.error(nextProps.error!.message || "Unexpected Error occured...");
                        nextState = {
                            ...nextState,
                            hasError: true
                        };
                    } else { }

                    break;

                case CONSTANTS.DELETE_SETTLEMENT_MATTER_REQUESTED:
                    nextState = {
                        ...nextState,
                        isWaiting: false
                    };

                    if (nextProps.gotResponse === true && nextProps.success === false) {
                        toastr.error(nextProps.error!.message || "Unexpected Error occured...");
                        nextState = {
                            ...nextState,
                            hasError: true
                        };
                    } else {
                        nextState = {
                            ...nextState,
                            adjustments: [prevState.adjustments![0]],
                            fees: [],
                            additionalRequirements: [],
                            payees: [],
                            ourRequirements: [],
                            showAdditionalRequirements: true,
                            showWaterUsage: false,
                            settlementData: new SettlementInfoViewModel()
                        };
                    }

                    break;

                default:
                    return nextState;
            }

            nextProps.clearSettlementState();
        }

        return nextState;
    }

    private calculate(item: any): number | any {      // item can be an adjustment or a fee

        switch (item.type) {
            case 'Contract Price':
                return item.value['price'] - item.value['deposit'];

            case 'Release Fee':
                return item.value['mortgages'] * item.value['each'];

            case 'Water Usage':
                let startDate = item.value['paidDate'];
                let endDate = item.value['searchDate'];

                let numberOfDays = Math.round((endDate - startDate) / (1000 * 60 * 60 * 24));
                numberOfDays = numberOfDays === 0 ? 1 : numberOfDays;
                let diffAmountReading = item.value['searchReadingAmount'] - item.value['paidReadingAmount'];
                let dailyUsage = numberOfDays ? diffAmountReading / numberOfDays : 0;

                let startJune = new Date(startDate.getTime());
                startJune.setMonth(5);
                startJune.setDate(30);

                let numberOfDaysToJune = Math.round((startJune.valueOf() - startDate) / (1000 * 60 * 60 * 24));
                let numberOfDaysFromJune = Math.round((this.state.matterDetails.adjustmentDate.valueOf() - startJune.valueOf()) / (1000 * 60 * 60 * 24));

                if (item.value['method'] === 'daily-average') {
                    dailyUsage = item.value['averageKlCount'];
                }

                let partDays = Math.round((this.state.matterDetails.settlementDate.valueOf() - startDate) / (1000 * 60 * 60 * 24));
                partDays = partDays === 0 ? 1 : partDays;
                let dailyAndDays = dailyUsage * partDays;

                let tier1Count = item.value['tier1KlCount'];
                let tier1Charge = item.value['tier1Charge'];
                let tier2Count = item.value['tier2KlCount'];
                let tier2Charge = item.value['tier2Charge'];

                let balanceCalc = dailyAndDays - tier1Count - tier2Count;
                balanceCalc = balanceCalc < 0 ? 0 : balanceCalc;

                let tier1Result = tier1Count * tier1Charge;
                let tier2Result = tier2Count * tier2Charge;

                let balanceResult = balanceCalc * item.value['balanceCharge'];
                let bulkResult = dailyAndDays * item.value['bulkCharge'];
                let waterUsageCalcTotal = 0;

                let tier1CalcResult = 0, tier2CalcResult = 0, balanceCalcResult = 0, bulkCalcResult = 0;

                if (item.value['tier1FeeIncrease'] > 0 || item.value['tier2FeeIncrease'] > 0 || item.value['balanceFeeIncrease'] > 0 || item.value['bulkFeeIncrease'] > 0) {
                    tier1CalcResult = tier1Count * numberOfDaysToJune / partDays * tier1Charge;
                    tier2CalcResult = tier2Count * numberOfDaysToJune / partDays * tier2Charge

                    balanceCalcResult = balanceCalc * numberOfDaysToJune / partDays * item.value['balanceCharge'];
                    bulkCalcResult = dailyAndDays * numberOfDaysToJune / partDays * item.value['bulkCharge'];

                    tier1Result = tier1Count * item.value['tier1FeeIncrease'] * numberOfDaysFromJune / partDays;
                    tier2Result = tier2Count * item.value['tier2FeeIncrease'] * numberOfDaysFromJune / partDays;

                    balanceResult = balanceCalc * item.value['balanceFeeIncrease'] * numberOfDaysFromJune / partDays;
                    bulkResult = dailyAndDays * item.value['bulkFeeIncrease'] * numberOfDaysFromJune / partDays;

                    waterUsageCalcTotal = tier1Result + tier2Result + balanceResult + bulkResult;

                    waterUsageCalcTotal = waterUsageCalcTotal + tier1CalcResult + tier2CalcResult + balanceCalcResult + bulkCalcResult;
                } else {
                    waterUsageCalcTotal = tier1Result + tier2Result + balanceResult + bulkResult;
                }

                let finalWaterUsageResult = 0;
                if (item.value['ctsOption'] === 'shared-percentage') {
                    let percent = item.value['percentage'] / 100;
                    finalWaterUsageResult = waterUsageCalcTotal * percent;
                } else if (item.value['ctsOption'] === 'do-not-apportion') {
                    finalWaterUsageResult = waterUsageCalcTotal;
                } else if (item.value['ctsOption'] === 'Entitlement') {
                    let entitlement = item.value['entitlementValue'].split('/');

                    let entitlement_final = parseInt(entitlement[1]) ? parseInt(entitlement[0]) / parseInt(entitlement[1]) : 0;
                    finalWaterUsageResult = waterUsageCalcTotal * entitlement_final;
                }

                return {
                    ...item.value,
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
                let contractPrice = this.state.adjustments![0].value.price - this.state.adjustments![0].value.deposit;
                return contractPrice * item.value['rate'] / 100 * item.value['days'] / 365;

            case 'Other Adjustment':
                return item.value['amount'];

            case 'Fee':
                return item.value['amount'];

            default:
                if (item.value['days'] === 0)
                    return 0;
                let result = item.value['amount'] * item.value['adjustDays'] / item.value['days'];
                return result;

        }
    }

    public toggleIncludeAdditionalRequirementsInTotal(checked: boolean): void {
        this.setState({
            includeAdditionalCostsInTotal: checked
        })
    }

    public render(): JSX.Element {
        const {
            matterDetails,
            adjustments,
            fees,
            additionalRequirements,
            payees,
            ourRequirements,
            additionalInfo,
            waterUsage
        } = this.generateUIData();

        const {
            showAdditionalRequirements,
            actionstepData,
            modifiedActionstepData,
            orgConnected,
            hasError,
            dataInputError,
            isInvalidMatter,
            actionstepPDF,
            showModal,
            savePDFSuccess,
            needConfirmation,
            isWaiting
        } = this.state;

        const {
            title,
            contractBalance,
            contractDebit,
            contractCredit,
            feeDebit,
            feeCredit,
            payeeDebit,
            payeeCredit,
            unallocated,
            additionalDebit,
            additionalCredit,
            hasWaterUsage
        } = additionalInfo!;

        const isVendor = matterDetails!.conveyType === 'vendor';

        const State = this.props.state;

        return (
            <div className="settlement-calculator">
                {isInvalidMatter
                    ? <SettlementInvalidMatter />
                    : orgConnected === true
                        ? (
                            <div className="wrapper wrapper-content animated fadeIn vertical-container">

                                <div className="ibox">
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
                                                            onClick={() => this.showModal(ModalIDs.matterDetails, { index: -1 })}
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
                                                                    {(matterDetails && matterDetails.matter && matterDetails.matter + ' (' + matterDetails.matterRef + ')') || ""}
                                                                </div>
                                                            </div>

                                                            <div className="ms-Grid-row detail-row">
                                                                <div className="ms-Grid-col ms-sm2">
                                                                    <b>Property</b>
                                                                </div>
                                                                <div className="ms-Grid-col ms-sm10">
                                                                    {matterDetails && matterDetails.property}
                                                                </div>
                                                            </div>

                                                            <div className="ms-Grid-row detail-row">
                                                                <div className="ms-Grid-col ms-sm2">
                                                                    <b>Adjustment Date</b>
                                                                </div>
                                                                <div className="ms-Grid-col ms-sm10">
                                                                    {(matterDetails && matterDetails.adjustmentDate && matterDetails.adjustmentDate.toDateString()) || ""}
                                                                </div>
                                                            </div>

                                                            <div className="ms-Grid-row detail-row">
                                                                <div className="ms-Grid-col ms-sm2">
                                                                    <b>Settlement Date</b>
                                                                </div>
                                                                <div className="ms-Grid-col ms-sm10">
                                                                    {(matterDetails && matterDetails.settlementDate && matterDetails.settlementDate.toDateString()) || ""}
                                                                </div>
                                                            </div>

                                                            <div className="ms-Grid-row detail-row">
                                                                <div className="ms-Grid-col ms-sm2">
                                                                    <b>Settlement Place</b>
                                                                </div>
                                                                <div className="ms-Grid-col ms-sm10">
                                                                    {matterDetails && matterDetails.settlementPlace}
                                                                </div>
                                                            </div>

                                                            <div className="detail-row ms-Grid-row">
                                                                <div className="ms-Grid-col ms-sm2">
                                                                    <b>Settlement Time</b>
                                                                </div>
                                                                <div className="ms-Grid-col ms-sm10">
                                                                    {matterDetails && matterDetails.settlementTime}
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
                                                    adjustments && adjustments.map((adjustment, index) => {
                                                        return (
                                                            <SettlementInfo
                                                                isVendor={isVendor}
                                                                toggleWaterUsage={() => this.toggleWaterUsage()}
                                                                key={'adjustment_' + index}
                                                                item={adjustment}
                                                                index={index}
                                                                adjustmentDate={matterDetails && matterDetails.adjustmentDate}
                                                                showModal={(modalID, additionalInfo) => this.showModal(modalID, additionalInfo)}
                                                            />
                                                        )
                                                    })
                                                }
                                                {State === 'SA' && fees && fees.map((fee, index) => {
                                                    if (fee.value['showOnAdjustment']) {
                                                        return (
                                                            <SettlementInfo
                                                                isVendor={isVendor}
                                                                key={'fee_' + index}
                                                                item={fee}
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
                                                            onClick={() => this.showModal(ModalIDs.addAdjustment, { index: -1 })}
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

                                        {State === 'SA' &&
                                            <div className="section">
                                                <div className="left-align-section-header">
                                                    <b>FEES</b>
                                                </div>

                                                <div className="section-body">

                                                    {
                                                        fees && fees.map((fee, index) => {
                                                            if (fee.value['showOnAdjustment'] === false) {
                                                                return (
                                                                    <SettlementInfo
                                                                        isVendor={isVendor}
                                                                        key={'fee_' + index}
                                                                        item={fee}
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
                                                                onClick={() => this.showModal(ModalIDs.fee, { index: -1 })}
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

                                        {State !== "VIC" &&
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

                                                        {additionalRequirements && additionalRequirements.map((additionalRequirement, index) => {
                                                            return (
                                                                <SettlementInfo
                                                                    isVendor={isVendor}
                                                                    key={'additional_requirement_' + index}
                                                                    item={additionalRequirement}
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
                                                                    onClick={() => this.showModal(ModalIDs.additionalRequirements, { index: -1 })}
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

                                                {State !== "VIC" &&
                                                    <div className="ms-Grid-col ms-sm6 ms-smPush6 right-align-section-header">
                                                        <Checkbox
                                                            label="Include additional costs in total"
                                                            onChange={(ev, checked) => this.toggleIncludeAdditionalRequirementsInTotal(checked || false)}
                                                        />
                                                    </div>
                                                }

                                                {payees && payees.map((payee, index) => {
                                                    return (
                                                        <SettlementInfo
                                                            isVendor={isVendor}
                                                            key={'payee_' + index}
                                                            item={payee}
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
                                                            onClick={() => this.showModal(ModalIDs.payeeDetails, { index: -1 })}
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

                                                {ourRequirements && ourRequirements.map((ourRequirement, index) => {
                                                    return (
                                                        <SettlementInfo
                                                            isVendor={isVendor}
                                                            key={'requirement_' + index}
                                                            item={ourRequirement}
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
                                                            onClick={() => this.showModal(ModalIDs.ourRequirements, { index: -1 })}
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

                                        {this.state.showWaterUsage && hasWaterUsage === true &&
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

                                        {(State === 'SA' && isVendor) ?
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
                                        {Object.keys(modifiedActionstepData!).map(key => {
                                            if (modifiedActionstepData![key].changed) {
                                                return (
                                                    <div className="ms-Grid-row" key={key}>
                                                        <div className="ms-Grid-col ms-sm4">
                                                            {modifiedActionstepData![key].label}
                                                        </div>
                                                        <div className="ms-Grid-col ms-sm4">
                                                            {modifiedActionstepData![key].oldValue}
                                                        </div>
                                                        <div className="ms-Grid-col ms-sm4">
                                                            {modifiedActionstepData![key].displayValue}
                                                        </div>
                                                    </div>
                                                );
                                            }

                                            return null;
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
                                    className={isWaiting !== null ? "animated fadeIn settlement-calculator" : "animated fadeOut settlement-calculator"}
                                >
                                    <div className="modal-body">

                                        <div className="ms-Grid-row loading-modal">
                                            <img src="/images/Konekta_loading.svg" className="col-lg-3"
                                                alt="Logo" />
                                            <h2>Please wait...</h2>
                                        </div>

                                    </div>
                                </Modal>

                                <Modal isOpen={savePDFSuccess}
                                    isBlocking={false}
                                    onDismiss={() => this.closeModal()}
                                    className={savePDFSuccess ? "animated fadeIn settlement-calculator" : "animated fadeOut settlement-calculator"}
                                >
                                    <div className="modal-header">
                                        <span className="modal-title">PDF saved to Actionstep successfully!</span>
                                    </div>
                                    <div className="modal-body">
                                        Your PDF has been saved to Actionstep at the following location: <Link rel="noopener noreferrer" href={actionstepPDF!.url} target="_blank" download >{actionstepPDF!.fileName}</Link> <br />
                                        or <Link rel="noopener noreferrer" href={actionstepPDF!.documentUrl} target="_blank"><b>view in Actionstep</b></Link> (This link requires you to be logged in to the related Actionstep org)
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
                                    isOpen={showModal !== null}
                                    isBlocking={false}
                                    onDismiss={() => this.closeModal()}
                                    className={showModal !== null ? "animated fadeIn settlement-calculator" : "animated fadeOut settlement-calculator"}
                                    dragOptions={this._dragOptions}
                                >

                                    {showModal !== null &&
                                        <Modals
                                            modalID={showModal}
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
                        ) : hasError === true ? (
                            <div className="wrapper wrapper-content animated fadeInRight vertical-container">
                                <div className="row">
                                    <div className="col-lg-12">
                                        <div className="panel panel-danger">
                                            <div className="panel-heading">
                                                <Icon iconName="StatsCircleExclamation" /> <span className="m-l-xs">Sorry, something went wrong</span>
                                            </div>
                                            <div className="panel-body p-v-xs">
                                                <p className="text-danger">
                                                    Please contact <Link href={"mailto:support@konekta.com.au?subject=Legacy%20Settlement%20Calculator"}> support@konekta.com.au</Link>.
                                                </p>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        ) : hasError === false
                                ?
                                <div className="wrapper wrapper-content animated fadeInRight vertical-container">
                                    <MessageBar messageBarType={MessageBarType.error}>You're not connected to <strong>Konekta</strong>.</MessageBar>
                                </div>
                                :
                                <div className="loading-widget">
                                    <img src="/images/Konekta_loading.svg" alt="Logo" height="150" /> Loading...
                                </div>
                }
            </div>
        );
    }

    public connectedToActionstep(): void {
        this.setState({
            orgConnected: null,
            hasError: null,
            isInvalidMatter: false,
        })
        this.loadActionstepMatter();
    }

    private generateUIData(isAdjustment = false): SettlementInfoViewModel {
        let contractDebit = 0, contractCredit = 0;
        let waterUsage = {}, hasWaterUsage = false;
        let { includeAdditionalCostsInTotal } = this.state;

        const adjustments = (this.state.adjustments && this.state.adjustments.map((adjustment, index) => {
            let result = this.calculate(adjustment);

            if (adjustment.type === 'Water Usage') {
                waterUsage = result;
                result = result['finalWaterUsageResult'];

                hasWaterUsage = true;
            }

            if (adjustment.type === 'Contract Price') {
                contractDebit += adjustment.value['price'];
                contractCredit += adjustment.value['deposit'];
            } else {
                if (adjustment.value['status'] === 'unpaid' || adjustment.value['status'] === 'less')
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
        })) || [];

        let feeDebit = contractDebit, feeCredit = contractCredit, fees: any[] = [];

        if (this.state.matterDetails.state === 'SA') {
            fees = this.state.fees.map((fee, index) => {
                let result = this.calculate(fee);

                if (fee.value['showOnAdjustment'] === true) {
                    if (fee.value['status'] === 'less') {
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
                    if (fee.value['status'] === 'less')
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

        let additionalDebit = feeDebit, additionalCredit = feeCredit;

        const additionalRequirements = this.state.additionalRequirements.map((additionalRequirement, index) => {
            let result = additionalRequirement.value['amount'];
            if (additionalRequirement.value['status'] === 'less') {
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

        let payeeDebit = includeAdditionalCostsInTotal ? additionalDebit : feeDebit,
            payeeCredit = includeAdditionalCostsInTotal ? additionalCredit : feeCredit,
            payeeTotal = 0;

        const payees = this.state.payees.map((payee, index) => {
            let result = payee.value['amount'];
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

        const ourRequirements = this.state.ourRequirements.map((ourRequirement, index) => {
            return {
                ...ourRequirement,
                result: 0,
                total: 0
            };
        })

        const State = this.state.matterDetails.state;
        const isVendor = this.state.matterDetails.conveyType && this.state.matterDetails.conveyType.toLowerCase() === 'vendor';;

        return new SettlementInfoViewModel(
            {
                matterDetails: this.state.matterDetails,
                adjustments,
                fees,
                additionalRequirements,
                payees,
                ourRequirements,
                waterUsage,
                additionalInfo: {
                    title: isAdjustment ? "ADJUSTMENT STATEMENT" : State === "VIC" ? (isVendor ? "Vendor's " : "Purchaser's ") + "Statement of Adjustments" :
                        (isVendor ? "VENDOR'S " : "PURCHASER'S ") + (State === "NSW" ? "SETTLEMENT ADJUSTMENT SHEET" : "SETTLEMENT STATEMENT"),
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
        );
    }

    private updateValue(newValue: any, whichValue: string, needRefresh: boolean = false): void {
        if (typeof (this.updatedState[whichValue]) === 'number') {
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

    private deleteData(index: number, modalID: ModalIDs): void {
        switch (modalID) {
            case ModalIDs.additionalRequirements:
                let additionalRequirements = [...this.state.additionalRequirements];
                additionalRequirements.splice(index, 1);
                this.setState({
                    additionalRequirements: additionalRequirements
                });

                break;

            case ModalIDs.payeeDetails:
                let payees = [...this.state.payees];
                payees.splice(index, 1);
                this.setState({
                    payees: payees
                });

                break;

            case ModalIDs.ourRequirements:
                let ourRequirements = [...this.state.ourRequirements];
                ourRequirements.splice(index, 1);
                this.setState({
                    ourRequirements: ourRequirements
                });

                break;

            case ModalIDs.fee:
                let fees = [...this.state.fees];
                fees.splice(index, 1);
                this.setState({
                    fees: fees
                });

                break;

            default:
                let adjustments = [...this.state.adjustments!];
                adjustments.splice(index, 1);
                this.setState({
                    adjustments: adjustments
                });

                break;
        }

        this.closeModal();
    }

    private showModal(modalID: ModalIDs, additionalInfo: any = null): void {
        let updatedState = {};

        switch (modalID) {
            case ModalIDs.matterDetails:
                updatedState = Object.assign({}, this.state.matterDetails);
                break;

            case ModalIDs.addAdjustment:
                updatedState = {
                    itemType: 'Water Usage'
                };
                break;

            case ModalIDs.contractPrice:
                updatedState = this.state.adjustments![additionalInfo['index']].value;
                break;

            case ModalIDs.releaseFee:

                if (additionalInfo['index'] < 0) {
                    updatedState = {
                        mortgages: 0,
                        each: 0,
                        status: 'less'
                    };
                } else {
                    updatedState = this.state.adjustments![additionalInfo['index']].value;
                }
                break;

            case ModalIDs.waterUsage:

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
                    updatedState = this.state.adjustments![additionalInfo['index']].value;
                }
                break;

            case ModalIDs.otherAdjustment:
                if (additionalInfo['index'] < 0) {
                    updatedState = {
                        description: '',
                        amount: 0,
                        status: 'plus'
                    };
                } else {
                    updatedState = this.state.adjustments![additionalInfo['index']].value;
                }
                break;

            case ModalIDs.penaltyInterest:
                if (additionalInfo['index'] < 0) {
                    updatedState = {
                        rate: 0,
                        from: null,
                        to: null,
                        status: 'paid'
                    };
                } else {
                    updatedState = this.state.adjustments![additionalInfo['index']].value;
                }
                break;

            case ModalIDs.otherAdjustmentDate:
                if (additionalInfo['index'] < 0) {
                    updatedState = {
                        description: '',
                        amount: 0,
                        from: null,
                        to: null,
                        status: 'paid'
                    };
                } else {
                    updatedState = this.state.adjustments![additionalInfo['index']].value;
                }
                break;

            case ModalIDs.fee:
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

            case ModalIDs.additionalRequirements:
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

            case ModalIDs.payeeDetails:
                if (additionalInfo['index'] < 0) {
                    updatedState = {
                        description: '',
                        amount: 0
                    };
                } else {
                    updatedState = this.state.payees[additionalInfo['index']].value;
                }
                break;

            case ModalIDs.ourRequirements:
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
                    updatedState = this.state.adjustments![additionalInfo['index']].value;
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
        let { matterDetails, adjustments } = this.state;

        let newAdjustments = adjustments!.map(adjustment => {
            if (adjustment.value['adjustDays']) {
                let adjustDays = adjustment.value['status'] === 'unpaid' ? Math.floor((matterDetails["adjustmentDate"].valueOf() - adjustment.value['from']) / (1000 * 60 * 60 * 24)) + 1
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
        let modalID = this.state.showModal;

        switch (modalID) {
            case ModalIDs.matterDetails:
                if (this.updatedState['adjustmentDate'] === null || this.updatedState['settlementDate'] === null) {
                    this.setState({
                        dataInputError: errorMessages.dateInputError
                    })
                    return;
                }

                this.updatedState['adjustmentDate'] = SettlementCalculator.convertStringToDate(this.updatedState['adjustmentDate'].toISOString());
                this.updatedState['settlementDate'] = SettlementCalculator.convertStringToDate(this.updatedState['settlementDate'].toISOString());

                await this.setState({
                    matterDetails: MatterDetails.fromJS(this.updatedState)
                })

                this.updateAdjustmentDates();

                this.props.changeState(this.updatedState["state"]);

                break;

            case ModalIDs.addAdjustment:
                let newModalID = this.updatedState['itemType'];

                this.showModal(newModalID, { index: -1 });
                return;

            case ModalIDs.penaltyInterest:
                if (this.updatedState['from'] === null || this.updatedState['to'] === null) {
                    this.setState({
                        dataInputError: errorMessages.dateInputError
                    })
                    return;
                }

                this.updatedState['from'] = SettlementCalculator.convertStringToDate(this.updatedState['from'].toISOString());
                this.updatedState['to'] = SettlementCalculator.convertStringToDate(this.updatedState['to'].toISOString());

                let newDays = Math.floor((this.updatedState['to'] - this.updatedState['from']) / (1000 * 60 * 60 * 24)) + 1;
                this.updatedState['days'] = newDays > 0 ? newDays : 0;

                {
                    let newAdjustments = [...this.state.adjustments!];

                    if (this.state.selectedIndex < 0) {
                        let newAdjustment = {
                            value: Object.assign({}, this.updatedState),
                            type: modalID
                        };

                        newAdjustments.push(newAdjustment);

                    } else {
                        newAdjustments[this.state.selectedIndex].value = this.updatedState;
                    }

                    this.setState({
                        adjustments: newAdjustments
                    });
                }

                break;

            case ModalIDs.waterUsage:
                if (this.updatedState['paidDate'] === null || this.updatedState['searchDate'] === null) {
                    this.setState({
                        dataInputError: errorMessages.dateInputError
                    })
                    return;
                }

                this.updatedState['paidDate'] = SettlementCalculator.convertStringToDate(this.updatedState['paidDate'].toISOString());
                this.updatedState['searchDate'] = SettlementCalculator.convertStringToDate(this.updatedState['searchDate'].toISOString());
                break;

            case ModalIDs.contractPrice:
            case ModalIDs.releaseFee:
            case ModalIDs.otherAdjustment:
                {
                    let newAdjustments = [...this.state.adjustments!];

                    if (this.state.selectedIndex < 0) {
                        let newAdjustment = {
                            value: Object.assign({}, this.updatedState),
                            type: modalID
                        };

                        newAdjustments.push(newAdjustment);

                    } else {
                        newAdjustments[this.state.selectedIndex].value = this.updatedState;
                    }

                    this.setState({
                        adjustments: newAdjustments
                    });
                }
                break;

            case ModalIDs.fee:

                let fees = [...this.state.fees];

                if (this.state.selectedIndex < 0) {
                    let newFee = {
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

            case ModalIDs.additionalRequirements:
                let additionalRequirements = [...this.state.additionalRequirements];

                if (this.state.selectedIndex < 0) {
                    let additionalRequirement = {
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

            case ModalIDs.payeeDetails:
                let payees = [...this.state.payees];

                if (this.state.selectedIndex < 0) {
                    let payee = {
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

            case ModalIDs.ourRequirements:
                let ourRequirements = [...this.state.ourRequirements];

                if (this.state.selectedIndex < 0) {
                    let ourRequirement = {
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

                if (this.updatedState['from'] === null || this.updatedState['to'] === null) {
                    this.setState({
                        dataInputError: errorMessages.dateInputError
                    })
                    return;
                }

                this.updatedState['from'] = SettlementCalculator.convertStringToDate(this.updatedState['from'].toISOString());
                this.updatedState['to'] = SettlementCalculator.convertStringToDate(this.updatedState['to'].toISOString());

                let adjustDays = this.updatedState['status'] === 'unpaid' ? Math.floor((this.state.matterDetails.adjustmentDate.valueOf() - this.updatedState['from']) / (1000 * 60 * 60 * 24)) + 1
                    : Math.floor((this.updatedState['to'] - this.state.matterDetails.adjustmentDate.valueOf()) / (1000 * 60 * 60 * 24));

                let days = Math.floor((this.updatedState['to'] - this.updatedState['from']) / (1000 * 60 * 60 * 24)) + 1;

                this.updatedState['adjustDays'] = adjustDays;
                this.updatedState['days'] = days;

                let adjustments = [...this.state.adjustments!];

                if (this.state.selectedIndex < 0) {
                    let newAdjustment = {
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
        const settlementData = this.generateUIData();
        const { matterInfo } = this.state;

        const params: SettlementMatterViewModel = new SettlementMatterViewModel({
            actionstepOrg: (matterInfo!.orgKey) || "",
            matterId: (matterInfo!.matterId) || 0,
            version: 0,
            settlementData: settlementData,
            actionstepData: undefined
        });

        this.props.saveSettlementMatter(params);

        this.setState({
            isWaiting: true
        })
    }

    private delete(): void {
        const { matterInfo } = this.state;

        this.props.deleteSettlementMatter(matterInfo!);

        this.setState({
            isWaiting: true
        })
    }

    private generatePDF(isAdjustment = false, saveToActionstep = false): void {
        let { matterDetails, adjustments, fees, additionalRequirements, payees, ourRequirements, additionalInfo, waterUsage } = this.generateUIData(isAdjustment);

        adjustments = adjustments && adjustments.map((adjustment, index) => {

            let newAdjustment = {
                ...adjustment,
                ...adjustment["value"]
            }

            delete newAdjustment.value;

            return newAdjustment;

        })

        adjustments = adjustments && adjustments.map((item, index) => {
            item["status"] = item["status"] === 'unpaid' ? "Less" : "Plus";

            return item;
        })

        fees = fees && fees.map((item, index) => {
            let newItem = {
                ...item,
                ...item.value
            }

            delete newItem.value;

            return newItem;
        });

        additionalRequirements = additionalRequirements && additionalRequirements.map((item, index) => {
            let newItem = {
                ...item,
                ...item["value"]
            }

            delete newItem.value;

            return newItem;
        });

        payees = payees && payees.map((item, index) => {
            let newItem = {
                ...item,
                ...item["value"]
            }

            delete newItem.value;

            return newItem;
        });

        ourRequirements = ourRequirements && ourRequirements.map((item, index) => {
            let newItem = {
                ...item,
                ...item["value"]
            }

            delete newItem.value;

            return newItem;
        });

        additionalInfo!["isAdjustment"] = isAdjustment;
        additionalInfo!["saveToActionstep"] = saveToActionstep;

        let settlementData: SettlementInfoViewModel = new SettlementInfoViewModel({
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
            actionstepOrg: (matterInfo!.orgKey) || "",
            matterId: (matterInfo!.matterId) || 0,
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

    private sendEmail(type: number): void {
        let uiData = this.generateUIData();

        let emailContent = this.state.matterDetails.matter + "%0AProperty: " + this.state.matterDetails.property + "%0A%0A";

        emailContent += `Dear Sir/Madam %0AWe confirm settlement of the above matter has been booked for ` + this.state.matterDetails.settlementTime +
            ` on ` + this.state.matterDetails.settlementDate.toDateString() + ' at the office of ' + this.state.matterDetails.settlementPlace + '.%0A';

        emailContent += `We hereby direct cheques to be made payable as follows:%0A%0A`;

        emailContent += 'The total of the cheque above is $' + ((uiData.additionalInfo && uiData.additionalInfo.totalPayee) || 0) + `.`;

        emailContent += `%0A%0ARegards.%0A`;

        let mailToLink = "mailto:" + this.state.lc + "?subject=Settlement Cheque Details - Matter Ref#" + this.state.matterDetails.matterRef + "&body=" + emailContent;

        window.location.href = mailToLink;
    }
}

const mapStateToProps = (state: AppState) => {
    return {
        state: state.settlementInfo.state,
        jwtMatterInfo: state.common.jwtMatterInfo,
        success: state.settlementInfo.success,
        gotResponse: state.settlementInfo.gotResponse,
        settlementMatter: state.settlementInfo.settlementMatter,
        actionstepPDF: state.settlementInfo.actionstepPDF,
        requestType: state.settlementInfo.requestType,
        orgConnected: state.common.orgConnected,
        error: state.settlementInfo.error
    }
}

const mapDispatchToProps = {
    generatePDF,
    savePDF,
    getSettlementMatter,
    saveSettlementMatter,
    deleteSettlementMatter,
    changeState,
    clearSettlementState
}

export default connect(mapStateToProps, mapDispatchToProps)(SettlementCalculator);