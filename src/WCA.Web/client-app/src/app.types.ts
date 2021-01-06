
import { RouterState } from "connected-react-router";

import { PexaState } from "containers/pexa/redux/reducer";
import { SettlementInfoState } from "containers/calculators/settlement/redux/reducer";
import { CommonState } from "redux/common/reducer";
import { FirstTitleState } from "containers/firsttitle/redux/reducer";
import { GlobalXState } from "containers/globalx/redux/reducer";

import { PexaActionTypes } from "containers/pexa/redux/actionTypes";
import { SettlementActionTypes } from "containers/calculators/settlement/redux/actionTypes";
import { CommonActionTypes } from "redux/common/actionTypes";
import { FirstTitleActionTypes } from "containers/firsttitle/redux/actionTypes";
import { GlobalXActionTypes } from "containers/globalx/redux/actionTypes";

import { ErrorViewModel } from "utils/wcaApiTypes";

export type AppState = {
    router: RouterState,
    common: CommonState,
    pexa: PexaState,
    globalx: GlobalXState,
    settlementInfo: SettlementInfoState,
    firstTitle: FirstTitleState
};

export type AppActionTypes = PexaActionTypes |
    GlobalXActionTypes |
    SettlementActionTypes |
    CommonActionTypes |
    FirstTitleActionTypes;

export interface IBasicAction {
    type: string
}

export interface IJWTInfo {
    action_id: number,
    action_type_id: number,
    action_type_name: string,
    aud: string,
    email: string,
    exp: number,
    iat: number,
    iss: string,
    jti: string,
    name: string,
    nbf: number,
    orgkey: string,
    sub: string,
    timezone: string
}

export class JwtMatterInfo {
    orgKey!: string;
    matterId!: number;
    actionTypeId!: number;
    actionTypeName!: string;
    timezone!: string;
}

export enum ReduxStatus {
    NotRequested,
    Requested,
    Success,
    Failed
}

export class ReduxData<T> {
    data?: T;
    status: ReduxStatus = ReduxStatus.NotRequested;
    error?: ErrorViewModel

    constructor(initialStatus: ReduxStatus = ReduxStatus.NotRequested, initialData: T | undefined = undefined) {
        this.data = initialData;
        this.status = initialStatus;
    }

    markAsSuccess = (data: T) => {
        this.data = data;
        this.status = ReduxStatus.Success;
        this.error = undefined;

        return this;
    }

    markAsFailed = (error: ErrorViewModel) => {
        this.data = undefined;
        this.error = error;
        this.status = ReduxStatus.Failed;

        return this;
    }
}