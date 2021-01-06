import * as React from 'react'
import { DefaultButton, PrimaryButton, IconButton, IButtonProps } from 'office-ui-fabric-react/lib/Button';
import { Dropdown, IDropdownOption } from 'office-ui-fabric-react/lib/Dropdown';
import { connect } from 'react-redux'

interface IMapStateToProps {
    state;
}

interface IMapDispatchToProps { }

interface IProps {
    updateValue;
}

type AppProps = IMapStateToProps & IProps & IMapDispatchToProps;

type AppStates = {}

export class AddAdjustment extends React.Component<AppProps, AppStates> {

    constructor(props: any) {
        super(props);

        this.adjustmentTypes = this.props.state == "NSW"
            ?
            [
                { key: 'Release Fee', text: 'Release Fee' },
                { key: 'Water Usage', text: 'Water Usage' },
                { key: 'Emergency Services Levy', text: 'Emergency Services Levy' },
                { key: 'Water and Sewerage Rates', text: 'Water and Sewerage Rates' },
                { key: 'Council Rates', text: 'Council Rates' },
                { key: 'Water/Sewerage Rates', text: 'Water/Sewerage Rates' },
                { key: 'Strata Levies', text: 'Strata Levies' },
                { key: 'Insurance', text: 'Insurance' },
                { key: 'Penalty Interest', text: 'Penalty Interest' },
                { key: 'Other Adjustment', text: 'Other Adjustment Fixed' },
                { key: 'Other Adjustment Date', text: 'Other Adjustment Date' },
            ]
            :
            this.props.state == "VIC" ? [
                { key: 'Release Fee', text: 'Release Fee' },
                { key: 'Water Usage', text: 'Water Usage' },
                { key: 'Emergency Services Levy', text: 'Emergency Services Levy' },
                { key: 'Water and Sewerage Rates', text: 'Water and Sewerage Rates' },
                { key: 'Council Rates', text: 'Council Rates, Charges & Levies' },
                { key: 'Water Drainage Fee', text: 'Water Drainage Fee' },
                { key: 'Parks Charge', text: 'Parks Charge' },
                { key: 'Water Service Charge', text: 'Water Service Charge' },
                { key: 'Water Rates, Charges & Levies', text: 'Water Rates, Charges & Levies' },
                { key: 'Sewerage Access Fee', text: 'Sewerage Service Charge' },
                { key: 'Sewerage Usage', text: 'Sewerage Usage' },
                { key: 'Owners Corporation Fees', text: 'Owners Corporation Fees' },
                { key: 'Owners Corporation - Administration Fund Fee', text: 'Owners Corporation - Administration Fund Fee' },
                { key: 'Owners Corporation - Maintenance Fund Fee', text: 'Owners Corporation - Maintenance Fund Fee' },
                { key: 'Owners Corporation - Sinking Fund Fee', text: 'Owners Corporation - Sinking Fund Fee' },
                { key: 'Owners Corporation - Insurance', text: 'Owners Corporation - Insurance' },
                { key: 'Land Tax', text: 'Land Tax' },
                { key: 'Penalty Interest', text: 'Penalty Interest' },
                { key: 'Rent', text: 'Rent' },
                { key: 'Other Adjustment', text: 'Other Adjustment Fixed' },
                { key: 'Other Adjustment Date', text: 'Other Adjustment Date' },
            ]
                :
                this.props.state == "QLD" ?
                    [
                        { key: 'Release Fee', text: 'Release Fee' },
                        { key: 'Water Usage', text: 'Water Usage' },
                        { key: 'Emergency Services Levy', text: 'Emergency Services Levy' },
                        { key: 'Water and Sewerage Rates', text: 'Water and Sewerage Rates' },
                        { key: 'Council Rates', text: 'Council Rates' },
                        { key: 'Water Access Fee', text: 'Water Access Fee' },
                        { key: 'Sewerage Access Fee', text: 'Sewerage Access Fee' },
                        { key: 'Water/Sewerage Rates', text: 'Water/Sewerage Rates' },
                        { key: 'Administration Fund', text: 'Administration Fund' },
                        { key: 'Sinking Fund', text: 'Sinking Fund' },
                        { key: 'Insurance', text: 'Insurance' },
                        { key: 'Penalty Interest', text: 'Penalty Interest' },
                        { key: 'Other Adjustment', text: 'Other Adjustment' },
                        { key: 'Other Adjustment Date', text: 'Other Adjustment Date' },
                    ]
                    :
                    [
                        { key: 'Release Fee', text: 'Release Fee' },
                        { key: 'Water Usage', text: 'Water Usage' },
                        { key: 'Emergency Services Levy', text: 'Emergency Services Levy' },
                        { key: 'Water and Sewerage Rates', text: 'Water and Sewerage Rates' },
                        { key: 'Council Rates', text: 'Council Rates' },
                        { key: 'Water Access Fee', text: 'Water Access Fee' },
                        { key: 'Sewerage Access Fee', text: 'Sewerage Access Fee' },
                        { key: 'Administration Fund', text: 'Administration Fund' },
                        { key: 'Sinking Fund', text: 'Sinking Fund' },
                        { key: 'Insurance', text: 'Insurance' },
                        { key: 'Penalty Interest', text: 'Penalty Interest' },
                        { key: 'Other Adjustment', text: 'Other Adjustment' },
                        { key: 'Other Adjustment Date', text: 'Other Adjustment Date' },
                    ];
    }

    adjustmentTypes: IDropdownOption[];

    public render(): JSX.Element {
        const adjustmentTypes = this.adjustmentTypes;

        return (
            <div className="modal-body">
                <div className="ms-Grid" dir="ltr">
                    <div className="ms-Grid-row modal-row">
                        <div className="ms-Grid-col ms-sm4 modal-label">New Item Type :</div>
                        <div className="ms-Grid-col ms-sm8">
                            <Dropdown
                                data-cy="adjustment_type_select"
                                id="adjustment_type_select"
                                options={adjustmentTypes}
                                defaultSelectedKey="Water Usage"
                                onChange={(ev, item) => this.props.updateValue(item.key, 'itemType')}
                            />
                        </div>
                    </div>

                </div>
            </div>
        )
    }
}

const mapStateToProps = state => {
    return {
        state: state.settlementInfo.state
    }
}

export default connect(mapStateToProps)(AddAdjustment);