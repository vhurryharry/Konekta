import { call, put, takeLatest, select } from 'redux-saga/effects'
import * as CONSTANTS from './constants'
import axios from 'axios'
import * as ACTIONS from './actions'
import { saveAs } from 'file-saver';
import { SettlementMatterViewModel, ActionstepDocument } from '../../services/wca-api-types'

async function postAndSaveFile(url, params) {
    var response = await axios.post(url, params, { responseType: 'blob' });

    // Log somewhat to show that the browser actually exposes the custom HTTP header
    const fileNameHeader = "x-suggested-filename";
    const fileName = response.headers[fileNameHeader];

    // Let the user save the file.
    saveAs(response.data, fileName);
}

export function* generatePDF(action) {
    try {
        yield call(postAndSaveFile, "/api/settlement-calculator/generate-pdf", action.params);

        yield put(ACTIONS.generatePDFSuccess('success'));
    } catch (error) {
        const response = error.response;
        yield put(ACTIONS.generatePDFFailed(response.data));
    }
}

export function* savePDF(action) {
    try {
        const response = yield call(axios.post, "/api/settlement-calculator/save-pdf", action.params);

        const data: ActionstepDocument = response.data;

        yield put(ACTIONS.savePDFSuccess(data));
    } catch (error) {
        const response = error.response;
        yield put(ACTIONS.savePDFFailed(response.data));
    }
}

export function* getSettlementMatter(action) {
    try {
        const response = yield call(axios.get, "/api/settlement-calculator/settlement-matter/" + action.params.actionstepOrg + "/" + action.params.matterId, null);

        const data: SettlementMatterViewModel = response.data;

        yield put(ACTIONS.getSettlementMatterSuccess(data));

    } catch (error) {
        const response = error.response;

        if (response.status == 401) {
            const authenticateHeaderValue = response.headers["www-authenticate"];
            if (authenticateHeaderValue && authenticateHeaderValue.startsWith("ActionstepConnection OrgKey=")) {
                yield put(ACTIONS.getSettlementMatterInvalidMatter(response.data));

                return;
            }
        }

        yield put(ACTIONS.getSettlementMatterFailed(response.data));
    }
}

export function* saveSettlementMatter(action) {
    try {
        const { data } = yield call(axios.post, "/api/settlement-calculator/settlement-matter/", action.params);

        yield put(ACTIONS.saveSettlementMatterSuccess(data));
    } catch (error) {
        const response = error.response;
        yield put(ACTIONS.saveSettlementMatterFailed(response.data));
    }
}

export function* deleteSettlementMatter(action) {
    try {
        const { data } = yield call(axios.delete, "/api/settlement-calculator/settlement-matter/" + action.params.actionstepOrg + "/" + action.params.matterId);

        yield put(ACTIONS.deleteSettlementMatterSuccess(data));
    } catch (error) {
        const response = error.response;
        yield put(ACTIONS.deleteSettlementMatterFailed(response.data));
    }
}

export default function* saga() {
    yield takeLatest(CONSTANTS.GENERATE_PDF_REQUESTED, generatePDF);
    yield takeLatest(CONSTANTS.SAVE_PDF_REQUESTED, savePDF);
    yield takeLatest(CONSTANTS.GET_SETTLEMENT_MATTER_REQUESTED, getSettlementMatter);
    yield takeLatest(CONSTANTS.SAVE_SETTLEMENT_MATTER_REQUESTED, saveSettlementMatter);
    yield takeLatest(CONSTANTS.DELETE_SETTLEMENT_MATTER_REQUESTED, deleteSettlementMatter);

}