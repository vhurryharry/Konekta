import * as CONSTANTS from './constants'
import { SettlementMatterViewModel, ErrorViewModel, ActionstepDocument } from 'utils/wcaApiTypes';
import {
    SettlementActionTypes,
    SavePDFSuccess,
    GetSettlementMatterSuccess,
    ChangeState,
    ActionFailed
} from 'containers/calculators/settlement/redux/actionTypes';

export interface SettlementInfoState {
    gotResponse: boolean,
    actionstepPDF: ActionstepDocument | undefined,
    state: string,
    success: boolean,
    settlementMatter: SettlementMatterViewModel | undefined,
    error: ErrorViewModel | undefined,
    requestType: string
}

const initialState: SettlementInfoState = {
    gotResponse: false,
    actionstepPDF: new ActionstepDocument(),
    state: 'VIC',
    success: false,
    settlementMatter: undefined,
    error: undefined,
    requestType: ""
}

function settlementInfoReducer(state: SettlementInfoState = initialState, action: SettlementActionTypes): SettlementInfoState {

    switch (action.type) {
        case CONSTANTS.GENERATE_PDF_REQUESTED:
        case CONSTANTS.SAVE_PDF_REQUESTED:
        case CONSTANTS.GET_SETTLEMENT_MATTER_REQUESTED:
        case CONSTANTS.SAVE_SETTLEMENT_MATTER_REQUESTED:
        case CONSTANTS.DELETE_SETTLEMENT_MATTER_REQUESTED:
            return {
                ...state,
                requestType: action.type
            };

        case CONSTANTS.GENERATE_PDF_SUCCESS:
            return {
                ...state,
                gotResponse: true,
                success: true
            };

        case CONSTANTS.SAVE_PDF_SUCCESS:
            return {
                ...state,
                actionstepPDF: (action as SavePDFSuccess).data,
                gotResponse: true,
                success: true
            };

        case CONSTANTS.GET_SETTLEMENT_MATTER_SUCCESS:
            return {
                ...state,
                gotResponse: true,
                success: true,
                settlementMatter: (action as GetSettlementMatterSuccess).data
            };

        case CONSTANTS.SAVE_SETTLEMENT_MATTER_SUCCESS:
            return {
                ...state,
                gotResponse: true,
                success: true
            };

        case CONSTANTS.DELETE_SETTLEMENT_MATTER_SUCCESS:
            return {
                ...state,
                gotResponse: true,
                success: true
            };

        case CONSTANTS.CLEAR_SETTLEMENT_INFO_STATE:
            return {
                ...state,
                gotResponse: false,
                success: false,
                settlementMatter: undefined,
                actionstepPDF: undefined,
                error: undefined,
                requestType: ""
            };

        case CONSTANTS.CHANGE_STATE:
            return {
                ...state,
                state: (action as ChangeState).data
            };

        case CONSTANTS.GENERATE_PDF_FAILED:
        case CONSTANTS.SAVE_PDF_FAILED:
        case CONSTANTS.GET_SETTLEMENT_MATTER_FAILED:
        case CONSTANTS.SAVE_SETTLEMENT_MATTER_FAILED:
        case CONSTANTS.DELETE_SETTLEMENT_MATTER_FAILED:
            return {
                ...state,
                gotResponse: true,
                success: false,
                error: (action as ActionFailed).error
            };

        default:
            return state;
    }
}

export default settlementInfoReducer;