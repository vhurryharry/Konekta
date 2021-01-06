import { combineReducers, Reducer } from 'redux';
import { connectRouter } from 'connected-react-router'
import { History } from 'history';

import { AppState, AppActionTypes } from 'app.types';

import pexaReducer from 'containers/pexa/redux/reducer'
import settlementInfoReducer from 'containers/calculators/settlement/redux/reducer';
import commonReducer from 'redux/common/reducer';
import firstTitleReducer from 'containers/firsttitle/redux/reducer';
import globalXReducer from 'containers/globalx/redux/reducer';

const createRootReducer = (history: History<any>): Reducer<AppState, AppActionTypes> => combineReducers(
    {
        router: connectRouter(history),
        common: commonReducer,
        pexa: pexaReducer,
        globalx: globalXReducer,
        settlementInfo: settlementInfoReducer,
        firstTitle: firstTitleReducer,
    }
)

export default createRootReducer