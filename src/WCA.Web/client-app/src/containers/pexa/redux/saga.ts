import { call, put, takeLatest, takeEvery } from 'redux-saga/effects'

import * as CONSTANTS from './constants'
import * as ACTIONS from './actions'
import {
    PEXAWorkspaceCreationRequestWithActionstepResponse,
    CreatePexaWorkspaceResponse,
    LandTitleReferenceVerificationResponseType,
    WorkspaceSummaryResponseType,
    CreateWorkspaceInvitationResponseType,
    RetrieveSettlementAvailabilityResponseType
} from 'utils/wcaApiTypes'
import {
    GetDataFromActionstep,
    SendDataToPexa,
    ValidateLandTitle,
    GetPexaWorkspaceSummary,
    CreateInvitation,
    GetAvailableSettlementTimes
} from 'containers/pexa/redux/actionTypes'
import { getRequest, postRequest } from 'utils/request'

export function* getDataFromActionstep(action: GetDataFromActionstep) {
    try {
        const response = yield call(getRequest, `/api/conveyancing/pexa-workspace-creation-request-from-matter?actionsteporg=${action.matterInfo.orgKey!}&matterid=${action.matterInfo.matterId}`);

        let data: PEXAWorkspaceCreationRequestWithActionstepResponse = PEXAWorkspaceCreationRequestWithActionstepResponse.fromJS(response);

        yield put(ACTIONS.getDataFromActionstepSuccess(data));

    } catch (error) {
        yield put(ACTIONS.getDataFromActionstepFailed(error));
    }
}

export function* getAvailableSettlementTimes(action: GetAvailableSettlementTimes) {
    const { params } = action;
    try {
        const response = yield call(postRequest, `/api/conveyancing/get-available-settlement-times`, params);

        let data: RetrieveSettlementAvailabilityResponseType = RetrieveSettlementAvailabilityResponseType.fromJS(response);

        yield put(ACTIONS.getAvailableSettlementTimesSuccess(params.jurisdiction!, params.settlementDate!, data));

    } catch (error) {
        yield put(ACTIONS.getAvailableSettlementTimesFailed(params.jurisdiction!, params.settlementDate!, error));
    }
}

export function* validateLandTitle(action: ValidateLandTitle) {
    try {
        const requestData = action.params.toJSON();
        const response = yield call(postRequest, `/api/conveyancing/validate-land-title-reference`, requestData);

        let data: LandTitleReferenceVerificationResponseType = LandTitleReferenceVerificationResponseType.fromJS(response);

        yield put(ACTIONS.validateLandTitleSuccess(data));

    } catch (error) {
        yield put(ACTIONS.validateLandTitleFailed(error));
    }
}

export function* getPexaWorkspaceSummary(action: GetPexaWorkspaceSummary) {
    const { params } = action;
    try {
        const response = yield call(postRequest, `/api/conveyancing/get-pexa-workspace-summary`, params);

        let data: WorkspaceSummaryResponseType = WorkspaceSummaryResponseType.fromJS(response);

        yield put(ACTIONS.getPexaWorkspaceSummarySuccess(params.workspaceId!, data));
    } catch (error) {
        yield put(ACTIONS.getPexaWorkspaceSummaryFailed(params.workspaceId!, error));
    }
}

export function* sendDataToPexa(action: SendDataToPexa) {
    try {
        const requestData = action.params.toJSON();
        const response = yield call(postRequest, `/api/conveyancing/pexa-workspace-creation-request-from-matter`, requestData);

        let data: CreatePexaWorkspaceResponse = response;

        yield put(ACTIONS.sendDataToPexaSuccess(data));

    } catch (error) {
        yield put(ACTIONS.sendDataToPexaFailed(error));
    }
}

export function* inviteSubscribers(action: CreateInvitation) {
    try {
        const { params } = action;
        const response = yield call(postRequest, `/api/conveyancing/invite-subscribers`, params);

        let data: CreateWorkspaceInvitationResponseType[] = response;

        yield put(ACTIONS.createInvitationSuccess(data));
    } catch (error) {
        yield put(ACTIONS.createInvitationFailed(error));
    }
}

export default function* pexaSaga() {
    yield takeLatest(CONSTANTS.GET_DATA_FROM_ACTIONSTEP_REQUESTED, getDataFromActionstep);
    yield takeLatest(CONSTANTS.GET_AVAILABLE_SETTLEMENT_TIMES_REQUESETD, getAvailableSettlementTimes);
    yield takeLatest(CONSTANTS.VALIDATE_LAND_TITLE_REQUESTED, validateLandTitle);
    yield takeLatest(CONSTANTS.SEND_DATA_TO_PEXA_REQUESTED, sendDataToPexa);
    yield takeLatest(CONSTANTS.CREATE_INVITATION_REQUESTED, inviteSubscribers);
    yield takeEvery(CONSTANTS.GET_PEXA_WORKSPACE_SUMMARY_REQUESTED, getPexaWorkspaceSummary)
}