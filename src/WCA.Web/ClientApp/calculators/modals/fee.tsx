import * as React from 'react';
import { DefaultButton, PrimaryButton, IconButton, IButtonProps } from 'office-ui-fabric-react/lib/Button';
import { TextField, MaskedTextField } from 'office-ui-fabric-react/lib/TextField';
import { ChoiceGroup, IChoiceGroupOption } from 'office-ui-fabric-react/lib/ChoiceGroup';
import { Separator } from 'office-ui-fabric-react/lib/Separator';
import { Checkbox } from 'office-ui-fabric-react/lib/Checkbox';

interface IMapStateToProps { }

interface IMapDispatchToProps { }

interface IProps {
    updatedState;
    updateValue;
    index;
}

type AppProps = IMapStateToProps & IProps & IMapDispatchToProps;

type AppStates = {}

export default class Fee extends React.Component<AppProps, AppStates> {

    constructor(props: any) {
        super(props);
    }

    public render(): JSX.Element {
        const updatedState = this.props.updatedState;

        return (
            <div className="modal-body">
                <div className="ms-Grid" dir="ltr">

                    <div className="ms-Grid-row modal-row">
                        <div className="ms-Grid-col ms-sm2">Description : </div>
                        <div className="ms-Grid-col ms-sm4">
                            <TextField
                                data-cy="description_input"
                                value={updatedState['description']}
                                onChange={(ev, newText) => this.props.updateValue(newText, 'description')}
                            />
                        </div>

                        <div className="ms-Grid-col ms-sm2">Amount : </div>
                        <div className="ms-Grid-col ms-sm4">
                            <TextField
                                data-cy="amount_input"
                                value={updatedState['amount']}
                                onChange={(ev, newText) => this.props.updateValue(newText, 'amount')}
                            />
                        </div>
                    </div>

                    <div className="ms-Grid-row modal-row">

                        <div className="ms-Grid-col ms-sm6">
                            <Separator>
                                <b>Status</b>
                            </Separator>
                            <div>
                                <ChoiceGroup
                                    className="defaultChoiceGroup"
                                    defaultSelectedKey={updatedState['status']}
                                    options={[
                                        {
                                            key: 'plus',
                                            text: 'Plus',
                                        },
                                        {
                                            key: 'less',
                                            text: 'Less'
                                        },
                                    ]}
                                    required={true}
                                    onChange={(ev, item) => this.props.updateValue(item.key, 'status', true)}
                                />

                            </div>
                        </div>

                        <div className="ms-Grid-col ms-sm6">
                            <Checkbox
                                label="Show on Adjustment Statement"
                                className="create-cheque-button"
                                data-cy="show_on_adjustment"
                                checked={updatedState['showOnAdjustment']}
                                onChange={(ev, checked) => this.props.updateValue(checked, 'showOnAdjustment', true)}
                            />
                        </div>
                    </div>

                </div>
            </div>
        )
    }
}