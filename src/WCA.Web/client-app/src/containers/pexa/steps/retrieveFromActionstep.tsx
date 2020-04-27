import * as React from 'react';
import { connect } from 'react-redux';

import { Spinner, SpinnerSize } from 'office-ui-fabric-react/lib/Spinner';
import { PrimaryButton, Stack, IStackProps } from 'office-ui-fabric-react';
import { MessageBar, MessageBarType } from 'office-ui-fabric-react';
import { Link } from 'office-ui-fabric-react/lib/Link'

import {
    getDataFromActionstep,
    clearPexaState,
    clearPexaFormData,
    setPexaWorkspaceCreation
} from 'containers/pexa/redux/actions';

import * as CONSTANTS from 'containers/pexa/redux/constants'
import { SubwayNavNodeState } from 'components/SubwayNav';

import { WorkspaceCreationRequest, ActionstepMatterInfo, PEXAWorkspaceCreationRequestWithActionstepResponse } from 'utils/wcaApiTypes';
import { AppState, JwtMatterInfo } from 'app.types';

import Tools from 'utils/tools'
import { WorkspaceCreationRequestWithMatterInfo } from 'containers/pexa/redux/actionTypes';

interface IMapStateToProps {
    success: boolean;
    pexaFormData: WorkspaceCreationRequestWithMatterInfo | undefined;
    pexaWorkspaceCreationData: PEXAWorkspaceCreationRequestWithActionstepResponse;
    jwtMatterInfo: JwtMatterInfo | undefined;
    gotResponse: boolean;
    pexaConnected: boolean | null;
    requestType: string;
}

interface IMapDispatchToProps {
    getDataFromActionstep: (params: ActionstepMatterInfo) => void;
    setPexaWorkspaceCreation: (data: WorkspaceCreationRequest) => void;
    clearPexaFormData: () => void;
    clearPexaState: () => void;
}

interface IAppProps {
    onChangeStep: (newState: SubwayNavNodeState, newStep?: number) => void;
}

type AppProps = IAppProps & IMapStateToProps & IMapDispatchToProps;

type AppStates = {
    dataLoaded: boolean,
    status: RetrieveState,
    matterInfo: ActionstepMatterInfo | null
}

enum RetrieveState {
    NotReady,
    Success,
    NoMatterSelected,
    PexaNotConnected,
    UnknownError
}

export class RetrieveFromActionstep extends React.Component<AppProps, AppStates> {
    constructor(props: Readonly<AppProps>) {
        super(props);

        this.state = {
            dataLoaded: false,
            status: RetrieveState.NotReady,
            matterInfo: null
        }
    }

    public componentDidMount(): void {
        this.loadActionstepMatter();
    }

    public shouldComponentUpdate(nextProps: AppProps, nextState: AppStates): boolean {
        const { status } = this.state;

        if (status === RetrieveState.PexaNotConnected) {
            Tools.PopupConnectToPexa(() => this.reloadActionstepMatter());
        }

        return true;
    }

    private loadActionstepMatter(): void {
        const { jwtMatterInfo } = this.props;
        let matterInfo: ActionstepMatterInfo | null = null;

        if (jwtMatterInfo) {
            matterInfo = new ActionstepMatterInfo({
                orgKey: jwtMatterInfo.orgKey,
                matterId: jwtMatterInfo.matterId
            });
        }

        if (matterInfo === null) {
            this.setState({
                status: RetrieveState.NoMatterSelected,
                dataLoaded: true
            });

            return;
        }

        this.setState({ matterInfo });

        this.props.getDataFromActionstep(matterInfo);
    }

    static getDerivedStateFromProps(nextProps: AppProps, prevState: AppStates): AppStates {
        let nextState = {} as AppStates;

        if (nextProps.gotResponse === true) {

            switch (nextProps.requestType) {
                case CONSTANTS.GET_DATA_FROM_ACTIONSTEP_REQUESTED:

                    nextState.dataLoaded = true;

                    if (nextProps.success) {
                        const { pexaFormData, pexaWorkspaceCreationData } = nextProps;

                        if (pexaWorkspaceCreationData.existingPexaWorkspace) {
                            nextProps.onChangeStep(SubwayNavNodeState.Error);
                        } else {
                            if (pexaFormData) {
                                const { matterInfo } = prevState;
                                if (matterInfo) {
                                    if (pexaFormData.matterInfo.orgKey === matterInfo.orgKey
                                        && pexaFormData.matterInfo.matterId === matterInfo.matterId)
                                        nextProps.setPexaWorkspaceCreation(pexaFormData.workspaceCreationRequest);
                                }
                            } else {
                                nextProps.clearPexaFormData();
                            }

                            nextProps.onChangeStep(SubwayNavNodeState.Completed);
                        }
                    } else {
                        if (!nextProps.pexaConnected) {
                            nextState.status = RetrieveState.PexaNotConnected;
                        } else {
                            nextState.status = RetrieveState.UnknownError;
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
        const { success, pexaWorkspaceCreationData } = this.props;
        const { existingPexaWorkspace } = pexaWorkspaceCreationData;

        const verticalStackProps: IStackProps = {
            styles: { root: { overflow: 'hidden', width: '100%' } },
            tokens: { childrenGap: 20 }
        };

        if (existingPexaWorkspace) {
            return (
                <MessageBar messageBarType={MessageBarType.error}>
                    A PEXA workspace for this matter already exists. <a href={existingPexaWorkspace.pexaWorkspaceUri}><i>Go to PEXA workspace: {existingPexaWorkspace.pexaWorkspaceId}</i></a>
                </MessageBar>
            );
        }

        return (
            <div className="animated fadeIn">

                {dataLoaded ?
                    (
                        success
                            ? <p>Data retrieved successfully from Actionstep. Click <b>Next</b> to check.<br /></p>
                            : (
                                status === RetrieveState.PexaNotConnected
                                    ? <div>
                                        <Stack {...verticalStackProps}>
                                            <MessageBar messageBarType={MessageBarType.error}>You're not connected to <strong>PEXA</strong>.</MessageBar>

                                            <p>
                                                if nothing happens, check that you have pop-ups enabled:
                                                <img src="/images/matter-blocked-popup-instructions.png" alt="Instructions for enabling pop-ups when blocked" width="100%" />
                                            </p>
                                        </Stack>
                                    </div>
                                    : status === RetrieveState.NoMatterSelected
                                        ? (this.errorPanel())
                                        : <p className="text-danger">
                                            An unexpected error has occured. Please <Link onClick={() => window.location.reload(false)} >refresh the page</Link> and try again. If you continue to experience problems, please
                                                <Link href={"https://support.konekta.com.au/support/tickets/new"} target="_blank" rel="noopener noreferrer"> log a support ticket</Link>.
                                          </p>
                            )
                    )
                    :
                    <Spinner size={SpinnerSize.large} />
                }

                {success &&
                    <PrimaryButton
                        className="button"
                        data-automation-id="next_button"
                        data-cy="next_button"
                        text="Next"
                        onClick={() => this.props.onChangeStep(SubwayNavNodeState.Completed)}
                        allowDisabledFocus={true}
                    />
                }
            </div>
        );
    }

    public reloadActionstepMatter(): void {
        this.setState({
            dataLoaded: false,
            status: RetrieveState.NotReady
        })

        this.loadActionstepMatter();
    }

    private errorPanel = (): JSX.Element => {
        return (
            <MessageBar messageBarType={MessageBarType.error}>
                <h4>No Matter Selected</h4>
                Cannot retrieve data from <strong>Actionstep</strong> because no matter is selected.
            </MessageBar>
        )
    }
}

const mapStateToProps = (state: AppState): IMapStateToProps => {
    return {
        gotResponse: state.pexa.gotResponse,
        success: state.pexa.success,
        requestType: state.pexa.requestType,
        pexaWorkspaceCreationData: state.pexa.pexaWorkspaceCreationData,
        pexaFormData: state.pexa.pexaFormData,
        jwtMatterInfo: state.common.jwtMatterInfo,
        pexaConnected: state.common.pexaConnected
    }
}

const mapDispatchToProps: IMapDispatchToProps = {
    getDataFromActionstep,
    setPexaWorkspaceCreation,
    clearPexaFormData,
    clearPexaState
}

export default connect(mapStateToProps, mapDispatchToProps)(RetrieveFromActionstep);