import * as React from 'react';
import { connect } from 'react-redux';

import { Link } from 'office-ui-fabric-react/lib/Link'
import { DefaultButton } from 'office-ui-fabric-react/lib/Button'
import { Modal } from 'office-ui-fabric-react/lib/Modal'

import { SubwayNavNodeState } from 'components/SubwayNav';
import { AppState, ReduxData, ReduxStatus } from 'app.types';
import { InviteParticipant } from 'containers/pexa/steps/components/inviteParticipant';
import { CreateWorkspaceInvitationRequestType, CreateWorkspaceInvitationResponseType } from 'utils/wcaApiTypes';

import {
    createInvitation
} from 'containers/pexa/redux/actions';
import LoadingWidget from 'components/common/loadingWidget';
import { MessageBar, MessageBarType } from 'office-ui-fabric-react/lib/MessageBar';

interface IMapStateToProps {
    pexaWorkspaceUri: string;
    pexaWorkspaceId: string;
    pexaInvitationUri: string;
    invitationResponseList: ReduxData<CreateWorkspaceInvitationResponseType[]> | undefined;
}

interface IMapDispatchToProps {
    createInvitation: (params: CreateWorkspaceInvitationRequestType[]) => void;
}

interface IAppProps {
    onChangeStep: (newState: SubwayNavNodeState) => void;
}

type AppProps = IAppProps & IMapStateToProps & IMapDispatchToProps;

type AppStates = {
    showInviteModal: boolean,
    participants: JSX.Element[],
    participantRefs: React.RefObject<InviteParticipant>[],
    dataStatus: ReduxStatus,
    refresh: boolean
}

export class WorkspaceCreated extends React.Component<AppProps, AppStates> {

    constructor(props: AppProps) {
        super(props);

        const initialRef = React.createRef<InviteParticipant>();
        this.state = {
            showInviteModal: false,
            participantRefs: [initialRef],
            participants: [(<InviteParticipant participantIndex={0}
                invitationUri={props.pexaInvitationUri}
                removeParticipant={this.removeParticipant}
                onChange={this.onChangeInput}
                key="0" ref={initialRef} />)],
            dataStatus: ReduxStatus.NotRequested,
            refresh: true
        }
    }

    static getDerivedStateFromProps(nextProps: AppProps, prevState: AppStates): AppStates {
        let nextState = {} as AppStates;

        if (prevState.dataStatus === ReduxStatus.Requested && nextProps.invitationResponseList) {
            if (nextProps.invitationResponseList.status === ReduxStatus.Success) {
                nextState.dataStatus = ReduxStatus.Success;
            } else if (nextProps.invitationResponseList.status === ReduxStatus.Failed) {
                nextState.dataStatus = ReduxStatus.Failed;
            }
        }
        return nextState;
    }

    render() {
        const { pexaWorkspaceUri } = this.props;
        const { showInviteModal, participants, participantRefs, dataStatus } = this.state;
        let errorCount = 0;

        return (
            <>
                <h4>
                    You may now <Link href={pexaWorkspaceUri} className="text-info" target="_blank" rel="noopener noreferrer">go to the workspace summary</Link> for this matter.
                </h4>

                <DefaultButton
                    className="button ms-Grid-col ms-sm4 ms-smPush4"
                    data-automation-id="show_invite_modal"
                    data-cy="show_invite_modal"
                    text="Invite Participants"
                    onClick={this.showInviteModal}
                    allowDisabledFocus={true}
                    disabled={dataStatus === ReduxStatus.Requested}
                />

                <br />
                <div>
                    {dataStatus === ReduxStatus.Requested &&
                        <LoadingWidget />
                    }
                    {dataStatus === ReduxStatus.Success &&
                        <MessageBar messageBarType={MessageBarType.success}>
                            Invited Participants successfully!
                    </MessageBar>
                    }
                    {dataStatus === ReduxStatus.Failed &&
                        <MessageBar messageBarType={MessageBarType.error}>
                            Error occured during invitation!
                    </MessageBar>
                    }
                </div>

                <Modal isOpen={showInviteModal}
                    isBlocking={true}
                    onDismiss={this.closeInviteModal}
                    className={showInviteModal !== null ? "animated fadeIn" : "animated fadeOut"}
                >
                    <div className="modal-header">
                        <span className="modal-title">Invite Participants</span>
                    </div>
                    <div className="modal-body">
                        {participants.map((participant, index) => {
                            if (!participantRefs[index].current || !participantRefs[index].current!.validate()) {
                                errorCount++;
                            }
                            return participant;
                        })}

                        <DefaultButton
                            className="button"
                            data-automation-id="add_another_participant"
                            data-cy="add_another_participant"
                            text="Add Another Participant"
                            onClick={this.addParticipant}
                            allowDisabledFocus={true}
                        />
                    </div>
                    <div className="modal-footer">
                        <DefaultButton
                            className="button"
                            data-automation-id="invite_participants"
                            data-cy="invite_participants"
                            text="Send Invitation"
                            onClick={this.sentInvitation}
                            allowDisabledFocus={true}
                            disabled={errorCount > 0 ? true : false}
                        />
                        &nbsp;
                        <DefaultButton
                            className="button"
                            data-automation-id="close_modal"
                            data-cy="close_modal"
                            text="Cancel"
                            onClick={this.closeInviteModal}
                            allowDisabledFocus={true}
                        />
                    </div>
                </Modal>
            </>
        );
    }

    showInviteModal = async () => {
        await this.setState({
            showInviteModal: true,
            dataStatus: ReduxStatus.NotRequested
        })
    }

    removeParticipant = (index: number) => {
        let newParticipants = [...this.state.participants];
        let newParticipantRefs = [...this.state.participantRefs];

        newParticipants.splice(index, 1);
        newParticipantRefs.splice(index, 1);

        this.setState({
            participants: newParticipants,
            participantRefs: newParticipantRefs
        });
    }

    addParticipant = () => {
        let newParticipants = [...this.state.participants];
        let newParticipantRefs = [...this.state.participantRefs];

        const index = newParticipants.length;
        const newRef = React.createRef<InviteParticipant>();

        newParticipants.push(<InviteParticipant removeParticipant={this.removeParticipant}
            invitationUri={this.props.pexaInvitationUri}
            participantIndex={index}
            onChange={this.onChangeInput}
            key={index} ref={newRef} />);
        newParticipantRefs.push(newRef);

        this.setState({
            participants: newParticipants,
            participantRefs: newParticipantRefs
        });
    }

    sentInvitation = () => {
        const { participantRefs } = this.state;
        const { pexaWorkspaceId } = this.props;

        let invitationRequests: CreateWorkspaceInvitationRequestType[] = [];
        for (var i = 0; i < participantRefs.length; i++) {
            const participant = participantRefs[i];

            if (participant.current) {
                let invitationRequest = participant.current.getSubscriberInfo();
                if (invitationRequest) {
                    invitationRequest.workspaceId = pexaWorkspaceId;
                    invitationRequests.push(invitationRequest);
                }
            }
        }

        if (invitationRequests.length > 0) {
            this.props.createInvitation(invitationRequests);
            this.setState({
                dataStatus: ReduxStatus.Requested
            });

            this.closeInviteModal();
        }
    }

    closeInviteModal = () => {
        this.setState({
            showInviteModal: false
        })
    }

    onChangeInput = () => {
        this.setState({
            refresh: !this.state.refresh
        })
    }

}

const mapStateToProps = (state: AppState): IMapStateToProps => {
    return {
        pexaWorkspaceId: state.pexa.createPexaWorkspaceResponse!.workspaceId!,
        pexaInvitationUri: state.pexa.createPexaWorkspaceResponse!.invitationUri!,
        invitationResponseList: state.pexa.invitationResponseList,
        pexaWorkspaceUri: state.pexa.createPexaWorkspaceResponse!.workspaceUri!,
    }
}

const mapDispatchToProps: IMapDispatchToProps = {
    createInvitation
}

export default connect(mapStateToProps, mapDispatchToProps)(WorkspaceCreated);