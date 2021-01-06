import { createStore, applyMiddleware, compose } from 'redux';
import { fromJS } from 'immutable';
import { routerMiddleware } from 'connected-react-router/immutable';
import createSagaMiddleware from 'redux-saga';
import { History } from 'history';

import createReducer from 'redux/reducers';
import rootSaga from 'redux/sagas'

const sagaMiddleware = createSagaMiddleware();

export default function configureStore(initialState = {}, history: History<any>) {

    // Create the store with two middlewares
    // 1. sagaMiddleware: Makes redux-sagas work
    // 2. routerMiddleware: Syncs the location/URL path to the state

    const middlewares = [sagaMiddleware, routerMiddleware(history)];

    const enhancers = [applyMiddleware(...middlewares)];

    const rootReducer = createReducer(history);

    const store = createStore(
        rootReducer,
        fromJS(initialState),
        compose(...enhancers),
    );

    sagaMiddleware.run(rootSaga);

    return store;
}