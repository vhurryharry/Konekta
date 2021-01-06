import * as React from 'react';

import { TextField } from 'office-ui-fabric-react/lib/TextField';
import { DatePicker, DayOfWeek } from 'office-ui-fabric-react/lib/DatePicker';

import { _onFormatDate } from 'utils/dataFormatter'

interface IMapStateToProps { }

interface IMapDispatchToProps { }

interface IProps {
    updatedState: any;
    updateValue: (newValue: any, whichValue: string, needRefresh?: boolean) => void;
}

type AppProps = IMapStateToProps & IProps & IMapDispatchToProps;

type AppStates = {}

export default class PenaltyInterest extends React.Component<AppProps, AppStates> {

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
                                defaultValue={updatedState['rate']}
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
                                formatDate={_onFormatDate}
                                placeholder="Select a date..."
                                showMonthPickerAsOverlay={true}
                                defaultValue={updatedState['from']}
                                onSelectDate={(newDate) => this.props.updateValue(newDate, 'from')}
                            />
                        </div>

                        <div className="ms-Grid-col ms-sm2">To Date : </div>
                        <div className="ms-Grid-col ms-sm4">
                            <DatePicker
                                data-cy="to_date_select"
                                firstDayOfWeek={DayOfWeek.Monday}
                                formatDate={_onFormatDate}
                                showMonthPickerAsOverlay={true}
                                placeholder="Select a date..."
                                defaultValue={updatedState['to']}
                                onSelectDate={(newDate) => this.props.updateValue(newDate, 'to')}
                            />
                        </div>
                    </div>

                </div>
            </div>
        )
    }
}