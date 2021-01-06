import * as CONSTANTS from './constants'
import {
    PEXAWorkspaceCreationRequestWithActionstepResponse,
    CreatePexaWorkspaceCommand,
    ErrorViewModel,
    LandTitleReferenceVerificationResponseType,
    WorkspaceSummaryResponseType,
    CreateWorkspaceInvitationResponseType,
    RetrieveSettlementAvailabilityResponseType,
    CreatePexaWorkspaceResponse
} from 'utils/wcaApiTypes';

import {
    PexaActionTypes,
    SetPexaWorkspaceCreation,
    GetDataFromActionstepSuccess,
    SendDataToPexaSuccess,
    ActionFailed,
    WorkspaceCreationRequestWithMatterInfo,
    SavePexaFormData,
    ValidateLandTitleSuccess,
    WorkspaceSummaryList,
    GetPexaWorkspaceSummary,
    GetPexaWorkspaceSummaryFailed,
    GetPexaWorkspaceSummarySuccess,
    CreateInvitationSuccess,
    AvailableSettlementTimes,
    GetAvailableSettlementTimes,
    GetAvailableSettlementTimesSuccess,
    GetAvailableSettlementTimesFailed
} from 'containers/pexa/redux/actionTypes';
import { ReduxData, ReduxStatus } from 'app.types';

export interface PexaState {
    gotResponse: boolean,
    success: boolean,
    pexaWorkspaceCreationData: PEXAWorkspaceCreationRequestWithActionstepResponse,
    createPexaWorkspaceResponse: CreatePexaWorkspaceResponse | undefined,
    availableSettlementTimes: AvailableSettlementTimes,
    validateLandTitleResponse: LandTitleReferenceVerificationResponseType | undefined,
    error: ErrorViewModel | undefined,
    requestType: string,
    pexaFormData: WorkspaceCreationRequestWithMatterInfo | undefined,
    workspaceSummaryList: WorkspaceSummaryList,
    invitationResponseList: ReduxData<CreateWorkspaceInvitationResponseType[]> | undefined
}

const initialState: PexaState = {
    gotResponse: false,
    success: false,
    pexaWorkspaceCreationData: new PEXAWorkspaceCreationRequestWithActionstepResponse(),
    createPexaWorkspaceResponse: undefined,
    availableSettlementTimes: {} as AvailableSettlementTimes,
    validateLandTitleResponse: undefined,
    error: undefined,
    requestType: "",
    pexaFormData: undefined,
    workspaceSummaryList: {} as WorkspaceSummaryList,
    invitationResponseList: undefined
}

function pexaReducer(state: PexaState = initialState, action: PexaActionTypes): PexaState {

    switch (action.type) {
        case CONSTANTS.GET_DATA_FROM_ACTIONSTEP_REQUESTED:
        case CONSTANTS.VALIDATE_LAND_TITLE_REQUESTED:
            return {
                ...state,
                requestType: action.type
            };

        case CONSTANTS.SEND_DATA_TO_PEXA_REQUESTED:
            return {
                ...state,
                createPexaWorkspaceResponse: undefined,
                requestType: action.type
            };

        case CONSTANTS.GET_PEXA_WORKSPACE_SUMMARY_REQUESTED:
            return {
                ...state,
                workspaceSummaryList: {
                    ...state.workspaceSummaryList,
                    [(action as GetPexaWorkspaceSummary).params.workspaceId!]: new ReduxData<WorkspaceSummaryResponseType>(ReduxStatus.Requested)
                }
            };

        case CONSTANTS.GET_AVAILABLE_SETTLEMENT_TIMES_REQUESETD:
            const settlementTimesParams = (action as GetAvailableSettlementTimes).params;
            return {
                ...state,
                availableSettlementTimes: {
                    ...state.availableSettlementTimes,
                    [settlementTimesParams.jurisdiction!]: {
                        ...state.availableSettlementTimes[settlementTimesParams.jurisdiction!],
                        [settlementTimesParams.settlementDate!]: new ReduxData<RetrieveSettlementAvailabilityResponseType>(ReduxStatus.Requested)
                    }
                }
            };

        case CONSTANTS.CREATE_INVITATION_REQUESTED:
            return {
                ...state,
                invitationResponseList: new ReduxData<CreateWorkspaceInvitationResponseType[]>(ReduxStatus.Requested)
            };

        case CONSTANTS.SET_PEXA_WORKSPACE_CREATION_REQUEST:
            return {
                ...state,
                pexaWorkspaceCreationData: new PEXAWorkspaceCreationRequestWithActionstepResponse({
                    ...state.pexaWorkspaceCreationData,
                    createPexaWorkspaceCommand: new CreatePexaWorkspaceCommand({
                        ...state.pexaWorkspaceCreationData.createPexaWorkspaceCommand!,
                        pexaWorkspaceCreationRequest: (action as SetPexaWorkspaceCreation).data
                    })
                })
            }

        case CONSTANTS.GET_DATA_FROM_ACTIONSTEP_SUCCESS:
            return {
                ...state,
                gotResponse: true,
                success: true,
                pexaWorkspaceCreationData: (action as GetDataFromActionstepSuccess).data
            };

        case CONSTANTS.VALIDATE_LAND_TITLE_SUCCESS:
            return {
                ...state,
                gotResponse: true,
                success: true,
                validateLandTitleResponse: (action as ValidateLandTitleSuccess).data
            }

        case CONSTANTS.SEND_DATA_TO_PEXA_SUCCESS:
            return {
                ...state,
                gotResponse: true,
                success: true,
                createPexaWorkspaceResponse: (action as SendDataToPexaSuccess).data
            };

        case CONSTANTS.GET_PEXA_WORKSPACE_SUMMARY_SUCCESS:
            const getPexaWorkspaceSummarySuccessAction = action as GetPexaWorkspaceSummarySuccess;
            return {
                ...state,
                workspaceSummaryList: {
                    ...state.workspaceSummaryList,
                    [getPexaWorkspaceSummarySuccessAction.workspaceId]: new ReduxData<WorkspaceSummaryResponseType>().markAsSuccess(getPexaWorkspaceSummarySuccessAction.data)
                }
            };

        case CONSTANTS.GET_AVAILABLE_SETTLEMENT_TIMES_SUCCESS:
            const availableTimesResponse = (action as GetAvailableSettlementTimesSuccess).data;
            return {
                ...state,
                availableSettlementTimes: {
                    ...state.availableSettlementTimes,
                    [(action as GetAvailableSettlementTimesSuccess).jurisdiction]: {
                        ...state.availableSettlementTimes[(action as GetAvailableSettlementTimesSuccess).jurisdiction],
                        [(action as GetAvailableSettlementTimesSuccess).settlementDate]: new ReduxData<RetrieveSettlementAvailabilityResponseType>().markAsSuccess(availableTimesResponse)
                    }
                }
            };

        case CONSTANTS.CREATE_INVITATION_SUCCESS:
            return {
                ...state,
                invitationResponseList: new ReduxData<CreateWorkspaceInvitationResponseType[]>().markAsSuccess((action as CreateInvitationSuccess).data)
            };

        case CONSTANTS.GET_DATA_FROM_ACTIONSTEP_FAILED:
        case CONSTANTS.VALIDATE_LAND_TITLE_FAILED:
        case CONSTANTS.SEND_DATA_TO_PEXA_FAILED:
            return {
                ...state,
                gotResponse: true,
                success: false,
                error: (action as ActionFailed).error
            };

        case CONSTANTS.GET_PEXA_WORKSPACE_SUMMARY_FAILED:
            const getPexaWorkspaceSummaryFailedAction = action as GetPexaWorkspaceSummaryFailed;
            return {
                ...state,
                error: getPexaWorkspaceSummaryFailedAction.error,
                workspaceSummaryList: {
                    ...state.workspaceSummaryList,
                    [getPexaWorkspaceSummaryFailedAction.workspaceId]: new ReduxData<WorkspaceSummaryResponseType>().markAsFailed(getPexaWorkspaceSummaryFailedAction.error)
                }
            };

        case CONSTANTS.GET_AVAILABLE_SETTLEMENT_TIMES_FAILED:
            return {
                ...state,
                availableSettlementTimes: {
                    ...state.availableSettlementTimes,
                    [(action as GetAvailableSettlementTimesFailed).jurisdiction]: {
                        ...state.availableSettlementTimes[(action as GetAvailableSettlementTimesFailed).jurisdiction],
                        [(action as GetAvailableSettlementTimesFailed).settlementDate]: new ReduxData<RetrieveSettlementAvailabilityResponseType>().markAsFailed((action as GetAvailableSettlementTimesFailed).error)
                    }
                }
            };

        case CONSTANTS.CREATE_INVITATION_FAILED:
            return {
                ...state,
                invitationResponseList: new ReduxData<CreateWorkspaceInvitationResponseType[]>().markAsFailed((action as ActionFailed).error)
            };

        case CONSTANTS.SAVE_PEXA_FORM_DATA:
            return {
                ...state,
                pexaFormData: (action as SavePexaFormData).pexaFormData
            }

        case CONSTANTS.CLEAR_PEXA_FORM_DATA:
            return {
                ...state,
                pexaFormData: undefined
            }

        case CONSTANTS.CLEAR_PEXA_STATE:
            return {
                ...state,
                gotResponse: false,
                success: false,
                requestType: ""
            };

        default:
            return state;
    }
}

export default pexaReducer;