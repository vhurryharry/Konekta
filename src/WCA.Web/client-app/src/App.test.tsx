import React from 'react';
import ReactDOM from 'react-dom';

import { Provider } from 'react-redux'
import { ConnectedRouter } from 'connected-react-router/immutable'

import { store, history } from 'redux/store'

import App from './App';

it('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(
    <Provider store={store}>
      <ConnectedRouter history={history}>
        <App />
      </ConnectedRouter>
    </Provider>,
    div);
  ReactDOM.unmountComponentAtNode(div);
});
