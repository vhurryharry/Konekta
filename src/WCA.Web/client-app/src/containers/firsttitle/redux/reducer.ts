import * as CONSTANTS from './constants'
import {
    FirstTitlePolicyRequestFromActionstepResponse,
    SendFirstTitlePolicyRequestResponse
} from '../../../utils/wcaApiTypes';

import {
    FirstTitleActionTypes,
    GetFirstTitlePolicyRequestFromActionstepSuccess,
    ActionFailed,
    CheckFirstTitleCredentialsSuccess,
    SendDataToFirstTitleSuccess
} from './actionTypes';
import { ReduxData, ReduxStatus } from 'app.types';

export interface FirstTitleState {
    firstTitlePolicyRequestFromActionstepResponse: ReduxData<FirstTitlePolicyRequestFromActionstepResponse> | undefined
    isValidCredentials: ReduxData<boolean> | undefined,
    sendFirstTitlePolicyRequestResponse: ReduxData<SendFirstTitlePolicyRequestResponse> | undefined
}

const initialState: FirstTitleState = {
    firstTitlePolicyRequestFromActionstepResponse: undefined,
    isValidCredentials: undefined,
    sendFirstTitlePolicyRequestResponse: undefined
}

function firstTitleReducer(state: FirstTitleState = initialState, action: FirstTitleActionTypes): FirstTitleState {

    switch (action.type) {
        case CONSTANTS.CHECK_FIRST_TITLE_CREDENTIALS_REQUESTED:
            return {
                ...state,
                isValidCredentials: new ReduxData<boolean>(ReduxStatus.Requested)
            };

        case CONSTANTS.CHECK_FIRST_TITLE_CREDENTIALS_SUCCESS:
            return {
                ...state,
                isValidCredentials: new ReduxData<boolean>().markAsSuccess((action as CheckFirstTitleCredentialsSuccess).isValid)
            };

        case CONSTANTS.CHECK_FIRST_TITLE_CREDENTIALS_FAILED:
            return {
                ...state,
                isValidCredentials: new ReduxData<boolean>().markAsFailed((action as ActionFailed).error)
            };

        case CONSTANTS.GET_FIRST_TITLE_POLICY_REQUEST_FROM_ACTIONSTEP_REQUESTED:
            return {
                ...state,
                firstTitlePolicyRequestFromActionstepResponse: new ReduxData<FirstTitlePolicyRequestFromActionstepResponse>(ReduxStatus.Requested)
            };

        case CONSTANTS.GET_FIRST_TITLE_POLICY_REQUEST_FROM_ACTIONSTEP_SUCCESS:
            return {
                ...state,
                firstTitlePolicyRequestFromActionstepResponse: new ReduxData<FirstTitlePolicyRequestFromActionstepResponse>().markAsSuccess((action as GetFirstTitlePolicyRequestFromActionstepSuccess).data)
            };

        case CONSTANTS.GET_FIRST_TITLE_POLICY_REQUEST_FROM_ACTIONSTEP_FAILED:
            return {
                ...state,
                firstTitlePolicyRequestFromActionstepResponse: new ReduxData<FirstTitlePolicyRequestFromActionstepResponse>().markAsFailed((action as ActionFailed).error)
            };

        case CONSTANTS.SEND_DATA_TO_FIRST_TITLE_REQUESTED:
            return {
                ...state,
                sendFirstTitlePolicyRequestResponse: new ReduxData<SendFirstTitlePolicyRequestResponse>(ReduxStatus.Requested)
            };

        case CONSTANTS.SEND_DATA_TO_FIRST_TITLE_SUCCESS:
            return {
                ...state,
                sendFirstTitlePolicyRequestResponse: new ReduxData<SendFirstTitlePolicyRequestResponse>().markAsSuccess((action as SendDataToFirstTitleSuccess).data)
            };

        case CONSTANTS.SEND_DATA_TO_FIRST_TITLE_FAILED:
            return {
                ...state,
                sendFirstTitlePolicyRequestResponse: new ReduxData<SendFirstTitlePolicyRequestResponse>().markAsFailed((action as ActionFailed).error)
            };

        default:
            return state;
    }
}

export default firstTitleReducer;