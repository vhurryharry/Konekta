import * as React from 'react'

export default class ConnectToActionstep extends React.Component {
    public render(): JSX.Element {
        return (
            <div className="ibox" data-cy="connect-to-actionstep-box">
                <div className="ibox-title">
                    <h2>We need your approval</h2>
                </div>
                <div className="ibox-content">
                    So that we can pre-populate the matter details we need your permission to access your Actionstep system. <br />
                    Please click <b>Connect to Actionstep</b>, to be redirected to Actionstep where they will ask you for your username and password.
                        </div>
                <div className="ibox-footer">
                    <button
                        className="btn btn-success"
                        data-cy="connect-to-actionstep"
                        onClick={() => this.connectToActionstep()}
                    >
                        Connect To Actionstep
                    </button>
                </div>
            </div>
        );
    }

    public connectToActionstep(): void {
        const returnUrl = window.location.href.replace("&", "%26");
        window.location.href = '/integrations/actionstep/connect?returnUrl=' + returnUrl;
    }
}