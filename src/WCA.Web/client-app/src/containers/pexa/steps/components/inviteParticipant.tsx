import * as React from 'react'

import { Label } from 'office-ui-fabric-react/lib/Label'
import { TextField } from 'office-ui-fabric-react/lib/TextField'
import { Dropdown } from 'office-ui-fabric-react/lib/Dropdown'
import { Link } from 'office-ui-fabric-react/lib/Link'

import { SubscriberRoles } from 'containers/pexa/steps/constants'
import { DefaultButton } from 'office-ui-fabric-react'
import SubscriberSearchBox from 'containers/pexa/steps/components/subscriberSearchBox'
import { CreateWorkspaceInvitationRequestType, ParticipantDetailsDetailType } from 'utils/wcaApiTypes'

type AppProps = {
    removeParticipant: (index: number) => void;
    onChange: () => void;
    participantIndex: number;
    invitationUri: string;
}

type AppStates = {
    subscriberId: string | null;
    role: string;
    address: string;
    notes: string;
}

export class InviteParticipant extends React.Component<AppProps, AppStates> {
    constructor(props: AppProps) {
        super(props);

        this.state = {
            subscriberId: null,
            role: "",
            address: "",
            notes: ""
        }
    }

    changeInfo = async (newValue: string | number, key: string) => {
        await this.setState({
            [key]: newValue
        } as Pick<AppStates, keyof AppStates>);

        this.props.onChange();
    }

    getSubscriberInfo = (): CreateWorkspaceInvitationRequestType | null => {
        const { subscriberId, role, notes } = this.state;
        if (subscriberId) {
            return new CreateWorkspaceInvitationRequestType({
                participantDetails: new ParticipantDetailsDetailType({
                    subscriberId: subscriberId
                }),
                subscriberId: "",
                participantRole: role,
                notes: notes
            })
        }

        return null;
    }

    validate = (): boolean => {
        const { role, subscriberId } = this.state;
        if (subscriberId === null || role === "")
            return false;
        return true;
    }

    render(): JSX.Element {
        const { removeParticipant, participantIndex, invitationUri } = this.props;
        const { role, address, notes } = this.state;
        const isValid = this.validate();

        return (
            <div className={`invite-participant-item ms-Grid-row ${isValid ? "" : "error-box"}`}>
                {participantIndex > 0 &&
                    <DefaultButton
                        className="button remove-button"
                        data-automation-id="remove_participant"
                        data-cy="remove_participant"
                        text="Remove"
                        onClick={() => removeParticipant(participantIndex)}
                        allowDisabledFocus={true}
                    />
                }
                <div className="select-participant invite-participant-field ms-Grid-col ms-sm12 ms-Grid-row">
                    <Label htmlFor='participant-name' className="ms-sm12 ms-Grid-col">
                        Participant
                    </Label>
                    <div className="ms-Grid-col ms-sm6">
                        <SubscriberSearchBox searchBoxId={participantIndex.toString()}
                            onChangeSubscriber={(newSubscriber) => this.changeInfo(newSubscriber, "subscriberId")}
                        />
                    </div>

                    <Label className="ms-sm6 ms-Grid-col">
                        <i>Limit of 1 selection (Please type at least 3 characters ...)</i>
                    </Label>
                    <Label className="ms-sm12 ms-Grid-col">
                        If you're not able to find a participant <Link href={invitationUri} target="_blank">click here</Link> to help them join PEXA.
                    </Label>
                </div>
                <div className="invite-participant-field ms-Grid-col ms-sm12 ms-Grid-row">
                    <Label htmlFor="participant-role" className="ms-sm12 ms-Grid-col">
                        Role
                    </Label>
                    <Dropdown id='participant-role'
                        options={SubscriberRoles}
                        className="ms-sm6 ms-Grid-col"
                        placeholder="Please select"
                        selectedKey={role}
                        errorMessage={role === "" ? " " : ""}
                        onChange={(event, item) => this.changeInfo(item!.key.toString(), "role")}
                    />
                </div>
                <div className="invite-participant-field ms-Grid-col ms-sm12 ms-Grid-row">
                    <Label htmlFor="participant-address" className="ms-sm12 ms-Grid-col">
                        Property Address <i>optional</i>
                    </Label>
                    <TextField id='participant-address'
                        className="ms-sm6 ms-Grid-col"
                        value={address}
                        onChange={(event, newValue) => this.changeInfo(newValue!, "address")}
                    />
                </div>
                <div className="invite-participant-field ms-Grid-col ms-sm12 ms-Grid-row">
                    <Label htmlFor="participant-notes" className="ms-sm12 ms-Grid-col">
                        Notes <i>optional</i>
                    </Label>
                    <TextField id='participant-notes'
                        multiline={true}
                        className="ms-sm6 ms-Grid-col"
                        value={notes}
                        onChange={(event, newValue) => this.changeInfo(newValue!, "notes")}
                    />
                </div>
            </div>
        )
    }
}