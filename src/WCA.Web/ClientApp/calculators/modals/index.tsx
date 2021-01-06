import * as React from 'react'
import { DefaultButton, PrimaryButton, IconButton, IButtonProps } from 'office-ui-fabric-react/lib/Button';
import { MessageBar, MessageBarType } from 'office-ui-fabric-react/lib/MessageBar';

import AddAdjustment from './add-adjustment'
import MatterDetails from './matter-details'
import ContractPrice from './contract-price'
import ReleaseFee from './release-fee';
import WaterUsage from './water-usage';
import AdjustmentTemplate from './adjustment-template';
import PenaltyInterest from './penalty-interest';
import OtherAdjustment from './other-adjustment';
import PayeeDetails from './payee-details';
import OtherAdjustmentDate from './other-adjustment-date';
import OurRequirements from './our-requirements';
import AdditionalRequirements from './additional-requirements';
import Fee from './fee';

interface IMapStateToProps { }

interface IMapDispatchToProps { }

interface IProps {
    modalID;
    modalIDs;
    matterDetails;
    index;
    balanceFunds;
    actionstepData;
    closeModal;
    saveModal;
    deleteData;
    updateValue;
    updatedState;
    dataInputError;
}

type AppProps = IMapStateToProps & IProps & IMapDispatchToProps;

type AppStates = {}

export default class Modals extends React.Component<AppProps, AppStates> {
    constructor(props: any) {
        super(props);
    }

    private modalContent(): JSX.Element {
        const { modalID, modalIDs, index, actionstepData } = this.props;

        switch (modalID) {
            case modalIDs.matterDetails:
                return (
                    <MatterDetails
                        updateValue={(newValue, whichValue, needRefresh = false) => this.props.updateValue(newValue, whichValue, needRefresh)}
                        matterDetails={this.props.matterDetails}
                        index={index}
                        actionstepData={actionstepData}
                    />
                );

            case modalIDs.addAdjustment:
                return (
                    <AddAdjustment
                        updateValue={(newValue, whichValue, needRefresh = false) => this.props.updateValue(newValue, whichValue, needRefresh)}
                        index={index}
                    />
                );

            case modalIDs.contractPrice:
                return (
                    <ContractPrice
                        updateValue={(newValue, whichValue, needRefresh = false) => this.props.updateValue(newValue, whichValue, needRefresh)}
                        updatedState={this.props.updatedState}
                        index={index}
                    />
                );

            case modalIDs.releaseFee:
                return (
                    <ReleaseFee
                        updateValue={(newValue, whichValue, needRefresh = false) => this.props.updateValue(newValue, whichValue, needRefresh)}
                        updatedState={this.props.updatedState}
                        index={index}
                    />
                );

            case modalIDs.waterUsage:
                return (
                    <WaterUsage
                        updateValue={(newValue, whichValue, needRefresh = false) => this.props.updateValue(newValue, whichValue, needRefresh)}
                        updatedState={this.props.updatedState}
                        index={index}
                    />
                );

            case modalIDs.councilRates:
            case modalIDs.waterAccessFee:
            case modalIDs.sewerageAccessFee:
            case modalIDs.administrationFund:
            case modalIDs.sinkingFund:
            case modalIDs.insurance:
            case modalIDs.strataLevies:
            case modalIDs.waterDrainageFee:
            case modalIDs.parksCharge:
            case modalIDs.waterServiceCharge:
            case modalIDs.sewerageUsage:
            case modalIDs.ownersCorporationFees:
            case modalIDs.maintenanceFund:
            case modalIDs.landTax:
            case modalIDs.rent:
            case modalIDs.councilRatesChargesLevies:
            case modalIDs.waterRatesChargesLevies:
            case modalIDs.sewerageServiceCharge:
            case modalIDs.ownersAdministrationFundFee:
            case modalIDs.ownersMaintenanceFundFee:
            case modalIDs.ownersSinkingFundFee:
            case modalIDs.ownersInsurance:
            case modalIDs.waterSewerageRates:
            case modalIDs.emergencyServicesLevy:
            case modalIDs.waterAndSewerageRates:
                return (
                    <AdjustmentTemplate
                        updateValue={(newValue, whichValue, needRefresh = false) => this.props.updateValue(newValue, whichValue, needRefresh)}
                        updatedState={this.props.updatedState}
                        index={index}
                    />
                );

            case modalIDs.penaltyInterest:
                return (
                    <PenaltyInterest
                        updateValue={(newValue, whichValue, needRefresh = false) => this.props.updateValue(newValue, whichValue, needRefresh)}
                        updatedState={this.props.updatedState}
                        index={index}
                    />
                );

            case modalIDs.otherAdjustment:
                return (
                    <OtherAdjustment
                        updateValue={(newValue, whichValue, needRefresh = false) => this.props.updateValue(newValue, whichValue, needRefresh)}
                        updatedState={this.props.updatedState}
                        index={index}
                    />
                );

            case modalIDs.otherAdjustmentDate:
                return (
                    <OtherAdjustmentDate
                        updateValue={(newValue, whichValue, needRefresh = false) => this.props.updateValue(newValue, whichValue, needRefresh)}
                        updatedState={this.props.updatedState}
                        index={index}
                    />
                );

            case modalIDs.fee:
                return (
                    <Fee
                        updateValue={(newValue, whichValue, needRefresh = false) => this.props.updateValue(newValue, whichValue, needRefresh)}
                        updatedState={this.props.updatedState}
                        index={index}
                    />
                );

            case modalIDs.additionalRequirements:
                return (
                    <AdditionalRequirements
                        updateValue={(newValue, whichValue, needRefresh = false) => this.props.updateValue(newValue, whichValue, needRefresh)}
                        updatedState={this.props.updatedState}
                        index={index}
                    />
                );

            case modalIDs.payeeDetails:
                return (
                    <PayeeDetails
                        updateValue={(newValue, whichValue, needRefresh = false) => this.props.updateValue(newValue, whichValue, needRefresh)}
                        updatedState={this.props.updatedState}
                        balanceFunds={this.props.balanceFunds}
                        index={index}
                    />
                );

            case modalIDs.ourRequirements:
                return (
                    <OurRequirements
                        updateValue={(newValue, whichValue, needRefresh = false) => this.props.updateValue(newValue, whichValue, needRefresh)}
                        updatedState={this.props.updatedState}
                        index={index}
                    />
                );

            default:
                break;
        }

        return (
            <div></div>
        )
    }

    public render(): JSX.Element {
        var { index, modalID, modalIDs, dataInputError } = this.props;

        return (
            <div>
                <div className="modal-header">
                    <span className="modal-title">{modalID}</span>

                    <IconButton
                        className="modal-close-button"
                        onClick={() => this.props.closeModal()}
                        iconProps={{
                            iconName: 'ChromeClose',
                            style: {
                                fontSize: 12,
                                fontWeight: 500
                            }
                        }}
                    />
                </div>

                {dataInputError != "" &&
                    <MessageBar
                        messageBarType={MessageBarType.severeWarning}
                    >
                        {dataInputError}
                    </MessageBar>
                }

                {this.modalContent()}

                <div className="modal-footer">

                    {index >= 0 && (modalID != modalIDs.contractPrice) &&
                        <DefaultButton
                            className="button red-button"
                            data-automation-id="delete_button"
                            data-cy="modal_delete_button"
                            text="Delete"
                            onClick={() => this.props.deleteData(index, modalID)}
                            allowDisabledFocus={true}
                        />
                    }

                    <DefaultButton
                        className="button gray-button"
                        data-automation-id="cancel_button"
                        data-cy="cancel_button"
                        text="Cancel"
                        onClick={() => this.props.closeModal()}
                        allowDisabledFocus={true}
                    />

                    <PrimaryButton
                        className="button"
                        data-automation-id="email_button"
                        text="OK"
                        data-cy="modal_save_button"
                        onClick={() => this.props.saveModal()}
                        allowDisabledFocus={true}
                    />
                </div>
            </div>
        )
    }
}