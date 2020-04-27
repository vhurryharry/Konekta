import * as CONSTANTS from './constants'
import {
    ErrorViewModel,
    FirstTitlePolicyRequestFromActionstepResponse,
    SendFirstTitlePolicyRequestResponse,
    ActionstepMatterInfo,
    FirstTitleCredential
} from 'utils/wcaApiTypes'
import {
    CheckFirstTitleCredentials,
    CheckFirstTitleCredentialsSuccess,
    GetFirstTitlePolicyRequestFromActionstep,
    GetFirstTitlePolicyRequestFromActionstepSuccess,
    SendDataToFirstTitle,
    SendDataToFirstTitleSuccess,
    ActionFailed
} from './actionTypes'

export function getFirstTitlePolicyRequestFromActionstep(params: ActionstepMatterInfo): GetFirstTitlePolicyRequestFromActionstep {
    return {
        type: CONSTANTS.GET_FIRST_TITLE_POLICY_REQUEST_FROM_ACTIONSTEP_REQUESTED,
        params,
    }
}

export function getFirstTitlePolicyRequestFromActionstepSuccess(data: FirstTitlePolicyRequestFromActionstepResponse): GetFirstTitlePolicyRequestFromActionstepSuccess {
    return {
        type: CONSTANTS.GET_FIRST_TITLE_POLICY_REQUEST_FROM_ACTIONSTEP_SUCCESS,
        data,
    }
}

export function getFirstTitlePolicyRequestFromActionstepFailed(error: ErrorViewModel): ActionFailed {
    return {
        type: CONSTANTS.GET_FIRST_TITLE_POLICY_REQUEST_FROM_ACTIONSTEP_FAILED,
        error,
    }
}

export function sendDataToFirstTitle(params: FirstTitlePolicyRequestFromActionstepResponse): SendDataToFirstTitle {
    return {
        type: CONSTANTS.SEND_DATA_TO_FIRST_TITLE_REQUESTED,
        params,
    }
}

export function sendDataToFirstTitleSuccess(data: SendFirstTitlePolicyRequestResponse): SendDataToFirstTitleSuccess {
    return {
        type: CONSTANTS.SEND_DATA_TO_FIRST_TITLE_SUCCESS,
        data
    }
}

export function sendDataToFirstTitleFailed(error: ErrorViewModel): ActionFailed {
    return {
        type: CONSTANTS.SEND_DATA_TO_FIRST_TITLE_FAILED,
        error
    }
}

export function checkFirstTitleCredentials(params: FirstTitleCredential): CheckFirstTitleCredentials {
    return {
        type: CONSTANTS.CHECK_FIRST_TITLE_CREDENTIALS_REQUESTED,
        params,
    }
}

export function checkFirstTitleCredentialsSuccess(isValid: boolean): CheckFirstTitleCredentialsSuccess {
    return {
        type: CONSTANTS.CHECK_FIRST_TITLE_CREDENTIALS_SUCCESS,
        isValid
    }
}

export function checkFirstTitleCredentialsFailed(error: ErrorViewModel): ActionFailed {
    return {
        type: CONSTANTS.CHECK_FIRST_TITLE_CREDENTIALS_FAILED,
        error
    }
}