import { call, put, takeLatest } from 'redux-saga/effects'

import * as CONSTANTS from './constants'
import * as ACTIONS from './actions'
import {
    AccountModel,
    ActionstepMatterInfo
} from 'utils/wcaApiTypes'
import {
    GetAccountInfo,
    GetMatterInfo,
    LogAuthFailure
} from './actionTypes'
import {
    getRequest, rawGetRequest
} from 'utils/request'

export function* getAccountInfo(action: GetAccountInfo) {
    try {
        const jwtParam = action.encodedJwt ? `?jwt=${action.encodedJwt}` : '';
        const response = yield call(rawGetRequest, `/api/account/currentUser${jwtParam}`);
        let data: AccountModel = AccountModel.fromJS(response);

        yield put(ACTIONS.getAccountInfoSuccess(data));

    } catch (error) {
        yield put(ACTIONS.getAccountInfoFailed(error));
    }
}

export function* getMatterInfo(action: GetMatterInfo) {
    try {
        const response = yield call(getRequest, `/api/actionstep/matter/${action.jwtMatterInfo.orgKey}/${action.jwtMatterInfo.matterId}`);
        let data: ActionstepMatterInfo = ActionstepMatterInfo.fromJS(response);

        yield put(ACTIONS.getMatterInfoSuccess(data));

    } catch (error) {
        yield put(ACTIONS.getMatterInfoFailed(error));
    }
}

export function* logAuthFailure(action: LogAuthFailure) {
    try {
        const jwtParam = action.encodedJwt ? `?jwt=${action.encodedJwt}` : '';
        yield call(getRequest, `/api/account/authFailed${jwtParam}`);
    } catch (error) {
    }
}

export default function* commonSaga() {
    yield takeLatest(CONSTANTS.GET_ACCOUNT_INFO_REQUESTED, getAccountInfo);
    yield takeLatest(CONSTANTS.GET_MATTER_INFO_REQUESTED, getMatterInfo);
    yield takeLatest(CONSTANTS.LOG_AUTH_FAILURE, logAuthFailure);
}