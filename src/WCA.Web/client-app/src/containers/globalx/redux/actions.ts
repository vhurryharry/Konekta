import * as CONSTANTS from './constants'
import {
    ActionstepMatterInfo,
    RequestPropertyInformationFromActionstepResponse,
    ErrorViewModel
} from 'utils/wcaApiTypes'
import {
    ActionFailed,
    GetPropertyInformation,
    GetPropertyInformationSuccess
} from 'containers/globalx/redux/actionTypes'

export function getPropertyInformation(matterInfo: ActionstepMatterInfo, entryPoint: string, embed: boolean): GetPropertyInformation {
    return {
        type: CONSTANTS.GET_PROPERTY_INFORMATION_REQUESTED,
        matterInfo,
        entryPoint,
        embed
    }
}

export function getPropertyInformationSuccess(data: RequestPropertyInformationFromActionstepResponse): GetPropertyInformationSuccess {
    return {
        type: CONSTANTS.GET_PROPERTY_INFORMATION_SUCCESS,
        data
    }
}

export function getPropertyInformationFailed(error: ErrorViewModel): ActionFailed {
    return {
        type: CONSTANTS.GET_PROPERTY_INFORMATION_FAILED,
        error
    }
}