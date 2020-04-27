import {
    ErrorViewModel,
    ActionstepMatterInfo,
    RequestPropertyInformationFromActionstepResponse
} from "utils/wcaApiTypes";
import { IBasicAction } from "app.types";

export interface ActionFailed extends IBasicAction {
    error: ErrorViewModel;
}

export interface GetPropertyInformation extends IBasicAction {
    matterInfo: ActionstepMatterInfo,
    entryPoint: string,
    embed: boolean,
}

export interface GetPropertyInformationSuccess extends IBasicAction {
    data: RequestPropertyInformationFromActionstepResponse
}

export type GlobalXActionTypes = GetPropertyInformation
    | GetPropertyInformationSuccess
    | ActionFailed;