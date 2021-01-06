import * as CONSTANTS from '../constants'

const initialState = {
    gotResponse: false,
    actionstepPDF: "",
    state: 'VIC',
    success: false,
    settlementMatter: null,
    isOrgConnected: true,
    error: null,
    requestType: ""
}

function settlementInfoReducer(state = initialState, action) {

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
                actionstepPDF: action.data,
                gotResponse: true,
                success: true
            };

        case CONSTANTS.GET_SETTLEMENT_MATTER_SUCCESS:
            return {
                ...state,
                gotResponse: true,
                success: true,
                settlementMatter: action.data,
                isOrgConnected: true
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
                settlementMatter: null,
                isOrgConnected: true,
                actionstepPDF: "",
                error: null,
                requestType: ""
            };

        case CONSTANTS.CHANGE_STATE:
            return {
                ...state,
                state: action.data
            };

        case CONSTANTS.GET_SETTLEMENT_MATTER_INVALID_MATTER:
            return {
                ...state,
                gotResponse: true,
                success: false,
                error: action.error,
                isOrgConnected: false
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
                error: action.error
            };

        default:
            return state;
    }
}

export default settlementInfoReducer;