import * as React from 'react'
import { connect } from 'react-redux';
import { DefaultButton } from 'office-ui-fabric-react';

import Tools from 'utils/tools'
import { AppState } from 'app.types';

import "./connectToActionstep.css"

interface IMapStateToProps {
    isFirstTime: boolean
}

interface IMapDispatchToProps {
}

interface IAppProps {
    callback: () => void;
}

type AppProps = IAppProps & IMapStateToProps & IMapDispatchToProps;

export class ConnectToActionstep extends React.Component<AppProps> {
    public render(): JSX.Element {
        const { isFirstTime } = this.props;

        return (
            <div className="connect-box" data-cy="connect-to-actionstep-box">
                <p className="connect-message">
                    {isFirstTime
                        ? "It looks like this is the first time you're using Konekta integrations. Before you can use Konekta, you'll need to link your Actionstep and Konekta accounts."
                        : "We need to re-link your Actionstep and Konekta accounts."
                    }
                </p>

                <div className="connect-box-footer">
                    <DefaultButton
                        className="button green-bg"
                        data-automation-id="delete_button"
                        data-cy="delete_button"
                        text={isFirstTime ? "Link to Konekta" : "Re-link Konekta"}
                        onClick={() => this.connectToActionstep()}
                        allowDisabledFocus={true}
                    />
                    {!isFirstTime &&
                        <span className="connect-tip">
                            You should only need to do this once. If you continue to see this message, please <a href="https://support.konekta.com.au/support/tickets/new" target="_blank" rel="noopener noreferrer">submit a ticket</a>.
                        </span>
                    }
                </div>
            </div>
        );
    }

    public connectToActionstep(): void {
        Tools.PopupConnectToActionstep(this.props.callback);
    }
}

const mapStateToProps = (state: AppState): IMapStateToProps => {
    return {
        isFirstTime: state.common.isFirstTime
    }
}

export default connect(mapStateToProps)(ConnectToActionstep);