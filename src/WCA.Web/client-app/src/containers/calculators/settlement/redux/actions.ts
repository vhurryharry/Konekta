
import * as CONSTANTS from './constants'
import {
    GeneratePDF,
    GeneratePDFSuccess,
    ActionFailed,
    SavePDF,
    SavePDFSuccess,
    SaveSettlementMatter,
    SaveSettlementMatterSuccess,
    DeleteSettlementMatter,
    DeleteSettlementMatterSuccess,
    GetSettlementMatter,
    GetSettlementMatterSuccess,
    ChangeState,
    ClearSettlementInfoState
} from 'containers/calculators/settlement/redux/actionTypes'

import {
    SettlementMatterViewModel,
    ErrorViewModel,
    ActionstepDocument,
    SettlementMatter,
    ActionstepMatterInfo
} from 'utils/wcaApiTypes'

export function generatePDF(params: SettlementMatterViewModel): GeneratePDF {
    return {
        type: CONSTANTS.GENERATE_PDF_REQUESTED,
        params
    }
}

export function generatePDFSuccess(data: string): GeneratePDFSuccess {
    return {
        type: CONSTANTS.GENERATE_PDF_SUCCESS,
        data
    }
}

export function generatePDFFailed(error: ErrorViewModel): ActionFailed {
    return {
        type: CONSTANTS.GENERATE_PDF_FAILED,
        error
    }
}

export function savePDF(params: SettlementMatterViewModel): SavePDF {
    return {
        type: CONSTANTS.SAVE_PDF_REQUESTED,
        params
    }
}

export function savePDFSuccess(data: ActionstepDocument): SavePDFSuccess {
    return {
        type: CONSTANTS.SAVE_PDF_SUCCESS,
        data
    }
}

export function savePDFFailed(error: ErrorViewModel): ActionFailed {
    return {
        type: CONSTANTS.SAVE_PDF_FAILED,
        error
    }
}

export function saveSettlementMatter(params: SettlementMatterViewModel): SaveSettlementMatter {
    return {
        type: CONSTANTS.SAVE_SETTLEMENT_MATTER_REQUESTED,
        params
    }
}

export function saveSettlementMatterSuccess(data: SettlementMatter): SaveSettlementMatterSuccess {
    return {
        type: CONSTANTS.SAVE_SETTLEMENT_MATTER_SUCCESS,
        data
    }
}

export function saveSettlementMatterFailed(error: ErrorViewModel): ActionFailed {
    return {
        type: CONSTANTS.SAVE_SETTLEMENT_MATTER_FAILED,
        error
    }
}

export function deleteSettlementMatter(params: ActionstepMatterInfo): DeleteSettlementMatter {
    return {
        type: CONSTANTS.DELETE_SETTLEMENT_MATTER_REQUESTED,
        params
    }
}

export function deleteSettlementMatterSuccess(data: SettlementMatter): DeleteSettlementMatterSuccess {
    return {
        type: CONSTANTS.DELETE_SETTLEMENT_MATTER_SUCCESS,
        data
    }
}

export function deleteSettlementMatterFailed(error: ErrorViewModel): ActionFailed {
    return {
        type: CONSTANTS.DELETE_SETTLEMENT_MATTER_FAILED,
        error
    }
}

export function getSettlementMatter(params: ActionstepMatterInfo): GetSettlementMatter {
    return {
        type: CONSTANTS.GET_SETTLEMENT_MATTER_REQUESTED,
        params
    }
}

export function getSettlementMatterSuccess(data: SettlementMatterViewModel): GetSettlementMatterSuccess {
    return {
        type: CONSTANTS.GET_SETTLEMENT_MATTER_SUCCESS,
        data
    }
}

export function getSettlementMatterFailed(error: ErrorViewModel): ActionFailed {
    return {
        type: CONSTANTS.GET_SETTLEMENT_MATTER_FAILED,
        error
    }
}

export function changeState(data: string): ChangeState {
    return {
        type: CONSTANTS.CHANGE_STATE,
        data
    }
}

export function clearSettlementState(): ClearSettlementInfoState {
    return {
        type: CONSTANTS.CLEAR_SETTLEMENT_INFO_STATE
    }
}