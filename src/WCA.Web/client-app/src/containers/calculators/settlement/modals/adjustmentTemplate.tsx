import * as React from 'react';

import { TextField } from 'office-ui-fabric-react/lib/TextField';
import { DatePicker, DayOfWeek } from 'office-ui-fabric-react/lib/DatePicker';
import { ChoiceGroup } from 'office-ui-fabric-react/lib/ChoiceGroup';
import { Separator } from 'office-ui-fabric-react/lib/Separator';
import { Checkbox } from 'office-ui-fabric-react/lib/Checkbox';

import { _onFormatDate } from 'utils/dataFormatter'

interface IMapStateToProps { }

interface IMapDispatchToProps { }

interface IProps {
    updatedState: any;
    updateValue: (newValue: any, whichValue: string, needRefresh?: boolean) => void;
}

type AppProps = IMapStateToProps & IProps & IMapDispatchToProps;

type AppStates = {}

export default class AdjustmentTemplate extends React.Component<AppProps, AppStates> {

    public render(): JSX.Element {
        const updatedState = this.props.updatedState;

        return (
            <div className="modal-body">
                <div className="ms-Grid" dir="ltr">

                    <div className="ms-Grid-row modal-row">
                        <div className="ms-Grid-col ms-sm2">Amount : </div>
                        <div className="ms-Grid-col ms-sm4">
                            <TextField
                                data-cy="amount_input"
                                defaultValue={updatedState['amount']}
                                onChange={(ev, newText) => this.props.updateValue(newText, 'amount')}
                            />
                        </div>
                        <div className="ms-Grid-col ms-sm6"></div>
                    </div>

                    <div className="ms-Grid-row modal-row">
                        <div className="ms-Grid-col ms-sm2">Period : </div>
                        <div className="ms-Grid-col ms-sm4">
                            <DatePicker
                                data-cy="from_date_select"
                                firstDayOfWeek={DayOfWeek.Monday}
                                formatDate={_onFormatDate}
                                showMonthPickerAsOverlay={true}
                                defaultValue={updatedState['from']}
                                placeholder="Select a date..."
                                onSelectDate={(newDate) => this.props.updateValue(newDate, 'from')}
                            />
                        </div>

                        <div className="ms-Grid-col ms-sm2">To : </div>
                        <div className="ms-Grid-col ms-sm4">
                            <DatePicker
                                data-cy="to_date_select"
                                firstDayOfWeek={DayOfWeek.Monday}
                                formatDate={_onFormatDate}
                                showMonthPickerAsOverlay={true}
                                defaultValue={updatedState['to']}
                                placeholder="Select a date..."
                                onSelectDate={(newDate) => this.props.updateValue(newDate, 'to')}
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
                                        key: 'paid',
                                        text: 'Paid',
                                    },
                                    {
                                        key: 'unpaid',
                                        text: 'Unpaid'
                                    },
                                    {
                                        key: 'adjust-as-paid',
                                        text: 'Adjust as paid'
                                    }
                                ]}
                                data-cy="status_select"
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