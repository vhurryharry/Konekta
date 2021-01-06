import {
    ErrorViewModel,
    PEXAWorkspaceCreationRequestWithActionstepResponse,
    WorkspaceCreationRequest,
    CreatePexaWorkspaceCommand,
    ActionstepMatterInfo,
    CreatePexaWorkspaceResponse,
    LandTitleReferenceAndJurisdiction,
    LandTitleReferenceVerificationResponseType,
    WorkspaceSummaryResponseType,
    RetrieveWorkspaceSummaryParameters,
    CreateWorkspaceInvitationRequestType,
    CreateWorkspaceInvitationResponseType,
    RetrieveSettlementAvailabilityParams,
    RetrieveSettlementAvailabilityResponseType
} from "utils/wcaApiTypes";
import { IBasicAction, ReduxData } from "app.types";
import { PartyAddressType } from "containers/pexa/steps/constants";

export interface ActionFailed extends IBasicAction {
    error: ErrorViewModel;
}

export interface GetDataFromActionstep extends IBasicAction {
    matterInfo: ActionstepMatterInfo
}

export interface GetDataFromActionstepSuccess extends IBasicAction {
    data: PEXAWorkspaceCreationRequestWithActionstepResponse
}

export interface SetPexaWorkspaceCreation extends IBasicAction {
    data: WorkspaceCreationRequest
}

export interface ValidateLandTitle extends IBasicAction {
    params: LandTitleReferenceAndJurisdiction
}

export interface ValidateLandTitleSuccess extends IBasicAction {
    data: LandTitleReferenceVerificationResponseType
}

export interface GetPexaWorkspaceSummary extends IBasicAction {
    params: RetrieveWorkspaceSummaryParameters
}

export interface GetPexaWorkspaceSummarySuccess extends IBasicAction {
    data: WorkspaceSummaryResponseType,
    workspaceId: string
}

export interface GetPexaWorkspaceSummaryFailed extends ActionFailed {
    workspaceId: string
}

export interface GetAvailableSettlementTimes extends IBasicAction {
    params: RetrieveSettlementAvailabilityParams
}

export interface GetAvailableSettlementTimesSuccess extends IBasicAction {
    data: RetrieveSettlementAvailabilityResponseType,
    jurisdiction: string,
    settlementDate: string
}

export interface GetAvailableSettlementTimesFailed extends ActionFailed {
    jurisdiction: string,
    settlementDate: string
}

export interface SendDataToPexa extends IBasicAction {
    params: CreatePexaWorkspaceCommand
}

export interface SendDataToPexaSuccess extends IBasicAction {
    data: CreatePexaWorkspaceResponse
}

export interface CreateInvitation extends IBasicAction {
    params: CreateWorkspaceInvitationRequestType[]
}

export interface CreateInvitationSuccess extends IBasicAction {
    data: CreateWorkspaceInvitationResponseType[]
}

export interface SavePexaFormData extends IBasicAction {
    pexaFormData: WorkspaceCreationRequestWithMatterInfo
}

export interface ClearPexaFormData extends IBasicAction { }

export class WorkspaceCreationRequestWithMatterInfo {
    workspaceCreationRequest: WorkspaceCreationRequest;
    addressTypes: (PartyAddressType | null)[];
    matterInfo: ActionstepMatterInfo;
    validateLandTitleResponse: LandTitleReferenceVerificationResponseType;

    constructor(workspaceCreationRequest: WorkspaceCreationRequest,
        addressTypes: (PartyAddressType | null)[],
        matterInfo: ActionstepMatterInfo,
        validateLandTitleResponse: LandTitleReferenceVerificationResponseType | null
    ) {
        this.workspaceCreationRequest = WorkspaceCreationRequest.fromJS(workspaceCreationRequest.toJSON());
        this.addressTypes = addressTypes;
        this.matterInfo = matterInfo;
        this.validateLandTitleResponse = LandTitleReferenceVerificationResponseType.fromJS(validateLandTitleResponse);
    }
}

export interface ClearPexaState extends IBasicAction { }

export interface WorkspaceSummaryList {
    [workspaceId: string]: ReduxData<WorkspaceSummaryResponseType>
}

export interface AvailableSettlementTimesForJurisdiction {
    [dateString: string]: ReduxData<RetrieveSettlementAvailabilityResponseType>
}

export interface AvailableSettlementTimes {
    [jurisdiction: string]: AvailableSettlementTimesForJurisdiction
}

export type PexaActionTypes = GetDataFromActionstep
    | GetDataFromActionstepSuccess
    | ValidateLandTitle
    | ValidateLandTitleSuccess
    | GetPexaWorkspaceSummary
    | GetPexaWorkspaceSummarySuccess
    | GetPexaWorkspaceSummaryFailed
    | SetPexaWorkspaceCreation
    | SendDataToPexa
    | SendDataToPexaSuccess
    | ClearPexaState
    | ActionFailed;