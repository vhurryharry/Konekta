import * as React from 'react'

interface IMapStateToProps { }

interface IMapDispatchToProps { }

interface IProps {
    waterUsage;
}

type AppProps = IMapStateToProps & IProps & IMapDispatchToProps;

type AppStates = {}

export default class WaterUsageSection extends React.Component<AppProps, AppStates> {
    constructor(props: any) {
        super(props);
    }

    public render(): JSX.Element {
        const { waterUsage } = this.props;

        var adjustmentFlag: boolean = (waterUsage["tier1FeeIncrease"] != 0 || waterUsage["tier2FeeIncrease"] != 0 ||
            waterUsage["balanceFeeIncrease"] != 0 || waterUsage["bulkFeeIncrease"] != 0);

        return (
            <div className="section">

                <div className="section">
                    <div className="left-align-section-header section-body">
                        <b>WATER USAGE CALCULATION</b>
                    </div>

                    <div className="ms-Grid-row">
                        <div className="ms-Grid-col ms-sm8">
                            Date Water Paid To: {waterUsage["paidDate"].toDateString()}
                        </div>
                        <div className="ms-Grid-col ms-sm4">
                            Reading: {waterUsage["searchReadingAmount"].toLocaleString("en-AU", { minimumFractionDigits: 3, maximumFractionDigits: 3 })}kL
                        </div>
                    </div>

                    <div className="ms-Grid-row">
                        <div className="ms-Grid-col ms-sm8">
                            Date Of Search Reading: {waterUsage['searchDate'].toDateString()}
                        </div>
                        <div className="ms-Grid-col ms-sm4">
                            Reading: {waterUsage["paidReadingAmount"].toLocaleString("en-AU", { minimumFractionDigits: 3, maximumFractionDigits: 3 })}kL
                        </div>
                    </div>
                </div>

                <div className="section">
                    <div className="ms-Grid-row">
                        <div className="ms-Grid-col ms-sm8">
                            <b>Average Daily Usage</b>
                        </div>
                        <div className="ms-Grid-col ms-sm4">
                            <b>Charge Per kL</b>
                        </div>
                    </div>

                    <div className="ms-Grid-row">
                        <div className="ms-Grid-col ms-sm8">
                            Days between readings: {waterUsage['numberOfDays'].toLocaleString("en-AU", { maximumFractionDigits: 0 })}
                        </div>
                        <div className="ms-Grid-col ms-sm4">
                            ${waterUsage['tier1Charge'].toLocaleString("en-AU", { minimumFractionDigits: 3, maximumFractionDigits: 3 })} for first {waterUsage['tier1KlCount'].toLocaleString("en-AU", { minimumFractionDigits: 3, maximumFractionDigits: 3 })}KL
                        </div>
                    </div>

                    <div className="ms-Grid-row">
                        <div className="ms-Grid-col ms-sm8">
                            {waterUsage["method"] == "daily-average" ? waterUsage["averageKlCount"].toLocaleString("en-AU", { minimumFractionDigits: 2, maximumFractionDigits: 2 })
                                : `${waterUsage["searchReadingAmount"].toLocaleString("en-AU", { minimumFractionDigits: 3, maximumFractionDigits: 3 })}kL - ${waterUsage["paidReadingAmount"].toLocaleString("en-AU", { minimumFractionDigits: 3, maximumFractionDigits: 3 })}kL = ${waterUsage["diffAmountReading"].toLocaleString("en-AU", { minimumFractionDigits: 3, maximumFractionDigits: 3 })}kL / ${waterUsage["numberOfDays"].toLocaleString("en-AU", { maximumFractionDigits: 0 })} = ${waterUsage["dailyUsage"].toLocaleString("en-AU", { minimumFractionDigits: 3, maximumFractionDigits: 3 })}kl`}
                        </div>
                        <div className="ms-Grid-col ms-sm4">
                            ${waterUsage['tier2Charge']} for next {waterUsage['tier2KlCount'].toLocaleString("en-AU", { minimumFractionDigits: 3, maximumFractionDigits: 3 })}KL
                        </div>
                    </div>

                    <div className="ms-Grid-row">
                        <div className="ms-Grid-col ms-sm8">
                            Days from Date Paid to Settlement = {waterUsage['partDays'].toLocaleString("en-AU", { maximumFractionDigits: 0 })}
                        </div>
                        <div className="ms-Grid-col ms-sm4">
                            ${waterUsage['balanceCharge']} for the Balance
                        </div>
                    </div>

                    <div className="ms-Grid-row">
                        <div className="ms-Grid-col ms-sm8">
                            {waterUsage["dailyUsage"].toLocaleString("en-AU", { minimumFractionDigits: 3, maximumFractionDigits: 3 })}kL x {waterUsage["partDays"].toLocaleString("en-AU", { maximumFractionDigits: 0 })}days = <b>{waterUsage["dailyAndDays"].toLocaleString("en-AU", { minimumFractionDigits: 3, maximumFractionDigits: 3 })}kL</b>
                        </div>
                        <div className="ms-Grid-col ms-sm4">
                            Bulk water ${waterUsage['bulkCharge']}
                        </div>
                    </div>

                    <div className="ms-Grid-row">
                        <div className="ms-Grid-col ms-sm8">
                            (All kL results are rounded to whole litres ie 3 decimal places)
                        </div>
                    </div>
                </div>

                <div className="section">
                    <div className="left-align-section-header">
                        <b><i>Water Adjustment</i></b>
                    </div>

                    <div className="ms-Grid-row">
                        <div className="ms-Grid-col ms-sm6">
                            Days from date paid to 30 June = {waterUsage["numberOfDaysToJune"].toLocaleString("en-AU", { maximumFractionDigits: 0 })}<br />
                            Days from 1 July to settlement = {waterUsage["numberOfDaysFromJune"].toLocaleString("en-AU", { maximumFractionDigits: 0 })}
                        </div>
                        <div className="ms-Grid-col ms-sm6">
                            <b><i>Charge per kL from 1 July</i></b><br />
                            ${waterUsage["tier1FeeIncrease"]} for first ${waterUsage["tier1KlCount"].toLocaleString("en-AU", { minimumFractionDigits: 3, maximumFractionDigits: 3 })} kL<br />
                            ${waterUsage["tier2FeeIncrease"]} for next ${waterUsage["tier2KlCount"].toLocaleString("en-AU", { minimumFractionDigits: 3, maximumFractionDigits: 3 })}  kL<br />
                            ${waterUsage["balanceFeeIncrease"]}  for the balance<br /> Bulk water ${waterUsage["bulkFeeIncrease"]}
                        </div>
                    </div>

                    <div className="ms-Grid-row">
                        <div className="ms-Grid-col ms-sm6">
                            <i>(All kL results are rounded to whole litres ie 3 decimal places)</i>
                        </div>
                        <div className="ms-Grid-col ms-sm6">
                        </div>
                    </div>

                    <div className="ms-Grid-row">
                        <div className="ms-Grid-col ms-sm6">
                            Balance
                        </div>
                        {!adjustmentFlag ?
                            <div className="ms-Grid-col ms-sm3">
                                ${waterUsage["balanceCalc"].toLocaleString("en-AU", { minimumFractionDigits: 3, maximumFractionDigits: 3 })}kL x {waterUsage["balanceCharge"]}
                            </div>
                            :
                            <div className="ms-Grid-col ms-sm3">
                                ${waterUsage["balanceCalc"].toLocaleString("en-AU", { minimumFractionDigits: 3, maximumFractionDigits: 3 })}kL x {waterUsage["balanceFeeIncrease"].toLocaleString("en-AU", { minimumFractionDigits: 3, maximumFractionDigits: 3 })} x {waterUsage["numberOfDaysFromJune"].toLocaleString("en-AU", { maximumFractionDigits: 0 })} / {waterUsage["partDays"].toLocaleString("en-AU", { maximumFractionDigits: 0 })}
                            </div>
                        }
                        <div className="ms-Grid-col ms-sm3">
                            ${waterUsage["balanceResult"].toLocaleString("en-AU", { minimumFractionDigits: 3, maximumFractionDigits: 3 })}
                        </div>
                    </div>

                    <div className="ms-Grid-row">
                        <div className="ms-Grid-col ms-sm6">
                            Bulk Water
                        </div>
                        {!adjustmentFlag ?
                            <div className="ms-Grid-col ms-sm3">
                                {waterUsage["dailyAndDays"].toLocaleString("en-AU", { minimumFractionDigits: 3, maximumFractionDigits: 3 })}kL x {waterUsage["bulkCharge"]}
                            </div>
                            :
                            <div className="ms-Grid-col ms-sm3">
                                {waterUsage["dailyAndDays"].toLocaleString("en-AU", { minimumFractionDigits: 3, maximumFractionDigits: 3 })}kL x {waterUsage["bulkFeeIncrease"].toLocaleString("en-AU", { minimumFractionDigits: 3, maximumFractionDigits: 3 })} x {waterUsage["numberOfDaysFromJune"].toLocaleString("en-AU", { maximumFractionDigits: 0 })} / {waterUsage["partDays"].toLocaleString("en-AU", { maximumFractionDigits: 0 })}
                            </div>
                        }
                        <div className="ms-Grid-col ms-sm3">
                            ${waterUsage["bulkResult"].toLocaleString("en-AU", { minimumFractionDigits: 3, maximumFractionDigits: 3 })}
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}