
import * as CONSTANTS from './constants'

import {
    GetAccountInfo,
    ActionFailed,
    GetAccountInfoSuccess,
    ClearCommonState,
    SetJwtMatterInfo,
    SetUIDefinitions,
    ActionstepOrgNotConnected,
    ActionstepOrgConnected,
    PexaNotConnected,
    PexaConnected,
    GetMatterInfo,
    GetMatterInfoSuccess,
    LogAuthFailure,
    FirstTitleNotConnected,
    FirstTitleConnected
} from 'redux/common/actionTypes'

import {
    ErrorViewModel,
    AccountModel,
    UISettings,
    ActionstepMatterInfo
} from 'utils/wcaApiTypes'

import { JwtMatterInfo } from 'app.types'

export function getAccountInfo(encodedJwt: string | null): GetAccountInfo {
    return {
        type: CONSTANTS.GET_ACCOUNT_INFO_REQUESTED,
        encodedJwt
    }
}

export function getAccountInfoSuccess(data: AccountModel): GetAccountInfoSuccess {
    return {
        type: CONSTANTS.GET_ACCOUNT_INFO_SUCCESS,
        data
    }
}

export function getAccountInfoFailed(error: ErrorViewModel): ActionFailed {
    return {
        type: CONSTANTS.GET_ACCOUNT_INFO_FAILED,
        error
    }
}

export function setUIDefinitions(uiDefinitions: UISettings): SetUIDefinitions {
    return {
        type: CONSTANTS.SET_UI_DEFINITIONS,
        uiDefinitions
    }
}

export function getMatterInfo(jwtMatterInfo: JwtMatterInfo): GetMatterInfo {
    return {
        type: CONSTANTS.GET_MATTER_INFO_REQUESTED,
        jwtMatterInfo
    }
}

export function getMatterInfoSuccess(data: ActionstepMatterInfo): GetMatterInfoSuccess {
    return {
        type: CONSTANTS.GET_MATTER_INFO_SUCCESS,
        data
    }
}

export function getMatterInfoFailed(error: ErrorViewModel): ActionFailed {
    return {
        type: CONSTANTS.GET_MATTER_INFO_FAILED,
        error
    }
}

export function logAuthFailure(encodedJwt: string | null): LogAuthFailure {
    return {
        type: CONSTANTS.LOG_AUTH_FAILURE,
        encodedJwt
    }
}

export function setJwtMatterInfo(data: JwtMatterInfo): SetJwtMatterInfo {
    return {
        type: CONSTANTS.SET_JWT_MATTER_INFO,
        data
    }
}

export function actionstepOrgNotConnected(isFirstTime: boolean = true): ActionstepOrgNotConnected {
    return {
        type: CONSTANTS.ACTIONSTEP_ORG_NOT_CONNECTED,
        isFirstTime
    }
}

export function actionstepOrgConnected(): ActionstepOrgConnected {
    return {
        type: CONSTANTS.ACTIONSTEP_ORG_CONNECTED
    }
}

export function pexaNotConnected(): PexaNotConnected {
    return {
        type: CONSTANTS.PEXA_NOT_CONNECTED
    }
}

export function pexaConnected(): PexaConnected {
    return {
        type: CONSTANTS.PEXA_CONNECTED
    }
}

export function firstTitleNotConnected(isFirstTime: boolean = true): FirstTitleNotConnected {
    return {
        type: CONSTANTS.FIRST_TITLE_NOT_CONNECTED,
        isFirstTime
    }
}

export function firstTitleConnected(): FirstTitleConnected {
    return {
        type: CONSTANTS.FIRST_TITLE_CONNECTED
    }
}

export function clearCommonState(): ClearCommonState {
    return {
        type: CONSTANTS.CLEAR_COMMON_STATE
    }
}