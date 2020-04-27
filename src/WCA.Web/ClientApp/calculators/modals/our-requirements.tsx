import * as React from 'react';
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

export default class OurRequirements extends React.Component<AppProps, AppStates> {

    constructor(props: any) {
        super(props);
    }

    public render(): JSX.Element {
        const updatedState = this.props.updatedState;

        return (
            <div className="modal-body">
                <div className="ms-Grid" dir="ltr">

                    <div className="ms-Grid-row modal-row">
                        <div className="ms-Grid-col ms-sm4">Detail : </div>
                        <div className="ms-Grid-col ms-sm8">
                            <TextField
                                data-cy="detail_input"
                                value={updatedState['detail']}
                                onChange={(ev, newText) => this.props.updateValue(newText, 'detail')}
                            />
                        </div>
                    </div>

                </div>
            </div>
        )
    }
}