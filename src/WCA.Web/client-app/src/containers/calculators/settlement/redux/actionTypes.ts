
import {
    SettlementMatterViewModel,
    ErrorViewModel,
    ActionstepDocument,
    SettlementMatter,
    ActionstepMatterInfo
} from "utils/wcaApiTypes";

import { IBasicAction } from "app.types";

export interface ActionFailed extends IBasicAction {
    error: ErrorViewModel;
}

export interface GeneratePDF extends IBasicAction {
    params: SettlementMatterViewModel;
}

export interface GeneratePDFSuccess extends IBasicAction {
    data: string;
}

export interface SavePDF extends IBasicAction {
    params: SettlementMatterViewModel
}

export interface SavePDFSuccess extends IBasicAction {
    data: ActionstepDocument
}

export interface SaveSettlementMatter extends IBasicAction {
    params: SettlementMatterViewModel
}

export interface SaveSettlementMatterSuccess extends IBasicAction {
    data: SettlementMatter
}

export interface DeleteSettlementMatter extends IBasicAction {
    params: ActionstepMatterInfo
}

export interface DeleteSettlementMatterSuccess extends IBasicAction {
    data: SettlementMatter
}

export interface GetSettlementMatter extends IBasicAction {
    params: ActionstepMatterInfo
}

export interface GetSettlementMatterSuccess extends IBasicAction {
    data: SettlementMatterViewModel
}

export interface ChangeState extends IBasicAction {
    data: string
}

export interface ClearSettlementInfoState extends IBasicAction { }

export type SettlementActionTypes = GeneratePDF
    | GeneratePDFSuccess
    | SavePDF
    | SavePDFSuccess
    | SaveSettlementMatter
    | SaveSettlementMatterSuccess
    | DeleteSettlementMatter
    | DeleteSettlementMatterSuccess
    | GetSettlementMatter
    | GetSettlementMatterSuccess
    | ChangeState
    | ClearSettlementInfoState
    | ActionFailed;