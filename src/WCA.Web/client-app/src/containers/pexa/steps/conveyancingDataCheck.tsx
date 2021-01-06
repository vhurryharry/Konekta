import * as React from 'react';
import { connect } from 'react-redux';

import { Modal } from 'office-ui-fabric-react/lib/Modal';
import { Dropdown, IDropdownOption } from 'office-ui-fabric-react/lib/Dropdown';
import { TextField } from 'office-ui-fabric-react/lib/TextField';
import { DatePicker } from 'office-ui-fabric-react/lib/DatePicker';
import { MessageBar, MessageBarType } from 'office-ui-fabric-react/lib/MessageBar';
import { DefaultButton, PrimaryButton } from 'office-ui-fabric-react';

import { _onFormatDate, convertDateToUTC, formatDateTime12HourString, formatISODateString } from 'utils/dataFormatter'

import {
    WorkspaceCreationRequest,
    PEXAWorkspaceCreationRequestWithActionstepResponse,
    ConveyancingMatter,
    PexaRole,
    FullName,
    GivenNameOrderType,
    Business,
    Party,
    ActionstepMatterInfo,
    LandTitleReferenceAndJurisdiction,
    LandTitleReferenceVerificationResponseType,
    ErrorViewModel,
    RetrieveSettlementAvailabilityParams
} from 'utils/wcaApiTypes';
import { SubwayNavNodeState } from 'components/SubwayNav';
import {
    OrganisationTypes,
    PostalDeliveryTypeCodes,
    RoadSuffixes,
    RoadTypeCodes,
    PartyTypes,
    PartyDetailStates,
    SubscriberRoles,
    Jurisdictions,
    BooleanItems,
    ParticipantSettlementAcceptanceStatus,
    PartyAddressType,
    AddressTypes,
    SubDwellingUnitTypes
} from 'containers/pexa/steps/constants';

import {
    getAvailableSettlementTimes,
    setPexaWorkspaceCreation,
    validateLandTitle,
    savePexaFormData,
    clearPexaState,
    clearPexaFormData
} from 'containers/pexa/redux/actions';
import * as CONSTANTS from 'containers/pexa/redux/constants'

import { AppState, JwtMatterInfo, ReduxStatus } from 'app.types';
import { WorkspaceCreationRequestWithMatterInfo, AvailableSettlementTimes } from 'containers/pexa/redux/actionTypes';
import Tools from 'utils/tools';

import LoadingWidget from 'components/common/loadingWidget';
import ValidateLandTitle from 'containers/pexa/steps/components/validateLandTitle';

interface IMapStateToProps {
    availableSettlementTimes: AvailableSettlementTimes;
    jwtMatterInfo: JwtMatterInfo | undefined;
    pexaWorkspaceCreationData: PEXAWorkspaceCreationRequestWithActionstepResponse;
    pexaFormData: WorkspaceCreationRequestWithMatterInfo | undefined;
    validateLandTitleResponse: LandTitleReferenceVerificationResponseType | undefined;
    requestType: string;
    success: boolean;
    gotResponse: boolean;
    error: ErrorViewModel | undefined;
}

interface IMapDispatchToProps {
    getAvailableSettlementTimes: (params: RetrieveSettlementAvailabilityParams) => void;
    setPexaWorkspaceCreation: (data: WorkspaceCreationRequest) => void;
    validateLandTitle: (params: LandTitleReferenceAndJurisdiction) => void;
    savePexaFormData: (pexaFormData: WorkspaceCreationRequestWithMatterInfo) => void;
    clearPexaState: () => void;
    clearPexaFormData: () => void;
}

interface IAppProps {
    onChangeStep: (newState: SubwayNavNodeState, newStep?: number) => void;
}

type AppProps = IAppProps & IMapStateToProps & IMapDispatchToProps;

type AppStates = {
    workspaceCreationRequest: WorkspaceCreationRequest;
    actionstepMatter: ConveyancingMatter;
    addressTypes: (PartyAddressType | null)[];
    pexaRoleSpecified: boolean;
    errorMessage: string | null;
    showValidateModal: boolean;
    validateLandTitleResponse: LandTitleReferenceVerificationResponseType | null;
    validationError: ErrorViewModel | null;
    createNewWorkspace: boolean | null;
    availableWorkgroups: IDropdownOption[];
    availableSettlementTimes: IDropdownOption[] | null;
}

export class ConveyancingDataCheck extends React.Component<AppProps, AppStates> {

    constructor(props: Readonly<AppProps>) {
        super(props);

        const createPexaWorkspaceCommand = this.props.pexaWorkspaceCreationData.createPexaWorkspaceCommand;
        const workspaceCreationRequest: WorkspaceCreationRequest = (createPexaWorkspaceCommand && createPexaWorkspaceCommand.pexaWorkspaceCreationRequest) || new WorkspaceCreationRequest();

        let addressTypes: (PartyAddressType | null)[] = workspaceCreationRequest.partyDetails ? workspaceCreationRequest.partyDetails.map(party => null) : [];

        let availableSettlementTimes: IDropdownOption[] = [];
        if (this.isValid(workspaceCreationRequest.settlementDate) && this.isValid(workspaceCreationRequest.jurisdiction)) {
            const jurisdiction = workspaceCreationRequest.jurisdiction;
            const settlementDate = formatISODateString(workspaceCreationRequest.settlementDate!);

            if (props.availableSettlementTimes && props.availableSettlementTimes[jurisdiction] && props.availableSettlementTimes[jurisdiction][settlementDate] &&
                props.availableSettlementTimes[jurisdiction][settlementDate].status === ReduxStatus.Success) {

                const availabilityResponse = props.availableSettlementTimes[jurisdiction][settlementDate].data!;
                if (availabilityResponse.settlementAvailability === "Yes") {
                    availableSettlementTimes = availabilityResponse.availableSettlementTime!.map((settlementTime, index) => {
                        return {
                            key: index.toString(),
                            text: formatDateTime12HourString(settlementTime)
                        };
                    })
                }
            }
        }

        let availableWorkgroups = [
            {
                key: "",
                text: "Please select..."
            }
        ];
        if (this.props.pexaWorkspaceCreationData.workgroupList) {
            this.props.pexaWorkspaceCreationData.workgroupList.forEach((workgroup, index) => {
                availableWorkgroups.push({
                    key: workgroup.workgroupId!,
                    text: workgroup.workgroupName!
                });
            })
        }

        this.state = {
            workspaceCreationRequest: workspaceCreationRequest,
            actionstepMatter: this.props.pexaWorkspaceCreationData.actionstepData || new ConveyancingMatter(),
            addressTypes: addressTypes,
            pexaRoleSpecified: this.props.pexaWorkspaceCreationData.pexaRoleSpecified,
            errorMessage: null,
            showValidateModal: false,
            validateLandTitleResponse: null,
            validationError: null,
            createNewWorkspace: null,
            availableSettlementTimes: availableSettlementTimes,
            availableWorkgroups: availableWorkgroups
        }
    }

    public componentDidMount(): void {
        const { pexaFormData } = this.props;

        if (pexaFormData) {
            this.props.clearPexaFormData();

            this.setState({
                addressTypes: pexaFormData.addressTypes,
                validateLandTitleResponse: pexaFormData.validateLandTitleResponse,
                createNewWorkspace: true
            });
        }

        if (!this.state.availableSettlementTimes || !this.state.availableSettlementTimes.length) {
            this.checkAvailableSettlementTime();
        }
    }

    private changeWorkspaceCreationRequest = (newValue: any, keyPath: string): void => {
        let newWorkspaceCreationRequest = this.state.workspaceCreationRequest;

        if (typeof newValue === "string" && newValue === "") newValue = null;

        Tools.assign(newWorkspaceCreationRequest, keyPath, newValue);

        this.setState({
            workspaceCreationRequest: newWorkspaceCreationRequest
        });

        if (keyPath === "role") {
            this.setState({
                pexaRoleSpecified: newValue ? true : false
            });
        }
    }

    private checkAvailableSettlementTime = () => {
        const { workspaceCreationRequest } = this.state;

        if (this.isValid(workspaceCreationRequest.jurisdiction) && this.isValid(workspaceCreationRequest.settlementDate)) {
            const params = new RetrieveSettlementAvailabilityParams({
                jurisdiction: workspaceCreationRequest.jurisdiction,
                settlementDate: formatISODateString(workspaceCreationRequest.settlementDate!)
            });
            if (this.props.getAvailableSettlementTimes) {
                this.props.getAvailableSettlementTimes(params);
            }

            this.setState({
                availableSettlementTimes: []
            })
        }
    }

    private changeAddressType = (newValue: PartyAddressType, index: number): void => {
        let newAddressTypes = [...this.state.addressTypes];
        newAddressTypes[index] = newValue;

        this.setState({
            addressTypes: newAddressTypes
        })
    }

    private _validate = (): (string | null) => {
        const { workspaceCreationRequest, createNewWorkspace } = this.state;

        if (!createNewWorkspace) {
            return "Please click 'Validate' button to validate the Land Title Reference value";
        }

        if (workspaceCreationRequest.financialSettlement === "Yes" && workspaceCreationRequest.landTitleDetails.parentTitle === "No") {
            if (!this.isValid(workspaceCreationRequest.settlementDate)) {
                return `The settlement date must be provided when Financial Settlement is "Yes" and Parent Title = "No"`;
            }
        }

        const today = new Date();
        const settlementDate = new Date(workspaceCreationRequest.settlementDate!);

        var months;
        months = (settlementDate.getFullYear() - today.getFullYear()) * 12;
        months -= today.getMonth() + 1;
        months += settlementDate.getMonth();
        months = months <= 0 ? 0 : months;

        if (settlementDate.getTime() < today.getTime() || months > 6)
            return "The settlement date cannot be greater than 6 months into the future and must not be prior to the current date.";

        return null;
    }

    private loadActionstepMatterInfo = (): ActionstepMatterInfo | null => {
        const { jwtMatterInfo } = this.props;
        let matterInfo: ActionstepMatterInfo | null = null;

        if (jwtMatterInfo === undefined) {

            const queryString = require('query-string');

            const urlParams = queryString.parse(window.location.search);

            matterInfo = Tools.ParseActionstepMatterInfo(urlParams);

        } else {
            matterInfo = new ActionstepMatterInfo({
                orgKey: jwtMatterInfo.orgKey,
                matterId: jwtMatterInfo.matterId
            });
        }

        return matterInfo;
    }

    private approveData = (): void => {
        let validation = this._validate();

        this.setState({
            errorMessage: validation
        });

        if (validation) {
            return;
        }

        const addressTypes = this.state.addressTypes;
        let workspaceCreationRequest = WorkspaceCreationRequest.fromJS({ ...this.state.workspaceCreationRequest });
        const matterInfo = this.loadActionstepMatterInfo();

        const pexaFormData: WorkspaceCreationRequestWithMatterInfo = new WorkspaceCreationRequestWithMatterInfo(workspaceCreationRequest, addressTypes, matterInfo!, this.state.validateLandTitleResponse);
        this.props.savePexaFormData(pexaFormData);

        if (workspaceCreationRequest.role !== PexaRole.Incoming_Proprietor) {
            workspaceCreationRequest.partyDetails = undefined;
        }

        if (workspaceCreationRequest.partyDetails) {
            for (let i = 0; i < workspaceCreationRequest.partyDetails.length; i++) {
                let party = workspaceCreationRequest.partyDetails[i];

                if (addressTypes[i] === PartyAddressType.Correspondence_Address) {
                    party.currentAddress!.overseasAddress = undefined;
                    party.currentAddress!.streetAddress = undefined;
                } else if (addressTypes[i] === PartyAddressType.Overseas_Address) {
                    party.currentAddress!.correspondenceAddress = undefined;
                    party.currentAddress!.streetAddress = undefined;
                } else {
                    party.currentAddress!.overseasAddress = undefined;

                    party.currentAddress!.streetAddress!.road = party.currentAddress!.correspondenceAddress!.road;
                    party.currentAddress!.streetAddress!.localityName = party.currentAddress!.correspondenceAddress!.localityName;
                    party.currentAddress!.streetAddress!.postcode = party.currentAddress!.correspondenceAddress!.postcode;
                    party.currentAddress!.streetAddress!.state = party.currentAddress!.correspondenceAddress!.state;
                    if (addressTypes[i] !== PartyAddressType.Unit_Apartment) {
                        party.currentAddress!.streetAddress!.subDwellingUnitType = undefined;
                    }

                    party.currentAddress!.correspondenceAddress = undefined;
                }

                if (party.partyType === "Organisation") {
                    party.fullName = undefined;
                } else {
                    party.business = undefined;

                    if (party.fullName && party.fullName.dateOfBirth)
                        party.fullName.dateOfBirth = convertDateToUTC(party.fullName.dateOfBirth);
                }

                party.foreignPartyDetails = undefined;
                party.futureAddress = undefined;
                party.partyCapacityDetails = undefined;

                workspaceCreationRequest.partyDetails[i] = party;
            }
        }

        if (!this.isValid(workspaceCreationRequest.settlementDate)) {
            if (!(workspaceCreationRequest.financialSettlement === "Yes" && workspaceCreationRequest.landTitleDetails.parentTitle === "No")) {
                workspaceCreationRequest.settlementDate = undefined;
                workspaceCreationRequest.settlementDateValueSpecified = false;
                workspaceCreationRequest.settlementDateAndTime = undefined;
                workspaceCreationRequest.settlementDateAndTimeValueSpecified = false;
            }
        } else {
            if (this.isValid(workspaceCreationRequest.settlementDateAndTime)) {
                workspaceCreationRequest.settlementDate = undefined;
                workspaceCreationRequest.settlementDateValueSpecified = false;
                workspaceCreationRequest.settlementDateAndTimeValueSpecified = true;
            } else {
                workspaceCreationRequest.settlementDate = convertDateToUTC(workspaceCreationRequest.settlementDate!);
                workspaceCreationRequest.settlementDateValueSpecified = true;
                workspaceCreationRequest.settlementDateAndTime = undefined;
                workspaceCreationRequest.settlementDateAndTimeValueSpecified = false;
            }
        }

        if (workspaceCreationRequest.financialSettlement === "No") {
            workspaceCreationRequest.settlementDate = undefined;
            workspaceCreationRequest.settlementDateValueSpecified = false;
            workspaceCreationRequest.settlementDateAndTime = undefined;
            workspaceCreationRequest.settlementDateAndTimeValueSpecified = false;
        }

        this.props.setPexaWorkspaceCreation(workspaceCreationRequest);

        this.props.onChangeStep(SubwayNavNodeState.Completed);
    }

    private isValid(value: any): boolean {
        if (value == null)
            return false;

        if (Object.prototype.toString.call(value) === '[object Date]') {
            return !isNaN(value.getTime());
        }

        if (typeof value == "string") {
            if (value === "") return false;
        }

        return true;
    }

    static getDerivedStateFromProps(nextProps: AppProps, prevState: AppStates): AppStates {
        let nextState = {} as AppStates;

        if (nextProps.gotResponse) {
            if (nextProps.requestType === CONSTANTS.VALIDATE_LAND_TITLE_REQUESTED) {
                if (nextProps.success && nextProps.validateLandTitleResponse) {
                    nextState.validateLandTitleResponse = nextProps.validateLandTitleResponse;
                    nextState.validationError = null;
                } else {
                    nextState.validateLandTitleResponse = null;
                    nextState.validationError = nextProps.error!;
                    nextState.showValidateModal = false;
                }

                nextProps.clearPexaState();
            }
        }

        if (prevState.workspaceCreationRequest.jurisdiction !== "" && prevState.workspaceCreationRequest.settlementDate !== null) {
            const jurisdiction = prevState.workspaceCreationRequest.jurisdiction;
            const settlementDate = formatISODateString(prevState.workspaceCreationRequest.settlementDate!);

            if (nextProps.availableSettlementTimes && nextProps.availableSettlementTimes[jurisdiction] && nextProps.availableSettlementTimes[jurisdiction][settlementDate]) {
                if (nextProps.availableSettlementTimes[jurisdiction][settlementDate].status === ReduxStatus.Success) {
                    const availabilityResponse = nextProps.availableSettlementTimes[jurisdiction][settlementDate].data!;
                    if (availabilityResponse.settlementAvailability === "Yes") {
                        const settlementTimes: IDropdownOption[] = availabilityResponse.availableSettlementTime!.map((settlementTime, index) => {
                            return {
                                key: formatDateTime12HourString(settlementTime),
                                text: formatDateTime12HourString(settlementTime)
                            };
                        })

                        nextState.availableSettlementTimes = settlementTimes;
                    }
                } else if (nextProps.availableSettlementTimes[jurisdiction][settlementDate].status === ReduxStatus.Failed) {
                    nextState.availableSettlementTimes = null;
                    nextState.errorMessage = nextProps.availableSettlementTimes[jurisdiction][settlementDate].error!.errorList![0];
                }
            }
        }

        return nextState;
    }

    private async validateLandTitle(): Promise<void> {
        const { workspaceCreationRequest } = this.state;
        const { landTitleDetails } = workspaceCreationRequest;

        const params = new LandTitleReferenceAndJurisdiction({
            landTitleReference: landTitleDetails.landTitle[0].landTitleReference,
            jurisdiction: workspaceCreationRequest.jurisdiction
        })
        this.props.validateLandTitle(params);

        await this.setState({
            showValidateModal: true,
            validateLandTitleResponse: null,
            validationError: null
        });
    }

    private closeValidateModal(): void {
        this.setState({
            showValidateModal: false,
            createNewWorkspace: false
        });
    }

    private createNewWorkspace(): void {
        const { validateLandTitleResponse } = this.state;
        this.closeValidateModal();
        if (validateLandTitleResponse && validateLandTitleResponse.landTitleReferenceReport && validateLandTitleResponse.landTitleReferenceReport.propertyDetails) {
            this.setState({
                createNewWorkspace: true
            })
        } else {
            this.setState({
                createNewWorkspace: false
            })
        }
    }

    render() {
        const { workspaceCreationRequest, actionstepMatter, addressTypes, pexaRoleSpecified, errorMessage, showValidateModal,
            validateLandTitleResponse, validationError, createNewWorkspace, availableWorkgroups, availableSettlementTimes } = this.state;
        const { landTitleDetails } = workspaceCreationRequest;
        let errorCount = 0;

        return (
            <div className="ms-Grid-row ms-sm12">
                <h3>
                    <big><b data-cy="pexa_wizard_title">PEXA WORKSPACE CREATION REQUEST</b></big>
                </h3>
                <br />
                <table className="pexa-data-list has-bottom-border">
                    <tbody>
                        <tr className="ms-Grid-row">
                            <td className="ms-Grid-col ms-sm3">
                                <big><b></b></big>
                            </td>
                            <td className="ms-Grid-col ms-sm9">
                                <table className="pexa-data-list">
                                    <tbody>
                                        <tr className="ms-Grid-row">
                                            <td className="pexa-data-key ms-Grid-col ms-sm6">
                                                Jurisdiction
                                            </td>
                                            <td className="pexa-data-value ms-Grid-col ms-sm6">
                                                <Dropdown
                                                    options={Jurisdictions}
                                                    data-cy="pexa_jurisdictions"
                                                    placeholder="Please select..."
                                                    errorMessage={this.isValid(workspaceCreationRequest.jurisdiction) ? "" : (++errorCount, " ")}
                                                    selectedKey={workspaceCreationRequest.jurisdiction}
                                                    onChange={(event, item) => this.changeWorkspaceCreationRequest(item!.key.toString(), "jurisdiction")}
                                                />
                                            </td>
                                        </tr>
                                        <tr className="ms-Grid-row">
                                            <td className="pexa-data-key ms-Grid-col ms-sm6">
                                                Role
                                            </td>
                                            <td className="pexa-data-value ms-Grid-col ms-sm6">
                                                <Dropdown
                                                    options={SubscriberRoles}
                                                    data-cy="pexa_role"
                                                    placeholder="Please select..."
                                                    errorMessage={pexaRoleSpecified ? "" : (++errorCount, " ")}
                                                    selectedKey={workspaceCreationRequest.role}
                                                    onChange={(event, item) => this.changeWorkspaceCreationRequest(item!.key, "role")}
                                                />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </tbody>
                </table>

                <table className="pexa-data-list has-bottom-border">
                    <tbody>
                        <tr className="ms-Grid-row">
                            <td className="ms-Grid-col ms-sm3">
                                <big><b>Land Title Details</b></big>
                            </td>
                            <td className="ms-Grid-col ms-sm9">
                                <table className="pexa-data-list">
                                    <tbody>
                                        <tr className="ms-Grid-row">
                                            <td className="ms-Grid-col ms-sm6 pexa-data-key">
                                                Parent Title:
                                            </td>
                                            <td className="ms-Grid-col ms-sm6 pexa-data-value">
                                                <Dropdown
                                                    options={BooleanItems}
                                                    data-cy="pexa_parent_title"
                                                    placeholder="Please select..."
                                                    errorMessage={this.isValid(landTitleDetails.parentTitle) ? "" : (++errorCount, " ")}
                                                    selectedKey={landTitleDetails.parentTitle}
                                                    onChange={(event, item) => this.changeWorkspaceCreationRequest(item!.key.toString(), "landTitleDetails.parentTitle")}
                                                />
                                            </td>
                                        </tr>

                                        <tr className="ms-Grid-row">
                                            <td className="ms-Grid-col ms-sm6 pexa-data-key">
                                                Land Title Reference:
                                            </td>
                                            <td className="ms-Grid-col ms-sm6 pexa-data-value">
                                                <TextField
                                                    value={landTitleDetails.landTitle[0].landTitleReference}
                                                    data-cy="pexa_land_title_reference"
                                                    errorMessage={this.isValid(landTitleDetails.landTitle[0].landTitleReference) ? "" : (++errorCount, " ")}
                                                    onChange={(event, newValue) => this.changeWorkspaceCreationRequest(newValue, "landTitleDetails.landTitle.0.landTitleReference")}
                                                />
                                            </td>
                                        </tr>

                                        <tr className="ms-Grid-row">
                                            <td className="ms-Grid-col ms-sm6 pexa-data-key"></td>
                                            <td className="ms-Grid-col ms-sm6 pexa-data-value">
                                                <PrimaryButton className="button ms-Grid-col ms-sm12"
                                                    text="Validate"
                                                    disabled={!this.isValid(landTitleDetails.landTitle[0].landTitleReference)}
                                                    onClick={() => this.validateLandTitle()}
                                                />
                                                {createNewWorkspace ?
                                                    <MessageBar messageBarType={MessageBarType.success}>
                                                        {validateLandTitleResponse!.landTitleReferenceReport!.propertyDetails!}
                                                    </MessageBar>
                                                    : !validationError ? <MessageBar messageBarType={MessageBarType.info}>
                                                        Please validate the Land Title reference to proceed
                                                    </MessageBar>
                                                        : <MessageBar messageBarType={MessageBarType.error}>
                                                            Invalid Land Title Reference value, please fix it and validate again
                                                        </MessageBar>
                                                }
                                            </td>
                                        </tr>

                                        <tr className="ms-Grid-row">
                                            <td className="ms-Grid-col ms-sm6 pexa-data-key">
                                                Unregistered Lot Reference:
                                            </td>
                                            <td className="ms-Grid-col ms-sm6 pexa-data-value">
                                                <TextField
                                                    value={landTitleDetails.landTitle[0].unregisteredLotReference}
                                                    data-cy="pexa_unregistered_lot_reference"
                                                    onChange={(event, newValue) => this.changeWorkspaceCreationRequest(newValue, "landTitleDetails.landTitle.0.unregisteredLotReference")}
                                                />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </tbody>
                </table>

                <fieldset
                    disabled={!createNewWorkspace}
                    style={!createNewWorkspace ? { pointerEvents: "none", opacity: "0.4" } : {}}
                    className="pexa-create-workspace-fieldset"
                >
                    {
                        workspaceCreationRequest.partyDetailsSpecified && workspaceCreationRequest.partyDetails &&
                        (workspaceCreationRequest.role === PexaRole.Incoming_Proprietor || workspaceCreationRequest.role === null) &&
                        <table className="pexa-data-list has-bottom-border">
                            <tbody>
                                <tr className="ms-Grid-row">
                                    <td className="ms-Grid-col ms-sm3">
                                        <big><b>Transaction Details</b></big>
                                    </td>
                                    <td className="ms-Grid-col ms-sm9">
                                        <table className="pexa-data-list">
                                            <tbody>
                                                <tr className="ms-Grid-row">
                                                    <td className="pexa-data-key ms-Grid-col ms-sm6">
                                                        Financial Settlement
                                                    </td>
                                                    <td className="pexa-data-value ms-Grid-col ms-sm6">
                                                        <Dropdown
                                                            options={BooleanItems}
                                                            data-cy="pexa_financial_settlement"
                                                            placeholder="Please select..."
                                                            errorMessage={this.isValid(workspaceCreationRequest.financialSettlement) ? "" : (++errorCount, " ")}
                                                            selectedKey={workspaceCreationRequest.financialSettlement}
                                                            onChange={(event, item) => this.changeWorkspaceCreationRequest(item!.key.toString(), "financialSettlement")}
                                                        />
                                                    </td>
                                                </tr>
                                                <tr className="ms-Grid-row">
                                                    <td className="pexa-data-key ms-Grid-col ms-sm6">
                                                        Participant Settlement Acceptance Status
                                                    </td>
                                                    <td className="pexa-data-value ms-Grid-col ms-sm6">
                                                        <Dropdown
                                                            options={ParticipantSettlementAcceptanceStatus}
                                                            data-cy="pexa_participant_settlement_acceptance_status"
                                                            placeholder="Please select..."
                                                            selectedKey={workspaceCreationRequest.participantSettlementAcceptanceStatus}
                                                            onChange={(event, item) => this.changeWorkspaceCreationRequest(item!.key.toString(), "participantSettlementAcceptanceStatus")}
                                                        />
                                                    </td>
                                                </tr>
                                                <tr className="ms-Grid-row">
                                                    <td className="pexa-data-key ms-Grid-col ms-sm6">
                                                        Request Land Title Data
                                                    </td>
                                                    <td className="pexa-data-value ms-Grid-col ms-sm6">
                                                        <Dropdown
                                                            options={BooleanItems}
                                                            data-cy="pexa_request_land_title_data"
                                                            placeholder="Please select..."
                                                            errorMessage={this.isValid(workspaceCreationRequest.requestLandTitleData) ? "" : (++errorCount, " ")}
                                                            selectedKey={workspaceCreationRequest.requestLandTitleData}
                                                            onChange={(event, item) => this.changeWorkspaceCreationRequest(item!.key.toString(), "requestLandTitleData")}
                                                        />
                                                    </td>
                                                </tr>
                                                {workspaceCreationRequest.financialSettlement === "Yes" &&
                                                    <tr className="ms-Grid-row">
                                                        <td className="pexa-data-key ms-Grid-col ms-sm6">
                                                            Settlement Date
                                                        </td>
                                                        <td className="pexa-data-value ms-Grid-col ms-sm6">
                                                            <DatePicker
                                                                showMonthPickerAsOverlay={true}
                                                                data-cy="pexa_settlement_date"
                                                                allowTextInput={true}
                                                                value={new Date(workspaceCreationRequest.settlementDate!)}
                                                                placeholder="Please select..."
                                                                formatDate={_onFormatDate}
                                                                onSelectDate={(newDate) => this.changeWorkspaceCreationRequest(newDate, "settlementDate")}
                                                            />
                                                        </td>
                                                    </tr>
                                                }

                                                {workspaceCreationRequest.financialSettlement === "Yes" &&
                                                    <tr className="ms-Grid-row">
                                                        <td className="pexa-data-key ms-Grid-col ms-sm6">
                                                            Settlement Time
                                                        </td>
                                                        <td className="pexa-data-value ms-Grid-col ms-sm6">
                                                            <Dropdown
                                                                options={availableSettlementTimes ? availableSettlementTimes : []}
                                                                disabled={!availableSettlementTimes || !availableSettlementTimes.length}
                                                                errorMessage={!availableSettlementTimes ? errorMessage! : ""}
                                                                data-cy="pexa_settlement_time"
                                                                placeholder="Please select..."
                                                                selectedKey={workspaceCreationRequest.settlementDateAndTime ? formatDateTime12HourString(workspaceCreationRequest.settlementDateAndTime) : ""}
                                                                onChange={(event, item) => this.changeWorkspaceCreationRequest(new Date(item!.text), "settlementDateAndTime")}
                                                            />
                                                        </td>
                                                    </tr>
                                                }

                                                <tr className="ms-Grid-row">
                                                    <td className="pexa-data-key ms-Grid-col ms-sm6">
                                                        Workgroup
                                                </td>
                                                    <td className="pexa-data-value ms-Grid-col ms-sm6">
                                                        <Dropdown
                                                            options={availableWorkgroups}
                                                            disabled={!availableWorkgroups || !availableWorkgroups.length}
                                                            data-cy="pexa_workgroup"
                                                            placeholder="Please select..."
                                                            selectedKey={workspaceCreationRequest.workgroupId ? workspaceCreationRequest.workgroupId : ""}
                                                            onChange={(event, item) => this.changeWorkspaceCreationRequest(item!.key, "workgroupId")}
                                                        />
                                                    </td>
                                                </tr>
                                                <tr className="ms-Grid-row">
                                                    <td className="pexa-data-key ms-Grid-col ms-sm6">
                                                        Subscriber Reference
                                                    </td>
                                                    <td className="pexa-data-value ms-Grid-col ms-sm6">
                                                        <TextField
                                                            value={workspaceCreationRequest.subscriberReference}
                                                            data-cy="pexa_subscriber_reference"
                                                            errorMessage={this.isValid(workspaceCreationRequest.subscriberReference) ? "" : (++errorCount, " ")}
                                                            onChange={(event, newValue) => this.changeWorkspaceCreationRequest(newValue, "subscriberReference")}
                                                        />
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    }

                    {workspaceCreationRequest.partyDetailsSpecified && workspaceCreationRequest.partyDetails &&
                        (workspaceCreationRequest.role === PexaRole.Incoming_Proprietor || workspaceCreationRequest.role === null) &&
                        <table className="pexa-data-list">
                            <tbody>
                                <tr className="ms-Grid-row">
                                    <td className="ms-Grid-col ms-sm3">
                                        <big><b>Party Details</b></big>
                                    </td>
                                    <td className="ms-Grid-col ms-sm9">
                                        {workspaceCreationRequest.partyDetails.map((partyDetail, index) => {
                                            const actionstepParty = (actionstepMatter.buyers && actionstepMatter.buyers[index]) || new Party();
                                            const addressType = addressTypes[index];

                                            let partyName = "";
                                            if (partyDetail.partyType === "Organisation") {
                                                if (partyDetail.business === undefined) {
                                                    partyDetail.business = new Business();
                                                    partyDetail.business.businessName = "";
                                                    partyDetail.business.legalEntityName = "";
                                                }

                                                partyName = partyDetail.business!.legalEntityName;
                                            } else {
                                                if (partyDetail.fullName === undefined) partyDetail.fullName = new FullName({
                                                    dateOfBirthValue: new Date(),
                                                    dateOfBirthValueSpecified: false,
                                                    givenName: [new GivenNameOrderType({
                                                        value: ""
                                                    })],
                                                    familyName: ""
                                                });

                                                partyName = partyDetail.fullName
                                                    ? ((partyDetail.fullName.givenName && partyDetail.fullName.givenName.length > 0) ? partyDetail.fullName.givenName[0].value + " " : "") + partyDetail.fullName.familyName
                                                    : "Unknown";
                                            }

                                            return (
                                                <table className="pexa-data-list" key={index}>
                                                    <tbody>
                                                        <tr>
                                                            <td colSpan={2} className="has-bottom-border">
                                                                <big><b>{partyName}</b></big>
                                                            </td>
                                                        </tr>
                                                        <tr className="ms-Grid-row">
                                                            <td className="pexa-data-key ms-Grid-col ms-sm6">
                                                                Name:
                                                            </td>
                                                            {partyDetail.partyType === "Organisation"
                                                                ?
                                                                <td className="pexa-data-value ms-Grid-col ms-sm6">
                                                                    <TextField
                                                                        value={partyDetail.business!.legalEntityName}
                                                                        data-cy={"pexa_legal_entity_name_" + index}
                                                                        onChange={(event, newValue) => this.changeWorkspaceCreationRequest(newValue, "partyDetails." + index + ".business.legalEntityName")}
                                                                    />
                                                                </td>
                                                                :
                                                                <>
                                                                    <td className="pexa-data-value ms-Grid-col ms-sm6 pexa-name-list">
                                                                        <TextField
                                                                            value={partyDetail.fullName!.givenName![0].value}
                                                                            data-cy={"pexa_first_name_" + index}
                                                                            onChange={(event, newValue) => this.changeWorkspaceCreationRequest(newValue, "partyDetails." + index + ".fullName.givenName.0.value")}
                                                                        />
                                                                        <TextField
                                                                            value={partyDetail.fullName!.givenName![1].value}
                                                                            data-cy={"pexa_middle_name_" + index}
                                                                            onChange={(event, newValue) => this.changeWorkspaceCreationRequest(newValue, "partyDetails." + index + ".fullName.givenName.1.value")}
                                                                        />
                                                                        <TextField
                                                                            value={partyDetail.fullName!.familyName}
                                                                            data-cy={"pexa_family_name_" + index}
                                                                            onChange={(event, newValue) => this.changeWorkspaceCreationRequest(newValue, "partyDetails." + index + ".fullName.familyName")}
                                                                        />
                                                                    </td>
                                                                </>
                                                            }
                                                        </tr>
                                                        <tr className="ms-Grid-row">
                                                            <td className="pexa-data-key ms-Grid-col ms-sm6">
                                                                Party Type:
                                                        </td>
                                                            <td className="pexa-data-value ms-Grid-col ms-sm6">
                                                                <Dropdown
                                                                    options={PartyTypes}
                                                                    data-cy={"pexa_party_type_" + index}
                                                                    errorMessage={this.isValid(partyDetail.partyType) ? "" : (++errorCount, " ")}
                                                                    placeholder="Please select..."
                                                                    selectedKey={partyDetail.partyType}
                                                                    onChange={(event, item) => this.changeWorkspaceCreationRequest(item!.key.toString(), "partyDetails." + index + ".partyType")}
                                                                />
                                                            </td>
                                                        </tr>
                                                        <tr className="ms-Grid-row">
                                                            <td className="pexa-data-key ms-Grid-col ms-sm6">
                                                                Representing Party:
                                                            </td>
                                                            <td className="pexa-data-value ms-Grid-col ms-sm6">
                                                                <Dropdown
                                                                    options={BooleanItems}
                                                                    data-cy={"pexa_representing_party_" + index}
                                                                    placeholder="Please select..."
                                                                    errorMessage={this.isValid(partyDetail.representingParty) ? "" : (++errorCount, " ")}
                                                                    selectedKey={partyDetail.representingParty}
                                                                    onChange={(event, item) => this.changeWorkspaceCreationRequest(item!.key.toString(), "partyDetails." + index + ".representingParty")}
                                                                />
                                                            </td>
                                                        </tr>

                                                        {partyDetail.partyType === "Individual" &&
                                                            <tr className="ms-Grid-row">
                                                                <td className="pexa-data-key ms-Grid-col ms-sm6">
                                                                    Date of Birth:
                                                                </td>
                                                                <td className="pexa-data-value ms-Grid-col ms-sm6">
                                                                    <DatePicker
                                                                        showMonthPickerAsOverlay={true}
                                                                        data-cy={"pexa_data_of_birth_" + index}
                                                                        allowTextInput={true}
                                                                        value={new Date(partyDetail.fullName!.dateOfBirth!).getTime() !== 0 ? new Date(partyDetail.fullName!.dateOfBirth!) : undefined}
                                                                        placeholder="Please select..."
                                                                        formatDate={_onFormatDate}
                                                                        onSelectDate={(newDate) => this.changeWorkspaceCreationRequest(newDate, "partyDetails." + index + ".fullName.dateOfBirth")}
                                                                    />
                                                                </td>
                                                            </tr>
                                                        }

                                                        {partyDetail.partyType === "Organisation" &&
                                                            <tr className="ms-Grid-row">
                                                                <td className="pexa-data-key ms-Grid-col ms-sm6">
                                                                    Business Name:
                                                                </td>
                                                                <td className="pexa-data-value ms-Grid-col ms-sm6">
                                                                    <TextField
                                                                        value={partyDetail.business!.businessName}
                                                                        data-cy={"pexa_business_name_" + index}
                                                                        errorMessage={this.isValid(partyDetail.business!.businessName) ? "" : (++errorCount, " ")}
                                                                        onChange={(event, newValue) => this.changeWorkspaceCreationRequest(newValue, "partyDetails." + index + ".business.businessName")}
                                                                    />
                                                                </td>
                                                            </tr>
                                                        }

                                                        {partyDetail.partyType === "Organisation" &&
                                                            <tr className="ms-Grid-row">
                                                                <td className="pexa-data-key ms-Grid-col ms-sm6">
                                                                    Organisation Type:
                                                                </td>
                                                                <td className="pexa-data-value ms-Grid-col ms-sm6">
                                                                    <Dropdown
                                                                        options={OrganisationTypes}
                                                                        data-cy={"pexa_organisation_type_" + index}
                                                                        placeholder="Please select..."
                                                                        selectedKey={partyDetail.business!.organisationType}
                                                                        errorMessage={this.isValid(partyDetail.business!.organisationType) ? "" : (++errorCount, " ")}
                                                                        onChange={(event, item) => this.changeWorkspaceCreationRequest(item!.key.toString(), "partyDetails." + index + ".business.organisationType")}
                                                                    />
                                                                </td>
                                                            </tr>
                                                        }

                                                        <tr className="ms-Grid-row">
                                                            <td className="pexa-data-key ms-Grid-col ms-sm6">
                                                                Address Type:
                                                            </td>
                                                            <td className="pexa-data-value ms-Grid-col ms-sm6">
                                                                <Dropdown
                                                                    options={AddressTypes}
                                                                    data-cy={"pexa_address_type_" + index}
                                                                    errorMessage={this.isValid(addressType) ? "" : (++errorCount, " ")}
                                                                    selectedKey={addressType}
                                                                    placeholder="Please select..."
                                                                    onChange={(event, item) => this.changeAddressType(item!.key as PartyAddressType, index)}
                                                                />
                                                            </td>
                                                        </tr>

                                                        {addressType === PartyAddressType.Unit_Apartment &&
                                                            <tr>
                                                                <td colSpan={2}>
                                                                    <div className="ms-Grid-col ms-sm6">
                                                                        <div className="pexa-data-key">
                                                                            Unit Type:
                                                                    </div>
                                                                        <div className="pexa-data-value">
                                                                            <Dropdown
                                                                                options={SubDwellingUnitTypes}
                                                                                data-cy={"pexa_unit_type_" + index}
                                                                                placeholder="Please select..."
                                                                                errorMessage={this.isValid(partyDetail.currentAddress!.streetAddress!.subDwellingUnitType!.unitTypeCode) ? "" : (++errorCount, " ")}
                                                                                selectedKey={partyDetail.currentAddress!.streetAddress!.subDwellingUnitType!.unitTypeCode}
                                                                                onChange={(event, item) => this.changeWorkspaceCreationRequest(item!.key.toString(), "partyDetails." + index + ".currentAddress.streetAddress.subDwellingUnitType.unitTypeCode")}
                                                                            />
                                                                        </div>
                                                                    </div>
                                                                    <div className="ms-Grid-col ms-sm6">
                                                                        <div className="pexa-data-key">
                                                                            Unit Number:
                                                                        </div>
                                                                        <TextField
                                                                            value={partyDetail.currentAddress!.streetAddress!.subDwellingUnitType!.unitNumber}
                                                                            data-cy={"pexa_unit_number_" + index}
                                                                            errorMessage={this.isValid(partyDetail.currentAddress!.streetAddress!.subDwellingUnitType!.unitNumber) ? "" : (++errorCount, " ")}
                                                                            onChange={(event, newValue) => this.changeWorkspaceCreationRequest(newValue, "partyDetails." + index + ".currentAddress.streetAddress.subDwellingUnitType.unitNumber")}
                                                                        />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        }

                                                        {addressType === PartyAddressType.Correspondence_Address &&
                                                            <tr>
                                                                <td colSpan={2}>
                                                                    <div className="ms-Grid-col ms-sm6">
                                                                        <div className="pexa-data-key">
                                                                            Postal Delivery Type:
                                                                        </div>
                                                                        <div className="pexa-data-value">
                                                                            <Dropdown
                                                                                options={PostalDeliveryTypeCodes}
                                                                                data-cy={"pexa_postal_delivery_type_" + index}
                                                                                placeholder="Please select..."
                                                                                errorMessage={this.isValid(partyDetail.currentAddress!.correspondenceAddress!.postalDelivery.postalDeliveryTypeCode) ? "" : (++errorCount, " ")}
                                                                                selectedKey={partyDetail.currentAddress!.correspondenceAddress!.postalDelivery.postalDeliveryTypeCode}
                                                                                onChange={(event, item) => this.changeWorkspaceCreationRequest(item!.key.toString(), "partyDetails." + index + ".currentAddress.correspondenceAddress.postalDelivery.postalDeliveryTypeCode")}
                                                                            />
                                                                        </div>
                                                                    </div>
                                                                    <div className="ms-Grid-col ms-sm6">
                                                                        <div className="pexa-data-key">
                                                                            Postal Delivery Number:
                                                                        </div>
                                                                        <TextField
                                                                            value={partyDetail.currentAddress!.correspondenceAddress!.postalDelivery.postalDeliveryNumber}
                                                                            data-cy={"pexa_postal_delivery_number_" + index}
                                                                            onChange={(event, newValue) => this.changeWorkspaceCreationRequest(newValue, "partyDetails." + index + ".currentAddress.correspondenceAddress.postalDelivery.postalDeliveryNumber")}
                                                                        />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        }

                                                        <tr className="ms-Grid-row">
                                                            <td colSpan={2} className="ms-Grid-col ms-sm12">
                                                                <MessageBar styles={{
                                                                    root: {
                                                                        background: 'rgba(113, 175, 229, 0.2)'
                                                                    }
                                                                }}>
                                                                    Actionstep Data: <br />
                                                                    <b>Address Line 1: </b> {actionstepParty.addressLine1} <br />
                                                                    <b>Address Line 2: </b> {actionstepParty.addressLine2}
                                                                </MessageBar>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td className="ms-Grid-row" colSpan={2}>
                                                                <div className="ms-Grid-col ms-sm3">
                                                                    <div className="pexa-data-key">
                                                                        Road Number:
                                                                    </div>
                                                                    <TextField
                                                                        value={partyDetail.currentAddress!.correspondenceAddress!.road.roadNumber}
                                                                        data-cy={"pexa_road_number_" + index}
                                                                        onChange={(event, newValue) => this.changeWorkspaceCreationRequest(newValue, "partyDetails." + index + ".currentAddress.correspondenceAddress.road.roadNumber")}
                                                                    />
                                                                </div>
                                                                <div className="ms-Grid-col ms-sm3">
                                                                    <div className="pexa-data-key">
                                                                        Road Name:
                                                                    </div>
                                                                    <TextField
                                                                        value={partyDetail.currentAddress!.correspondenceAddress!.road.roadName}
                                                                        data-cy={"pexa_road_name_" + index}
                                                                        errorMessage={this.isValid(partyDetail.currentAddress!.correspondenceAddress!.road.roadName) ? "" : (++errorCount, " ")}
                                                                        onChange={(event, newValue) => this.changeWorkspaceCreationRequest(newValue, "partyDetails." + index + ".currentAddress.correspondenceAddress.road.roadName")}
                                                                    />
                                                                </div>
                                                                <div className="ms-Grid-col ms-sm3">
                                                                    <div className="pexa-data-key">
                                                                        Road Type:
                                                                    </div>
                                                                    <Dropdown
                                                                        options={RoadTypeCodes}
                                                                        data-cy={"pexa_road_type_code_" + index}
                                                                        placeholder="Please select..."
                                                                        errorMessage={this.isValid(partyDetail.currentAddress!.correspondenceAddress!.road.roadTypeCode) ? "" : (++errorCount, " ")}
                                                                        selectedKey={partyDetail.currentAddress!.correspondenceAddress!.road.roadTypeCode}
                                                                        onChange={(event, item) => this.changeWorkspaceCreationRequest(item!.key.toString(), "partyDetails." + index + ".currentAddress.correspondenceAddress.road.roadTypeCode")}
                                                                    />
                                                                </div>
                                                                <div className="ms-Grid-col ms-sm3">
                                                                    <div className="pexa-data-key">
                                                                        Road Suffix:
                                                                    </div>
                                                                    <Dropdown
                                                                        options={RoadSuffixes}
                                                                        data-cy={"pexa_road_suffix_" + index}
                                                                        placeholder="Please select..."
                                                                        selectedKey={partyDetail.currentAddress!.correspondenceAddress!.road.roadSuffixCode}
                                                                        onChange={(event, item) => this.changeWorkspaceCreationRequest(item!.key.toString(), "partyDetails." + index + ".currentAddress.correspondenceAddress.road.roadSuffixCode")}
                                                                    />
                                                                </div>
                                                            </td>
                                                        </tr>

                                                        {addressType === PartyAddressType.Unit_Apartment &&
                                                            <tr>
                                                                <td className="ms-Grid-row">
                                                                    <div className="ms-Grid-col ms-sm12">
                                                                        <div className="pexa-data-key">
                                                                            Place Name:
                                                                        </div>
                                                                        <TextField
                                                                            value={partyDetail.currentAddress!.streetAddress!.addressSiteName}
                                                                            data-cy={"pexa_place_name_" + index}
                                                                            onChange={(event, newValue) => this.changeWorkspaceCreationRequest(newValue, "partyDetails." + index + ".currentAddress.streetAddress.addressSiteName")}
                                                                        />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        }

                                                        <tr className="ms-Grid-row">
                                                            <td colSpan={2} className="ms-Grid-col ms-sm12">
                                                                <MessageBar styles={{
                                                                    root: {
                                                                        background: 'rgba(113, 175, 229, 0.2)'
                                                                    }
                                                                }}>
                                                                    Actionstep Data: <br />
                                                                    <b>City: </b> {actionstepParty.city} <br />
                                                                    <b>State/Province: </b> {actionstepParty.stateProvince} <br />
                                                                    <b>ZIP/Post Code: </b> {actionstepParty.postCode}
                                                                </MessageBar>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td className="ms-Grid-row">
                                                                <div className="ms-Grid-col ms-sm4">
                                                                    <div className="pexa-data-key">
                                                                        City/Suburb:
                                                                    </div>
                                                                    <TextField
                                                                        value={partyDetail.currentAddress!.correspondenceAddress!.localityName}
                                                                        data-cy={"pexa_city_suburb_" + index}
                                                                        errorMessage={this.isValid(partyDetail.currentAddress!.correspondenceAddress!.localityName) ? "" : (++errorCount, " ")}
                                                                        onChange={(event, newValue) => this.changeWorkspaceCreationRequest(newValue, "partyDetails." + index + ".currentAddress.correspondenceAddress.localityName")}
                                                                    />
                                                                </div>
                                                                <div className="ms-Grid-col ms-sm4">
                                                                    <div className="pexa-data-key">
                                                                        State:
                                                                    </div>
                                                                    <Dropdown
                                                                        options={PartyDetailStates}
                                                                        data-cy={"pexa_state_" + index}
                                                                        placeholder="Please select..."
                                                                        errorMessage={this.isValid(partyDetail.currentAddress!.correspondenceAddress!.state) ? "" : (++errorCount, " ")}
                                                                        selectedKey={partyDetail.currentAddress!.correspondenceAddress!.state}
                                                                        onChange={(event, item) => this.changeWorkspaceCreationRequest(item!.key.toString(), "partyDetails." + index + ".currentAddress.correspondenceAddress.state")}
                                                                    />
                                                                </div>
                                                                <div className="ms-Grid-col ms-sm4">
                                                                    <div className="pexa-data-key">
                                                                        Post Code:
                                                                    </div>
                                                                    <TextField
                                                                        value={partyDetail.currentAddress!.correspondenceAddress!.postcode}
                                                                        data-cy={"pexa_post_code_" + index}
                                                                        errorMessage={this.isValid(partyDetail.currentAddress!.correspondenceAddress!.postcode) ? "" : (++errorCount, " ")}
                                                                        onChange={(event, newValue) => this.changeWorkspaceCreationRequest(newValue, "partyDetails." + index + ".currentAddress.correspondenceAddress.postcode")}
                                                                    />
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            );
                                        })}
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    }
                </fieldset>
                <br />

                <DefaultButton
                    className="button"
                    data-automation-id="pexa_create_button"
                    disabled={errorCount !== 0 || !createNewWorkspace}
                    data-cy="pexa_create_button"
                    text="Create PEXA Workspace"
                    onClick={() => this.approveData()}
                    allowDisabledFocus={true}
                />

                {
                    errorMessage &&
                    <MessageBar messageBarType={MessageBarType.error}>
                        {errorMessage}
                    </MessageBar>
                }

                <Modal isOpen={showValidateModal}
                    isBlocking={true}
                    onDismiss={() => this.closeValidateModal()}
                    className={showValidateModal !== null ? "animated fadeIn" : "animated fadeOut"}
                >
                    <div className="modal-header">
                        <span className="modal-title">Validate Land Title Reference</span>
                    </div>
                    <div className="modal-body">
                        {validateLandTitleResponse ?
                            <ValidateLandTitle
                                warnings={validateLandTitleResponse.warnings}
                                landTitleReference={landTitleDetails.landTitle[0].landTitleReference}
                                landTitleReferenceReport={validateLandTitleResponse.landTitleReferenceReport}
                                subscriberRole={workspaceCreationRequest.role}
                            />
                            : validationError ?
                                <MessageBar messageBarType={MessageBarType.error}>
                                    {validationError.message}
                                </MessageBar>
                                : <LoadingWidget />}
                    </div>
                    {validateLandTitleResponse &&
                        <div className="modal-footer">
                            <DefaultButton
                                className="button"
                                data-automation-id="create_new_workspace"
                                data-cy="create_new_workspace"
                                text="Create New Workspace"
                                onClick={() => this.createNewWorkspace()}
                                allowDisabledFocus={true}
                            />
                            &nbsp;
                            <DefaultButton
                                className="button"
                                data-automation-id="close_modal"
                                data-cy="close_modal"
                                text="Cancel"
                                onClick={() => this.closeValidateModal()}
                                allowDisabledFocus={true}
                            />
                        </div>
                    }
                </Modal>
            </div>
        );
    }
}

const mapStateToProps = (state: AppState): IMapStateToProps => {
    return {
        availableSettlementTimes: state.pexa.availableSettlementTimes,
        jwtMatterInfo: state.common.jwtMatterInfo,
        pexaWorkspaceCreationData: state.pexa.pexaWorkspaceCreationData,
        pexaFormData: state.pexa.pexaFormData,
        validateLandTitleResponse: state.pexa.validateLandTitleResponse,
        requestType: state.pexa.requestType,
        success: state.pexa.success,
        error: state.pexa.error,
        gotResponse: state.pexa.gotResponse
    }
}

const mapDispatchToProps: IMapDispatchToProps = {
    getAvailableSettlementTimes,
    setPexaWorkspaceCreation,
    validateLandTitle,
    savePexaFormData,
    clearPexaState,
    clearPexaFormData
}

export default connect(mapStateToProps, mapDispatchToProps)(ConveyancingDataCheck);