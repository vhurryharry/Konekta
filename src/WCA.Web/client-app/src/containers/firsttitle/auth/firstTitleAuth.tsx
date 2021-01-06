import * as React from 'react'
import { connect } from 'react-redux';

import { TextField } from 'office-ui-fabric-react/lib/TextField';
import { PrimaryButton } from 'office-ui-fabric-react';
import { MessageBar, MessageBarType } from 'office-ui-fabric-react';
import { Spinner } from 'office-ui-fabric-react/lib/Spinner';

import { ReduxData, AppState, ReduxStatus } from 'app.types';
import {
    checkFirstTitleCredentials
} from 'containers/firsttitle/redux/actions'

import "./firstTitleAuth.css"
import { FirstTitleCredential } from 'utils/wcaApiTypes';

interface IMapDispatchToProps {
    checkFirstTitleCredentials: (params: FirstTitleCredential) => void;
}

interface IMapStateToProps {
    isValidCredentials: ReduxData<boolean> | undefined
}

interface IAppProps {
    onConnection: () => void;
}

type AppProps = IMapStateToProps & IMapDispatchToProps & IAppProps;

type AppStates = {
    username: string | undefined;
    password: string | undefined;
    hasError: boolean;
    errorMessage: string | undefined;
    isLoading: boolean;
}

export class FirstTitleAuth extends React.Component<AppProps, AppStates> {
    constructor(props: AppProps) {
        super(props);

        this.state = {
            username: "",
            password: "",
            hasError: false,
            errorMessage: undefined,
            isLoading: false
        };
    }

    onChangeUsername = (newUsername: string | undefined) => {
        this.setState({
            username: newUsername,
            hasError: false
        });
    }

    onChangePassword = (newPassword: string | undefined) => {
        this.setState({
            password: newPassword,
            hasError: false
        });
    }

    connectFirstTitle = () => {
        const { username, password } = this.state;

        const params = new FirstTitleCredential({
            username,
            password
        });

        this.props.checkFirstTitleCredentials(params);

        this.setState({
            isLoading: true,
            hasError: false
        })
    }

    static getDerivedStateFromProps(nextProps: AppProps, prevState: AppStates): AppStates {
        let nextState = {} as AppStates;

        if (nextProps.isValidCredentials) {
            if (nextProps.isValidCredentials.status === ReduxStatus.Requested) {
                nextState.isLoading = true;
                nextState.hasError = false;
            } else if (nextProps.isValidCredentials.status === ReduxStatus.Success) {
                nextState.isLoading = false;

                if (nextProps.isValidCredentials.data) {
                    nextState.hasError = false;
                    nextProps.onConnection();
                } else {
                    nextState.hasError = true;
                    nextState.errorMessage = "Invalid username or password.";
                }
            } else if (nextProps.isValidCredentials.status === ReduxStatus.Failed) {
                nextState.isLoading = false;

                nextState.hasError = true;
                nextState.errorMessage = nextProps.isValidCredentials.error!.message;
            }
        }

        return nextState;
    }

    render(): JSX.Element {
        const { username, password, hasError, errorMessage, isLoading } = this.state;

        return (
            <>
                <h3>
                    You're not connected with First Title. <br />
                    Please login with First Title credentials to get connected.
                </h3>

                <div className="first-title-auth-row">
                    <TextField
                        label="Username:"
                        value={username}
                        onChange={(ev, newValue) => this.onChangeUsername(newValue)}
                    />
                </div>

                <div className="first-title-auth-row">
                    <TextField
                        label="Password:"
                        type="password"
                        value={password}
                        onChange={(ev, newValue) => this.onChangePassword(newValue)}
                    />
                </div>

                {isLoading &&
                    <div className="first-title-auth-row">
                        <Spinner label="Please wait..." ariaLive="assertive" labelPosition="right" />
                    </div>
                }

                {hasError &&
                    <div className="first-title-auth-row">
                        <MessageBar messageBarType={MessageBarType.error}>
                            {errorMessage}
                        </MessageBar>
                    </div>
                }

                <div className="first-title-auth-row">
                    <PrimaryButton text="Connect First Title"
                        onClick={() => this.connectFirstTitle()}
                    />
                </div>
            </>
        );
    }
}

const mapStateToProps = (state: AppState) => {
    return {
        isValidCredentials: state.firstTitle.isValidCredentials
    }
}

const mapDispatchToProps = {
    checkFirstTitleCredentials
}

export default connect(mapStateToProps, mapDispatchToProps)(FirstTitleAuth);