import * as CONSTANTS from './constants'

export function generatePDF(params) {
    return {
        type: CONSTANTS.GENERATE_PDF_REQUESTED,
        params
    }
}

export function generatePDFSuccess(data) {
    return {
        type: CONSTANTS.GENERATE_PDF_SUCCESS,
        data
    }
}

export function generatePDFFailed(error) {
    return {
        type: CONSTANTS.GENERATE_PDF_FAILED,
        error
    }
}

export function savePDF(params) {
    return {
        type: CONSTANTS.SAVE_PDF_REQUESTED,
        params
    }
}

export function savePDFSuccess(data) {
    return {
        type: CONSTANTS.SAVE_PDF_SUCCESS,
        data
    }
}

export function savePDFFailed(error) {
    return {
        type: CONSTANTS.SAVE_PDF_FAILED,
        error
    }
}

export function saveSettlementMatter(params) {
    return {
        type: CONSTANTS.SAVE_SETTLEMENT_MATTER_REQUESTED,
        params
    }
}

export function saveSettlementMatterSuccess(data) {
    return {
        type: CONSTANTS.SAVE_SETTLEMENT_MATTER_SUCCESS,
        data
    }
}

export function saveSettlementMatterFailed(error) {
    return {
        type: CONSTANTS.SAVE_SETTLEMENT_MATTER_FAILED,
        error
    }
}

export function deleteSettlementMatter(params) {
    return {
        type: CONSTANTS.DELETE_SETTLEMENT_MATTER_REQUESTED,
        params
    }
}

export function deleteSettlementMatterSuccess(data) {
    return {
        type: CONSTANTS.DELETE_SETTLEMENT_MATTER_SUCCESS,
        data
    }
}

export function deleteSettlementMatterFailed(error) {
    return {
        type: CONSTANTS.DELETE_SETTLEMENT_MATTER_FAILED,
        error
    }
}

export function getSettlementMatter(params) {
    return {
        type: CONSTANTS.GET_SETTLEMENT_MATTER_REQUESTED,
        params
    }
}

export function getSettlementMatterSuccess(data) {
    return {
        type: CONSTANTS.GET_SETTLEMENT_MATTER_SUCCESS,
        data
    }
}

export function getSettlementMatterFailed(error) {
    return {
        type: CONSTANTS.GET_SETTLEMENT_MATTER_FAILED,
        error
    }
}

export function getSettlementMatterInvalidMatter(error) {
    return {
        type: CONSTANTS.GET_SETTLEMENT_MATTER_INVALID_MATTER,
        error
    }
}

export function changeState(data) {
    return {
        type: CONSTANTS.CHANGE_STATE,
        data
    }
}

export function clearSettlementState() {
    return {
        type: CONSTANTS.CLEAR_SETTLEMENT_INFO_STATE
    }
}