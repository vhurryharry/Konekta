import {
    ErrorViewModel,
    FirstTitlePolicyRequestFromActionstepResponse,
    SendFirstTitlePolicyRequestResponse,
    ActionstepMatterInfo,
    FirstTitleCredential
} from "../../../utils/wcaApiTypes";
import { IBasicAction } from "../../../app.types";

export interface ActionFailed extends IBasicAction {
    error: ErrorViewModel;
}

export interface GetFirstTitlePolicyRequestFromActionstep extends IBasicAction {
    params: ActionstepMatterInfo
}

export interface GetFirstTitlePolicyRequestFromActionstepSuccess extends IBasicAction {
    data: FirstTitlePolicyRequestFromActionstepResponse
}

export interface SendDataToFirstTitle extends IBasicAction {
    params: FirstTitlePolicyRequestFromActionstepResponse
}

export interface SendDataToFirstTitleSuccess extends IBasicAction {
    data: SendFirstTitlePolicyRequestResponse
}

export interface CheckFirstTitleCredentials extends IBasicAction {
    params: FirstTitleCredential
}

export interface CheckFirstTitleCredentialsSuccess extends IBasicAction {
    isValid: boolean
}

export type FirstTitleActionTypes =
    | ActionFailed
    | GetFirstTitlePolicyRequestFromActionstep
    | GetFirstTitlePolicyRequestFromActionstepSuccess
    | SendDataToFirstTitle
    | SendDataToFirstTitleSuccess
    | CheckFirstTitleCredentials
    | CheckFirstTitleCredentialsSuccess;