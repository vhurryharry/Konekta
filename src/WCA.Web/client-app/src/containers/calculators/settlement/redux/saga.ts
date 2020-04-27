
import { call, put, takeLatest } from 'redux-saga/effects'
import { saveAs } from 'file-saver'

import * as ACTIONS from './actions'
import * as CONSTANTS from './constants'

import {
    SettlementMatterViewModel,
    ActionstepDocument,
    SettlementMatter
} from 'utils/wcaApiTypes'
import {
    GeneratePDF,
    SavePDF,
    GetSettlementMatter,
    SaveSettlementMatter,
    DeleteSettlementMatter
} from 'containers/calculators/settlement/redux/actionTypes';
import { rawRequest, postRequest, getRequest, deleteRequest } from 'utils/request'

async function postAndSaveFile(url: string, params: any) {
    let response = await rawRequest(url, params, { responseType: 'blob' });

    // Log somewhat to show that the browser actually exposes the custom HTTP header
    const fileNameHeader = "x-suggested-filename";
    const fileName = response.headers[fileNameHeader];

    // Let the user save the file.
    saveAs(response.data, fileName);
}

export function* generatePDF(action: GeneratePDF) {
    try {
        yield call(postAndSaveFile, "/api/settlement-calculator/generate-pdf", action.params);

        yield put(ACTIONS.generatePDFSuccess('success'));
    } catch (error) {
        yield put(ACTIONS.generatePDFFailed(error));
    }
}

export function* savePDF(action: SavePDF) {
    try {
        const response = yield call(postRequest, "/api/settlement-calculator/save-pdf", action.params);

        const data: ActionstepDocument = response as ActionstepDocument;

        yield put(ACTIONS.savePDFSuccess(data));
    } catch (error) {
        yield put(ACTIONS.savePDFFailed(error));
    }
}

export function* getSettlementMatter(action: GetSettlementMatter) {
    try {
        const response = yield call(getRequest, "/api/settlement-calculator/settlement-matter/" + action.params.orgKey + "/" + action.params.matterId);

        const data: SettlementMatterViewModel = response as SettlementMatterViewModel;

        yield put(ACTIONS.getSettlementMatterSuccess(data));

    } catch (error) {
        yield put(ACTIONS.getSettlementMatterFailed(error));
    }
}

export function* saveSettlementMatter(action: SaveSettlementMatter) {
    try {
        const response = yield call(postRequest, "/api/settlement-calculator/settlement-matter/", action.params);
        const data: SettlementMatter = response as SettlementMatter;

        yield put(ACTIONS.saveSettlementMatterSuccess(data));
    } catch (error) {
        yield put(ACTIONS.saveSettlementMatterFailed(error));
    }
}

export function* deleteSettlementMatter(action: DeleteSettlementMatter) {
    try {
        const { data } = yield call(deleteRequest, "/api/settlement-calculator/settlement-matter/" + action.params.orgKey + "/" + action.params.matterId);

        yield put(ACTIONS.deleteSettlementMatterSuccess(data));
    } catch (error) {
        yield put(ACTIONS.deleteSettlementMatterFailed(error));
    }
}

export default function* settlementInfoSaga() {
    yield takeLatest(CONSTANTS.GENERATE_PDF_REQUESTED, generatePDF);
    yield takeLatest(CONSTANTS.SAVE_PDF_REQUESTED, savePDF);
    yield takeLatest(CONSTANTS.GET_SETTLEMENT_MATTER_REQUESTED, getSettlementMatter);
    yield takeLatest(CONSTANTS.SAVE_SETTLEMENT_MATTER_REQUESTED, saveSettlementMatter);
    yield takeLatest(CONSTANTS.DELETE_SETTLEMENT_MATTER_REQUESTED, deleteSettlementMatter);

}