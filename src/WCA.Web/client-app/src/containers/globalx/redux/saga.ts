import { call, put, takeLatest } from 'redux-saga/effects'

import * as CONSTANTS from './constants'
import * as ACTIONS from './actions'
import { getRequest } from 'utils/request'
import { GetPropertyInformation } from 'containers/globalx/redux/actionTypes'
import { RequestPropertyInformationFromActionstepResponse } from 'utils/wcaApiTypes'

export function* getPropertyInformation(action: GetPropertyInformation) {
    try {
        const response = yield call(getRequest, `/api/globalx/conveyancing-data-from-actionstep?actionsteporg=${action.matterInfo.orgKey!}&matterid=${action.matterInfo.matterId}&entryPoint=${action.entryPoint}&embed=${action.embed}`);

        let data: RequestPropertyInformationFromActionstepResponse = response;

        yield put(ACTIONS.getPropertyInformationSuccess(data));

    } catch (error) {
        yield put(ACTIONS.getPropertyInformationFailed(error));
    }
}

export default function* globalXSaga() {
    yield takeLatest(CONSTANTS.GET_PROPERTY_INFORMATION_REQUESTED, getPropertyInformation);
}