import * as React from 'react'
import { connect } from 'react-redux';

import {
    clearPexaState,
    getPexaWorkspaceSummary
} from 'containers/pexa/redux/actions';

// import { MessageBar, MessageBarType } from 'office-ui-fabric-react';

import {
    LandTitleReferenceVerificationResponseTypeWarningsWorkspace,
    WorkspaceSummaryResponseType,
    ErrorViewModel,
    PexaRole,
    RetrieveWorkspaceSummaryParameters,
    FullNameType,
    FullName
} from 'utils/wcaApiTypes'
import { AppState, ReduxStatus } from 'app.types';
import { WorkspaceSummaryList } from 'containers/pexa/redux/actionTypes';
import LoadingWidget from 'components/common/loadingWidget';
import { MessageBar, MessageBarType } from 'office-ui-fabric-react';

interface IMapStateToProps {
    success: boolean;
    gotResponse: boolean;
    requestType: string;
    workspaceSummaryList: WorkspaceSummaryList
}

interface IMapDispatchToProps {
    getPexaWorkspaceSummary: (params: RetrieveWorkspaceSummaryParameters) => void;
    clearPexaState: () => void;
}

interface IAppProps {
    workspace: LandTitleReferenceVerificationResponseTypeWarningsWorkspace,
    subscriberRole: PexaRole
}

type AppProps = IAppProps & IMapStateToProps & IMapDispatchToProps;

type AppStates = {
    dataLoaded: boolean,
    workspaceSummary: WorkspaceSummaryResponseType | null;
    error: ErrorViewModel | null;
}

export class ValidateLandTitleWorkspace extends React.Component<AppProps, AppStates> {

    constructor(props: AppProps) {
        super(props);

        const workspaceSummary = props.workspaceSummaryList[props.workspace.workspaceId!];
        const dataLoaded = workspaceSummary !== undefined && workspaceSummary.status === ReduxStatus.Success;

        this.state = {
            dataLoaded: dataLoaded,
            workspaceSummary: dataLoaded ? workspaceSummary.data! : null,
            error: dataLoaded ? workspaceSummary.error! : null
        }

        if (!dataLoaded) {
            const { subscriberRole, workspace } = props;
            const params = new RetrieveWorkspaceSummaryParameters({
                subscriberRole: subscriberRole,
                workspaceId: workspace.workspaceId
            });

            props.getPexaWorkspaceSummary(params);
        }
    }

    static getDerivedStateFromProps(nextProps: AppProps, prevState: AppStates): AppStates {
        let nextState = {} as AppStates;
        const newWorkspaceSummary = nextProps.workspaceSummaryList[nextProps.workspace.workspaceId!];

        if (!prevState.dataLoaded && newWorkspaceSummary !== undefined) {
            if (newWorkspaceSummary.status === ReduxStatus.Success || newWorkspaceSummary.status === ReduxStatus.Failed) {
                if (newWorkspaceSummary.status === ReduxStatus.Success) {
                    nextState.dataLoaded = true;
                    nextState.workspaceSummary = newWorkspaceSummary.data!;
                    nextState.error = null;
                } else {
                    nextState.dataLoaded = true;
                    nextState.workspaceSummary = null;
                    nextState.error = newWorkspaceSummary.error!;
                }
            }
        }

        return nextState;
    }

    generateFullName(fullName: FullNameType | FullName): string {
        let fullNameString = "";

        if (fullName.givenName) {
            const givenNameList = fullName.givenName.sort((a, b) => {
                const orderA = a.order ? parseInt(a.order) : 0;
                const orderB = b.order ? parseInt(b.order) : 0;
                return orderA - orderB;
            })

            givenNameList.forEach((givenName, index) => {
                fullNameString += givenName.value!;
                if (index < givenNameList.length - 1) fullNameString += " ";
            })
        }

        if (fullName.familyName) {
            if (fullName.familyNameOrder && fullName.familyNameOrder === "First") fullNameString = fullName.familyName + " " + fullNameString;
            else fullNameString += " " + fullName.familyName;
        }

        return fullNameString;
    }

    render(): JSX.Element {
        const { dataLoaded, workspaceSummary, error } = this.state;

        if (!dataLoaded) {
            return <LoadingWidget />;
        }

        const proprietorsOnTitle = [];
        if (workspaceSummary && workspaceSummary.partyDetails) {
            for (let i = 0; i < workspaceSummary.partyDetails.length; i++) {
                if (workspaceSummary.partyDetails[i].partyRole === "Proprietor on Title") {
                    proprietorsOnTitle.push(workspaceSummary.partyDetails[i]);
                }
            }
        }

        return (
            <div className="validate-pexa-workspace animated fadeIn">
                {workspaceSummary && workspaceSummary.participantDetails && workspaceSummary.participantDetails.length > 0 &&
                    <div className="validate-pexa-info-group">
                        <div className="ms-Grid-row validate-pexa-participant">
                            <div className="ms-Grid-col ms-sm6">
                                <b>Participants in workspace</b>
                            </div>
                            <div className="ms-Grid-col ms-sm3">
                                <b>Role</b>
                            </div>
                            <div className="ms-Grid-col ms-sm3">
                                <b>Invitation Status</b>
                            </div>
                        </div>
                        {workspaceSummary.participantDetails.map((participant, index) => {
                            return (
                                <div className="ms-Grid-row validate-pexa-participant" key={index}>
                                    <div className="ms-Grid-col ms-sm6">
                                        {participant.subscriberName}
                                    </div>
                                    <div className="ms-Grid-col ms-sm3">
                                        {participant.workspace![0].role}
                                    </div>
                                    <div className="ms-Grid-col ms-sm3">
                                        Accepted
                                </div>
                                </div>
                            )
                        })}
                    </div>
                }
                {workspaceSummary && proprietorsOnTitle.length > 0 &&
                    <div className="validate-pexa-info-group">
                        <div className="ms-Grid-row validate-pexa-participant">
                            <div className="ms-Grid-col ms-sm6">
                                <b>Proprietors on Title</b>
                            </div>
                            {/* <div className="ms-Grid-col ms-sm3">
                                <b>Transaction type</b>
                            </div> */}
                            <div className="ms-Grid-col ms-sm6">
                                <b>Jurisdiction</b>
                            </div>
                        </div>
                        {proprietorsOnTitle.map((party, index) => {
                            if (party.partyRole === "Proprietor on Title") {
                                return (
                                    <div className="ms-Grid-row validate-pexa-participant" key={index}>
                                        <div className="ms-Grid-col ms-sm6">
                                            {party.fullName && this.generateFullName(party.fullName)}
                                        </div>
                                        {/* <div className="ms-Grid-col ms-sm3">
                                            Non-transfer
                                        </div> */}
                                        <div className="ms-Grid-col ms-sm6">
                                            {workspaceSummary.jurisdiction}
                                        </div>
                                    </div>
                                )
                            }
                            else {
                                return null;
                            }
                        })
                        }
                    </div>
                }
                {workspaceSummary && workspaceSummary.landTitleDetails &&
                    <div className="validate-pexa-info-group">
                        <div className="ms-Grid-row validate-pexa-participant">
                            <div className="ms-Grid-col ms-sm6">
                                <b>Property Address</b>
                            </div>
                        </div>
                        <div className="ms-Grid-row validate-pexa-participant">
                            <div className="ms-Grid-col ms-sm6">
                                {workspaceSummary.landTitleDetails[0].propertyDetails}
                            </div>
                        </div>
                    </div>
                }

                {workspaceSummary === null && error !== null &&
                    <MessageBar messageBarType={MessageBarType.info}>
                        This workspace belongs to others
                    </MessageBar>
                }

            </div>
        )
    }
}

const mapStateToProps = (state: AppState): IMapStateToProps => {
    return {
        gotResponse: state.pexa.gotResponse,
        success: state.pexa.success,
        requestType: state.pexa.requestType,
        workspaceSummaryList: state.pexa.workspaceSummaryList
    }
}

const mapDispatchToProps: IMapDispatchToProps = {
    getPexaWorkspaceSummary,
    clearPexaState
}

export default connect(mapStateToProps, mapDispatchToProps)(ValidateLandTitleWorkspace);