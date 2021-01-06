import { combineReducers } from 'redux'

import settlementInfoReducer from './reducers/settlement-info-reducer'

const appReducer = combineReducers({
    settlementInfo: settlementInfoReducer
});

const rootReducer = (state, action) => {
    return appReducer(state, action);
}

export default rootReducer;