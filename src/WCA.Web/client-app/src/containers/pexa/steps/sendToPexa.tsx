import * as React from 'react';
import { connect } from 'react-redux';
import { Spinner, SpinnerSize } from 'office-ui-fabric-react/lib/Spinner';

import {
    sendDataToPexa,
    clearPexaFormData,
    clearPexaState
} from 'containers/pexa/redux/actions';
import * as CONSTANTS from 'containers/pexa/redux/constants'
import { ErrorViewModel, PEXAWorkspaceCreationRequestWithActionstepResponse, CreatePexaWorkspaceCommand, CreatePexaWorkspaceResponse } from 'utils/wcaApiTypes';
import { SubwayNavNodeState } from 'components/SubwayNav';
import { AppState } from 'app.types';
import Tools from 'utils/tools';
import { MessageBar, MessageBarType } from 'office-ui-fabric-react/lib/MessageBar';

interface IMapStateToProps {
    success: boolean;
    gotResponse: boolean;
    pexaWorkspaceCreationData: PEXAWorkspaceCreationRequestWithActionstepResponse;
    createPexaWorkspaceResponse: CreatePexaWorkspaceResponse | undefined;
    pexaConnected: boolean | null;
    error: ErrorViewModel | undefined;
    requestType: string;
}

interface IMapDispatchToProps {
    sendDataToPexa: (params: CreatePexaWorkspaceCommand) => void;
    clearPexaFormData: () => void;
    clearPexaState: () => void;
}

interface IAppProps {
    onChangeStep: (newState: SubwayNavNodeState, newStep?: number) => void;
}

type AppProps = IAppProps & IMapStateToProps & IMapDispatchToProps;

type AppStates = {
    dataLoaded: boolean,
    status: SendState
}

enum SendState {
    NotReady,
    Success,
    PexaNotConnected,
    UnknownError,
    WorkspaceAlreadyExists
}

export class SendToPexa extends React.Component<AppProps, AppStates> {

    constructor(props: AppProps) {
        super(props);

        this.state = {
            dataLoaded: false,
            status: SendState.NotReady
        }
    }

    public componentDidMount(): void {
        this.props.sendDataToPexa(this.props.pexaWorkspaceCreationData.createPexaWorkspaceCommand!);
    }

    public shouldComponentUpdate(nextProps: AppProps, nextState: AppStates): boolean {
        const { status } = this.state;

        if (status === SendState.PexaNotConnected) {
            Tools.PopupConnectToPexa(() => this.reloadActionstepMatter());
        }

        return true;
    }

    static getDerivedStateFromProps(nextProps: AppProps, prevState: AppStates): AppStates {
        let nextState = {} as AppStates;

        if (nextProps.gotResponse === true) {
            switch (nextProps.requestType) {
                case CONSTANTS.SEND_DATA_TO_PEXA_REQUESTED:

                    nextState.dataLoaded = true;

                    if (nextProps.success) {
                        if (nextProps.createPexaWorkspaceResponse!.workspaceExists) {
                            nextProps.onChangeStep(SubwayNavNodeState.Error);
                            nextState.status = SendState.WorkspaceAlreadyExists;
                        } else {
                            nextProps.onChangeStep(SubwayNavNodeState.Completed, 4);
                        }

                        nextProps.clearPexaFormData();
                    } else {
                        nextProps.onChangeStep(SubwayNavNodeState.Error);

                        if (!nextProps.pexaConnected) {
                            nextState.status = SendState.PexaNotConnected;
                        } else {
                            nextState.status = SendState.UnknownError;
                            nextProps.onChangeStep(SubwayNavNodeState.Error, 3);
                        }
                    }
                    break;

                default:
                    return nextState;
            }

            nextProps.clearPexaState();
        }

        return nextState;
    }

    public render(): JSX.Element {
        const { dataLoaded, status } = this.state;
        const { createPexaWorkspaceResponse } = this.props;

        return (
            <div className="animated fadeIn">
                {dataLoaded
                    ? (
                        status === SendState.PexaNotConnected ?
                            <MessageBar messageBarType={MessageBarType.error} isMultiline={false}>
                                You're not connected to <b>PEXA</b>
                            </MessageBar>
                            : status === SendState.WorkspaceAlreadyExists && createPexaWorkspaceResponse &&
                            <MessageBar messageBarType={MessageBarType.error} isMultiline={false}>
                                A PEXA workspace for this matter already exists. <a href={createPexaWorkspaceResponse.workspaceUri}><i>Go to PEXA workspace: {createPexaWorkspaceResponse.workspaceId}</i></a>
                            </MessageBar>
                    )
                    : <Spinner size={SpinnerSize.large} />
                }
            </div>
        );
    }

    public reloadActionstepMatter(): void {
        this.setState({
            dataLoaded: false,
            status: SendState.NotReady
        });

        this.props.sendDataToPexa(this.props.pexaWorkspaceCreationData.createPexaWorkspaceCommand!);
    }
}

const mapStateToProps = (state: AppState): IMapStateToProps => {
    return {
        success: state.pexa.success,
        gotResponse: state.pexa.gotResponse,
        pexaWorkspaceCreationData: state.pexa.pexaWorkspaceCreationData,
        createPexaWorkspaceResponse: state.pexa.createPexaWorkspaceResponse,
        pexaConnected: state.common.pexaConnected,
        requestType: state.pexa.requestType,
        error: state.pexa.error
    }
}

const mapDispatchToProps: IMapDispatchToProps = {
    sendDataToPexa,
    clearPexaFormData,
    clearPexaState
}

export default connect(mapStateToProps, mapDispatchToProps)(SendToPexa);