import { call, put, takeLatest } from 'redux-saga/effects'

import * as CONSTANTS from './constants'
import * as ACTIONS from './actions'
import {
    FirstTitlePolicyRequestFromActionstepResponse,
    SendFirstTitlePolicyRequestResponse
} from 'utils/wcaApiTypes'
import {
    CheckFirstTitleCredentials,
    GetFirstTitlePolicyRequestFromActionstep,
    SendDataToFirstTitle
} from './actionTypes'
import { getRequest, postRequest } from 'utils/request'

export function* checkFirstTitleCredentials(action: CheckFirstTitleCredentials) {
    try {
        const response = yield call(postRequest, `/api/insurance/first-title-check-and-update-credentials`, action.params);

        let isValid: boolean = response;

        yield put(ACTIONS.checkFirstTitleCredentialsSuccess(isValid));

    } catch (error) {
        yield put(ACTIONS.checkFirstTitleCredentialsFailed(error));
    }
}

export function* getFirstTitlePolicyRequestFromActionstep(action: GetFirstTitlePolicyRequestFromActionstep) {
    try {
        const response = yield call(getRequest, `/api/insurance/first-title-request-from-matter?actionsteporg=${action.params.orgKey}&matterid=${action.params.matterId}`);

        let data: FirstTitlePolicyRequestFromActionstepResponse = FirstTitlePolicyRequestFromActionstepResponse.fromJS(response);

        yield put(ACTIONS.getFirstTitlePolicyRequestFromActionstepSuccess(data));

    } catch (error) {
        yield put(ACTIONS.getFirstTitlePolicyRequestFromActionstepFailed(error));
    }
}

export function* sendDataToFirstTitle(action: SendDataToFirstTitle) {
    try {
        const response = yield call(postRequest, `/api/insurance/first-title-request-from-matter`, action.params);

        let data: SendFirstTitlePolicyRequestResponse = SendFirstTitlePolicyRequestResponse.fromJS(response);

        yield put(ACTIONS.sendDataToFirstTitleSuccess(data));

    } catch (error) {
        yield put(ACTIONS.sendDataToFirstTitleFailed(error));
    }
}

export default function* pexaSaga() {
    yield takeLatest(CONSTANTS.CHECK_FIRST_TITLE_CREDENTIALS_REQUESTED, checkFirstTitleCredentials);
    yield takeLatest(CONSTANTS.GET_FIRST_TITLE_POLICY_REQUEST_FROM_ACTIONSTEP_REQUESTED, getFirstTitlePolicyRequestFromActionstep);
    yield takeLatest(CONSTANTS.SEND_DATA_TO_FIRST_TITLE_REQUESTED, sendDataToFirstTitle);
}