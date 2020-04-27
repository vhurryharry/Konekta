import * as React from 'react';

import { TextField } from 'office-ui-fabric-react/lib/TextField';
import { DatePicker, DayOfWeek } from 'office-ui-fabric-react/lib/DatePicker';
import { Dropdown, IDropdownOption } from 'office-ui-fabric-react/lib/Dropdown';
import { ChoiceGroup } from 'office-ui-fabric-react/lib/ChoiceGroup';
import { Separator } from 'office-ui-fabric-react/lib/Separator';

import { _onFormatDate } from 'utils/dataFormatter'

interface IMapStateToProps { }

interface IMapDispatchToProps { }

interface IProps {
    updatedState: any;
    updateValue: (newValue: any, whichValue: string, needRefresh?: boolean) => void;
}

type AppProps = IMapStateToProps & IProps & IMapDispatchToProps;

type AppStates = {}

export default class WaterUsage extends React.Component<AppProps, AppStates> {

    ctsOptions: IDropdownOption[] = [
        { key: 'do-not-apportion', text: 'Do not Apportion' },
        { key: 'shared-percentage', text: 'Shared Percentage' },
        { key: 'entitlement', text: 'Entitlement' },
    ];

    public render(): JSX.Element {
        const updatedState = this.props.updatedState;
        const ctsOptions = this.ctsOptions;

        return (
            <div className="modal-body">
                <div className="ms-Grid" dir="ltr">
                    <div className="ms-Grid-row modal-row">
                        <div className="ms-Grid-col ms-sm4 modal-label">Method :</div>
                        <div className="ms-Grid-col ms-sm8">
                            <ChoiceGroup
                                className="defaultChoiceGroup"
                                defaultSelectedKey={updatedState['method']}
                                options={[
                                    {
                                        key: 'search-reading',
                                        text: 'Search Reading',
                                    },
                                    {
                                        key: 'daily-average',
                                        text: 'Daily Average'
                                    }
                                ]}
                                required={true}
                                onChange={(ev, item) => this.props.updateValue(item!.key, 'method', true)}
                            />
                        </div>
                    </div>

                    <div className="ms-Grid-row modal-row">
                        <div className="ms-Grid-col ms-sm6">
                            <Separator>
                                <b>Paid To Reading</b>
                            </Separator>

                            <div className="ms-Grid-row modal-row">

                                <div className="ms-Grid-col ms-sm2">Date:</div>
                                <div className="ms-Grid-col ms-sm4">
                                    <DatePicker
                                        data-cy="paid_to_date_input"
                                        firstDayOfWeek={DayOfWeek.Monday}
                                        formatDate={_onFormatDate}
                                        showMonthPickerAsOverlay={true}
                                        placeholder="Select a date..."
                                        ariaLabel="Select a date"
                                        defaultValue={updatedState['paidDate']}
                                        onSelectDate={(newDate) => this.props.updateValue(newDate, 'paidDate')}
                                    />
                                </div>

                                <div className="ms-Grid-col ms-sm2">Reading Amount :</div>
                                <div className="ms-Grid-col ms-sm4">
                                    <TextField
                                        data-cy="paid_reading_amount_input"
                                        defaultValue={updatedState['paidReadingAmount']}
                                        onChange={(ev, newText) => this.props.updateValue(newText, 'paidReadingAmount')}
                                    />
                                </div>
                            </div>
                        </div>

                        <div className="ms-Grid-col ms-sm6">
                            <Separator>
                                <b>Search Reading</b>
                            </Separator>

                            <div className="ms-Grid-row modal-row">

                                <div className="ms-Grid-col ms-sm2">Date:</div>
                                <div className="ms-Grid-col ms-sm4">
                                    <DatePicker
                                        data-cy="search_date_input"
                                        firstDayOfWeek={DayOfWeek.Monday}
                                        formatDate={_onFormatDate}
                                        showMonthPickerAsOverlay={true}
                                        placeholder="Select a date..."
                                        disabled={updatedState['method'] === 'daily-average'}
                                        ariaLabel="Select a date"
                                        defaultValue={updatedState['searchDate']}
                                        onSelectDate={(newDate) => this.props.updateValue(newDate, 'searchDate')}
                                    />
                                </div>

                                <div className="ms-Grid-col ms-sm2">Reading Amount :</div>
                                <div className="ms-Grid-col ms-sm4">
                                    <TextField
                                        data-cy="search_reading_amount_input"
                                        defaultValue={updatedState['searchReadingAmount']}
                                        disabled={updatedState['method'] === 'daily-average'}
                                        onChange={(ev, newText) => this.props.updateValue(newText, 'searchReadingAmount')}
                                    />
                                </div>
                            </div>
                        </div>
                    </div>

                    <Separator>
                        <b>Tier 1 Change</b>
                    </Separator>

                    <div className="ms-Grid-row modal-row">

                        <div className="ms-Grid-col ms-sm1">Charge:</div>
                        <div className="ms-Grid-col ms-sm2">
                            <TextField
                                data-cy="tier1_charge_input"
                                defaultValue={updatedState['tier1Charge']}
                                onChange={(ev, newText) => this.props.updateValue(newText, 'tier1Charge')}
                            />
                        </div>

                        <div className="ms-Grid-col ms-sm1">kl count:</div>
                        <div className="ms-Grid-col ms-sm2">
                            <TextField
                                data-cy="tier1_kl_count_input"
                                defaultValue={updatedState['tier1KlCount']}
                                onChange={(ev, newText) => this.props.updateValue(newText, 'tier1KlCount')}
                            />
                        </div>

                        <div className="ms-Grid-col ms-sm3">fee increase (if app) from 1 July:</div>
                        <div className="ms-Grid-col ms-sm3">
                            <TextField
                                data-cy="tier1_fee_increase_input"
                                defaultValue={updatedState['tier1FeeIncrease']}
                                onChange={(ev, newText) => this.props.updateValue(newText, 'tier1FeeIncrease')}
                            />
                        </div>
                    </div>

                    <Separator>
                        <b>Tier 2 Change</b>
                    </Separator>

                    <div className="ms-Grid-row modal-row">

                        <div className="ms-Grid-col ms-sm1">Charge:</div>
                        <div className="ms-Grid-col ms-sm2">
                            <TextField
                                data-cy="tier2_charge_input"
                                defaultValue={updatedState['tier2Charge']}
                                onChange={(ev, newText) => this.props.updateValue(newText, 'tier2Charge')}
                            />
                        </div>

                        <div className="ms-Grid-col ms-sm1">kl count:</div>
                        <div className="ms-Grid-col ms-sm2">
                            <TextField
                                data-cy="tier2_kl_count_input"
                                defaultValue={updatedState['tier2KlCount']}
                                onChange={(ev, newText) => this.props.updateValue(newText, 'tier2KlCount')}
                            />
                        </div>

                        <div className="ms-Grid-col ms-sm3">fee increase (if app) from 1 July:</div>
                        <div className="ms-Grid-col ms-sm3">
                            <TextField
                                data-cy="tier2_fee_increase_input"
                                defaultValue={updatedState['tier2FeeIncrease']}
                                onChange={(ev, newText) => this.props.updateValue(newText, 'tier2FeeIncrease')}
                            />
                        </div>
                    </div>

                    <div className="ms-Grid-row modal-row">
                        <div className="ms-Grid-col ms-sm6">
                            <Separator>
                                <b>Balance</b>
                            </Separator>

                            <div className="ms-Grid-row modal-row">

                                <div className="ms-Grid-col ms-sm2">Charge:</div>
                                <div className="ms-Grid-col ms-sm4">
                                    <TextField
                                        data-cy="balance_charge_input"
                                        defaultValue={updatedState['balanceCharge']}
                                        onChange={(ev, newText) => this.props.updateValue(newText, 'balanceCharge')}
                                    />
                                </div>

                                <div className="ms-Grid-col ms-sm2">fee increase (if app) from 1 July:</div>
                                <div className="ms-Grid-col ms-sm4">
                                    <TextField
                                        data-cy="balance_fee_increase"
                                        defaultValue={updatedState['balanceFeeIncrease']}
                                        onChange={(ev, newText) => this.props.updateValue(newText, 'balanceFeeIncrease')}
                                    />
                                </div>
                            </div>
                        </div>

                        <div className="ms-Grid-col ms-sm6">
                            <Separator>
                                <b>Bulk Water</b>
                            </Separator>

                            <div className="ms-Grid-row modal-row">

                                <div className="ms-Grid-col ms-sm2">Charge:</div>
                                <div className="ms-Grid-col ms-sm4">
                                    <TextField
                                        data-cy="bulk_charge_input"
                                        defaultValue={updatedState['bulkCharge']}
                                        onChange={(ev, newText) => this.props.updateValue(newText, 'bulkCharge')}
                                    />
                                </div>

                                <div className="ms-Grid-col ms-sm2">fee increase (if app) from 1 July:</div>
                                <div className="ms-Grid-col ms-sm4">
                                    <TextField
                                        data-cy="bulk_fee_increase"
                                        defaultValue={updatedState['bulkFeeIncrease']}
                                        onChange={(ev, newText) => this.props.updateValue(newText, 'bulkFeeIncrease')}
                                    />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div className="ms-Grid-row modal-row">
                        <div className="ms-Grid-col ms-sm6">
                            <Separator>
                                <b>Apportionment</b>
                            </Separator>

                            <div className="ms-Grid-row modal-row">

                                <div className="ms-Grid-col ms-sm2">CTS Option:</div>
                                <div className="ms-Grid-col ms-sm4">
                                    <Dropdown
                                        data-cy="cts_option_select"
                                        id="cts_option_select"
                                        options={ctsOptions}
                                        defaultSelectedKey={updatedState['ctsOption']}
                                        onChange={(ev, item) => this.props.updateValue(item!.key, 'ctsOption', true)}
                                    />
                                </div>

                                {updatedState['ctsOption'] === 'shared-percentage' &&
                                    <div className="ms-Grid-col ms-sm6">
                                        <div className="ms-Grid-col ms-sm4">Percentage:</div>
                                        <div className="ms-Grid-col ms-sm8">
                                            <TextField
                                                data-cy="percentage_input"
                                                defaultValue={updatedState['percentage']}
                                                onChange={(ev, newText) => this.props.updateValue(newText, 'percentage')}
                                            />
                                        </div>
                                    </div>
                                }

                                {updatedState['ctsOption'] === 'entitlement' &&
                                    <div className="ms-Grid-col ms-sm6">
                                        <div className="ms-Grid-col ms-sm4">Value:</div>
                                        <div className="ms-Grid-col ms-sm8">
                                            <TextField
                                                defaultValue={updatedState['entitlementValue']}
                                                onChange={(ev, newText) => this.props.updateValue(newText, 'entitlementValue')}
                                            />
                                        </div>
                                    </div>
                                }

                            </div>
                        </div>

                        <div className="ms-Grid-col ms-sm6">
                            <Separator>
                                <b>Average Daily Amount</b>
                            </Separator>

                            <div className="ms-Grid-row modal-row">

                                <div className="ms-Grid-col ms-sm6">kl count:</div>
                                <div className="ms-Grid-col ms-sm6">
                                    <TextField
                                        defaultValue={updatedState['averageKlCount']}
                                        disabled={updatedState['method'] === 'search-reading'}
                                        onChange={(ev, newText) => this.props.updateValue(newText, 'averageKlCount')}
                                    />
                                </div>

                            </div>
                        </div>
                    </div>

                </div>
            </div>
        )
    }
}