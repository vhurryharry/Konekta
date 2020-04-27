import * as React from 'react';

import { TextField } from 'office-ui-fabric-react/lib/TextField';

interface IMapStateToProps { }

interface IMapDispatchToProps { }

interface IProps {
    updatedState: any;
    updateValue: (newValue: any, whichValue: string, needRefresh?: boolean) => void;
}

type AppProps = IMapStateToProps & IProps & IMapDispatchToProps;

type AppStates = {}

export default class ContractPrice extends React.Component<AppProps, AppStates> {

    public render(): JSX.Element {
        const updatedState = this.props.updatedState;

        return (
            <div className="modal-body">
                <div className="ms-Grid" dir="ltr">
                    <div className="ms-Grid-row modal-row">
                        <div className="ms-Grid-col ms-sm4 modal-label">Purchase Price : </div>
                        <div className="ms-Grid-col ms-sm8">
                            <TextField
                                defaultValue={updatedState['price']}
                                data-cy="contract_price_input"
                                onChange={(ev, newText) => this.props.updateValue(newText, 'price')}
                            />
                        </div>
                    </div>

                    <div className="ms-Grid-row modal-row">
                        <div className="ms-Grid-col ms-sm4 modal-label">Deposit : </div>
                        <div className="ms-Grid-col ms-sm8">
                            <TextField
                                defaultValue={updatedState['deposit']}
                                data-cy="deposit_input"
                                onChange={(ev, newText) => this.props.updateValue(newText, 'deposit')}
                            />
                        </div>
                    </div>

                </div>
            </div>
        )
    }
}