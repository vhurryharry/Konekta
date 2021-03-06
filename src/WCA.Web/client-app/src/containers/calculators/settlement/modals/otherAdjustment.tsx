import * as React from 'react';

import { TextField } from 'office-ui-fabric-react/lib/TextField';
import { ChoiceGroup } from 'office-ui-fabric-react/lib/ChoiceGroup';
import { Separator } from 'office-ui-fabric-react/lib/Separator';
import { Checkbox } from 'office-ui-fabric-react/lib/Checkbox';

interface IMapStateToProps { }

interface IMapDispatchToProps { }

interface IProps {
    updatedState: any;
    updateValue: (newValue: any, whichValue: string, needRefresh?: boolean) => void;
}

type AppProps = IMapStateToProps & IProps & IMapDispatchToProps;

type AppStates = {}

export default class OtherAdjustment extends React.Component<AppProps, AppStates> {

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
                                defaultValue={updatedState['description']}
                                onChange={(ev, newText) => this.props.updateValue(newText, 'description')}
                            />
                        </div>

                        <div className="ms-Grid-col ms-sm2">Amount : </div>
                        <div className="ms-Grid-col ms-sm4">
                            <TextField
                                data-cy="amount_input"
                                defaultValue={updatedState['amount']}
                                onChange={(ev, newText) => this.props.updateValue(newText, 'amount')}
                            />
                        </div>
                    </div>

                    <Separator>
                        <b>Status</b>
                    </Separator>

                    <div className="ms-Grid-row modal-row">

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
                                onChange={(ev, item) => this.props.updateValue(item!.key, 'status', true)}
                            />

                            {updatedState['status'] === 'adjust-as-paid' &&
                                <Checkbox label="Create Cheque" className="create-cheque-button" />
                            }
                        </div>
                    </div>

                </div>
            </div>
        )
    }
}