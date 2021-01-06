import * as React from 'react'
import { DefaultButton, PrimaryButton, IconButton, IButtonProps } from 'office-ui-fabric-react/lib/Button';
import { Checkbox } from 'office-ui-fabric-react/lib/Checkbox';

import { connect } from 'react-redux'

interface IMapStateToProps {
    state: string;
}

interface IMapDispatchToProps {
}

interface IProps {
    info;
    index;
    modalIDs;
    key;
    toggleWaterUsage;
    showModal;
    toggleActionstepValue;
    isVendor;
    adjustmentDate?: Date;
}

type AppProps = IMapStateToProps & IProps & IMapDispatchToProps;

type AppStates = {}

export class SettlementInfo extends React.Component<AppProps, AppStates> {
    constructor(props: any) {
        super(props);
    }

    private getUnitTotal(): JSX.Element {
        var { info, state, isVendor } = this.props;
        var { debit, credit } = info;
        if (state == 'QLD') {
            return (
                <div>
                    <div className="ms-Grid-col ms-sm12 separator">
                        --------------
                    </div>

                    <div className="ms-Grid-row">
                        <div className="ms-Grid-col ms-smPush2 ms-sm10">
                            <div className="ms-Grid-row detail-row">
                                <div className="ms-Grid-col ms-sm2 ms-smPush8 price-info">
                                    <b>{isVendor ? credit.toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 }) : debit.toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</b>
                                </div>
                                <div className="ms-Grid-col ms-sm2 ms-smPush8 price-info">
                                    <b>{isVendor ? debit.toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 }) : credit.toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</b>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            )
        }

        return null;
    }

    public render(): JSX.Element {
        const { info, index, modalIDs, state, isVendor, adjustmentDate } = this.props;

        var { result } = info;

        switch (info.type) {
            case 'Contract Price':
                return (
                    <div>
                        <div className="ms-Grid-row state-row">
                            <div className="ms-Grid-col ms-sm2">
                                <IconButton
                                    className="button blue-icon-button"
                                    data-automation-id="save_button"
                                    onClick={() => this.props.showModal(modalIDs.contractPrice, { index })}
                                    allowDisabledFocus={true}
                                    iconProps={{ iconName: 'ChevronRightSmall' }}
                                    data-cy={"adjustment_info_" + index}
                                />
                            </div>
                            <div className="ms-Grid-col ms-sm10">
                                <div className="ms-Grid" dir="ltr">

                                    <div className="ms-Grid-row detail-row">
                                        <div className="ms-Grid-col ms-sm8">
                                            <b>Contract Price</b>
                                        </div>
                                        <div className={"ms-Grid-col ms-sm2 price-info " + (isVendor ? "ms-smPush2" : "")} data-cy={"adjustment_result_" + index}>
                                            {info.value['price'].toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                        {info.value['deposit'] > 0 &&
                            <div className="ms-Grid-row">
                                <div className="ms-Grid-col ms-smPush2 ms-sm10">
                                    <div className="ms-Grid-row detail-row">
                                        <div className="ms-Grid-col ms-sm8">
                                            <b>Less Deposit</b>
                                        </div>
                                        <div className={"ms-Grid-col ms-sm2 price-info " + (!isVendor ? "ms-smPush2" : "")}>
                                            {info.value['deposit'].toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}
                                        </div>
                                    </div>
                                </div>
                            </div>
                        }

                        {this.getUnitTotal()}

                        {state == 'SA' &&
                            <div className="ms-Grid-row sa-link">
                                <div className="ms-Grid-col ms-smPush2">
                                    <a href="https://www.sa.gov.au/topics/planning-and-property/buying-and-selling/property-transfer-fee-calculator" target="_blank">
                                        Calculate property transfer fees on the sa.gov.au site
                                    </a>
                                </div>
                            </div>
                        }

                    </div>
                );

            case 'Release Fee':
                return (
                    <div>
                        <div className="ms-Grid-row state-row">
                            <div className="ms-Grid-col ms-sm2">
                                <IconButton
                                    className="button blue-icon-button"
                                    data-automation-id="save_button"
                                    onClick={() => this.props.showModal(modalIDs.releaseFee, { index })}
                                    allowDisabledFocus={true}
                                    iconProps={{ iconName: 'ChevronRightSmall' }}
                                    data-cy={"adjustment_info_" + index}
                                />
                            </div>
                            <div className="ms-Grid-col ms-sm10">
                                <div className="ms-Grid" dir="ltr">

                                    <div className="ms-Grid-row detail-row">
                                        <div className="ms-Grid-col ms-sm8">
                                            <b>Seller's Release Fee</b>
                                            <p>{info.value["mortgages"].toLocaleString("en-AU", { minimumFractionDigits: 0, maximumFractionDigits: 0 })} @ {info.value["each"].toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</p>
                                        </div>

                                        <div className={"ms-Grid-col ms-sm2 price-info " + (!isVendor ? "ms-smPush2" : "")} data-cy={"adjustment_result_" + index}>
                                            {result.toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>

                        {this.getUnitTotal()}

                    </div>
                );

            case 'Water Usage':
                return (
                    <div>
                        <div className="ms-Grid-row state-row">
                            <div className="ms-Grid-col ms-sm2">
                                <IconButton
                                    className="button blue-icon-button"
                                    data-automation-id="save_button"
                                    onClick={() => this.props.showModal(modalIDs.waterUsage, { index })}
                                    allowDisabledFocus={true}
                                    iconProps={{ iconName: 'ChevronRightSmall' }}
                                    data-cy={"adjustment_info_" + index}
                                />
                            </div>
                            <div className="ms-Grid-col ms-sm10">
                                <div className="ms-Grid" dir="ltr">

                                    <div className="ms-Grid-row detail-row">
                                        <div className="ms-Grid-col ms-sm8">
                                            <b>Water Usage</b>
                                            <p>(see calculation following)</p>
                                            <Checkbox label="Show Water Usage" onChange={() => this.props.toggleWaterUsage()} />
                                        </div>
                                        <div className={"ms-Grid-col ms-sm2 price-info " + (!isVendor ? "ms-smPush2" : "")} data-cy={"adjustment_result_" + index}>
                                            {result.toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>

                        {this.getUnitTotal()}
                    </div>
                );

            case 'Penalty Interest':
                return (
                    <div>
                        <div className="ms-Grid-row state-row">
                            <div className="ms-Grid-col ms-sm2">
                                <IconButton
                                    className="button blue-icon-button"
                                    data-automation-id="save_button"
                                    onClick={() => this.props.showModal(modalIDs.penaltyInterest, { index })}
                                    allowDisabledFocus={true}
                                    iconProps={{ iconName: 'ChevronRightSmall' }}
                                    data-cy={"adjustment_info_" + index}
                                />
                            </div>
                            <div className="ms-Grid-col ms-sm10">
                                <div className="ms-Grid" dir="ltr">

                                    <div className="ms-Grid-row detail-row">
                                        <div className="ms-Grid-col ms-sm8">
                                            <b>Penalty Interest</b>
                                            <p>from {info.value['from'] == null ? "" : info.value['from'].toDateString()} to {info.value['to'] == null ? "" : info.value['to'].toDateString()} -
                                                &nbsp;{info.value['days']} days @ {info.value['rate']}%
                                            </p>
                                        </div>
                                        <div className={"ms-Grid-col ms-sm2 price-info " + (isVendor ? "ms-smPush2" : "")} data-cy={"adjustment_result_" + index}>
                                            {result.toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>

                        {this.getUnitTotal()}

                    </div>
                );

            case 'Other Adjustment':
                return (
                    <div>
                        <div className="ms-Grid-row state-row">
                            <div className="ms-Grid-col ms-sm2">
                                <IconButton
                                    className="button blue-icon-button"
                                    data-automation-id="save_button"
                                    onClick={() => this.props.showModal(modalIDs.otherAdjustment, { index })}
                                    allowDisabledFocus={true}
                                    iconProps={{ iconName: 'ChevronRightSmall' }}
                                    data-cy={"adjustment_info_" + index}
                                />
                            </div>
                            <div className="ms-Grid-col ms-sm10">
                                <div className="ms-Grid" dir="ltr">

                                    <div className="ms-Grid-row detail-row">
                                        <div className="ms-Grid-col ms-sm8">
                                            <b>{info.value['description']}</b>
                                        </div>
                                        {((info.value['status'] == 'less' && isVendor == false) || (info.value['status'] != 'less' && isVendor == true)) ?
                                            <div className="ms-Grid-col ms-sm2 ms-smPush2 price-info" data-cy={"adjustment_result_" + index}>
                                                {info.value['amount'].toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}
                                            </div>
                                            :
                                            <div className="ms-Grid-col ms-sm2 price-info" data-cy={"adjustment_result_" + index}>
                                                {info.value['amount'].toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}
                                            </div>
                                        }
                                    </div>

                                </div>
                            </div>
                        </div>

                        {this.getUnitTotal()}

                    </div>
                );

            case 'Fee':
                return (
                    <div>
                        <div className="ms-Grid-row state-row">
                            <div className="ms-Grid-col ms-sm2">
                                <IconButton
                                    className="button blue-icon-button"
                                    data-automation-id="save_button"
                                    onClick={() => this.props.showModal(modalIDs.fee, { index })}
                                    allowDisabledFocus={true}
                                    iconProps={{ iconName: 'ChevronRightSmall' }}
                                    data-cy={"fee_info_" + index}
                                />
                            </div>
                            <div className="ms-Grid-col ms-sm10">
                                <div className="ms-Grid" dir="ltr">

                                    <div className="ms-Grid-row detail-row">
                                        <div className="ms-Grid-col ms-sm8">
                                            <b>{info.value['description']}</b>
                                        </div>
                                        {((info.value['status'] == 'less' && isVendor == false) || (info.value['status'] != 'less' && isVendor == true)) ?
                                            <div className="ms-Grid-col ms-sm2 ms-smPush2 price-info">
                                                {info.value['amount'].toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}
                                            </div>
                                            :
                                            <div className="ms-Grid-col ms-sm2 price-info">
                                                {info.value['amount'].toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}
                                            </div>
                                        }
                                    </div>

                                </div>
                            </div>
                        </div>

                        {this.getUnitTotal()}

                    </div>
                );

            case 'Additional Requirements':
                return (
                    <div>
                        <div className="ms-Grid-row state-row">
                            <div className="ms-Grid-col ms-sm2">
                                <IconButton
                                    className="button blue-icon-button"
                                    data-automation-id="save_button"
                                    onClick={() => this.props.showModal(modalIDs.additionalRequirements, { index })}
                                    allowDisabledFocus={true}
                                    iconProps={{ iconName: 'ChevronRightSmall' }}
                                    data-cy={"adjustment_info_" + index}
                                />
                            </div>
                            <div className="ms-Grid-col ms-sm10">
                                <div className="ms-Grid" dir="ltr">

                                    <div className="ms-Grid-row detail-row">
                                        <div className="ms-Grid-col ms-sm8">
                                            <b>{info.value['description']}</b>
                                        </div>
                                        {((info.value['status'] == 'less' && isVendor == false) || (info.value['status'] != 'less' && isVendor == true)) ?
                                            <div className="ms-Grid-col ms-sm2 ms-smPush2 price-info">
                                                {info.value['amount'].toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}
                                            </div>
                                            :
                                            <div className="ms-Grid-col ms-sm2 price-info">
                                                {info.value['amount'].toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}
                                            </div>
                                        }
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                );

            case 'Cheque Payee':

                return (
                    <div>
                        <div className="ms-Grid-row state-row">
                            <div className="ms-Grid-col ms-sm2">
                                <IconButton
                                    className="button blue-icon-button"
                                    data-automation-id="save_button"
                                    onClick={() => this.props.showModal(modalIDs.payeeDetails, { index })}
                                    allowDisabledFocus={true}
                                    iconProps={{ iconName: 'ChevronRightSmall' }}
                                    data-cy={"adjustment_info_" + index}
                                />
                            </div>
                            <div className="ms-Grid-col ms-sm10">
                                <div className="ms-Grid" dir="ltr">

                                    <div className="ms-Grid-row detail-row">
                                        <div className="ms-Grid-col ms-sm8">
                                            <b>{index + 1}. {info.value['description']}</b>
                                        </div>
                                        <div className={"ms-Grid-col ms-sm2 price-info " + (!isVendor ? "ms-smPush2" : "")}>
                                            {info.value['amount'].toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                );

            case 'Our Requirements':
                return (
                    <div>
                        <div className="ms-Grid-row state-row">
                            <div className="ms-Grid-col ms-sm2">
                                <IconButton
                                    className="button blue-icon-button"
                                    data-automation-id="save_button"
                                    onClick={() => this.props.showModal(modalIDs.ourRequirements, { index })}
                                    allowDisabledFocus={true}
                                    iconProps={{ iconName: 'ChevronRightSmall' }}
                                    data-cy={"adjustment_info_" + index}
                                />
                            </div>
                            <div className="ms-Grid-col ms-sm10">
                                <div className="ms-Grid" dir="ltr">

                                    <div className="ms-Grid-row detail-row">
                                        <div className="ms-Grid-col ms-sm12">
                                            <b>{info.value['detail']}</b>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                );

            default:
                return (
                    <div>
                        <div className="ms-Grid-row state-row">
                            <div className="ms-Grid-col ms-sm2">
                                <IconButton
                                    className="button blue-icon-button"
                                    data-automation-id="save_button"
                                    onClick={() => this.props.showModal(modalIDs.councilRates, { index })}
                                    allowDisabledFocus={true}
                                    iconProps={{ iconName: 'ChevronRightSmall' }}
                                    data-cy={"adjustment_info_" + index}
                                />
                            </div>
                            <div className="ms-Grid-col ms-sm10">
                                <div className="ms-Grid" dir="ltr">

                                    <div className="ms-Grid-row detail-row">
                                        <div className="ms-Grid-col ms-sm8">
                                            <b>{info.type == 'Other Adjustment Date' ? info.value['description'] : info.type}</b>
                                            {(state == "VIC" || state == "NSW") ? (
                                                <p>
                                                    For period {info.value['from'] == null ? "" : info.value['from'].toDateString()} to {info.value['to'] == null ? "" : info.value['to'].toDateString()} - {info.value["days"]} days <br />
                                                    ${info.value["amount"]} {(info.value["status"] == "unpaid" ? "Unpaid" : "Paid")} <br />
                                                    {info.value["status"] == "unpaid" ? (<span>Vendor allows {info.value["adjustDays"]} days</span>) :
                                                        (<span>Purchaser allows {info.value["adjustDays"]} days</span>)} <br />
                                                    For period {adjustmentDate.toDateString()} to {info.value['to'] == null ? "" : info.value['to'].toDateString()}
                                                </p>
                                            ) : (
                                                    <p>
                                                        ${info.value['amount'].toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 })} for the period {info.value['from'] == null ? "" : info.value['from'].toDateString()} to {info.value['to'] == null ? "" : info.value['to'].toDateString()} <br />
                                                        Proportion being {info.value['adjustDays']} / {info.value['days']} days
                                                </p>
                                                )}
                                        </div>
                                        {((info.value['status'] == 'unpaid' && isVendor == false) || (info.value['status'] != 'unpaid' && isVendor == true)) ?
                                            <div className="ms-Grid-col ms-sm2 ms-smPush2 price-info" data-cy={"adjustment_result_" + index}>
                                                {result.toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}
                                            </div>
                                            :
                                            <div className="ms-Grid-col ms-sm2 price-info" data-cy={"adjustment_result_" + index}>
                                                {result.toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}
                                            </div>
                                        }
                                    </div>

                                </div>
                            </div>
                        </div>

                        {this.getUnitTotal()}

                    </div>
                );
        }

        return (
            <div className="state-row">
            </div>
        )
    }
}

const mapStateToProps = state => {
    return {
        state: state.settlementInfo.state
    }
}

const mapDispatchToProps = dispatch => {
    return {
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(SettlementInfo);