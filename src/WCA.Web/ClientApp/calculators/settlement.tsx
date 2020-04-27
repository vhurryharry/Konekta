import * as React from 'react';
import SettlementCalculator from './settlement-calculator'

import { Provider } from 'react-redux'
import configureStore from './redux/configure-store'
import { AppInfoService } from '../services/app-info-service';

const store = configureStore();

interface IAppProps {
    appInfoService: AppInfoService;
}

type AppProps = IAppProps;

export default class SettlementCalculatorApp extends React.Component<AppProps, any> {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <Provider store={store}>
                <SettlementCalculator appInfoService={this.props.appInfoService} />
            </Provider>
        )
    }
}