import * as CONSTANTS from './constants'
import { ReduxData, ReduxStatus } from 'app.types';
import { RequestPropertyInformationFromActionstepResponse } from 'utils/wcaApiTypes';
import {
    GlobalXActionTypes,
    GetPropertyInformationSuccess,
    ActionFailed
} from 'containers/globalx/redux/actionTypes';

export interface GlobalXState {
    propertyInformation: ReduxData<RequestPropertyInformationFromActionstepResponse> | undefined
}

const initialState: GlobalXState = {
    propertyInformation: undefined
}

function globalXReducer(state: GlobalXState = initialState, action: GlobalXActionTypes): GlobalXState {
    switch (action.type) {
        case CONSTANTS.GET_PROPERTY_INFORMATION_REQUESTED:
            return {
                ...state,
                propertyInformation: new ReduxData<RequestPropertyInformationFromActionstepResponse>(ReduxStatus.Requested)
            };

        case CONSTANTS.GET_PROPERTY_INFORMATION_SUCCESS:
            return {
                ...state,
                propertyInformation: new ReduxData<RequestPropertyInformationFromActionstepResponse>().markAsSuccess((action as GetPropertyInformationSuccess).data)
            };

        case CONSTANTS.GET_PROPERTY_INFORMATION_FAILED:
            return {
                ...state,
                propertyInformation: new ReduxData<RequestPropertyInformationFromActionstepResponse>().markAsFailed((action as ActionFailed).error)
            };

        default:
            return state;
    }
}

export default globalXReducer;