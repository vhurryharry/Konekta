import * as CONSTANTS from './constants'
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
    CreateWorkspaceInvitationResponseType,
    CreateWorkspaceInvitationRequestType,
    RetrieveSettlementAvailabilityParams,
    RetrieveSettlementAvailabilityResponseType
} from 'utils/wcaApiTypes'
import {
    GetDataFromActionstep,
    GetDataFromActionstepSuccess,
    ActionFailed,
    SetPexaWorkspaceCreation,
    SendDataToPexa,
    SendDataToPexaSuccess,
    ClearPexaState,
    WorkspaceCreationRequestWithMatterInfo,
    SavePexaFormData,
    ClearPexaFormData,
    ValidateLandTitle,
    ValidateLandTitleSuccess,
    GetPexaWorkspaceSummary,
    GetPexaWorkspaceSummarySuccess,
    GetPexaWorkspaceSummaryFailed,
    CreateInvitation,
    CreateInvitationSuccess,
    GetAvailableSettlementTimes,
    GetAvailableSettlementTimesSuccess,
    GetAvailableSettlementTimesFailed
} from './actionTypes'

export function getDataFromActionstep(matterInfo: ActionstepMatterInfo): GetDataFromActionstep {
    return {
        type: CONSTANTS.GET_DATA_FROM_ACTIONSTEP_REQUESTED,
        matterInfo
    }
}

export function getDataFromActionstepSuccess(data: PEXAWorkspaceCreationRequestWithActionstepResponse): GetDataFromActionstepSuccess {
    return {
        type: CONSTANTS.GET_DATA_FROM_ACTIONSTEP_SUCCESS,
        data
    }
}

export function getDataFromActionstepFailed(error: ErrorViewModel): ActionFailed {
    return {
        type: CONSTANTS.GET_DATA_FROM_ACTIONSTEP_FAILED,
        error
    }
}

export function setPexaWorkspaceCreation(data: WorkspaceCreationRequest): SetPexaWorkspaceCreation {
    return {
        type: CONSTANTS.SET_PEXA_WORKSPACE_CREATION_REQUEST,
        data
    }
}

export function validateLandTitle(params: LandTitleReferenceAndJurisdiction): ValidateLandTitle {
    return {
        type: CONSTANTS.VALIDATE_LAND_TITLE_REQUESTED,
        params
    }
}

export function validateLandTitleSuccess(data: LandTitleReferenceVerificationResponseType): ValidateLandTitleSuccess {
    return {
        type: CONSTANTS.VALIDATE_LAND_TITLE_SUCCESS,
        data
    }
}

export function validateLandTitleFailed(error: ErrorViewModel): ActionFailed {
    return {
        type: CONSTANTS.VALIDATE_LAND_TITLE_FAILED,
        error
    }
}

export function getPexaWorkspaceSummary(params: RetrieveWorkspaceSummaryParameters): GetPexaWorkspaceSummary {
    return {
        type: CONSTANTS.GET_PEXA_WORKSPACE_SUMMARY_REQUESTED,
        params
    }
}

export function getPexaWorkspaceSummarySuccess(workspaceId: string, data: WorkspaceSummaryResponseType): GetPexaWorkspaceSummarySuccess {
    return {
        type: CONSTANTS.GET_PEXA_WORKSPACE_SUMMARY_SUCCESS,
        workspaceId,
        data
    }
}

export function getPexaWorkspaceSummaryFailed(workspaceId: string, error: ErrorViewModel): GetPexaWorkspaceSummaryFailed {
    return {
        type: CONSTANTS.GET_PEXA_WORKSPACE_SUMMARY_FAILED,
        workspaceId,
        error
    }
}

export function getAvailableSettlementTimes(params: RetrieveSettlementAvailabilityParams): GetAvailableSettlementTimes {
    return {
        type: CONSTANTS.GET_AVAILABLE_SETTLEMENT_TIMES_REQUESETD,
        params
    }
}

export function getAvailableSettlementTimesSuccess(jurisdiction: string, settlementDate: string, data: RetrieveSettlementAvailabilityResponseType): GetAvailableSettlementTimesSuccess {
    return {
        type: CONSTANTS.GET_AVAILABLE_SETTLEMENT_TIMES_SUCCESS,
        data,
        jurisdiction,
        settlementDate
    }
}

export function getAvailableSettlementTimesFailed(jurisdiction: string, settlementDate: string, error: ErrorViewModel): GetAvailableSettlementTimesFailed {
    return {
        type: CONSTANTS.GET_AVAILABLE_SETTLEMENT_TIMES_FAILED,
        error,
        jurisdiction,
        settlementDate
    }
}

export function sendDataToPexa(params: CreatePexaWorkspaceCommand): SendDataToPexa {
    return {
        type: CONSTANTS.SEND_DATA_TO_PEXA_REQUESTED,
        params
    }
}

export function sendDataToPexaSuccess(data: CreatePexaWorkspaceResponse): SendDataToPexaSuccess {
    return {
        type: CONSTANTS.SEND_DATA_TO_PEXA_SUCCESS,
        data
    }
}

export function sendDataToPexaFailed(error: ErrorViewModel): ActionFailed {
    return {
        type: CONSTANTS.SEND_DATA_TO_PEXA_FAILED,
        error
    }
}

export function createInvitation(params: CreateWorkspaceInvitationRequestType[]): CreateInvitation {
    return {
        type: CONSTANTS.CREATE_INVITATION_REQUESTED,
        params
    }
}

export function createInvitationSuccess(data: CreateWorkspaceInvitationResponseType[]): CreateInvitationSuccess {
    return {
        type: CONSTANTS.CREATE_INVITATION_SUCCESS,
        data
    }
}

export function createInvitationFailed(error: ErrorViewModel): ActionFailed {
    return {
        type: CONSTANTS.CREATE_INVITATION_FAILED,
        error
    }
}

export function savePexaFormData(pexaFormData: WorkspaceCreationRequestWithMatterInfo): SavePexaFormData {
    return {
        type: CONSTANTS.SAVE_PEXA_FORM_DATA,
        pexaFormData
    }
}

export function clearPexaFormData(): ClearPexaFormData {
    return {
        type: CONSTANTS.CLEAR_PEXA_FORM_DATA
    }
}

export function clearPexaState(): ClearPexaState {
    return {
        type: CONSTANTS.CLEAR_PEXA_STATE
    }
}