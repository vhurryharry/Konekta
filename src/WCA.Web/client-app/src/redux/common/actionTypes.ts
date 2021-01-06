
import {
    ErrorViewModel,
    AccountModel,
    UISettings,
    ActionstepMatterInfo
} from "utils/wcaApiTypes";
import {
    IBasicAction,
    JwtMatterInfo
} from "app.types";

export interface ActionFailed extends IBasicAction {
    error: ErrorViewModel;
}

export interface GetAccountInfo extends IBasicAction {
    encodedJwt: string | null;
}

export interface GetAccountInfoSuccess extends IBasicAction {
    data: AccountModel
}

export interface SetUIDefinitions extends IBasicAction {
    uiDefinitions: UISettings
}

export interface GetMatterInfo extends IBasicAction {
    jwtMatterInfo: JwtMatterInfo
}

export interface GetMatterInfoSuccess extends IBasicAction {
    data: ActionstepMatterInfo
}

export interface LogAuthFailure extends IBasicAction {
    encodedJwt: string | null;
}

export interface SetJwtMatterInfo extends IBasicAction {
    data: JwtMatterInfo
}

export interface ActionstepOrgNotConnected extends IBasicAction {
    isFirstTime: boolean
}

export interface ActionstepOrgConnected extends IBasicAction { }

export interface PexaNotConnected extends IBasicAction { }

export interface PexaConnected extends IBasicAction { }

export interface FirstTitleNotConnected extends IBasicAction {
    isFirstTime: boolean
}

export interface FirstTitleConnected extends IBasicAction { }

export interface ClearCommonState extends IBasicAction { }

export type CommonActionTypes = GetAccountInfo
    | GetAccountInfoSuccess
    | SetUIDefinitions
    | GetMatterInfo
    | GetMatterInfoSuccess
    | SetJwtMatterInfo
    | ActionstepOrgNotConnected
    | ActionstepOrgConnected
    | PexaNotConnected
    | PexaConnected
    | ClearCommonState
    | ActionFailed;