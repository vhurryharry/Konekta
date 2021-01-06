import * as React from 'react';
import { DefaultButton, PrimaryButton, IconButton, IButtonProps } from 'office-ui-fabric-react/lib/Button';
import { TextField, MaskedTextField } from 'office-ui-fabric-react/lib/TextField';
import { DatePicker, DayOfWeek } from 'office-ui-fabric-react/lib/DatePicker';
import { Dropdown, DropdownMenuItemType, IDropdownStyles, IDropdownOption } from 'office-ui-fabric-react/lib/Dropdown';

const states: IDropdownOption[] = [
    { key: 'General', text: 'General' },
    { key: 'VIC', text: 'VIC' },
    { key: 'QLD', text: 'QLD' },
    { key: 'NSW', text: 'NSW' },
    { key: 'SA', text: 'SA' }
];

const conveyTypes: IDropdownOption[] = [
    { key: 'Vendor', text: 'Vendor' },
    { key: 'Purchaser', text: 'Purchaser' },
];

interface IMapStateToProps { }

interface IMapDispatchToProps { }

interface IProps {
    updateValue;
    index;
    actionstepData;
    matterDetails;
}

type AppProps = IMapStateToProps & IProps & IMapDispatchToProps;

type AppStates = {}

export default class MatterDetails extends React.Component<AppProps, AppStates> {

    constructor(props: any) {
        super(props);
    }

    private _onFormatDate = (date: Date): string => {
        return date.toLocaleString('en-au', { month: 'long' }) + ' ' + date.getDate() + " " + date.getFullYear();
    }

    public render(): JSX.Element {
        var { matterDetails, actionstepData } = this.props;
        var disabled: boolean = actionstepData['matterRef'] != 0 && actionstepData['matterRef'] != null;

        return (
            <div className="modal-body">
                <div className="ms-Grid" dir="ltr">
                    <div className="ms-Grid-row modal-row">
                        <div className="ms-Grid-col ms-sm4 modal-label">Matter Ref#:</div>
                        <div className="ms-Grid-col ms-sm8">
                            <TextField
                                disabled
                                data-cy="matter_ref_input"
                                defaultValue={matterDetails.matterRef.toString()}
                            />
                        </div>
                    </div>

                    <div className="ms-Grid-row modal-row">
                        <div className="ms-Grid-col ms-sm4 modal-label">Matter Name:</div>
                        <div className="ms-Grid-col ms-sm8">
                            <TextField
                                data-cy="matter_name_input"
                                disabled={disabled}
                                value={matterDetails.matter.toString()}
                                onChange={(ev, newText) => this.props.updateValue(newText, 'matter')}
                            />
                        </div>
                    </div>

                    <div className="ms-Grid-row modal-row">
                        <div className="ms-Grid-col ms-sm4 modal-label">Property:</div>
                        <div className="ms-Grid-col ms-sm8">
                            <TextField
                                data-cy="property_input"
                                disabled={disabled}
                                value={matterDetails.property.toString()}
                                onChange={(ev, newText) => this.props.updateValue(newText, 'property')}
                            />
                        </div>
                    </div>

                    <div className="ms-Grid-row modal-row">
                        <div className="ms-Grid-col ms-sm4 modal-label"></div>
                        <div className="ms-Grid-col ms-sm8">
                            <Dropdown
                                placeholder="Select State"
                                data-cy="state_input"
                                label=""
                                options={states}
                                defaultSelectedKey={matterDetails.state}
                                onChange={(ev, newItem) => this.props.updateValue(newItem.text, "state")}
                            />
                        </div>
                    </div>

                    <div className="ms-Grid-row modal-row">
                        <div className="ms-Grid-col ms-sm4 modal-label">Adjustment Date:</div>
                        <div className="ms-Grid-col ms-sm8">
                            <DatePicker
                                data-cy="adjustment_date_input"
                                disabled={disabled}
                                firstDayOfWeek={DayOfWeek.Monday}
                                formatDate={this._onFormatDate}
                                showMonthPickerAsOverlay={true}
                                placeholder="Select an adjustment date..."
                                ariaLabel="Select an adjustment date"
                                value={matterDetails.adjustmentDate}
                                onSelectDate={(newDate) => this.props.updateValue(newDate, 'adjustmentDate')}
                            />
                        </div>
                    </div>

                    <div className="ms-Grid-row modal-row">
                        <div className="ms-Grid-col ms-sm4 modal-label">Settlement Date:</div>
                        <div className="ms-Grid-col ms-sm8">
                            <DatePicker
                                data-cy="settlement_date_input"
                                disabled={disabled}
                                firstDayOfWeek={DayOfWeek.Monday}
                                formatDate={this._onFormatDate}
                                showMonthPickerAsOverlay={true}
                                placeholder="Select a settlement date..."
                                ariaLabel="Select a settlement date"
                                value={matterDetails.settlementDate}
                                onSelectDate={(newDate) => this.props.updateValue(newDate, 'settlementDate')}
                            />
                        </div>
                    </div>

                    <div className="ms-Grid-row modal-row">
                        <div className="ms-Grid-col ms-sm4 modal-label">Settlement Place:</div>
                        <div className="ms-Grid-col ms-sm8">
                            <TextField
                                data-cy="settlement_place_input"
                                value={matterDetails.settlementPlace.toString()}
                                onChange={(ev, newText) => this.props.updateValue(newText, 'settlementPlace')}
                            />
                        </div>
                    </div>

                    <div className="ms-Grid-row modal-row">
                        <div className="ms-Grid-col ms-sm4 modal-label">Settlement Time:</div>
                        <div className="ms-Grid-col ms-sm8">
                            <TextField
                                data-cy="settlement_time_input"
                                value={matterDetails.settlementTime.toString()}
                                onChange={(ev, newText) => this.props.updateValue(newText, 'settlementTime')}
                            />
                        </div>
                    </div>

                    <div className="ms-Grid-row modal-row">
                        <div className="ms-Grid-col ms-sm4 modal-label">Convey Type</div>
                        <div className="ms-Grid-col ms-sm8">
                            <Dropdown
                                placeholder="Select Type"
                                data-cy="type_input"
                                label=""
                                options={conveyTypes}
                                defaultSelectedKey={matterDetails.conveyType}
                                onChange={(ev, newItem) => this.props.updateValue(newItem.text, "conveyType")}
                            />
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}