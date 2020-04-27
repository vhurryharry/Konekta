import * as React from 'react'
import { connect } from 'react-redux'

import { IconButton } from 'office-ui-fabric-react/lib/Button';
import { Checkbox } from 'office-ui-fabric-react/lib/Checkbox';
import { Link } from 'office-ui-fabric-react/lib/Link'

import { AppState } from 'app.types';
import { ModalIDs } from 'containers/calculators/settlement/common';

interface IMapStateToProps {
    state: string;
}

interface IMapDispatchToProps {
}

interface IProps {
    item: any;
    index: number;
    key: string;
    toggleWaterUsage?: () => void;
    showModal: (modalID: ModalIDs, additionalInfo: any) => void;
    isVendor: boolean;
    adjustmentDate?: Date;
}

type AppProps = IMapStateToProps & IProps & IMapDispatchToProps;

type AppStates = {}

export class SettlementInfo extends React.Component<AppProps, AppStates> {

    private getUnitTotal(): (JSX.Element | null) {
        let { item: info, state, isVendor } = this.props;
        let { debit, credit } = info;

        if (state === 'QLD') {
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
        const { item: info, index, state, isVendor, adjustmentDate } = this.props;

        let { result } = info;

        switch (info.type) {
            case 'Contract Price':
                return (
                    <div>
                        <div className="ms-Grid-row state-row">
                            <div className="ms-Grid-col ms-sm2">
                                <IconButton
                                    className="button blue-icon-button"
                                    data-automation-id="save_button"
                                    onClick={() => this.props.showModal(ModalIDs.contractPrice, { index })}
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

                        {state === 'SA' &&
                            <div className="ms-Grid-row sa-link">
                                <div className="ms-Grid-col ms-smPush2">
                                    <Link href="https://www.sa.gov.au/topics/planning-and-property/buying-and-selling/property-transfer-fee-calculator" target="_blank" rel="noopener noreferrer">
                                        Calculate property transfer fees on the sa.gov.au site
                                    </Link>
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
                                    onClick={() => this.props.showModal(ModalIDs.releaseFee, { index })}
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
                                            <div>{info.value["mortgages"].toLocaleString("en-AU", { minimumFractionDigits: 0, maximumFractionDigits: 0 })} @ {info.value["each"].toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</div>
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
                                    onClick={() => this.props.showModal(ModalIDs.waterUsage, { index })}
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
                                            <div>(see calculation following)</div>
                                            <Checkbox label="Show Water Usage" onChange={() => this.props.toggleWaterUsage!()} />
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
                                    onClick={() => this.props.showModal(ModalIDs.penaltyInterest, { index })}
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
                                            <div>from {info.value['from'] === null ? "" : info.value['from'].toDateString()} to {info.value['to'] === null ? "" : info.value['to'].toDateString()} -
                                                &nbsp;{info.value['days']} days @ {info.value['rate']}%
                                            </div>
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
                                    onClick={() => this.props.showModal(ModalIDs.otherAdjustment, { index })}
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
                                        {((info.value['status'] === 'less' && isVendor === false) || (info.value['status'] !== 'less' && isVendor === true)) ?
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
                                    onClick={() => this.props.showModal(ModalIDs.fee, { index })}
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
                                        {((info.value['status'] === 'less' && isVendor === false) || (info.value['status'] !== 'less' && isVendor === true)) ?
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
                                    onClick={() => this.props.showModal(ModalIDs.additionalRequirements, { index })}
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
                                        {((info.value['status'] === 'less' && isVendor === false) || (info.value['status'] !== 'less' && isVendor === true)) ?
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
                                    onClick={() => this.props.showModal(ModalIDs.payeeDetails, { index })}
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
                                    onClick={() => this.props.showModal(ModalIDs.ourRequirements, { index })}
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
                                    onClick={() => this.props.showModal(ModalIDs.councilRates, { index })}
                                    allowDisabledFocus={true}
                                    iconProps={{ iconName: 'ChevronRightSmall' }}
                                    data-cy={"adjustment_info_" + index}
                                />
                            </div>
                            <div className="ms-Grid-col ms-sm10">
                                <div className="ms-Grid" dir="ltr">

                                    <div className="ms-Grid-row detail-row">
                                        <div className="ms-Grid-col ms-sm8">
                                            <b>{info.type === 'Other Adjustment Date' ? info.value['description'] : info.type}</b>
                                            {(state === "VIC" || state === "NSW") ? (
                                                <div>
                                                    For period {info.value['from'] === null ? "" : info.value['from'].toDateString()} to {info.value['to'] === null ? "" : info.value['to'].toDateString()} - {info.value["days"]} days <br />
                                                    ${info.value["amount"]} {(info.value["status"] === "unpaid" ? "Unpaid" : "Paid")} <br />
                                                    {info.value["status"] === "unpaid" ? (<span>Vendor allows {info.value["adjustDays"]} days</span>) :
                                                        (<span>Purchaser allows {info.value["adjustDays"]} days</span>)} <br />
                                                    For period {adjustmentDate!.toDateString()} to {info.value['to'] === null ? "" : info.value['to'].toDateString()}
                                                </div>
                                            ) : (
                                                    <div>
                                                        ${info.value['amount'].toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 })} for the period {info.value['from'] === null ? "" : info.value['from'].toDateString()} to {info.value['to'] === null ? "" : info.value['to'].toDateString()} <br />
                                                        Proportion being {info.value['adjustDays']} / {info.value['days']} days
                                                    </div>
                                                )}
                                        </div>
                                        {((info.value['status'] === 'unpaid' && isVendor === false) || (info.value['status'] !== 'unpaid' && isVendor === true)) ?
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
    }
}

const mapStateToProps = (state: AppState): IMapStateToProps => {
    return {
        state: state.settlementInfo.state
    }
}

export default connect(mapStateToProps)(SettlementInfo);