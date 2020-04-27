import * as React from 'react';
import { TextField, MaskedTextField } from 'office-ui-fabric-react/lib/TextField';
import { DatePicker, DayOfWeek } from 'office-ui-fabric-react/lib/DatePicker';

interface IMapStateToProps { }

interface IMapDispatchToProps { }

interface IProps {
    updatedState;
    updateValue;
    index;
}

type AppProps = IMapStateToProps & IProps & IMapDispatchToProps;

type AppStates = {}

export default class PenaltyInterest extends React.Component<AppProps, AppStates> {

    constructor(props: any) {
        super(props);
    }

    private _onFormatDate = (date: Date): string => {
        return date.toLocaleString('en-au', { month: 'long' }) + ' ' + date.getDate() + " " + date.getFullYear();
    }

    public render(): JSX.Element {
        const updatedState = this.props.updatedState;

        return (
            <div className="modal-body">
                <div className="ms-Grid" dir="ltr">

                    <div className="ms-Grid-row modal-row">
                        <div className="ms-Grid-col ms-sm2">Rate(%) : </div>
                        <div className="ms-Grid-col ms-sm4">
                            <TextField
                                data-cy="rate_input"
                                value={updatedState['rate']}
                                onChange={(ev, newText) => this.props.updateValue(newText, 'rate')}
                            />
                        </div>
                        <div className="ms-Grid-col ms-sm6"></div>
                    </div>

                    <div className="ms-Grid-row modal-row">
                        <div className="ms-Grid-col ms-sm2">From Date : </div>
                        <div className="ms-Grid-col ms-sm4">
                            <DatePicker
                                data-cy="from_date_select"
                                firstDayOfWeek={DayOfWeek.Monday}
                                formatDate={this._onFormatDate}
                                placeholder="Select a date..."
                                showMonthPickerAsOverlay={true}
                                value={updatedState['from']}
                                onSelectDate={(newDate) => this.props.updateValue(newDate, 'from')}
                            />
                        </div>

                        <div className="ms-Grid-col ms-sm2">To Date : </div>
                        <div className="ms-Grid-col ms-sm4">
                            <DatePicker
                                data-cy="to_date_select"
                                firstDayOfWeek={DayOfWeek.Monday}
                                formatDate={this._onFormatDate}
                                showMonthPickerAsOverlay={true}
                                placeholder="Select a date..."
                                value={updatedState['to']}
                                onSelectDate={(newDate) => this.props.updateValue(newDate, 'to')}
                            />
                        </div>
                    </div>

                </div>
            </div>
        )
    }
}