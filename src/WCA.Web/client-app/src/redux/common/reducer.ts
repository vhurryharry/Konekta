import * as CONSTANTS from './constants'
import {
    ErrorViewModel,
    AccountModel,
    UISettings,
    ActionstepMatterInfo
} from 'utils/wcaApiTypes';
import { JwtMatterInfo } from 'app.types';
import {
    CommonActionTypes,
    GetAccountInfoSuccess,
    SetJwtMatterInfo,
    ActionFailed,
    ActionstepOrgNotConnected,
    GetMatterInfoSuccess,
    SetUIDefinitions,
    FirstTitleNotConnected
} from 'redux/common/actionTypes';

export interface CommonState {
    accountInfo: AccountModel | undefined,
    gotResponse: boolean,
    success: boolean,
    error: ErrorViewModel | undefined,
    requestType: string,
    jwtMatterInfo: JwtMatterInfo | undefined,
    matterInfo: ActionstepMatterInfo | undefined,
    uiDefinitions: UISettings | undefined,
    orgConnected: boolean | null,
    isFirstTime: boolean,
    pexaConnected: boolean | null,
    firstTitleConnected: boolean | null,
    firstTitleMissingCredentials: boolean
}

const initialState: CommonState = {
    accountInfo: undefined,
    gotResponse: false,
    success: false,
    error: undefined,
    requestType: "",
    jwtMatterInfo: undefined,
    matterInfo: undefined,
    uiDefinitions: undefined,
    orgConnected: null,
    isFirstTime: true,      // isFirstTime for Actionstep
    pexaConnected: null,
    firstTitleConnected: null,
    firstTitleMissingCredentials: true      // isFirstTime for First Title
}

function commonReducer(state: CommonState = initialState, action: CommonActionTypes): CommonState {
    state = {
        ...state,
        requestType: action.type
    };

    switch (action.type) {
        case CONSTANTS.GET_ACCOUNT_INFO_SUCCESS:
            return {
                ...state,
                gotResponse: true,
                success: true,
                accountInfo: (action as GetAccountInfoSuccess).data
            };

        case CONSTANTS.GET_MATTER_INFO_SUCCESS:
            return {
                ...state,
                gotResponse: true,
                success: true,
                matterInfo: (action as GetMatterInfoSuccess).data
            };

        case CONSTANTS.GET_MATTER_INFO_FAILED:
        case CONSTANTS.GET_ACCOUNT_INFO_FAILED:
            return {
                ...state,
                gotResponse: true,
                success: false,
                error: (action as ActionFailed).error
            };

        case CONSTANTS.SET_UI_DEFINITIONS:
            return {
                ...state,
                uiDefinitions: (action as SetUIDefinitions).uiDefinitions
            };

        case CONSTANTS.SET_JWT_MATTER_INFO:
            return {
                ...state,
                jwtMatterInfo: { ...(action as SetJwtMatterInfo).data }
            };

        case CONSTANTS.CLEAR_COMMON_STATE:
            return {
                ...state,
                gotResponse: false,
                success: false,
                requestType: "",
                error: undefined
            };

        case CONSTANTS.ACTIONSTEP_ORG_CONNECTED:
            return {
                ...state,
                orgConnected: true,
                isFirstTime: false
            };

        case CONSTANTS.ACTIONSTEP_ORG_NOT_CONNECTED:
            return {
                ...state,
                orgConnected: false,
                isFirstTime: (action as ActionstepOrgNotConnected).isFirstTime
            };

        case CONSTANTS.PEXA_CONNECTED:
            return {
                ...state,
                pexaConnected: true
            };

        case CONSTANTS.PEXA_NOT_CONNECTED:
            return {
                ...state,
                pexaConnected: false
            };

        case CONSTANTS.FIRST_TITLE_CONNECTED:
            return {
                ...state,
                firstTitleConnected: true,
                firstTitleMissingCredentials: false
            };

        case CONSTANTS.FIRST_TITLE_NOT_CONNECTED:
            return {
                ...state,
                firstTitleConnected: false,
                firstTitleMissingCredentials: (action as FirstTitleNotConnected).isFirstTime
            };

        default:
            return state;
    }
}

export default commonReducer;