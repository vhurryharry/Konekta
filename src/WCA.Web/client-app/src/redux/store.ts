import history from 'utils/history'

import configureStore from 'redux/configureStore'

const initialState = {};
const store = configureStore(initialState, history);

export { store, history };