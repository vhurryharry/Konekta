import * as React from 'react';
import { DefaultButton, PrimaryButton, IconButton, IButtonProps } from 'office-ui-fabric-react/lib/Button';
import { TextField, MaskedTextField } from 'office-ui-fabric-react/lib/TextField';

interface IMapStateToProps { }

interface IMapDispatchToProps { }

interface IProps {
    updatedState;
    updateValue;
    index;
}

type AppProps = IMapStateToProps & IProps & IMapDispatchToProps;

type AppStates = {}

export default class ReleaseFee extends React.Component<AppProps, AppStates> {

    constructor(props: any) {
        super(props);
    }

    public render(): JSX.Element {
        const updatedState = this.props.updatedState;

        return (
            <div className="modal-body">
                <div className="ms-Grid" dir="ltr">
                    <div className="ms-Grid-row modal-row">
                        <div className="ms-Grid-col ms-sm4 modal-label">Mortgages : </div>
                        <div className="ms-Grid-col ms-sm8">
                            <TextField
                                data-cy="release_mortgages_input"
                                value={updatedState['mortgages']}
                                onChange={(ev, newText) => this.props.updateValue(newText, 'mortgages')}
                            />
                        </div>
                    </div>

                    <div className="ms-Grid-row modal-row">
                        <div className="ms-Grid-col ms-sm4 modal-label">@ each : </div>
                        <div className="ms-Grid-col ms-sm8">
                            <TextField
                                data-cy="release_each_input"
                                value={updatedState['each']}
                                onChange={(ev, newText) => this.props.updateValue(newText, 'each')}
                            />
                        </div>
                    </div>

                </div>
            </div>

        )
    }
}