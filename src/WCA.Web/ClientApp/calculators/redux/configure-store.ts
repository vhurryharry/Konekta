import { createStore, applyMiddleware } from 'redux'
import createSagaMiddleware from 'redux-saga'

import reducer from './reducer'
import saga from './saga'

const sagaMiddleware = createSagaMiddleware();

export default function configureStore() {
    const store = createStore(reducer, applyMiddleware(sagaMiddleware));
    sagaMiddleware.run(saga);
    return store;
}