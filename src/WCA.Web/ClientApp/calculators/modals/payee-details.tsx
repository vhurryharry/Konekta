import * as React from 'react';
import { DefaultButton, PrimaryButton, IconButton, IButtonProps } from 'office-ui-fabric-react/lib/Button';
import { TextField, MaskedTextField } from 'office-ui-fabric-react/lib/TextField';
import { Checkbox } from 'office-ui-fabric-react/lib/Checkbox';

interface IMapStateToProps { }

interface IMapDispatchToProps { }

interface IProps {
    updatedState;
    updateValue;
    index;
    balanceFunds;
}

type AppProps = IMapStateToProps & IProps & IMapDispatchToProps;

type AppStates = {}

export default class PayeeDetails extends React.Component<AppProps, AppStates> {

    constructor(props: any) {
        super(props);
    }

    state = {
        changed: false
    }

    public onBalanceFundChange = (ev: React.FormEvent<HTMLElement>, checked: boolean): void => {

        this.setState({
            changed: checked
        });

        if (checked == true) {
            this.props.updateValue(this.props.balanceFunds, 'amount');
        } else {
            this.props.updateValue('0', 'amount');
        }
    }

    public render(): JSX.Element {
        const { index, updatedState } = this.props;

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
                                value={updatedState['amount'].toString()}
                                onChange={(ev, newText) => this.props.updateValue(newText, 'amount')}
                            />
                        </div>
                    </div>

                    <Checkbox label="Balance Fund" onChange={this.onBalanceFundChange} />

                </div>
            </div>
        )
    }
}